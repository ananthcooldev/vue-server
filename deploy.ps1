# Vue .NET CRUD Deployment Script
# Run as Administrator

param(
    [string]$ApiOutputPath = "C:\inetpub\wwwroot\VueNetCrudApi",
    [string]$VueOutputPath = "C:\inetpub\wwwroot\VueNetCrudApp",
    [string]$ProjectRoot = "C:\Learn\VueNetCrud"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Vue .NET CRUD Deployment Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if running as administrator
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Host "ERROR: This script must be run as Administrator!" -ForegroundColor Red
    exit 1
}

# Step 1: Build Vue.js Application
Write-Host "[1/4] Building Vue.js application..." -ForegroundColor Green
Set-Location "$ProjectRoot\vue-client"
if (Test-Path "node_modules") {
    Write-Host "  Dependencies already installed" -ForegroundColor Gray
} else {
    Write-Host "  Installing dependencies..." -ForegroundColor Gray
    npm install
}
Write-Host "  Building for production..." -ForegroundColor Gray
npm run build
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Vue build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "  ✓ Vue build complete" -ForegroundColor Green
Write-Host ""

# Step 2: Publish .NET Core API
Write-Host "[2/4] Publishing .NET Core API..." -ForegroundColor Green
Set-Location "$ProjectRoot\VueNetCrud.Server"
Write-Host "  Publishing to: $ApiOutputPath" -ForegroundColor Gray
if (Test-Path $ApiOutputPath) {
    Write-Host "  Cleaning existing output directory..." -ForegroundColor Gray
    Remove-Item "$ApiOutputPath\*" -Recurse -Force -ErrorAction SilentlyContinue
}
dotnet publish -c Release -o $ApiOutputPath
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: API publish failed!" -ForegroundColor Red
    exit 1
}
Write-Host "  ✓ API published successfully" -ForegroundColor Green
Write-Host ""

# Step 3: Copy Vue.js build files
Write-Host "[3/4] Copying Vue.js build files..." -ForegroundColor Green
if (Test-Path $VueOutputPath) {
    Write-Host "  Cleaning existing Vue app directory..." -ForegroundColor Gray
    Remove-Item "$VueOutputPath\*" -Recurse -Force -ErrorAction SilentlyContinue
} else {
    Write-Host "  Creating Vue app directory..." -ForegroundColor Gray
    New-Item -ItemType Directory -Path $VueOutputPath -Force | Out-Null
}
Write-Host "  Copying files from dist to: $VueOutputPath" -ForegroundColor Gray
Copy-Item "$ProjectRoot\vue-client\dist\*" -Destination $VueOutputPath -Recurse -Force

# Copy web.config for Vue.js routing
if (Test-Path "$ProjectRoot\vue-client\web.config") {
    Copy-Item "$ProjectRoot\vue-client\web.config" -Destination $VueOutputPath -Force
    Write-Host "  ✓ web.config copied" -ForegroundColor Gray
} else {
    Write-Host "  WARNING: web.config not found. Create it manually for Vue routing." -ForegroundColor Yellow
}
Write-Host "  ✓ Vue files copied successfully" -ForegroundColor Green
Write-Host ""

# Step 4: Set Permissions
Write-Host "[4/4] Setting folder permissions..." -ForegroundColor Green
Write-Host "  Setting API folder permissions..." -ForegroundColor Gray
icacls $ApiOutputPath /grant "IIS_IUSRS:(OI)(CI)F" /T /Q | Out-Null
Write-Host "  Setting Vue app folder permissions..." -ForegroundColor Gray
icacls $VueOutputPath /grant "IIS_IUSRS:(OI)(CI)F" /T /Q | Out-Null
Write-Host "  ✓ Permissions set" -ForegroundColor Green
Write-Host ""

# Step 5: Restart IIS
Write-Host "[5/5] Restarting IIS..." -ForegroundColor Green
iisreset
Write-Host "  ✓ IIS restarted" -ForegroundColor Green
Write-Host ""

# Summary
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Deployment Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "API Location: $ApiOutputPath" -ForegroundColor Yellow
Write-Host "Vue App Location: $VueOutputPath" -ForegroundColor Yellow
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Cyan
Write-Host "1. Configure IIS Application Pools and Websites" -ForegroundColor White
Write-Host "2. Update CORS settings in appsettings.json for production" -ForegroundColor White
Write-Host "3. Update API base URL in Vue app if needed" -ForegroundColor White
Write-Host "4. Test the deployment" -ForegroundColor White
Write-Host ""

