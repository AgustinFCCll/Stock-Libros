@echo off
title Gestion de Libros
color 1F
echo ========================================
echo    Gestion de Libros - Iniciando...
echo ========================================
echo.

:: Cerrar procesos que puedan estar usando los puertos
echo Cerrando procesos anteriores...
taskkill /F /IM dotnet.exe >nul 2>&1
taskkill /F /IM node.exe >nul 2>&1
timeout /t 2 /nobreak >nul

:: Verificar .NET SDK
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: .NET SDK no esta instalado.
    echo Descargalo de: https://dotnet.microsoft.com/download/dotnet/10.0
    pause
    exit /b 1
)

:: Verificar Node.js
node --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: Node.js no esta instalado.
    echo Descargalo de: https://nodejs.org/
    pause
    exit /b 1
)

echo.
echo ========================================
echo    Iniciando Backend (Puerto 5052)...
echo ========================================
start "Backend" cmd /k "dotnet run"

echo Esperando a que el backend este listo...
timeout /t 5 /nobreak >nul

echo.
echo ========================================
echo    Iniciando Frontend...
echo ========================================
cd /d "%~dp0frontend"
start "Frontend" cmd /k "npm run dev"

echo.
echo ========================================
echo    Listo! 
echo    Backend: http://localhost:5052
echo    Frontend: http://localhost:5173
echo ========================================
echo.
pause
