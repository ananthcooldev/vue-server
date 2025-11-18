# IIS Deployment Guide

Complete guide to deploy both .NET Core API and Vue.js application to IIS.

## 📋 Prerequisites

### 1. Install Required Software

1. **IIS (Internet Information Services)**
   - Enable via Windows Features
   - Or: `dism.exe /online /enable-feature /featurename:IIS-WebServerRole`

2. **ASP.NET Core Hosting Bundle**
   - Download from: https://dotnet.microsoft.com/download/dotnet/9.0
   - Install the **Hosting Bundle** (includes .NET Runtime + IIS Module)
   - **Important**: Restart IIS after installation

3. **URL Rewrite Module** (for Vue.js routing)
   - Download: https://www.iis.net/downloads/microsoft/url-rewrite
   - Install the module

### 2. Verify Installation

```powershell
# Check IIS is running
Get-Service -Name W3SVC

# Check .NET Runtime
dotnet --list-runtimes
```

---

## 🚀 Step 1: Build Vue.js Application for Production

### 1.1 Build the Vue App

```powershell
cd C:\Learn\VueNetCrud\vue-client
npm run build
```

This creates a `dist` folder with production-ready files.

### 1.2 Verify Build Output

You should see:
```
dist/
├── index.html
├── assets/
│   ├── index-[hash].js
│   └── index-[hash].css
└── ...
```

---

## 🔧 Step 2: Publish .NET Core API

### 2.1 Publish API to Folder

```powershell
cd C:\Learn\VueNetCrud\VueNetCrud.Server
dotnet publish -c Release -o C:\inetpub\wwwroot\VueNetCrudApi
```

Or use Visual Studio:
- Right-click project → **Publish**
- Select **Folder**
- Choose: `C:\inetpub\wwwroot\VueNetCrudApi`
- Click **Publish**

### 2.2 Verify Published Files

Check that these files exist:
- `VueNetCrud.Server.dll`
- `web.config`
- `appsettings.json`
- All DLL dependencies

---

## 🌐 Step 3: Configure IIS for .NET Core API

### 3.1 Create Application Pool

1. Open **IIS Manager**
2. Right-click **Application Pools** → **Add Application Pool**
3. Configure:
   - **Name**: `VueNetCrudApiPool`
   - **.NET CLR Version**: **No Managed Code** (important!)
   - **Managed Pipeline Mode**: **Integrated**
4. Click **OK**

### 3.2 Configure Application Pool Settings

1. Select `VueNetCrudApiPool`
2. Click **Advanced Settings**
3. Set:
   - **Start Mode**: `AlwaysRunning`
   - **Idle Timeout**: `0` (or your preference)
   - **Identity**: `ApplicationPoolIdentity` (or custom account)

### 3.3 Create IIS Website/Application

1. Right-click **Sites** → **Add Website**
2. Configure:
   - **Site name**: `VueNetCrudApi`
   - **Application pool**: `VueNetCrudApiPool`
   - **Physical path**: `C:\inetpub\wwwroot\VueNetCrudApi`
   - **Binding**:
     - **Type**: `http`
     - **IP address**: `All Unassigned` or specific IP
     - **Port**: `80` (or `8080`, `5000`, etc.)
     - **Host name**: `api.yourdomain.com` (optional)
3. Click **OK**

### 3.4 Verify API is Running

1. Open browser: `http://localhost/api/swagger` (or your port)
2. Should see Swagger UI
3. Test endpoint: `http://localhost/api/Product`

---

## 🎨 Step 4: Configure IIS for Vue.js Application

### 4.1 Copy Vue Build Files

```powershell
# Copy dist folder contents to IIS directory
xcopy /E /I C:\Learn\VueNetCrud\vue-client\dist C:\inetpub\wwwroot\VueNetCrudApp
```

### 4.2 Create web.config for Vue.js

Create `C:\inetpub\wwwroot\VueNetCrudApp\web.config`:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <!-- Handle client-side routing -->
        <rule name="Vue Routes" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
            <add input="{REQUEST_URI}" pattern="^/api" negate="true" />
          </conditions>
          <action type="Rewrite" url="/" />
        </rule>
      </rules>
    </rewrite>
    
    <!-- Static file caching -->
    <staticContent>
      <clientCache cacheControlMode="UseMaxAge" cacheControlMaxAge="365.00:00:00" />
    </staticContent>
    
    <!-- MIME types -->
    <staticContent>
      <mimeMap fileExtension=".json" mimeType="application/json" />
      <mimeMap fileExtension=".woff" mimeType="application/font-woff" />
      <mimeMap fileExtension=".woff2" mimeType="application/font-woff2" />
    </staticContent>
  </system.webServer>
