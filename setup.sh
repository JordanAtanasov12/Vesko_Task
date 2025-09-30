#!/bin/bash

echo "Numbers Management System - Setup Script"
echo "========================================"

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET 8.0 SDK is not installed."
    echo ""
    echo "Please install .NET 8.0 SDK from:"
    echo "https://dotnet.microsoft.com/download/dotnet/8.0"
    echo ""
    echo "For macOS, you can also use Homebrew:"
    echo "brew install --cask dotnet"
    echo ""
    exit 1
fi

echo "✅ .NET SDK found: $(dotnet --version)"

# Navigate to the API project
cd NumbersAPI

echo ""
echo "📦 Restoring NuGet packages..."
dotnet restore

if [ $? -eq 0 ]; then
    echo "✅ Packages restored successfully"
else
    echo "❌ Failed to restore packages"
    exit 1
fi

echo ""
echo "🔨 Building the application..."
dotnet build

if [ $? -eq 0 ]; then
    echo "✅ Application built successfully"
else
    echo "❌ Build failed"
    exit 1
fi

echo ""
echo "🚀 Starting the application..."
echo "The application will be available at:"
echo "- HTTPS: https://localhost:7xxx"
echo "- HTTP:  http://localhost:5xxx"
echo ""
echo "Press Ctrl+C to stop the application"
echo ""

dotnet run
