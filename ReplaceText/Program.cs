using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ReplaceText
{
    internal class Program
    {
        static Program()
        {
            // 在 .NET Core/.NET 5+ 中，需要註冊編碼提供者以支援 Big5、GBK 等編碼
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        // 中央化副檔名清單，使用 HashSet 提升查詢與維護性
        private static readonly HashSet<string> CodeExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".sln", ".cs", ".js", ".vb", ".vbs", ".jsl", ".xsd", ".settings", ".htm", ".html",
            ".cshtml", ".vbhtml", ".aspx", ".ascx", ".ashx", ".master", ".xslt", ".resx",
            ".config", ".cd", ".rdlc", ".wsf", ".css", ".sitemap", ".skin", ".browser", ".disco",
            ".wsdl", ".discomap", ".asa", ".asax", ".asp", ".as", ".asmx", ".webinfo", ".wdproj",
            ".csproj", ".vbproj", ".xsl", ".edmx", ".dbml"
        };

        // 預設會被視為文字但在預設 (bModifyTextFile=false) 時忽略的副檔名。
        // 當 bModifyTextFile 為 true 時，會把這些副檔名包含進掃描範圍。
        private static readonly HashSet<string> TextExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".txt", ".md", ".log", ".sql", ".csv", ".ini", ".json", ".xml", ".yml", ".yaml",
            ".properties", ".toml", ".env", ".lock", ".conf", ".cfg"
        };

        // 已知的二進位副檔名，所有可能會破壞或不應該當成文字處理的檔案
        private static readonly HashSet<string> BinaryExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".exe", ".dll", ".pdb", ".suo", ".cer", ".pfx", ".user", ".bak", ".cab",
            ".0", ".1", ".bc", ".bc!", ".bdf", ".bgf", ".bi", ".bin", ".ctx", ".com",
            ".dmg", ".dmt", ".db", ".sqlite", ".zip", ".rar", ".7z", ".gz", ".flv",
            ".fla", ".swf", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx", ".pdf",
            ".vsd", ".chm", ".rp", ".cp", ".xmind", ".cache", ".resources", ".licenses",
            ".sis", ".wmv", ".avi", ".mp3", ".mp4", ".mdf", ".ldf", ".msg", ".jpg",
            ".gif", ".png", ".bmp", ".tif", ".psd", ".ai", ".ico", ".ttf", ".ttc",
            // development/build artifacts
            ".class", ".jar", ".war", ".ear", ".dex", ".apk", ".aar", ".so", ".dylib",
            ".lib", ".o", ".obj", ".a", ".pyc", ".pyo", ".pyd", ".nupkg", ".vsix",
            ".sdf", ".db3", ".sqlite3", ".sqlite-shm", ".sqlite-wal", ".db-journal",
            ".p12", ".der", ".keystore"
        };

        /// <summary>
        /// 是否為測試執行模式
        /// </summary>
        private static bool bTestRun = false;

        /// <summary>
        /// 是否為詳細輸出模式
        /// </summary>
        private static bool bVerbose = false;

        /// <summary>
        /// 是否顯示完整路徑 (預設為輸出相對路徑)
        /// </summary>
        private static bool bShowFullPath = false;

        /// <summary>
        /// 是否修改已知的文字檔案 (預設會跳過文字資料檔，僅修改 Visual Studio 2010 程式相關檔案)
        /// </summary>
        private static bool bModifyTextFile = false;

        /// <summary>
        /// 讓 GBK (GB18030) 字集優先於 Big5 判斷
        /// </summary>
        private static bool bGBKFirst = false;

        /// <summary>
        /// 是否自動判斷未知檔案類型 (預設僅處理已知的檔案類型)
        /// </summary>
        private static bool bUnknownFileType = false;


        private static void Main(string[] args)
        {
            int argCounter = 0;

            string? path = null;
            string? oldValue = null;
            string? newValue = null;

            #region Argument Checking

            foreach (string item in args)
            {
                if (item.ToUpper() == "/T" || item.ToLower() == "-t")
                {
                    bTestRun = true;
                    continue;
                }
                else if (item.ToUpper() == "/M" || item.ToLower() == "-m")
                {
                    bModifyTextFile = true;
                    continue;
                }
                else if (item.ToUpper() == "/F" || item.ToUpper() == "/FULLPATH" || item.ToLower() == "-f")
                {
                    bShowFullPath = true;
                    continue;
                }
                else if (item.ToUpper() == "/V" || item.ToLower() == "-v")
                {
                    bShowFullPath = true;
                    bVerbose = true;
                    continue;
                }
                // 讓 GBK (GB18030) 字集優先於 Big5 判斷
                else if (item.ToUpper() == "/GBK" || item.ToLower() == "-gbk")
                {
                    bGBKFirst = true;
                    continue;
                }
                // 是否自動判斷未知檔案類型
                else if (item.ToUpper() == "/U" || item.ToLower() == "-u")
                {
                    bUnknownFileType = true;
                    continue;
                }
                else
                {
                    switch (argCounter)
                    {
                        case 0:
                            path = item;
                            argCounter++;
                            break;
                        case 1:
                            oldValue = item;
                            argCounter++;
                            break;
                        case 2:
                            newValue = item;
                            argCounter++;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(path))
            {
                ShowHelp();
                return;
            }

            #endregion

            bool isDir;

            try
            {
                isDir = ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory);
            }
            catch (Exception ex)
            {
                throw new Exception("Path not found: " + path + "(" + ex.Message + ")");
            }

            if (isDir)
            {
                // 使用 EnumerateFiles 並以 extension 交叉比對，確保掃描與 IsIgnored / IsBinary 的判斷使用同一份清單
                var allowedExts = new HashSet<string>(CodeExtensions, StringComparer.OrdinalIgnoreCase);

                if (bModifyTextFile)
                {
                    // 當允許修改文字檔案時，加入文字類型副檔名到掃描清單
                    allowedExts.UnionWith(TextExtensions);
                }

                foreach (string filePath in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                {
                    string ext = Path.GetExtension(filePath);

                    if (string.IsNullOrEmpty(ext))
                    {
                        // 無副檔名的檔案,若啟用 /U 參數則進行文字檔判斷
                        if (bUnknownFileType && IsTextFile(filePath))
                        {
                            ProcessFile(filePath, oldValue, newValue);
                        }
                        continue;
                    }

                    if (allowedExts.Contains(ext))
                    {
                        ProcessFile(filePath, oldValue, newValue);
                    }
                    else if (bUnknownFileType && !BinaryExtensions.Contains(ext) && IsTextFile(filePath))
                    {
                        // 啟用 /U 參數且不在二進位清單中,且通過文字檔判斷
                        ProcessFile(filePath, oldValue, newValue);
                    }
                }

                //foreach (string filePath in files)
                //{
                //    ProcessFile(filePath, oldValue, newValue);
                //}
            }
            else
            {
                ProcessFile(path, oldValue, newValue);
            }

        }

        private static void ShowHelp()
        {
            Console.WriteLine("ReplaceText.exe /T /M /V /F /GBK /U <Directory|File>");
            Console.WriteLine();
            Console.WriteLine("/T\t測試執行模式,不會寫入檔案 (Dry Run)");
            Console.WriteLine("/M\t是否修改已知的文字檔案 (預設會跳過文字資料檔,僅修改 Visual Studio 2010 程式相關檔案)");
            Console.WriteLine("/V\t顯示詳細輸出模式,會顯示所有掃描的檔案清單");
            Console.WriteLine("/F\t顯示完整的檔案路徑(預設僅顯示相對路徑)");
            Console.WriteLine("/GBK\t讓 GBK (GB18030) 字集優先於 Big5 判斷");
            Console.WriteLine("/U\t自動判斷未知檔案類型 (預設僅處理已知的檔案類型)");
            Console.WriteLine();
        }

        private static void ProcessFile(string filePath, string? oldValue, string? newValue)
        {
            if (File.Exists(filePath) && !IsBinaryFileExtenstion(filePath) && !IsSkipFolder(filePath) && !IsIgnoredFileExtenstion(filePath))
            {
                bool is_valid_charset = false;
                string encoding = "UTF8";

                string oldContent = "";

                string oldContent_BIG5 = File.ReadAllText(filePath, Encoding.GetEncoding("Big5"));
                string oldContent_BIG5_Only = GetAllBIG5Chars(filePath);

                string oldContent_GBK = File.ReadAllText(filePath, Encoding.GetEncoding("GBK"));
                string oldContent_GBK_Only = GetAllGBKChars(filePath);

                string oldContent_ISO88591 = File.ReadAllText(filePath, Encoding.GetEncoding("ISO-8859-1"));
                string oldContent_ISO88591_Only = GetAllISO88591Chars(filePath);

                string oldContent_UTF8 = File.ReadAllText(filePath, Encoding.UTF8);
                string oldContent_UTF8_Only = GetAllUTF8Chars(filePath);

                string oldContent_Unicode = File.ReadAllText(filePath, Encoding.Unicode);

                #region ...  判斷 UTF-16 與 UTF-8 編碼  ...

                int b1 = 0;
                int b2 = 0;
                int b3 = 0;

                using (FileStream fs = File.OpenRead(filePath))
                {
                    if (fs.Length > 2)
                    {
                        b1 = fs.ReadByte();
                        b2 = fs.ReadByte();
                        b3 = fs.ReadByte();
                    }
                }

                // UTF-16 (BE) 的 BOM 字元 ( FE FF )
                // http://en.wikipedia.org/wiki/Byte-order_mark
                if (b1 == 0xFE && b2 == 0xFF)
                {
                    is_valid_charset = true;

                    oldContent = oldContent_Unicode;

                    encoding = "Unicode";
                }

                // UTF-16 (LE) 的 BOM 字元 ( FF FE )
                // http://en.wikipedia.org/wiki/Byte-order_mark
                if (b1 == 0xFF && b2 == 0xFE)
                {
                    is_valid_charset = true;

                    oldContent = oldContent_Unicode;

                    encoding = "Unicode";
                }

                // UTF-8 的 BOM 字元 ( EF BB BF )
                // http://zh.wikipedia.org/zh-tw/%E4%BD%8D%E5%85%83%E7%B5%84%E9%A0%86%E5%BA%8F%E8%A8%98%E8%99%9F
                if (b1 == 0xEF && b2 == 0xBB && b3 == 0xBF)
                {
                    is_valid_charset = true;

                    oldContent = oldContent_UTF8;

                    encoding = "UTF8";
                }

                // 判斷沒有 BOM 的 UTF-8 字元
                if (!is_valid_charset && oldContent_UTF8 == oldContent_UTF8_Only)
                {
                    is_valid_charset = true;

                    oldContent = oldContent_UTF8;

                    encoding = "UTF8";
                }

                if (!is_valid_charset && oldContent_Unicode == oldContent_UTF8_Only)
                {
                    is_valid_charset = true;

                    oldContent = oldContent_Unicode;

                    encoding = "Unicode";
                }

                #endregion

                if (bGBKFirst)
                {
                    #region ...  判斷 GBK 編碼  ...

                    if (!is_valid_charset && oldContent_GBK == oldContent_GBK_Only)
                    {
                        is_valid_charset = true;

                        oldContent = oldContent_GBK;

                        encoding = "GBK";
                    }

                    #endregion

                    #region ...  判斷 BIG5 編碼  ...

                    if (!is_valid_charset && oldContent_BIG5 == oldContent_BIG5_Only)
                    {
                        is_valid_charset = true;

                        oldContent = oldContent_BIG5;

                        encoding = "BIG5";
                    }

                    #endregion

                }
                else
                {

                    #region ...  判斷 BIG5 編碼  ...

                    if (!is_valid_charset && oldContent_BIG5 == oldContent_BIG5_Only)
                    {
                        is_valid_charset = true;

                        oldContent = oldContent_BIG5;

                        encoding = "BIG5";
                    }

                    #endregion

                    #region ...  判斷 GBK 編碼  ...

                    if (!is_valid_charset && oldContent_GBK == oldContent_GBK_Only)
                    {
                        is_valid_charset = true;

                        oldContent = oldContent_GBK;

                        encoding = "GBK";
                    }

                    #endregion

                }

                #region ...  判斷 ISO-8859-1 編碼  ...

                if (!is_valid_charset && oldContent_ISO88591 == oldContent_ISO88591_Only)
                {
                    is_valid_charset = true;

                    oldContent = oldContent_ISO88591;

                    encoding = "ISO-8859-1";
                }

                #endregion

                if (!is_valid_charset)
                {
                    Console.Write(StripCurrentPath(filePath));
                    ConsoleWriteLineWithColor(" 含無效文字或錯誤編碼(僅支援UTF-8、Unicode、Big5、GBK與ISO-8859-1編碼)", ConsoleColor.Yellow);
                    return;
                }

                string newContent = oldContent;

                #region 執行字串取代動作

                if (!bTestRun && !string.IsNullOrEmpty(oldValue) && newValue != null)
                {
                    newContent = oldContent.Replace(oldValue, newValue);
                }

                #endregion

                if (!string.IsNullOrEmpty(newContent))
                {
                    if (oldContent != newContent)
                    {
                        Console.Write(StripCurrentPath(filePath));
                        ConsoleWriteWithColor(" 寫入中(" + encoding + ")", ConsoleColor.Green);

                        if (!bTestRun)
                        {
                            File.WriteAllText(filePath, newContent, Encoding.UTF8);
                        }

                        ConsoleWriteLineWithColor("done", ConsoleColor.Green);
                    }
                    else if (encoding == "BIG5")
                    {
                        Console.Write(StripCurrentPath(filePath));
                        ConsoleWriteLineWithColor(" (BIG5 -> UTF-8)", ConsoleColor.Green);

                        if (!bTestRun)
                        {
                            File.WriteAllText(filePath, newContent, Encoding.UTF8);
                        }
                    }
                    else if (encoding == "GBK")
                    {
                        Console.Write(StripCurrentPath(filePath));
                        ConsoleWriteLineWithColor(" (GBK -> UTF-8)", ConsoleColor.DarkGreen);

                        if (!bTestRun)
                        {
                            File.WriteAllText(filePath, newContent, Encoding.UTF8);
                        }
                    }
                    else if (encoding == "Unicode")
                    {
                        Console.Write(StripCurrentPath(filePath));
                        ConsoleWriteLineWithColor(" (Unicode -> UTF-8)", ConsoleColor.Green);

                        if (!bTestRun)
                        {
                            File.WriteAllText(filePath, newContent, Encoding.UTF8);
                        }
                    }
                    else if (encoding == "ISO-8859-1")
                    {
                        Console.Write(StripCurrentPath(filePath));
                        ConsoleWriteLineWithColor(" (ISO-8859-1 -> UTF-8)", ConsoleColor.Green);

                        if (!bTestRun)
                        {
                            File.WriteAllText(filePath, newContent, Encoding.UTF8);
                        }
                    }
                    else if (bVerbose)
                    {
                        Console.Write(StripCurrentPath(filePath));
                        ConsoleWriteLineWithColor(" 檔案為 UTF-8 編碼，直接跳過", ConsoleColor.Gray);
                    }
                }
            }
            else
            {
                if (bVerbose)
                {
                    Console.Write(StripCurrentPath(filePath));
                    Console.WriteLine(" 檔案不存在、檔案在忽略清單目錄中或此檔案為已知的二進位檔案");
                }
            }
        }

        private static string StripCurrentPath(string path)
        {
            if (bShowFullPath)
            {
                return path;
            }
            else
            {
                return path.Replace(Environment.CurrentDirectory, "");
            }
        }

        private static void ConsoleWriteLineWithColor(string msg, ConsoleColor color)
        {
            ConsoleColor tmpColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = tmpColor;
        }

        private static void ConsoleWriteWithColor(string msg, ConsoleColor color)
        {
            ConsoleColor tmpColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ForegroundColor = tmpColor;
        }

        private static string ReadAllChars(string path)
        {
            byte[] _bytes = File.ReadAllBytes(path);

            List<char> _cList = new List<char>();

            foreach (byte b in _bytes)
            {
                _cList.Add((char)b);
            }

            return new string(_cList.ToArray());
        }

        private static string GetAllISO88591Chars(string path)
        {
            // http://en.wikipedia.org/wiki/ISO/IEC_8859-1

            Regex rx = new Regex(@"[\x00]
                                 | [\x09\x0A\x0D\x20-\x7E]            # ASCII
                                 | [\xA0-\xFF]                        # Western European (ISO-8859-1) range
                                 ", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);

            StringBuilder sb = new StringBuilder();

            string data = ReadAllChars(path);

            foreach (Match mx in rx.Matches(data))
            {
                byte[] bb = new byte[mx.Value.Length];

                for (int i = 0; i < mx.Value.Length; i++)
                {
                    bb[i] = (byte)mx.Value[i];
                }

                string a = Encoding.GetEncoding("ISO-8859-1").GetString(bb);

                sb.Append(a);
            }

            return sb.ToString();
        }

        private static string GetAllGBKChars(string path)
        {
            // GB18030
            // http://publib.boulder.ibm.com/infocenter/aix/v6r1/index.jsp?topic=/com.ibm.aix.nls/doc/nlsgdrf/code_range_big5.htm

            // GB2312 (Simplified Chinese) character code table
            // http://ash.jp/code/cn/gb2312tbl.htm

            Regex rx = new Regex(@"[\x00]
                                 | [\x09\x0A\x0D\x20-\x7E]   # ASCII
                                 | [\xA1-\xA9][\xA1-\xFE]    # GB2312, GB12345 (GBK/1)
                                 | [\xA8-\xA9][\x40-\xA0]    # Big5, Symbols (GBK/5)
                                 | [\xB0-\xF7][\xA1-\xFE]    # GB2312 (GBK/2)
                                 | [\x81-\xA0][\x40-\xFE]    # GB13000 (GBK/3)
                                 | [\xAA-\xFE][\x40-\xA0]    # GB13000 (GBK/4)
                                 | [\xAA-\xAF][\xA1-\xFE]    # User defined 1
                                 | [\xF8-\xFE][\xA1-\xFE]    # User defined 2
                                 | [\xA1-\xA7][\x40-\xA0]    # User defined 3
                                 ", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);

            StringBuilder sb = new StringBuilder();

            string data = ReadAllChars(path);

            foreach (Match mx in rx.Matches(data))
            {
                byte[] bb = new byte[mx.Value.Length];

                for (int i = 0; i < mx.Value.Length; i++)
                {
                    bb[i] = (byte)mx.Value[i];
                }

                string a = Encoding.GetEncoding("GBK").GetString(bb);

                sb.Append(a);
            }

            return sb.ToString();
        }

        private static string GetAllBIG5Chars(string path)
        {
            // http://www.ascc.sinica.edu.tw/nl/85/1208/02.txt
            // http://publib.boulder.ibm.com/infocenter/aix/v6r1/index.jsp?topic=/com.ibm.aix.nls/doc/nlsgdrf/code_range_big5.htm
            // http://ash.jp/code/cn/big5tbl.htm
            // http://kura.hanazono.ac.jp/paper/codes.html

            // Code range for Big5 locale
            // http://publib.boulder.ibm.com/infocenter/aix/v6r1/index.jsp?topic=/com.ibm.aix.nls/doc/nlsgdrf/code_range_big5.htm

            Regex rx = new Regex(@"[\x00]
                                 | [\x09\x0A\x0D\x20-\x7E]            # ASCII
                                 | [\xA1-\xFE][\x40-\x7E\xA1-\xFE]    # big5 range
                                 ", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);

            StringBuilder sb = new StringBuilder();

            string data = ReadAllChars(path);

            foreach (Match mx in rx.Matches(data))
            {
                byte[] bb = new byte[mx.Value.Length];

                for (int i = 0; i < mx.Value.Length; i++)
                {
                    bb[i] = (byte)mx.Value[i];
                }

                string a = Encoding.GetEncoding("Big5").GetString(bb);

                sb.Append(a);
            }

            return sb.ToString();
        }

        private static string GetAllUTF8Chars(string path)
        {
            // From http://w3.org/International/questions/qa-forms-utf-8.html
            // It will return true if $p_string is UTF-8, and false otherwise.

            Regex rx = new Regex(@"[\x00]
                                 | [\x09\x0A\x0D\x20-\x7E]            # ASCII
                                 | [\xC2-\xDF][\x80-\xBF]             # non-overlong 2-byte
                                 |  \xE0[\xA0-\xBF][\x80-\xBF]        # excluding overlongs
                                 | [\xE1-\xEC\xEE\xEF][\x80-\xBF]{2}  # straight 3-byte
                                 |  \xED[\x80-\x9F][\x80-\xBF]        # excluding surrogates
                                 |  \xF0[\x90-\xBF][\x80-\xBF]{2}     # planes 1-3
                                 | [\xF1-\xF3][\x80-\xBF]{3}          # planes 4-15
                                 |  \xF4[\x80-\x8F][\x80-\xBF]{2}     # plane 16
                                 ", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);

            StringBuilder sb = new StringBuilder();

            string data = ReadAllChars(path);

            foreach (Match mx in rx.Matches(data))
            {
                byte[] bb = new byte[mx.Value.Length];

                for (int i = 0; i < mx.Value.Length; i++)
                {
                    bb[i] = (byte)mx.Value[i];
                }

                // UTF-8 的 BOM 字元 ( EF BB BF )
                // http://zh.wikipedia.org/zh-tw/%E4%BD%8D%E5%85%83%E7%B5%84%E9%A0%86%E5%BA%8F%E8%A8%98%E8%99%9F
                if (bb[0] == 0xEF && bb[1] == 0xBB && bb[2] == 0xBF)
                {
                    continue;
                }

                string a = Encoding.UTF8.GetString(bb);

                sb.Append(a);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 判斷是否為已知的二進位檔案
        /// </summary>
        /// <seealso cref="http://www.file-extensions.org/filetype/extension/name/binary-files"/>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsBinaryFileExtenstion(string path)
        {
            string ext = Path.GetExtension(path);

            if (string.IsNullOrEmpty(ext))
                return false;

            return BinaryExtensions.Contains(ext);

        }

        /// <summary>
        /// 判斷是否為已知的忽略文字檔案
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsIgnoredFileExtenstion(string path)
        {
            if (!bModifyTextFile)
            {
                string ext = Path.GetExtension(path);

                if (string.IsNullOrEmpty(ext))
                    return false;

                // 當 bModifyTextFile 為 false 時，預設會跳過這些常見資料/文字檔
                if (!bModifyTextFile && TextExtensions.Contains(ext))
                {
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }

        }
        private static bool IsSkipFolder(string path)
        {
            if (path.ToLower().EndsWith(".bat")
                || path.ToLower().Contains(@"\.svn\") || path.ToLower().Contains(@"\_svn\")
                // 跳過一些 zh-cn 的檔案,因為這些可能就是簡體文字,不應該用 Big5 轉換成 UTF-8!
                || path.ToLower().Contains(@"zh-cn")
                || (path.ToLower().EndsWith(".js") && path.ToLower().Contains("lang"))
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 精準判斷檔案是否為文字檔案
        /// 使用多種啟發式規則來檢測二進位內容
        /// </summary>
        /// <param name="filePath">檔案路徑</param>
        /// <returns>如果是文字檔案則返回 true,否則返回 false</returns>
        private static bool IsTextFile(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);

                // 空檔案視為文字檔
                if (fileInfo.Length == 0)
                    return true;

                // 太大的檔案跳過 (超過 100MB 可能是大型資料檔)
                if (fileInfo.Length > 100 * 1024 * 1024)
                    return false;

                // 讀取檔案開頭部分進行分析 (最多讀取 8KB 或整個檔案)
                int bytesToCheck = (int)Math.Min(fileInfo.Length, 8192);
                byte[] buffer = new byte[bytesToCheck];

                using (FileStream fs = File.OpenRead(filePath))
                {
                    fs.Read(buffer, 0, bytesToCheck);
                }

                // 檢查常見的二進位檔案標記 (Magic Numbers)
                if (HasBinarySignature(buffer))
                    return false;

                // 統計可疑字元
                int nullBytes = 0;           // NULL 字元數量
                int controlChars = 0;        // 控制字元數量 (排除常見的換行、Tab 等)
                int highBytes = 0;           // 高位元組 (>= 0x80) 數量
                int printableChars = 0;      // 可列印字元數量

                for (int i = 0; i < buffer.Length; i++)
                {
                    byte b = buffer[i];

                    if (b == 0x00)
                    {
                        nullBytes++;
                    }
                    else if (b == 0x09 || b == 0x0A || b == 0x0D || (b >= 0x20 && b <= 0x7E))
                    {
                        // Tab, LF, CR, 或 ASCII 可列印字元
                        printableChars++;
                    }
                    else if (b < 0x20 && b != 0x1B) // 控制字元 (排除 ESC)
                    {
                        controlChars++;
                    }
                    else if (b >= 0x80)
                    {
                        highBytes++;
                    }
                }

                // 判斷規則:
                // 1. 如果有 NULL 字元且比例超過 1%,很可能是二進位檔
                if (nullBytes > 0 && (double)nullBytes / buffer.Length > 0.01)
                    return false;

                // 2. 如果控制字元比例超過 5%,可能是二進位檔
                if ((double)controlChars / buffer.Length > 0.05)
                    return false;

                // 3. 計算文字字元比例 (可列印 ASCII + 高位元組視為潛在的多位元組字元)
                double textRatio = (double)(printableChars + highBytes) / buffer.Length;

                // 如果文字字元比例低於 85%,可能不是文字檔
                if (textRatio < 0.85)
                    return false;

                // 4. 檢查是否包含有效的 UTF-8 序列或其他文字編碼
                if (!ContainsValidTextEncoding(buffer))
                    return false;

                return true;
            }
            catch
            {
                // 無法讀取的檔案視為非文字檔
                return false;
            }
        }

        /// <summary>
        /// 檢查檔案開頭是否包含已知的二進位檔案標記
        /// </summary>
        private static bool HasBinarySignature(byte[] buffer)
        {
            if (buffer.Length < 4)
                return false;

            // 檢查常見的二進位檔案 Magic Numbers
            // PE executable (Windows EXE/DLL)
            if (buffer[0] == 0x4D && buffer[1] == 0x5A) // "MZ"
                return true;

            // ELF executable (Linux)
            if (buffer[0] == 0x7F && buffer[1] == 0x45 && buffer[2] == 0x4C && buffer[3] == 0x46) // "\x7FELF"
                return true;

            // ZIP/JAR/Office documents
            if (buffer[0] == 0x50 && buffer[1] == 0x4B && buffer[2] == 0x03 && buffer[3] == 0x04) // "PK\x03\x04"
                return true;

            // PNG
            if (buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47)
                return true;

            // JPEG
            if (buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF)
                return true;

            // GIF
            if (buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46) // "GIF"
                return true;

            // PDF
            if (buffer[0] == 0x25 && buffer[1] == 0x50 && buffer[2] == 0x44 && buffer[3] == 0x46) // "%PDF"
                return true;

            // RAR
            if (buffer[0] == 0x52 && buffer[1] == 0x61 && buffer[2] == 0x72 && buffer[3] == 0x21) // "Rar!"
                return true;

            // 7z
            if (buffer.Length >= 6 && buffer[0] == 0x37 && buffer[1] == 0x7A && buffer[2] == 0xBC && buffer[3] == 0xAF && buffer[4] == 0x27 && buffer[5] == 0x1C)
                return true;

            // Class file (Java)
            if (buffer[0] == 0xCA && buffer[1] == 0xFE && buffer[2] == 0xBA && buffer[3] == 0xBE)
                return true;

            return false;
        }

        /// <summary>
        /// 檢查緩衝區是否包含有效的文字編碼序列
        /// </summary>
        private static bool ContainsValidTextEncoding(byte[] buffer)
        {
            // 嘗試將內容解碼為 UTF-8
            try
            {
                string text = Encoding.UTF8.GetString(buffer);

                // 如果包含 Unicode 替換字元 (U+FFFD) 的比例過高,可能不是有效的 UTF-8
                int replacementChars = text.Count(c => c == '\uFFFD');
                if (replacementChars > buffer.Length * 0.1)
                {
                    // UTF-8 解碼失敗率過高,嘗試其他編碼
                    // 檢查是否可能是其他單位元組或雙位元組編碼
                    return ContainsReasonableCharacters(buffer);
                }

                return true;
            }
            catch
            {
                return ContainsReasonableCharacters(buffer);
            }
        }

        /// <summary>
        /// 檢查緩衝區是否包含合理的字元分布
        /// </summary>
        private static bool ContainsReasonableCharacters(byte[] buffer)
        {
            // 統計常見的文字檔案特徵
            int lineBreaks = 0;
            int spaces = 0;
            int alphanumeric = 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                byte b = buffer[i];

                if (b == 0x0A || b == 0x0D) // LF 或 CR
                    lineBreaks++;
                else if (b == 0x20 || b == 0x09) // 空格或 Tab
                    spaces++;
                else if ((b >= 0x30 && b <= 0x39) || // 數字
                         (b >= 0x41 && b <= 0x5A) || // 大寫字母
                         (b >= 0x61 && b <= 0x7A))   // 小寫字母
                    alphanumeric++;
            }

            // 文字檔通常會有換行符號或空格
            if (lineBreaks > 0 || spaces > buffer.Length * 0.05)
                return true;

            // 或者包含大量英數字元
            if (alphanumeric > buffer.Length * 0.3)
                return true;

            return false;
        }
    }
}