</configuration>
```

### 4.3 Create IIS Website for Vue App

1. Right-click **Sites** → **Add Website**
2. Configure:
   - **Site name**: `VueNetCrudApp`
   - **Application pool**: `DefaultAppPool` (or create new)
   - **Physical path**: `C:\inetpub\wwwroot\VueNetCrudApp`
   - **Binding**:
     - **Port**: `80` (or different port like `8080`)
     - **Host name**: `yourdomain.com` (optional)
3. Click **OK**

---

## ⚙️ Step 5: Update API Configuration for Production

### 5.1 Update CORS in appsettings.json

Edit `C:\inetpub\wwwroot\VueNetCrudApi\appsettings.json`:

```json
{
  "Cors": {
    "AllowedOrigins": [
      "http://localhost",
      "http://yourdomain.com",
      "https://yourdomain.com"
    ]
  }
}
```

### 5.2 Update CORS Extension (if needed)

If using configuration-based CORS, update `CorsExtensions.cs`:

```csharp
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
policy.WithOrigins(allowedOrigins)
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials();
```

### 5.3 Update API Base URL in Vue App

Before building, update `vue-client/src/services/api.ts`:

```typescript
// For production
const API_BASE_URL = '/api';  // Relative path (same domain)

// Or absolute URL
const API_BASE_URL = 'http://yourdomain.com/api';
```

Then rebuild:
```powershell
cd vue-client
npm run build
```

---

## 🔐 Step 6: Security & Permissions

### 6.1 Set Folder Permissions

```powershell
# API folder
icacls "C:\inetpub\wwwroot\VueNetCrudApi" /grant "IIS_IUSRS:(OI)(CI)F" /T

# Vue app folder
icacls "C:\inetpub\wwwroot\VueNetCrudApp" /grant "IIS_IUSRS:(OI)(CI)F" /T
```

### 6.2 Application Pool Identity

1. Select Application Pool → **Advanced Settings**
2. **Identity**: `ApplicationPoolIdentity` (recommended)
3. Or use custom account with appropriate permissions

---

## 📝 Step 7: web.config for .NET Core API

Ensure `C:\inetpub\wwwroot\VueNetCrudApi\web.config` exists:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" 
                  arguments=".\VueNetCrud.Server.dll" 
                  stdoutLogEnabled="true" 
                  stdoutLogFile=".\logs\stdout" 
                  hostingModel="inprocess" />
    </system.webServer>
  </location>
</configuration>
```

**Note**: This is usually auto-generated during publish. Verify it exists.

---

## 🧪 Step 8: Testing Deployment

### 8.1 Test API

```powershell
# Test API endpoint
Invoke-WebRequest -Uri "http://localhost/api/Product" -Method GET

# Or in browser
http://localhost/api/swagger
```

### 8.2 Test Vue App

1. Open browser: `http://localhost` (or your configured port)
2. Should see Vue application
3. Try logging in
4. Check browser console for errors

### 8.3 Check Logs

```powershell
# API logs
Get-Content C:\inetpub\wwwroot\VueNetCrudApi\logs\stdout*.log -Tail 50

# IIS logs
Get-Content C:\inetpub\logs\LogFiles\W3SVC*\*.log -Tail 50
```

---

## 🔄 Step 9: Deployment Script (Optional)

Create `deploy.ps1`:

```powershell
# Build Vue app
Write-Host "Building Vue app..." -ForegroundColor Green
Set-Location "C:\Learn\VueNetCrud\vue-client"
npm run build

# Publish .NET API
Write-Host "Publishing .NET API..." -ForegroundColor Green
Set-Location "C:\Learn\VueNetCrud\VueNetCrud.Server"
dotnet publish -c Release -o "C:\inetpub\wwwroot\VueNetCrudApi"

# Copy Vue build files
Write-Host "Copying Vue build files..." -ForegroundColor Green
Remove-Item "C:\inetpub\wwwroot\VueNetCrudApp\*" -Recurse -Force
Copy-Item "C:\Learn\VueNetCrud\vue-client\dist\*" -Destination "C:\inetpub\wwwroot\VueNetCrudApp" -Recurse

# Restart IIS
Write-Host "Restarting IIS..." -ForegroundColor Green
iisreset

Write-Host "Deployment complete!" -ForegroundColor Green
```

