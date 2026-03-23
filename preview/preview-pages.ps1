param(
    [int]$Port = 4000,
    [switch]$NoServe
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$pagesSrc = Join-Path $repoRoot 'pages-src'
$siteDir = Join-Path $repoRoot '_site'

Write-Host 'Preparing local Pages preview source...' -ForegroundColor Cyan

if (Test-Path $pagesSrc) {
    Remove-Item $pagesSrc -Recurse -Force
}

if (Test-Path $siteDir) {
    Remove-Item $siteDir -Recurse -Force
}

New-Item -ItemType Directory -Path (Join-Path $pagesSrc 'blog') -Force | Out-Null

Copy-Item (Join-Path $repoRoot 'docs\_config.yml') (Join-Path $pagesSrc '_config.yml')
Copy-Item (Join-Path $repoRoot 'docs\index.md') (Join-Path $pagesSrc 'index.md')
Copy-Item (Join-Path $repoRoot 'docs\index.en.md') (Join-Path $pagesSrc 'index.en.md')
Copy-Item (Join-Path $repoRoot 'docs\navigation.md') (Join-Path $pagesSrc 'navigation.md')
Copy-Item (Join-Path $repoRoot 'docs\navigation.en.md') (Join-Path $pagesSrc 'navigation.en.md')
Copy-Item (Join-Path $repoRoot 'docs\ci-workflows.md') (Join-Path $pagesSrc 'ci-workflows.md')
Copy-Item (Join-Path $repoRoot 'docs\ci-workflows.en.md') (Join-Path $pagesSrc 'ci-workflows.en.md')
Copy-Item (Join-Path $repoRoot 'docs\WSL-Developer-Guide.md') (Join-Path $pagesSrc 'WSL-Developer-Guide.md')
Copy-Item (Join-Path $repoRoot 'docs\WSL-Developer-Guide.en.md') (Join-Path $pagesSrc 'WSL-Developer-Guide.en.md')
Copy-Item (Join-Path $repoRoot 'docs\blog-winforms-over-lvgl.md') (Join-Path $pagesSrc 'blog-winforms-over-lvgl.md')
Copy-Item (Join-Path $repoRoot 'docs\blog-nativeaot-gui.md') (Join-Path $pagesSrc 'blog-nativeaot-gui.md')
Copy-Item (Join-Path $repoRoot 'docs\blog-linux-hosts.md') (Join-Path $pagesSrc 'blog-linux-hosts.md')
Copy-Item (Join-Path $repoRoot 'docs\blog-architecture.md') (Join-Path $pagesSrc 'blog-architecture.md')
Copy-Item (Join-Path $repoRoot 'docs\blog\index.md') (Join-Path $pagesSrc 'blog\index.md')
Copy-Item (Join-Path $repoRoot 'docs\blog\index.en.md') (Join-Path $pagesSrc 'blog\index.en.md')
Copy-Item (Join-Path $repoRoot 'docs\blog-architecture.md') (Join-Path $pagesSrc 'blog\architecture.md')
Copy-Item (Join-Path $repoRoot 'docs\blog-nativeaot-gui.md') (Join-Path $pagesSrc 'blog\nativeaot-gui.md')
Copy-Item (Join-Path $repoRoot 'docs\blog-linux-hosts.md') (Join-Path $pagesSrc 'blog\linux-hosts.md')
Copy-Item (Join-Path $repoRoot 'docs\blog-winforms-over-lvgl.md') (Join-Path $pagesSrc 'blog\winforms-over-lvgl.md')
Copy-Item (Join-Path $repoRoot 'docs\blog\architecture.en.md') (Join-Path $pagesSrc 'blog\architecture.en.md')
Copy-Item (Join-Path $repoRoot 'docs\blog\nativeaot-gui.en.md') (Join-Path $pagesSrc 'blog\nativeaot-gui.en.md')
Copy-Item (Join-Path $repoRoot 'docs\blog\linux-hosts.en.md') (Join-Path $pagesSrc 'blog\linux-hosts.en.md')
Copy-Item (Join-Path $repoRoot 'docs\blog\why-winforms-over-lvgl.en.md') (Join-Path $pagesSrc 'blog\why-winforms-over-lvgl.en.md')
Copy-Item (Join-Path $repoRoot 'ROADMAP.md') (Join-Path $pagesSrc 'ROADMAP.md')
Copy-Item (Join-Path $repoRoot 'CHANGELOG.md') (Join-Path $pagesSrc 'CHANGELOG.md')

if (Test-Path (Join-Path $repoRoot 'docs\CNAME')) {
    Copy-Item (Join-Path $repoRoot 'docs\CNAME') (Join-Path $pagesSrc 'CNAME')
}

if (Test-Path (Join-Path $repoRoot 'docs\images')) {
    Copy-Item (Join-Path $repoRoot 'docs\images') (Join-Path $pagesSrc 'images') -Recurse -Force
}

Push-Location $repoRoot
try {
    $bundle = Get-Command bundle -ErrorAction SilentlyContinue
    if (-not $bundle) {
        throw "未找到 Bundler。请先安装 Ruby + Bundler，并在仓库根目录执行：bundle install"
    }

    Write-Host 'Checking Jekyll/GitHub Pages dependencies...' -ForegroundColor Cyan
    & bundle exec jekyll --version | Out-Host

    if ($LASTEXITCODE -ne 0) {
        throw "Jekyll 不可用。请先在仓库根目录执行：bundle install"
    }

    Write-Host 'Building site with Jekyll (bundle exec jekyll build)...' -ForegroundColor Cyan
    & bundle exec jekyll build --source $pagesSrc --destination $siteDir

    if ($LASTEXITCODE -ne 0) {
        throw "Jekyll 构建失败，请检查 bundle install 输出和站点配置。"
    }

    if ($NoServe) {
        Write-Host "Site prepared at: $siteDir" -ForegroundColor Green
        exit 0
    }

    Write-Host "Starting local preview at http://127.0.0.1:$Port/" -ForegroundColor Green
    $python = Get-Command python -ErrorAction SilentlyContinue
    if ($python) {
        Push-Location $siteDir
        try {
            & python -m http.server $Port
        }
        finally {
            Pop-Location
        }
    }
    else {
        Write-Warning '未找到 python，无法自动启动本地 HTTP 预览。HTML 已生成到 _site，可用任意静态服务器打开。'
    }
}
finally {
    Pop-Location
}
