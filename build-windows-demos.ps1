#!/usr/bin/env pwsh
[CmdletBinding()]
param(
    [switch]$Clean,
    [string]$Configuration = "Release",
    [ValidateSet("win-x64", "win-x86", "win-arm64")]
    [string]$Rid = "win-x64",
    [int]$Jobs = [Environment]::ProcessorCount,
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$Demo
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

function Fail {
    param([string]$Message)
    throw $Message
}

function Get-CommandPathOrNull {
    param([string]$Name)

    $command = Get-Command $Name -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($command) {
        return $command.Source
    }

    return $null
}

function Require-Command {
    param(
        [string]$Name,
        [string]$InstallHint = ""
    )

    $commandPath = Get-CommandPathOrNull $Name
    if (-not $commandPath) {
        $message = "缺少必需命令: $Name"
        if ($InstallHint) {
            $message += "`n安装建议: $InstallHint"
        }

        $message += "`n最小依赖: .NET SDK 10、CMake、Visual Studio 2022 C++ 生成工具(Desktop development with C++)。"
        Fail $message
    }

    return $commandPath
}

function Test-VsGeneratorAvailable {
    param([string]$Generator)

    $cmakePath = Get-CommandPathOrNull "cmake"
    if (-not $cmakePath) {
        return $false
    }

    $help = & $cmakePath --help 2>&1
    return [bool]($help | Select-String -SimpleMatch $Generator)
}

function Get-VswherePath {
    if (-not ${env:ProgramFiles(x86)}) {
        return $null
    }

    $vswhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
    if (Test-Path $vswhere) {
        return $vswhere
    }

    return $null
}

function Get-VisualStudioCppInstallationPath {
    $vswhere = Get-VswherePath
    if (-not $vswhere) {
        return $null
    }

    $installationPath = & $vswhere -latest -products * -requires Microsoft.VisualStudio.Component.VC.Tools.x86.x64 -property installationPath 2>$null | Select-Object -First 1
    if ([string]::IsNullOrWhiteSpace($installationPath)) {
        return $null
    }

    return $installationPath.Trim()
}

function Assert-BuildPrerequisites {
    $cmakePath = Require-Command "cmake" "安装 CMake 并确保 cmake.exe 在 PATH 中。"
    $dotnetPath = Require-Command "dotnet" "安装 .NET SDK 10，并确保 dotnet.exe 在 PATH 中。"

    Write-Host "==> 检查构建依赖"
    Write-Host "    dotnet: $dotnetPath"
    Write-Host "    cmake : $cmakePath"

    if (-not (Test-VsGeneratorAvailable "Visual Studio 17 2022")) {
        $vsInstallation = Get-VisualStudioCppInstallationPath
        $message = "CMake 未检测到生成器 'Visual Studio 17 2022'。"

        if ($vsInstallation) {
            $message += "`n已找到 Visual Studio 安装: $vsInstallation"
            $message += "`n请确认已安装 'Desktop development with C++' 工作负载，且从新的终端重新运行脚本。"
        }
        else {
            $message += "`n未找到带 C++ 工具链的 Visual Studio 2022 安装。"
            $message += "`n请安装 Visual Studio 2022 Build Tools 或 Visual Studio 2022，并勾选 'Desktop development with C++'。"
        }

        $message += "`nCI 最小依赖: windows-latest + .NET SDK 10；GitHub Hosted Runner 通常已自带 VS 2022 C++ 工具链。"
        Fail $message
    }

    $vsInstallation = Get-VisualStudioCppInstallationPath
    if ($vsInstallation) {
        Write-Host "    VS C++: $vsInstallation"
    }
    else {
        Write-Host "    VS C++: 未通过 vswhere 定位，将依赖 CMake 直接解析已安装生成器"
    }
}

function Show-MinimalRequirements {
    Write-Host "==> 最小安装清单"
    Write-Host "    1. .NET SDK 10.x"
    Write-Host "    2. CMake"
    Write-Host "    3. Visual Studio 2022 或 Build Tools 2022 + Desktop development with C++"
    Write-Host "    4. 建议使用 Developer PowerShell / 普通 PowerShell 均可，但安装完成后需重新打开终端"
}

function Get-DemoTargetFramework {
    param([string]$DemoName)

    switch ($DemoName) {
        "SerialPort" { return "net10.0-windows" }
        "WinFormsDemo" { return "net10.0-windows" }
        "PictureBoxDemo" { return "net10.0-windows" }
        "MusicDemo" { return "net10.0-windows" }
        "SmartWatchDemo" { return "net10.0-windows" }
        default { return "net10.0" }
    }
}

function Get-DemoProjectPath {
    param([string]$DemoName)

    switch ($DemoName) {
        "MusicDemo" { return (Join-Path $rootDir "src/Demos/MusicDemo/MusicDemo.csproj") }
        default { return (Join-Path $rootDir "src/Demos/$DemoName/$DemoName.csproj") }
    }
}

function Normalize-Demo {
    param([string]$Name)

    switch ($Name.ToLowerInvariant()) {
        "serialport" { return "SerialPort" }
        "winformsdemo" { return "WinFormsDemo" }
        "pictureboxdemo" { return "PictureBoxDemo" }
        "musicdemo" { return "MusicDemo" }
        "smartwatchdemo" { return "SmartWatchDemo" }
        default { return $null }
    }
}

$allDemos = @(
    "SerialPort",
    "WinFormsDemo",
    "PictureBoxDemo",
    "MusicDemo",
    "SmartWatchDemo"
)

$demoNames = @()
foreach ($item in $Demo) {
    $normalized = Normalize-Demo $item
    if (-not $normalized) {
        Fail "unknown demo: $item"
    }

    if ($demoNames -notcontains $normalized) {
        $demoNames += $normalized
    }
}

if ($demoNames.Count -eq 0) {
    $demoNames = $allDemos
}

switch ($Rid) {
    "win-x64" { $cmakeArch = "x64" }
    "win-x86" { $cmakeArch = "Win32" }
    "win-arm64" { $cmakeArch = "ARM64" }
    default { Fail "unsupported RID: $Rid" }
}

Show-MinimalRequirements
Assert-BuildPrerequisites

$rootDir = $PSScriptRoot
$lvglSourceDir = Join-Path $rootDir "libs/lvgl"
$lvglBuildDir = Join-Path $rootDir "libs/build/lvgl-$Rid"
$distDir = Join-Path $rootDir "dist/$Rid"
$lvBuildConfDir = [System.IO.Path]::GetFullPath((Join-Path $rootDir "libs"))

if ($Clean) {
    Remove-Item -Recurse -Force $distDir -ErrorAction SilentlyContinue
    Remove-Item -Recurse -Force $lvglBuildDir -ErrorAction SilentlyContinue
}

New-Item -ItemType Directory -Force -Path $distDir | Out-Null

Write-Host "==> Building LVGL shared library ($Rid)"
& cmake `
    -S $lvglSourceDir `
    -B $lvglBuildDir `
    -G "Visual Studio 17 2022" `
    -A $cmakeArch `
    -DBUILD_SHARED_LIBS=ON `
    -DCONFIG_LV_BUILD_EXAMPLES=OFF `
    -DCONFIG_LV_BUILD_DEMOS=OFF `
    -DCONFIG_LV_USE_THORVG_INTERNAL=OFF `
    -DCONFIG_LV_USE_PRIVATE_API=ON `
    "-DLV_BUILD_CONF_DIR=$lvBuildConfDir"
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

& cmake --build $lvglBuildDir --config $Configuration --parallel $Jobs
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

$lvglDll = $null
$preferredDllDir = Join-Path $lvglBuildDir $Configuration
if (Test-Path $preferredDllDir) {
    $lvglDll = Get-ChildItem -Path $preferredDllDir -Filter "lvgl.dll" -Recurse -ErrorAction SilentlyContinue |
        Sort-Object FullName |
        Select-Object -First 1
}

if (-not $lvglDll) {
    $lvglDll = Get-ChildItem -Path $lvglBuildDir -Filter "lvgl.dll" -Recurse -ErrorAction SilentlyContinue |
        Sort-Object FullName |
        Select-Object -First 1
}

if (-not $lvglDll) {
    Fail "missing built LVGL shared library under $lvglBuildDir"
}

function Publish-Demo {
    param([string]$DemoName)

    $projectPath = Get-DemoProjectPath $DemoName
    $publishDir = Join-Path $distDir $DemoName
    $executablePath = Join-Path $publishDir "$DemoName.exe"
    $targetFramework = Get-DemoTargetFramework $DemoName

    if (-not (Test-Path $projectPath)) {
        Fail "missing demo project: $projectPath"
    }

    Write-Host "==> Publishing $DemoName ($targetFramework)"
    Remove-Item -Recurse -Force $publishDir -ErrorAction SilentlyContinue

    & dotnet publish $projectPath `
        -f $targetFramework `
        -c $Configuration `
        -r $Rid `
        -o $publishDir `
        -p:EnableWindowsTargeting=true
    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }

    if (-not (Test-Path $executablePath)) {
        Fail "missing published executable: $executablePath"
    }

    Copy-Item -LiteralPath $lvglDll.FullName -Destination (Join-Path $publishDir "lvgl.dll") -Force

    Get-ChildItem -Path $publishDir -File -Filter "*.pdb" -ErrorAction SilentlyContinue |
        Remove-Item -Force -ErrorAction SilentlyContinue
    Get-ChildItem -Path $publishDir -File -Filter "*.dbg" -ErrorAction SilentlyContinue |
        Remove-Item -Force -ErrorAction SilentlyContinue

    Write-Host "    output: $publishDir"
}

foreach ($demoName in $demoNames) {
    Publish-Demo $demoName
}

Write-Host "==> Done"
Write-Host "Published demos under $distDir"
