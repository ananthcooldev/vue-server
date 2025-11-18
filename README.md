# Vue .NET CRUD Application

Full-stack application with Vue.js frontend and .NET Core backend.

## Project Structure

```
VueNetCrud/
├── VueNetCrud.Server/    # .NET Core API
└── vue-client/          # Vue.js Frontend
```

## Quick Start Guide

### Opening in VS Code

#### Option 1: Open Entire Solution
1. Open VS Code
2. Go to `File` → `Open Folder`
3. Navigate to `C:\Learn\VueNetCrud`
4. Click "Select Folder"

#### Option 2: Open Individual Projects
- **For API**: Open `C:\Learn\VueNetCrud\VueNetCrud.Server`
- **For Frontend**: Open `C:\Learn\VueNetCrud\vue-client`

### Running the Application

#### Step 1: Start the .NET Core API

1. Open terminal in VS Code (`Ctrl + ~`)
2. Navigate to the API project:
   ```bash
   cd VueNetCrud.Server
   ```
3. Run the API:
   ```bash
   dotnet run
   ```
4. The API will be available at `http://localhost:5280`

#### Step 2: Start the Vue.js Frontend

1. Open a **new terminal** in VS Code (click the `+` icon in terminal panel)
2. Navigate to the Vue client:
   ```bash
   cd vue-client
   ```
3. Install dependencies (first time only):
   ```bash
   npm install
   ```
4. Start the development server:
   ```bash
   npm run dev
   ```
5. The frontend will be available at `http://localhost:5173`

### Using VS Code Multi-Terminal Setup

VS Code allows you to run both projects simultaneously:

1. Open the root folder (`C:\Learn\VueNetCrud`) in VS Code
2. Open **two terminals**:
   - Terminal 1: `cd VueNetCrud.Server && dotnet run`
   - Terminal 2: `cd vue-client && npm run dev`
3. Both will run side by side

### Accessing the Application

- **Frontend**: Open your browser and go to `http://localhost:5173`
- **API Swagger**: `http://localhost:5280/swagger` (if Swagger is enabled)

### Default Login Credentials

- **Username**: `admin`
- **Password**: `123`

## Features

### Frontend (Vue.js)
- ✅ Authentication with JWT tokens
- ✅ Items CRUD operations (requires auth)
- ✅ Products CRUD operations
- ✅ Modern, responsive UI
- ✅ TypeScript support

### Backend (.NET Core)
- ✅ RESTful API
- ✅ JWT Authentication
- ✅ Items and Products endpoints
- ✅ Swagger documentation
- ✅ CORS configured for Vue app

## Troubleshooting

### Port Conflicts
- If port 5173 is in use, Vite will auto-select another port
- If port 5280 is in use, change it in `VueNetCrud.Server/Properties/launchSettings.json`

### CORS Issues
- Ensure the API is running before the Vue app
- Check that CORS is configured in the API for `http://localhost:5173`

### Module Not Found Errors
- Run `npm install` in the `vue-client` directory
- Delete `node_modules` and `package-lock.json`, then run `npm install` again

### API Not Responding
- Check the API terminal for errors
- Verify the API is running on the correct port
- Check firewall settings

## Development Tips

### VS Code Extensions (Recommended)
- **C#** - .NET Core development
- **Volar** - Vue 3 support
- **TypeScript Vue Plugin (Volar)** - Enhanced Vue + TS
- **ESLint** - Code linting
- **Prettier** - Code formatting

### Debugging
- Use VS Code's built-in debugger
- Set breakpoints in both Vue components and .NET controllers
- Check browser DevTools for frontend issues
- Check API terminal output for backend issues

## Next Steps

1. Open both projects in VS Code
2. Start the API server
3. Start the Vue development server
4. Open `http://localhost:5173` in your browser
5. Login and start using the application!