Run: `.\deploy.ps1`

---

## 🐛 Troubleshooting

### API Not Starting

1. **Check Application Pool**:
   - Ensure it's running
   - Check for errors in Event Viewer

2. **Check .NET Runtime**:
   ```powershell
   dotnet --list-runtimes
   ```
   - Must have ASP.NET Core 9.0 runtime

3. **Check web.config**:
   - Verify `aspNetCore` handler is configured
   - Check `processPath` points to correct DLL

4. **Check Permissions**:
   - Application Pool identity needs read/execute permissions

### Vue App Shows Blank Page

1. **Check web.config**:
   - URL Rewrite module installed?
   - Rewrite rules correct?

2. **Check Browser Console**:
   - Look for 404 errors
   - Check network tab for failed requests

3. **Check File Permissions**:
   - IIS_IUSRS needs read permissions

### CORS Errors

1. **Update CORS configuration**:
   - Add your domain to allowed origins
   - Restart API application pool

2. **Check API URL**:
   - Ensure Vue app points to correct API URL
   - Use relative paths if same domain

### 500 Errors

1. **Check Event Viewer**:
   - Windows Logs → Application
   - Look for .NET errors

2. **Check stdout logs**:
   ```powershell
   Get-Content C:\inetpub\wwwroot\VueNetCrudApi\logs\stdout*.log
   ```

3. **Enable Detailed Errors** (temporarily):
   - In `web.config`, add:
   ```xml
   <aspNetCore ... stdoutLogEnabled="true" />
   ```

---

## 📊 Deployment Architecture

```
IIS Server
├── Site: VueNetCrudApi (Port 80)
│   ├── Application Pool: VueNetCrudApiPool
│   └── Physical Path: C:\inetpub\wwwroot\VueNetCrudApi
│       └── API Endpoints: /api/*
│
└── Site: VueNetCrudApp (Port 80)
    ├── Application Pool: DefaultAppPool
    └── Physical Path: C:\inetpub\wwwroot\VueNetCrudApp
        └── Static Files: Vue.js app
```

**Alternative: Single Site with Virtual Directories**

```
IIS Site: VueNetCrud (Port 80)
├── Root: C:\inetpub\wwwroot\VueNetCrudApp (Vue.js)
└── Virtual Directory: /api
    └── Points to: C:\inetpub\wwwroot\VueNetCrudApi
```

---

## ✅ Deployment Checklist

- [ ] IIS installed and running
- [ ] ASP.NET Core Hosting Bundle installed
- [ ] URL Rewrite Module installed
- [ ] Vue.js app built (`npm run build`)
- [ ] .NET API published (`dotnet publish`)
- [ ] Application pools created and configured
- [ ] IIS websites/applications created
- [ ] web.config files in place
- [ ] Folder permissions set
- [ ] CORS configured for production domains
- [ ] API base URL updated in Vue app
- [ ] Tested API endpoints
- [ ] Tested Vue app functionality
- [ ] Logs directory created and writable
- [ ] Firewall rules configured (if needed)

---

## 🚀 Quick Deployment Commands

```powershell
# 1. Build Vue
cd C:\Learn\VueNetCrud\vue-client
npm run build

# 2. Publish API
cd C:\Learn\VueNetCrud\VueNetCrud.Server
dotnet publish -c Release -o C:\inetpub\wwwroot\VueNetCrudApi

# 3. Copy Vue files
xcopy /E /I /Y C:\Learn\VueNetCrud\vue-client\dist C:\inetpub\wwwroot\VueNetCrudApp

# 4. Restart IIS
iisreset
```

---

## 📚 Additional Resources

- [ASP.NET Core IIS Hosting](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/)
- [Vue.js Deployment](https://router.vuejs.org/guide/essentials/history-mode.html)
- [IIS URL Rewrite](https://www.iis.net/downloads/microsoft/url-rewrite)

---

**Need help?** Check the troubleshooting section or review IIS logs and Event Viewer.

