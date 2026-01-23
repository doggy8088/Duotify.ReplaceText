#!/usr/bin/env python3
"""
Duotify.ReplaceText Skill

安全的、可重複、可被 AI / Agent 呼叫的文字轉碼與批次替換能力。
基於 Duotify.ReplaceText .NET Global Tool。

用法:
    duotify-replacetext install              # 安裝工具
    duotify-replacetext doctor               # 環境檢查
    duotify-replacetext run [options]        # 執行轉換
"""

import argparse
import json
import os
import shutil
import subprocess
import sys
from dataclasses import dataclass, field
from datetime import datetime
from pathlib import Path
from typing import Optional


# ============================================================================
# Exit Codes
# ============================================================================
class ExitCode:
    SUCCESS = 0
    GENERAL_ERROR = 1
    TOOL_NOT_INSTALLED = 2
    TARGET_NOT_FOUND = 3
    PERMISSION_DENIED = 4


# ============================================================================
# Data Classes
# ============================================================================
@dataclass
class ReplaceTextOptions:
    """ReplaceText CLI 選項"""
    target: str
    old: Optional[str] = None
    new: Optional[str] = None
    flags: list[str] = field(default_factory=list)
    dry_run: bool = True  # Skill 預設為安全模式
    verbose: bool = False
    full_path: bool = False
    gbk: bool = False
    unknown: bool = False
    mode: Optional[str] = None  # M, MO, mo
    passthrough: list[str] = field(default_factory=list)


@dataclass
class RunResult:
    """執行結果"""
    success: bool
    mode: str
    target: str
    command: list[str]
    exit_code: int
    stdout: str
    stderr: str
    files_scanned: int = 0
    files_changed: int = 0


@dataclass
class MappingSpec:
    """批次替換規格"""
    target: str
    options: dict = field(default_factory=dict)
    replacements: list[dict] = field(default_factory=list)


# ============================================================================
# Utility Functions
# ============================================================================
def print_color(message: str, color: str = "default") -> None:
    """彩色輸出"""
    colors = {
        "red": "\033[0;31m",
        "green": "\033[0;32m",
        "yellow": "\033[1;33m",
        "cyan": "\033[0;36m",
        "default": "\033[0m"
    }
    reset = "\033[0m"
    color_code = colors.get(color, colors["default"])
    print(f"{color_code}{message}{reset}")


def print_step(message: str) -> None:
    print_color(f"➡️  {message}", "cyan")


def print_success(message: str) -> None:
    print_color(f"✅ {message}", "green")


def print_warning(message: str) -> None:
    print_color(f"⚠️  {message}", "yellow")


def print_error(message: str) -> None:
    print_color(f"❌ {message}", "red")


def check_dotnet() -> tuple[bool, str]:
    """檢查 .NET SDK"""
    try:
        result = subprocess.run(
            ["dotnet", "--version"],
            capture_output=True,
            text=True,
            timeout=30
        )
        if result.returncode == 0:
            version = result.stdout.strip()
            major = int(version.split('.')[0])
            if major >= 8:
                return True, version
            return False, f"版本過舊: {version} (需要 8.0+)"
        return False, "執行失敗"
    except FileNotFoundError:
        return False, "找不到 dotnet 命令"
    except Exception as e:
        return False, str(e)


def check_replacetext() -> tuple[bool, str]:
    """檢查 ReplaceText 工具"""
    try:
        result = subprocess.run(
            ["dotnet", "tool", "list", "-g"],
            capture_output=True,
            text=True,
            timeout=30
        )
        if result.returncode == 0:
            for line in result.stdout.lower().split('\n'):
                if 'duotify.replacetext' in line:
                    parts = line.split()
                    if len(parts) >= 2:
                        return True, parts[1]
            return False, "未安裝"
        return False, "無法取得工具清單"
    except Exception as e:
        return False, str(e)


def find_replacetext_command() -> Optional[str]:
    """找到 replacetext 命令"""
    # 嘗試直接執行
    if shutil.which("replacetext"):
        return "replacetext"

    # 嘗試 .NET tools 路徑
    home = Path.home()
    possible_paths = [
        home / ".dotnet" / "tools" / "replacetext",
        home / ".dotnet" / "tools" / "replacetext.exe",
    ]

    for path in possible_paths:
        if path.exists():
            return str(path)

    return None


