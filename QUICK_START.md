# Quick Start Guide - VS Code

## 🚀 How to Open and Run the Project in VS Code

### Step 1: Open the Project

**Option A: Open the entire solution**
1. Launch **VS Code**
2. Press `Ctrl + K`, then `Ctrl + O` (or `File` → `Open Folder`)
3. Navigate to: `C:\Learn\VueNetCrud`
4. Click **Select Folder**

**Option B: Open just the Vue client**
1. Launch **VS Code**
2. Press `Ctrl + K`, then `Ctrl + O`
3. Navigate to: `C:\Learn\VueNetCrud\vue-client`
4. Click **Select Folder**

### Step 2: Install Dependencies (First Time Only)

1. Open the terminal in VS Code:
   - Press `` Ctrl + ` `` (backtick) or
   - Go to `Terminal` → `New Terminal`

2. Navigate to vue-client:
   ```bash
   cd vue-client
   ```

3. Install dependencies:
   ```bash
   npm install
   ```

### Step 3: Run the Application

You need **TWO terminals** - one for the API and one for the Vue app.

#### Terminal 1: Start the .NET Core API

1. Open a terminal (`Ctrl + ` `)
2. Run:
   ```bash
   cd VueNetCrud.Server
   dotnet run
   ```
3. Wait for: `Now listening on: http://localhost:5280`

#### Terminal 2: Start the Vue.js App

1. Click the **`+`** button in the terminal panel to open a new terminal
2. Run:
   ```bash
   cd vue-client
   npm run dev
   ```
3. You'll see: `Local: http://localhost:5173/`

### Step 4: Open in Browser

1. Open your web browser
2. Go to: **http://localhost:5173**
3. You should see the Vue application!

### Step 5: Login

- **Username**: `admin`
- **Password**: `123`

---

## 📋 Quick Commands Reference

| Action | Command |
|--------|---------|
| Install Vue dependencies | `cd vue-client && npm install` |
| Start Vue dev server | `cd vue-client && npm run dev` |
| Start .NET API | `cd VueNetCrud.Server && dotnet run` |
| Build Vue for production | `cd vue-client && npm run build` |

---

## 🎯 VS Code Tips

### Multi-Terminal Setup
- Click the **split terminal** icon (or `Ctrl + Shift + 5`) to have both terminals visible
- Name terminals: Right-click terminal tab → **Rename**

### Keyboard Shortcuts
- `` Ctrl + ` `` - Toggle terminal
- `Ctrl + Shift + 5` - Split terminal
- `F5` - Start debugging (if configured)

### Recommended Extensions
Install these VS Code extensions for better development:
- **Volar** (Vue 3 support)
- **TypeScript Vue Plugin (Volar)**
- **ESLint**
- **C#** (for .NET development)

---

## ⚠️ Troubleshooting

**Problem**: Port 5173 already in use
- **Solution**: Vite will automatically use the next available port. Check the terminal output.

**Problem**: Cannot connect to API
- **Solution**: Make sure the .NET API is running first, then start the Vue app.

**Problem**: CORS errors
- **Solution**: Ensure the API is running on `http://localhost:5280` and CORS is configured.

**Problem**: Module not found
- **Solution**: Run `npm install` in the `vue-client` directory.

---

## ✅ Success Checklist

- [ ] VS Code is open with the project
- [ ] Dependencies installed (`npm install` completed)
- [ ] .NET API is running (Terminal 1 shows "Now listening on...")
- [ ] Vue app is running (Terminal 2 shows "Local: http://localhost:5173")
- [ ] Browser opens to http://localhost:5173
- [ ] Can login with admin/123
- [ ] Can see Items and Products pages

---

**Need help?** Check the main `README.md` file for more detailed information.

