# Disable Debugger in IIS (Production) - Keep for Local Dev

## Overview

You want to:
- ✅ **Disable debugger** in IIS deployment (production)
- ✅ **Keep debugger working** for local development

## Solution

### Step 1: Set Environment to Production in IIS

The `web.config` in IIS should set `ASPNETCORE_ENVIRONMENT=Production`. This:
- Disables detailed error pages
- Disables debug symbols
- Uses production logging
- Optimizes performance

**I've already updated the web.config** - it now sets:
```xml
<environmentVariables>
  <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
</environmentVariables>
```

### Step 2: Verify Local Dev Uses Development

Your `launchSettings.json` already sets:
```json
"ASPNETCORE_ENVIRONMENT": "Development"
```

This means when you run locally with `dotnet run`, it uses Development mode.

### Step 3: Rebuild and Redeploy

After updating web.config:

```powershell
cd C:\Learn\VueNetCrud\VueNetCrud.Server
dotnet publish -c Release -o C:\inetpub\wwwroot\VueNetCrudApi
```

The web.config will be automatically updated with Production environment.

### Step 4: Restart IIS Application Pool

```powershell
Import-Module WebAdministration
Restart-WebAppPool -Name "VueNetCrudApiPool"
```

Or in IIS Manager:
1. Application Pools → Your API Pool
2. Right-click → Recycle

## What This Does

### In IIS (Production):
- ❌ No detailed error pages
- ❌ No debug symbols
- ❌ No development features
- ✅ Optimized performance
- ✅ Production logging
- ✅ Security hardened

### In Local Dev:
- ✅ Detailed error pages
- ✅ Debug symbols
- ✅ Development features
- ✅ Swagger UI
- ✅ Detailed logging

## Verify It's Working

### Check IIS Environment:
```powershell
# Check if environment is set correctly
Get-Content "C:\inetpub\wwwroot\VueNetCrudApi\web.config" | Select-String "ASPNETCORE_ENVIRONMENT"
```

Should show: `value="Production"`

### Test Production Behavior:
1. Make an error in the API (intentionally)
2. Call from browser: `https://local.api.com/api/SomeInvalidEndpoint`
3. Should return generic error (not detailed stack trace)

### Test Local Dev:
1. Run: `dotnet run` locally
2. Make an error
3. Should show detailed error page with stack trace

## Additional Debugger Settings

### Disable Source Maps in Production (Vue.js)

If you want to disable source maps in production builds:

**vue-client/vite.config.ts:**
```typescript
build: {
  sourcemap: import.meta.env.PROD ? false : true, // Disable in production
}
```

### Disable Swagger in Production

If you want to disable Swagger UI in production:

**Program.cs:**
```csharp
if (!app.Environment.IsProduction())
{
    app.ConfigureSwaggerUI();
}
```

### Disable Detailed Errors in Production

Already handled by setting environment to Production. ASP.NET Core automatically:
- Hides detailed errors in Production
- Shows generic error messages
- Logs details to logs instead

## Environment Variables Summary

| Location | Environment | Debugger |
|----------|-------------|----------|
| IIS (web.config) | `Production` | ❌ Disabled |
| Local (launchSettings.json) | `Development` | ✅ Enabled |
| `dotnet run` | `Development` | ✅ Enabled |

## Troubleshooting

### Debugger Still Working in IIS?

1. **Check web.config:**
   ```powershell
   Get-Content "C:\inetpub\wwwroot\VueNetCrudApi\web.config"
   ```
   Should have `ASPNETCORE_ENVIRONMENT=Production`

2. **Check Application Pool:**
   - IIS Manager → Application Pools
   - Verify pool is restarted after web.config change

3. **Check IIS Environment Variables:**
   - IIS Manager → Your Site → Configuration Editor
   - Look for `ASPNETCORE_ENVIRONMENT`

### Local Dev Not Working?

1. **Check launchSettings.json:**
   - Should have `ASPNETCORE_ENVIRONMENT=Development`

2. **Run with explicit environment:**
   ```powershell
   $env:ASPNETCORE_ENVIRONMENT="Development"
   dotnet run
   ```

## Summary

✅ **IIS (Production)**: Environment set to `Production` in web.config
✅ **Local Dev**: Environment set to `Development` in launchSettings.json
✅ **Automatic**: ASP.NET Core handles debugger based on environment

The debugger is now disabled in IIS but will work for local development!

