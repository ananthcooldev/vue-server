# How to Start the .NET Core API

## The Problem
`ERR_CONNECTION_REFUSED` means the API server is **not running**. You need to start it before the Vue app can connect.

## Solution: Start the API Server

### In VS Code Terminal:

1. **Open a new terminal** (or use an existing one)
2. **Navigate to the API project:**
   ```powershell
   cd C:\Learn\VueNetCrud\VueNetCrud.Server
   ```

3. **Start the API:**
   ```powershell
   dotnet run
   ```

4. **Wait for this message:**
   ```
   Now listening on: http://localhost:5280
   ```

### Verify It's Running

You should see output like:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5280
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

## Running Both Applications

You need **TWO terminals** running simultaneously:

### Terminal 1: .NET Core API
```powershell
cd C:\Learn\VueNetCrud\VueNetCrud.Server
dotnet run
```

### Terminal 2: Vue.js App
```powershell
cd C:\Learn\VueNetCrud\vue-client
npm run dev
```

## Quick Test

Once the API is running, test it:
1. Open browser: http://localhost:5280/swagger (if Swagger is enabled)
2. Or test the login endpoint directly

## Troubleshooting

### Port Already in Use
If you see "Address already in use":
- Find and stop the process using port 5280
- Or change the port in `launchSettings.json`

### Build Errors
If `dotnet run` fails:
```powershell
dotnet build
```
Fix any errors, then try `dotnet run` again.

### Still Getting Connection Refused?
1. ✅ Check the API terminal - is it showing "Now listening on..."?
2. ✅ Check firewall settings
3. ✅ Try accessing http://localhost:5280/swagger directly in browser
4. ✅ Make sure you're using the correct port (5280)

