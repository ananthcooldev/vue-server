# Quick IIS Deployment Guide

## 🚀 Fast Deployment (5 Steps)

### Step 1: Install Prerequisites

```powershell
# Install ASP.NET Core Hosting Bundle
# Download from: https://dotnet.microsoft.com/download/dotnet/9.0
# Install and restart IIS: iisreset
```

### Step 2: Run Deployment Script

```powershell
# Run as Administrator
cd C:\Learn\VueNetCrud
.\deploy.ps1
```

### Step 3: Configure IIS

1. **Open IIS Manager**
2. **Create Application Pool**:
   - Name: `VueNetCrudApiPool`
   - .NET CLR: **No Managed Code**
   - Pipeline: **Integrated**

3. **Create API Website**:
   - Site name: `VueNetCrudApi`
   - Physical path: `C:\inetpub\wwwroot\VueNetCrudApi`
   - Port: `80` (or your choice)
   - Application pool: `VueNetCrudApiPool`

4. **Create Vue App Website**:
   - Site name: `VueNetCrudApp`
   - Physical path: `C:\inetpub\wwwroot\VueNetCrudApp`
   - Port: `80` (or different port)

### Step 4: Update CORS (if needed)

Edit `C:\inetpub\wwwroot\VueNetCrudApi\appsettings.json`:
- Add your production domain to CORS origins

### Step 5: Test

- API: `http://localhost/api/swagger`
- Vue App: `http://localhost`

---

## 📝 Manual Deployment

### Build Vue App
```powershell
cd vue-client
npm run build
```

### Publish API
```powershell
cd VueNetCrud.Server
dotnet publish -c Release -o C:\inetpub\wwwroot\VueNetCrudApi
```

### Copy Vue Files
```powershell
xcopy /E /I vue-client\dist C:\inetpub\wwwroot\VueNetCrudApp
```

### Restart IIS
```powershell
iisreset
```

---

## ⚠️ Common Issues

**API 500 Error**: Check Application Pool → .NET CLR = "No Managed Code"

**Vue Blank Page**: Install URL Rewrite Module, check web.config

**CORS Errors**: Update CORS origins in appsettings.json

---

See `IIS_DEPLOYMENT_GUIDE.md` for detailed instructions.

