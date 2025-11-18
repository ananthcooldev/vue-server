# Understanding the CORS Error

## The Error Explained

```
Access to XMLHttpRequest at 'https://local.api.com/api/Auth/login' 
from origin 'https://local.vueclient.com' has been blocked by CORS policy: 
Response to preflight request doesn't pass access control check: 
No 'Access-Control-Allow-Origin' header is present on the requested resource.
```

### What This Means:

1. **Preflight Request (OPTIONS)**: Before sending the actual POST request, the browser sends an OPTIONS request to check if CORS is allowed
2. **Missing Header**: The API server is NOT responding with `Access-Control-Allow-Origin` header
3. **Browser Blocks**: Browser blocks the actual POST request because preflight failed

### Why It's Happening:

The API server (`https://local.api.com`) is not configured to allow requests from `https://local.vueclient.com`.

## Root Cause

The CORS middleware in your .NET Core API is either:
1. **Not running** (IIS hasn't been restarted with new code)
2. **Not configured correctly** (CORS headers not being added)
3. **Being blocked** by another middleware before CORS can add headers

## Solution Steps

### Step 1: Verify API is Running New Code

Check if the API has the latest CORS configuration:

1. **Check the published DLL timestamp:**
   ```powershell
   Get-Item "C:\inetpub\wwwroot\VueNetCrudApi\VueNetCrud.Server.dll" | Select-Object LastWriteTime
   ```
   Should be recent (just now)

2. **Check if Application Pool was restarted:**
   - IIS Manager → Application Pools
   - Check when the pool was last recycled

### Step 2: Test OPTIONS Request Directly

Test if the API responds to OPTIONS requests:

```powershell
# Test OPTIONS request (preflight)
Invoke-WebRequest -Uri "https://local.api.com/api/Auth/login" `
  -Method OPTIONS `
  -Headers @{
    "Origin" = "https://local.vueclient.com"
    "Access-Control-Request-Method" = "POST"
    "Access-Control-Request-Headers" = "content-type"
  } `
  -UseBasicParsing
```

**Expected Response:**
- Status: 200 OK
- Headers should include:
  - `Access-Control-Allow-Origin: https://local.vueclient.com`
  - `Access-Control-Allow-Methods: *`
  - `Access-Control-Allow-Headers: *`

**If you get 404 or no CORS headers:**
- API is not handling OPTIONS requests
- CORS middleware is not working

### Step 3: Verify CORS Configuration

Check that CORS is properly configured in the code:

1. **Program.cs** - Should have `app.UseCors("ClientCors");` at the very beginning
2. **CorsExtensions.cs** - Should include `https://local.vueclient.com` in allowed origins
3. **Middleware order** - CORS must be BEFORE any other middleware

### Step 4: Check IIS Logs

Check if OPTIONS requests are reaching the API:

```powershell
# Check recent IIS logs
Get-Content "C:\inetpub\logs\LogFiles\W3SVC*\*.log" -Tail 50 | Select-String "OPTIONS"
```

Look for:
- OPTIONS requests to `/api/Auth/login`
- Response status codes
- Any errors

## Quick Fix Checklist

- [ ] API code has CORS configured correctly
- [ ] API has been rebuilt (`dotnet publish`)
- [ ] Files copied to IIS directory
- [ ] **Application Pool restarted in IIS** ← MOST IMPORTANT!
- [ ] Test OPTIONS request directly
- [ ] Check browser Network tab for OPTIONS request
- [ ] Verify CORS headers in response

## Most Common Issue

**The Application Pool in IIS hasn't been restarted!**

Even though you published new code, IIS is still running the old DLL in memory. You MUST restart the application pool for changes to take effect.

### How to Restart:

1. IIS Manager → Application Pools
2. Find your API pool
3. Right-click → **Recycle**

Or PowerShell:
```powershell
Import-Module WebAdministration
Restart-WebAppPool -Name "VueNetCrudApiPool"
```

## Debugging Steps

1. **Verify API is responding:**
   ```
   https://local.api.com/api/Product
   ```
   Should return data (not 404)

2. **Test OPTIONS manually:**
   Use PowerShell command above or browser DevTools

3. **Check response headers:**
   - Browser DevTools → Network tab
   - Click on the failed request
   - Check Response Headers
   - Should see CORS headers

4. **Verify middleware order:**
   - CORS must be FIRST
   - Before HTTPS redirection
   - Before authentication
   - Before authorization

## If Still Not Working

1. **Check if API is actually running:**
   - Test: `https://local.api.com/api/Product`
   - Should work

2. **Check IIS stdout logs:**
   ```powershell
   Get-Content "C:\inetpub\wwwroot\VueNetCrudApi\logs\stdout*.log" -Tail 50
   ```
   Look for errors or exceptions

3. **Try adding explicit OPTIONS handler:**
   See if we need to handle OPTIONS explicitly in a controller

4. **Check for IIS-level CORS blocking:**
   - Some IIS configurations can block CORS
   - Check IIS CORS module settings