# ============================================================================
# Command Builders
# ============================================================================
def build_replacetext_cmd(opt: ReplaceTextOptions) -> list[str]:
    """建構 replacetext 命令（100% 對齊 README）"""
    cmd = ["replacetext"]

    # 旗標順序: /T, /mode, 其他旗標, target, old, new

    # Dry Run
    if opt.dry_run:
        cmd.append("/T")

    # 處理模式
    if opt.mode:
        if opt.mode.upper() == "MO":
            cmd.append("/MO")
        elif opt.mode.lower() == "mo":
            cmd.append("-mo")
        elif opt.mode.upper() == "M":
            cmd.append("/M")

    # 其他旗標
    if opt.verbose:
        cmd.append("/V")

    if opt.full_path:
        cmd.append("/F")

    if opt.gbk:
        cmd.append("/GBK")

    if opt.unknown:
        cmd.append("/U")

    # 額外的 flags
    cmd.extend(opt.flags)

    # Passthrough 旗標（直接傳遞給 CLI）
    cmd.extend(opt.passthrough)

    # 目標路徑
    cmd.append(opt.target)

    # 替換字串
    if opt.old is not None and opt.new is not None:
        cmd.append(opt.old)
        cmd.append(opt.new)

    return cmd


# ============================================================================
# Commands
# ============================================================================
def cmd_install(args: argparse.Namespace) -> int:
    """安裝 ReplaceText 工具"""
    print()
    print_color("═" * 60, "cyan")
    print_color("  Duotify.ReplaceText Skill - 安裝程式", "cyan")
    print_color("═" * 60, "cyan")
    print()

    # 檢查 .NET SDK
    print_step("檢查 .NET SDK...")
    ok, version = check_dotnet()
    if not ok:
        print_error(f".NET SDK: {version}")
        print("  請前往 https://dotnet.microsoft.com/download/dotnet/8.0 下載")
        return ExitCode.TOOL_NOT_INSTALLED
    print_success(f".NET SDK 版本: {version}")

    # 檢查現有安裝
    print_step("檢查 ReplaceText 安裝狀態...")
    installed, tool_version = check_replacetext()

    if installed:
        print_warning(f"ReplaceText 已安裝，版本: {tool_version}")
        print_step("檢查更新...")

        result = subprocess.run(
            ["dotnet", "tool", "update", "--global", "Duotify.ReplaceText"],
            capture_output=True,
            text=True
        )

        if result.returncode == 0:
            if "already installed" in result.stdout.lower() or "已經是最新" in result.stdout:
                print_success("已是最新版本")
            else:
                print_success("已更新至最新版本")
        else:
            print_warning("更新檢查失敗，保持現有版本")
    else:
        print_step("安裝 Duotify.ReplaceText...")
        result = subprocess.run(
            ["dotnet", "tool", "install", "--global", "Duotify.ReplaceText"],
            capture_output=True,
            text=True
        )

        if result.returncode == 0:
            print_success("安裝成功！")
        else:
            print_error("安裝失敗")
            print(result.stderr)
            return ExitCode.GENERAL_ERROR

    # 驗證
    print_step("驗證安裝...")
    cmd = find_replacetext_command()
    if cmd:
        print_success("ReplaceText 可正常執行")
    else:
        print_warning("可能需要重新開啟終端機")

    print()
    print_color("═" * 60, "green")
    print_color("  安裝完成！", "green")
    print_color("═" * 60, "green")
    print()

    return ExitCode.SUCCESS


def cmd_doctor(args: argparse.Namespace) -> int:
    """環境檢查"""
    print()
    print_color("═" * 60, "cyan")
    print_color("  Duotify.ReplaceText Skill - 環境檢查", "cyan")
    print_color("═" * 60, "cyan")
    print()

    all_ok = True

    # 1. .NET SDK
    print_step("檢查 .NET SDK...")
    ok, version = check_dotnet()
    if ok:
        print_success(f".NET SDK: {version}")
    else:
        print_error(f".NET SDK: {version}")
        all_ok = False

    # 2. ReplaceText 工具
    print_step("檢查 ReplaceText 工具...")
    installed, tool_version = check_replacetext()
    if installed:
        print_success(f"ReplaceText: {tool_version}")
    else:
        print_error(f"ReplaceText: {tool_version}")
        all_ok = False

    # 3. 命令可用性
    print_step("檢查命令可用性...")
    cmd = find_replacetext_command()
    if cmd:
        print_success(f"命令路徑: {cmd}")
    else:
        print_warning("replacetext 命令可能不在 PATH 中")
        all_ok = False

    print()
    if all_ok:
        print_color("═" * 60, "green")
        print_color("  所有檢查通過！環境已準備就緒", "green")
        print_color("═" * 60, "green")
        return ExitCode.SUCCESS
    else:
        print_color("═" * 60, "yellow")
        print_color("  部分檢查未通過，請執行 'duotify-replacetext install'", "yellow")
        print_color("═" * 60, "yellow")
        return ExitCode.TOOL_NOT_INSTALLED


