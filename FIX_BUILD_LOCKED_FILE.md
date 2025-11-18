# Fix "File is Locked" Build Error

## Problem
```
Could not copy "apphost.exe" to "VueNetCrud.Server.exe". 
The file is locked by: "VueNetCrud.Server (PID)"
```

## Solution

### Quick Fix (One Command)

```powershell
# Stop all .NET processes and rebuild
Get-Process -Name "VueNetCrud.Server","dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force
cd C:\Learn\VueNetCrud\VueNetCrud.Server
dotnet build
```

### Step-by-Step

1. **Stop the running API process:**
   ```powershell
   # By process name
   Get-Process -Name "VueNetCrud.Server" -ErrorAction SilentlyContinue | Stop-Process -Force
   
   # Or by PID (if you know it)
   Stop-Process -Id 43104 -Force
   
   # Or by port
   Get-NetTCPConnection -LocalPort 5280 -ErrorAction SilentlyContinue | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }
   ```

2. **Stop all dotnet processes (if needed):**
   ```powershell
   Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force
   ```

3. **Wait a moment for file locks to release:**
   ```powershell
   Start-Sleep -Seconds 2
   ```

4. **Rebuild:**
   ```powershell
   cd C:\Learn\VueNetCrud\VueNetCrud.Server
   dotnet build
   ```

## Prevention

### Before Building/Publishing

Always stop the running application first:

```powershell
# Stop API if running
Get-Process -Name "VueNetCrud.Server" -ErrorAction SilentlyContinue | Stop-Process -Force

# Or stop by port
Get-NetTCPConnection -LocalPort 5280 -ErrorAction SilentlyContinue | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }
```

### If Running in IIS

If the API is running in IIS, stop the Application Pool:

1. IIS Manager → Application Pools
2. Find your API pool
3. Right-click → Stop
4. Build/Publish
5. Right-click → Start

## Alternative: Use Different Output Directory

If you need to build while the app is running:

```powershell
# Publish to a different directory
dotnet publish -c Release -o C:\temp\VueNetCrudApi

# Then manually copy files when ready
```

## Quick Script

Create `stop-and-build.ps1`:

```powershell
# Stop running API
Write-Host "Stopping API processes..." -ForegroundColor Yellow
Get-Process -Name "VueNetCrud.Server","dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force
Get-NetTCPConnection -LocalPort 5280 -ErrorAction SilentlyContinue | ForEach-Object { Stop-Process -Id $_.OwningProcess -Force }

Start-Sleep -Seconds 2

# Build
Write-Host "Building..." -ForegroundColor Green
cd C:\Learn\VueNetCrud\VueNetCrud.Server
dotnet build

# Publish
Write-Host "Publishing..." -ForegroundColor Green
dotnet publish -c Release -o C:\inetpub\wwwroot\VueNetCrudApi

Write-Host "Done!" -ForegroundColor Green
```

Run: `.\stop-and-build.ps1`

