#!/bin/bash

echo "Running Numbers Management System Tests"
echo "======================================"

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET 8.0 SDK is not installed."
    echo "Please install .NET 8.0 SDK first."
    exit 1
fi

echo "✅ .NET SDK found: $(dotnet --version)"

# Navigate to the test project
cd NumbersAPI.Tests

echo ""
echo "📦 Restoring test dependencies..."
dotnet restore

if [ $? -eq 0 ]; then
    echo "✅ Test dependencies restored successfully"
else
    echo "❌ Failed to restore test dependencies"
    exit 1
fi

echo ""
echo "🔨 Building test project..."
dotnet build

if [ $? -eq 0 ]; then
    echo "✅ Test project built successfully"
else
    echo "❌ Test project build failed"
    exit 1
fi

echo ""
echo "🧪 Running unit tests..."
echo "========================"

# Run tests with detailed output
dotnet test --verbosity normal --logger "console;verbosity=detailed"

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ All tests passed successfully!"
    echo ""
    echo "Test Summary:"
    echo "- Unit tests for NumberService"
    echo "- Unit tests for NumbersController"
    echo "- Integration tests for full API"
    echo "- Error handling tests"
    echo "- Session persistence tests"
else
    echo ""
    echo "❌ Some tests failed"
    exit 1
fi
