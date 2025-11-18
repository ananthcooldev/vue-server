# Quick API Deployment Script
# Stops running API, rebuilds, and publishes to IIS

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "API Deployment Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Stop running processes
Write-Host "[1/3] Stopping running API processes..." -ForegroundColor Yellow
Get-Process -Name "VueNetCrud.Server","dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force
Get-NetTCPConnection -LocalPort 5280 -ErrorAction SilentlyContinue | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }
Start-Sleep -Seconds 2
Write-Host "  ✓ Processes stopped" -ForegroundColor Green
Write-Host ""

# Step 2: Build
Write-Host "[2/3] Building API..." -ForegroundColor Yellow
Set-Location "C:\Learn\VueNetCrud\VueNetCrud.Server"
dotnet build -c Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ✗ Build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "  ✓ Build successful" -ForegroundColor Green
Write-Host ""

# Step 3: Publish
Write-Host "[3/3] Publishing to IIS..." -ForegroundColor Yellow
dotnet publish -c Release -o "C:\inetpub\wwwroot\VueNetCrudApi"
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ✗ Publish failed!" -ForegroundColor Red
    exit 1
}
Write-Host "  ✓ Published successfully" -ForegroundColor Green
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Deployment Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next Step: Restart API Application Pool in IIS" -ForegroundColor Yellow
Write-Host "1. Open IIS Manager" -ForegroundColor White
Write-Host "2. Application Pools → Your API Pool" -ForegroundColor White
Write-Host "3. Right-click → Recycle" -ForegroundColor White
Write-Host ""

