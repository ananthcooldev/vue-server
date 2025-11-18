# Understanding the CORS Error

## What the Error Means

```
Access to XMLHttpRequest at 'https://local.api.com/api/Auth/login' 
from origin 'https://local.vueclient.com' has been blocked by CORS policy: 
Response to preflight request doesn't pass access control check: 
No 'Access-Control-Allow-Origin' header is present on the requested resource.
```

### Breakdown:

1. **Your Vue app** (`https://local.vueclient.com`) is trying to call
2. **Your API** (`https://local.api.com/api/Auth/login`)
3. **Browser sends OPTIONS request first** (called "preflight") to check if CORS is allowed
4. **API doesn't respond with CORS headers**, so browser blocks the actual POST request

## Why It's Happening

The API server is **not sending CORS headers** in the response. This means:

- ❌ CORS middleware is not running
- ❌ OR CORS middleware is being blocked by something else
- ❌ OR IIS hasn't been restarted with the new code

## The Fix

### 1. Verify CORS is Configured

✅ **CorsExtensions.cs** - Includes `https://local.vueclient.com`
✅ **Program.cs** - Has `app.UseCors("ClientCors");` at the beginning
✅ **AuthController** - Has `[EnableCors("ClientCors")]` attribute

### 2. **CRITICAL: Restart IIS Application Pool**

**This is the most important step!** Even though code is published, IIS is still running the old DLL in memory.

**Steps:**
1. Open IIS Manager (`inetmgr`)
2. Click **Application Pools**
3. Find your API pool (e.g., `VueNetCrudApiPool`)
4. **Right-click → Recycle**

**Or PowerShell:**
```powershell
Import-Module WebAdministration
Restart-WebAppPool -Name "VueNetCrudApiPool"
```

### 3. Test After Restart

1. **Test API directly:**
   - `https://local.api.com/api/Product` - Should work

2. **Test from Vue app:**
   - Open: `https://local.vueclient.com`
   - Try to login
   - Check browser console - CORS errors should be gone

3. **Check Network tab:**
   - Look for OPTIONS request to `/api/Auth/login`
   - Should return 200 with CORS headers:
     - `Access-Control-Allow-Origin: https://local.vueclient.com`
     - `Access-Control-Allow-Methods: *`
     - `Access-Control-Allow-Headers: *`

## How CORS Works

### Step 1: Preflight (OPTIONS Request)
```
Browser → API: OPTIONS /api/Auth/login
Headers:
  Origin: https://local.vueclient.com
  Access-Control-Request-Method: POST
  Access-Control-Request-Headers: content-type

API → Browser: 200 OK
Headers:
  Access-Control-Allow-Origin: https://local.vueclient.com  ← MUST BE PRESENT
  Access-Control-Allow-Methods: *
  Access-Control-Allow-Headers: *
```

### Step 2: Actual Request (POST)
```
Browser → API: POST /api/Auth/login
Headers:
  Origin: https://local.vueclient.com
  Content-Type: application/json
  { username: "admin", password: "123" }

API → Browser: 200 OK
Headers:
  Access-Control-Allow-Origin: https://local.vueclient.com  ← MUST BE PRESENT
  { token: "..." }
```

## Current Status

✅ Code is configured correctly
✅ Code has been published to IIS
❌ **Application Pool needs to be restarted** ← DO THIS NOW!

## After Restarting

The CORS middleware will:
1. ✅ Handle OPTIONS requests automatically
2. ✅ Add CORS headers to all responses
3. ✅ Allow requests from `https://local.vueclient.com`

## Still Not Working?

1. **Check if API is running:**
   - Test: `https://local.api.com/api/Product`
   - If 404, API site is not running

2. **Check IIS logs:**
   - `C:\inetpub\logs\LogFiles\W3SVC*\*.log`
   - Look for OPTIONS requests

3. **Check stdout logs:**
   - `C:\inetpub\wwwroot\VueNetCrudApi\logs\stdout*.log`
   - Look for errors

4. **Verify web.config:**
   - Should exist at: `C:\inetpub\wwwroot\VueNetCrudApi\web.config`
   - Should have aspNetCore handler configured

## Summary

**The error means:** API is not sending CORS headers
**The fix:** Restart IIS Application Pool to load new code
**After restart:** CORS will work automatically