def cmd_run(args: argparse.Namespace) -> int:
    """執行 ReplaceText"""

    # 檢查目標路徑
    target = args.target
    if not target:
        print_error("請指定目標路徑 --target")
        return ExitCode.GENERAL_ERROR

    target_path = Path(target)
    if not target_path.exists():
        print_error(f"目標路徑不存在: {target}")
        return ExitCode.TARGET_NOT_FOUND

    # 檢查 spec 檔案（批次替換模式）
    if args.spec:
        return run_with_spec(args)

    # 建構選項
    opt = ReplaceTextOptions(
        target=str(target_path.resolve()),
        old=args.old,
        new=args.new,
        dry_run=not args.apply,  # --apply 時才真正執行
        verbose=args.verbose,
        full_path=args.full_path,
        gbk=args.gbk,
        unknown=args.unknown,
        mode=args.mode,
        passthrough=args.passthrough or []
    )

    # 備份（v2 功能）
    if args.backup and args.apply:
        backup_dir = target_path / ".replacetext-backup"
        timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
        backup_path = backup_dir / timestamp
        print_step(f"建立備份: {backup_path}")
        # TODO: 實作備份邏輯

    # 建構命令
    cmd = build_replacetext_cmd(opt)

    # 顯示資訊
    print()
    mode_str = "🔍 測試模式 (Dry Run)" if opt.dry_run else "⚡ 執行模式"
    print_color(f"{mode_str}", "cyan" if opt.dry_run else "yellow")
    print(f"📁 目標: {opt.target}")
    if opt.old and opt.new:
        print(f"🔄 替換: \"{opt.old}\" → \"{opt.new}\"")
    print(f"💻 命令: {' '.join(cmd)}")
    print()

    # 執行
    try:
        result = subprocess.run(
            cmd,
            capture_output=not args.verbose,
            text=True,
            cwd=str(target_path) if target_path.is_dir() else str(target_path.parent)
        )

        # 輸出結果
        if args.output == "json":
            output = {
                "success": result.returncode == 0,
                "mode": "dry-run" if opt.dry_run else "apply",
                "target": opt.target,
                "command": cmd,
                "exitCode": result.returncode,
                "stdout": result.stdout if not args.verbose else "",
                "stderr": result.stderr if not args.verbose else ""
            }
            print(json.dumps(output, ensure_ascii=False, indent=2))
        else:
            if not args.verbose and result.stdout:
                print(result.stdout)
            if result.stderr:
                print_warning(result.stderr)

        if result.returncode == 0:
            if opt.dry_run:
                print()
                print_success("測試完成！")
                print_color("  要實際執行，請加上 --apply 參數", "yellow")
            else:
                print()
                print_success("執行完成！")

        return result.returncode

    except FileNotFoundError:
        print_error("找不到 replacetext 命令")
        print("  請執行 'duotify-replacetext install' 安裝工具")
        return ExitCode.TOOL_NOT_INSTALLED
    except PermissionError:
        print_error("權限不足")
        return ExitCode.PERMISSION_DENIED
    except Exception as e:
        print_error(f"執行錯誤: {e}")
        return ExitCode.GENERAL_ERROR


