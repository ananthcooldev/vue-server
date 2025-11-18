# Restart API in IIS - CRITICAL STEP

## ⚠️ IMPORTANT: You MUST restart the API after deployment!

The code has been updated and published, but **IIS is still running the old code**. You must restart the application pool.

## Quick Restart Steps

### Method 1: IIS Manager (Recommended)

1. **Open IIS Manager**
   - Press `Windows + R`
   - Type `inetmgr` and press Enter

2. **Restart Application Pool:**
   - Click **Application Pools** (left panel)
   - Find your API pool (e.g., `VueNetCrudApiPool`)
   - **Right-click** → **Recycle**
   - Or: **Right-click** → **Stop**, then **Right-click** → **Start**

3. **Verify it's running:**
   - The pool status should show **Started**
   - Test: `https://local.api.com/api/Product`

### Method 2: PowerShell

```powershell
# Restart specific pool
Import-Module WebAdministration
Restart-WebAppPool -Name "VueNetCrudApiPool"

# Or restart all pools
Get-WebAppPoolState | Where-Object {$_.Value -eq "Started"} | Restart-WebAppPool
```

### Method 3: Command Line

```cmd
# Restart IIS completely (affects all sites)
iisreset
```

## After Restarting

1. **Test API directly:**
   ```
   https://local.api.com/api/Product
   ```
   Should return data (not 404 or CORS error)

2. **Test from Vue app:**
   - Open: `https://local.vueclient.com`
   - Try to login
   - Check browser console (F12) - CORS errors should be gone

3. **Check Network tab:**
   - Look at the OPTIONS request (preflight)
   - Should return 200 with CORS headers:
     - `Access-Control-Allow-Origin: https://local.vueclient.com`
     - `Access-Control-Allow-Methods: *`
     - `Access-Control-Allow-Headers: *`

## Troubleshooting

### Still seeing CORS errors?

1. **Verify pool is restarted:**
   - Check pool status in IIS Manager
   - Try stopping and starting again

2. **Check API is running:**
   - Test: `https://local.api.com/api/Product`
   - Should work directly

3. **Clear browser cache:**
   - Press `Ctrl + Shift + Delete`
   - Clear cached files
   - Or use Incognito mode

4. **Check IIS logs:**
   - `C:\inetpub\logs\LogFiles\W3SVC*`
   - Look for OPTIONS requests

5. **Verify web.config in IIS:**
   - Check: `C:\inetpub\wwwroot\VueNetCrudApi\web.config`
   - Should exist and be valid

## Current Fixes Applied

✅ CORS enabled at the very beginning of middleware pipeline
✅ HTTPS redirection skipped for OPTIONS requests
✅ CORS policy includes `https://local.vueclient.com`
✅ API published to IIS

**Now restart the application pool to activate the changes!**

