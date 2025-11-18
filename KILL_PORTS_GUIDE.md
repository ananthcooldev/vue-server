# Kill Ports Guide

Quick reference for killing processes on ports and stopping development servers.

## 🚀 Quick Commands

### Kill Specific Ports (Vue/API ports)

```powershell
cd vue-client
.\kill-ports.ps1
```

This kills processes on: **5173, 5174, 3000, 8080, 5280, 7200**

### Kill All Node.js Processes

```powershell
cd vue-client
.\kill-all-node.ps1
```

Kills all Node.js processes (useful for stopping all dev servers).

### Kill Custom Ports

```powershell
.\kill-ports.ps1 -Ports 5173, 8080, 3000
```

---

## 📋 Manual Methods

### Method 1: PowerShell (Single Port)

```powershell
# Find process using port
$port = 5173
$connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
if ($connection) {
    Stop-Process -Id $connection.OwningProcess -Force
    Write-Host "Killed process on port $port"
}
```

### Method 2: PowerShell (All Node.js)

```powershell
Get-Process -Name "node" | Stop-Process -Force
```

### Method 3: Command Prompt

```cmd
# Find process using port
netstat -ano | findstr :5173

# Kill process (replace <PID> with actual PID)
taskkill /PID <PID> /F
```

### Method 4: Kill All Node Processes (CMD)

```cmd
taskkill /IM node.exe /F
```

---

## 🎯 Common Development Ports

| Port | Typical Use |
|------|-------------|
| 5173 | Vite default port |
| 5174 | Vite fallback port |
| 3000 | React/Next.js default |
| 8080 | Alternative dev server |
| 5280 | .NET Core API (this project) |
| 7200 | .NET Core HTTPS |

---

## 🔧 Advanced: Kill Multiple Specific Ports

```powershell
# Kill ports 5173 and 5280
$ports = @(5173, 5280)
foreach ($port in $ports) {
    $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
    if ($connection) {
        Stop-Process -Id $connection.OwningProcess -Force
        Write-Host "Killed port $port"
    }
}
```

---

## 📝 Script Usage Examples

### Kill All Development Ports
```powershell
.\kill-ports.ps1
```

### Kill Specific Ports
```powershell
.\kill-ports.ps1 -Ports 5173, 5280
```

### Kill All Node + Check Ports
```powershell
.\kill-ports.ps1 -AllNode
```

### Kill All Node.js Processes
```powershell
.\kill-all-node.ps1
```

---

## ⚠️ Troubleshooting

### "Access Denied" Error

Run PowerShell as **Administrator**:
1. Right-click PowerShell
2. Select "Run as Administrator"
3. Run the script again

### Port Still Shows as In Use

1. Wait a few seconds (process cleanup time)
2. Check again: `netstat -ano | findstr :5173`
3. If still in use, restart your computer (last resort)

### Process Won't Die

```powershell
# Force kill with more aggressive method
$pid = <PID>
Stop-Process -Id $pid -Force
Start-Sleep -Seconds 2
# Check if still running
Get-Process -Id $pid -ErrorAction SilentlyContinue
```

---

## 🔍 Check What's Using a Port

```powershell
# Check specific port
$port = 5173
Get-NetTCPConnection -LocalPort $port | Select-Object LocalAddress, LocalPort, State, OwningProcess | Format-Table

# See process name
$connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
if ($connection) {
    $process = Get-Process -Id $connection.OwningProcess
    Write-Host "Port $port is used by: $($process.ProcessName) (PID: $($process.Id))"
}
```

---

## ✅ Quick Checklist

Before starting dev servers:

```powershell
# 1. Kill all Node processes
.\kill-all-node.ps1

# 2. Kill specific ports (if needed)
.\kill-ports.ps1

# 3. Verify ports are free
netstat -ano | findstr ":5173 :5280"

# 4. Start your servers
```

---

## 🎨 One-Liner Commands

### Kill Port 5173
```powershell
Get-Process -Id (Get-NetTCPConnection -LocalPort 5173).OwningProcess | Stop-Process -Force
```

### Kill Port 5280
```powershell
Get-Process -Id (Get-NetTCPConnection -LocalPort 5280).OwningProcess | Stop-Process -Force
```

### Kill All Node
```powershell
Get-Process node -ErrorAction SilentlyContinue | Stop-Process -Force
```

### Kill Both Vue and API Ports
```powershell
@(5173, 5280) | ForEach-Object { $conn = Get-NetTCPConnection -LocalPort $_ -ErrorAction SilentlyContinue; if ($conn) { Stop-Process -Id $conn.OwningProcess -Force } }
```

---

## 📚 Additional Resources

- **netstat**: `netstat -ano | findstr :PORT` - Find what's using a port
- **tasklist**: `tasklist | findstr node` - List all Node processes
- **Get-Process**: `Get-Process node` - PowerShell way to list processes

---

**Tip**: Add these scripts to your PATH or create aliases for quick access!