def run_with_spec(args: argparse.Namespace) -> int:
    """使用 spec 檔案執行批次替換（v2 功能）"""
    spec_path = Path(args.spec)
    if not spec_path.exists():
        print_error(f"Spec 檔案不存在: {args.spec}")
        return ExitCode.TARGET_NOT_FOUND

    try:
        with open(spec_path, 'r', encoding='utf-8') as f:
            spec_data = json.load(f)
    except json.JSONDecodeError as e:
        print_error(f"Spec 檔案格式錯誤: {e}")
        return ExitCode.GENERAL_ERROR

    spec = MappingSpec(
        target=spec_data.get("target", args.target),
        options=spec_data.get("options", {}),
        replacements=spec_data.get("replacements", [])
    )

    if not spec.replacements:
        print_warning("Spec 檔案中沒有替換規則")
        return ExitCode.SUCCESS

    print()
    print_color(f"📋 批次替換模式 - {len(spec.replacements)} 個規則", "cyan")
    print(f"📁 目標: {spec.target}")
    print()

    # 依序執行每個替換
    success_count = 0
    fail_count = 0

    for i, replacement in enumerate(spec.replacements, 1):
        old_str = replacement.get("old")
        new_str = replacement.get("new")

        if not old_str or new_str is None:
            print_warning(f"跳過規則 {i}: 缺少 old 或 new")
            continue

        print_step(f"[{i}/{len(spec.replacements)}] \"{old_str}\" → \"{new_str}\"")

        # 建構選項
        flags = []
        if spec.options.get("flags"):
            for flag in spec.options["flags"]:
                if flag.upper() == "MO":
                    flags.append("/MO")
                elif flag.upper() == "M":
                    flags.append("/M")
                elif flag.upper() == "GBK":
                    flags.append("/GBK")
                elif flag.upper() == "V":
                    flags.append("/V")
                elif flag.upper() == "F":
                    flags.append("/F")
                elif flag.upper() == "U":
                    flags.append("/U")

        opt = ReplaceTextOptions(
            target=spec.target,
            old=old_str,
            new=new_str,
            flags=flags,
            dry_run=spec.options.get("dryRun", not args.apply)
        )

        cmd = build_replacetext_cmd(opt)

        try:
            result = subprocess.run(cmd, capture_output=True, text=True)
            if result.returncode == 0:
                print_success(f"  完成")
                success_count += 1
            else:
                print_error(f"  失敗: {result.stderr}")
                fail_count += 1
        except Exception as e:
            print_error(f"  錯誤: {e}")
            fail_count += 1

    print()
    print_color("═" * 60, "green" if fail_count == 0 else "yellow")
    print_color(f"  完成: {success_count} 成功, {fail_count} 失敗", "green" if fail_count == 0 else "yellow")
    print_color("═" * 60, "green" if fail_count == 0 else "yellow")

    return ExitCode.SUCCESS if fail_count == 0 else ExitCode.GENERAL_ERROR


# ============================================================================
# Main
# ============================================================================
def main() -> int:
    parser = argparse.ArgumentParser(
        prog="duotify-replacetext",
        description="Duotify.ReplaceText Skill - 安全的文字轉碼與批次替換能力",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
範例:
  %(prog)s install                          # 安裝工具
  %(prog)s doctor                           # 環境檢查
  %(prog)s run --target ./MyProject         # 預覽變更（dry-run）
  %(prog)s run --target ./MyProject --apply # 實際執行
  %(prog)s run --target ./MyProject --old "foo" --new "bar" --apply
  %(prog)s run --target ./MyProject -- /MO /GBK /V
        """
    )

    subparsers = parser.add_subparsers(dest="command", help="可用命令")

    # install 命令
    install_parser = subparsers.add_parser("install", help="安裝或更新 ReplaceText 工具")
    install_parser.set_defaults(func=cmd_install)

    # doctor 命令
    doctor_parser = subparsers.add_parser("doctor", help="檢查環境與工具")
    doctor_parser.set_defaults(func=cmd_doctor)

    # run 命令
    run_parser = subparsers.add_parser("run", help="執行文字轉換或替換")
    run_parser.add_argument("--target", "-t", required=True, help="目標目錄或檔案")
    run_parser.add_argument("--old", "-o", help="要替換的原始字串")
    run_parser.add_argument("--new", "-n", help="替換後的新字串")
    run_parser.add_argument("--mode", "-m", choices=["M", "MO", "mo"], help="處理模式")
    run_parser.add_argument("--apply", "-a", action="store_true", help="確認執行（預設為 dry-run）")
    run_parser.add_argument("--verbose", "-v", action="store_true", help="詳細輸出")
    run_parser.add_argument("--full-path", "-f", action="store_true", help="顯示完整路徑")
    run_parser.add_argument("--gbk", action="store_true", help="GBK 優先模式")
    run_parser.add_argument("--unknown", "-u", action="store_true", help="自動判斷未知檔案")
    run_parser.add_argument("--backup", "-b", action="store_true", help="建立備份（v2）")
    run_parser.add_argument("--spec", "-s", help="批次替換 spec 檔案（v2）")
    run_parser.add_argument("--output", choices=["text", "json"], default="text", help="輸出格式")
    run_parser.add_argument("passthrough", nargs="*", help="傳遞給 replacetext CLI 的額外參數")
    run_parser.set_defaults(func=cmd_run)

    # 解析參數
    args = parser.parse_args()

    if not args.command:
        parser.print_help()
        return ExitCode.SUCCESS

    return args.func(args)


if __name__ == "__main__":
    sys.exit(main())
