# Debugger Settings: Disabled in IIS, Enabled for Local Dev

## ✅ Configuration Complete

I've configured your application so that:
- **IIS (Production)**: Debugger disabled, optimized for production
- **Local Dev**: Debugger enabled, full development features

## What Was Changed

### 1. IIS web.config (Production)
- Set `ASPNETCORE_ENVIRONMENT=Production`
- This disables:
  - Detailed error pages
  - Debug symbols
  - Development features
  - Swagger UI

### 2. Program.cs
- Swagger UI only enabled in Development
- Production mode hides Swagger

### 3. Vue.js Build
- Source maps disabled in production builds
- Minified for better performance

## Environment Configuration

| Location | Environment | Debugger | Swagger |
|----------|-------------|----------|---------|
| **IIS** (web.config) | `Production` | ❌ Disabled | ❌ Disabled |
| **Local Dev** (launchSettings.json) | `Development` | ✅ Enabled | ✅ Enabled |
| **dotnet run** | `Development` | ✅ Enabled | ✅ Enabled |

## How It Works

### IIS Deployment (Production)

**web.config sets:**
```xml
<environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
```

**Result:**
- ❌ No detailed error pages (security)
- ❌ No debug symbols
- ❌ No Swagger UI
- ✅ Optimized performance
- ✅ Production logging
- ✅ Generic error messages

### Local Development

**launchSettings.json sets:**
```json
"ASPNETCORE_ENVIRONMENT": "Development"
```

**Result:**
- ✅ Detailed error pages with stack traces
- ✅ Debug symbols available
- ✅ Swagger UI enabled
- ✅ Development logging
- ✅ Full debugging support

## Verify Configuration

### Check IIS Environment

```powershell
Get-Content "C:\inetpub\wwwroot\VueNetCrudApi\web.config" | Select-String "ASPNETCORE_ENVIRONMENT"
```

Should show: `value="Production"`

### Test Production Behavior

1. **Test Error Handling:**
   - Call invalid endpoint: `https://local.api.com/api/InvalidEndpoint`
   - Should return generic error (not detailed stack trace)

2. **Test Swagger:**
   - Try: `https://local.api.com/swagger`
   - Should return 404 (Swagger disabled in production)

3. **Test Local Dev:**
   - Run: `dotnet run`
   - Try: `http://localhost:5280/swagger`
   - Should show Swagger UI (enabled in development)

## Restart IIS After Changes

After publishing, restart the application pool:

```powershell
Import-Module WebAdministration
Restart-WebAppPool -Name "VueNetCrudApiPool"
```

Or in IIS Manager:
1. Application Pools → Your API Pool
2. Right-click → Recycle

## Additional Security (Optional)

### Disable Source Maps in Vue Production

Already configured! Source maps are disabled in production builds.

### Disable Detailed Errors

Already handled! Production environment automatically:
- Hides stack traces
- Shows generic error messages
- Logs details to log files instead

### Custom Error Pages

You can add custom error pages for production:

**Program.cs:**
```csharp
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
```

## Summary

✅ **IIS**: Production mode - No debugger, no Swagger, optimized
✅ **Local Dev**: Development mode - Full debugger, Swagger enabled
✅ **Automatic**: Environment determines behavior

Your application is now configured correctly:
- **IIS deployment**: Secure, optimized, no debugging
- **Local development**: Full debugging and development tools

