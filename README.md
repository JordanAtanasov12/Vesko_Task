# Numbers Management System

A full-stack web application that manages a list of numbers with server-side operations and session persistence.

## Features

- **Add Number**: Adds a random number (1-999) to the session state
- **Clear Numbers**: Removes all numbers from the session state
- **Sum Numbers**: Calculates and displays the sum of all numbers
- **Session Persistence**: Numbers persist across browser tabs
- **Real-time Updates**: Client-side UI updates only when needed
- **Comprehensive Testing**: Full unit and integration test coverage

## Technology Stack

### Backend
- **.NET 8.0** Web API
- **ASP.NET Core** with dependency injection
- **Session State** for data persistence
- **SOLID Principles** implementation
- **Clean Architecture** with separation of concerns
- **xUnit** for unit testing
- **Moq** for mocking dependencies

### Frontend
- **HTML5** with semantic markup
- **CSS3** with responsive design
- **Vanilla JavaScript** with modern ES6+ features
- **Clean UI** with grey buttons on white background

## Architecture

### Backend Architecture (SOLID Principles)

1. **Single Responsibility Principle (SRP)**
   - `NumberService`: Handles only number operations
   - `NumbersController`: Handles only HTTP requests/responses
   - `NumberItem`: Represents only number data

2. **Open/Closed Principle (OCP)**
   - `INumberService` interface allows extension without modification
   - Service implementations can be swapped without changing controllers

3. **Liskov Substitution Principle (LSP)**
   - Any implementation of `INumberService` can replace `NumberService`

4. **Interface Segregation Principle (ISP)**
   - `INumberService` contains only methods needed by clients
   - No unnecessary dependencies

5. **Dependency Inversion Principle (DIP)**
   - Controllers depend on `INumberService` abstraction
   - Dependencies injected through constructor

### Project Structure

```
NumbersAPI/
├── Controllers/
│   └── NumbersController.cs      # API endpoints
├── Services/
│   ├── INumberService.cs         # Service interface
│   └── NumberService.cs          # Service implementation
├── Models/
│   ├── NumberItem.cs             # Number entity
│   └── NumberResponse.cs         # API response model
├── wwwroot/
│   ├── css/
│   │   └── styles.css            # Frontend styles
│   ├── js/
│   │   └── app.js                # Frontend JavaScript
│   └── index.html                # Main HTML page
├── Program.cs                    # Application configuration
└── NumbersAPI.csproj            # Project file

NumbersAPI.Tests/                 # Test project
├── Controllers/
│   ├── NumbersControllerTests.cs # Controller unit tests
│   └── IntegrationTests.cs       # Integration tests
├── Services/
│   └── NumberServiceTests.cs     # Service unit tests
├── TestHelpers/
│   ├── TestDataBuilder.cs        # Test data creation
│   └── MockHttpContextAccessor.cs # Mock helpers
└── NumbersAPI.Tests.csproj       # Test project file
```

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A modern web browser

## Quick Start

### 1. Run the Application
```bash
./setup.sh
```

### 2. Run the Tests
```bash
./run-tests.sh
```

### Manual Setup

1. **Navigate to the API project**
   ```bash
   cd NumbersAPI
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Open your browser**
   Navigate to `https://localhost:7xxx` (port will be shown in console)

## Testing

### Test Coverage
- **Unit Tests**: 15+ test methods covering all business logic
- **Integration Tests**: 8+ end-to-end API tests
- **Error Scenarios**: Comprehensive error handling tests
- **Session Persistence**: Full session state testing

### Running Tests
```bash
# Quick test run
./run-tests.sh

# Manual test execution
cd NumbersAPI.Tests
dotnet test

# Detailed test output
dotnet test --verbosity normal --logger "console;verbosity=detailed"
```

### Test Categories
- **Service Layer Tests**: Business logic validation
- **Controller Tests**: HTTP endpoint testing
- **Integration Tests**: Full API workflow testing
- **Error Handling Tests**: Exception and edge case coverage

For detailed testing documentation, see [TESTING.md](TESTING.md).

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/numbers` | Get all numbers and current state |
| POST | `/api/numbers/add` | Add a random number |
| POST | `/api/numbers/clear` | Clear all numbers |
| GET | `/api/numbers/sum` | Get sum of all numbers |

## Usage

1. **Add Number**: Click "Add Number" to add a random number (1-999) to the list
2. **Clear Numbers**: Click "Clear Numbers" to remove all numbers
3. **Sum Numbers**: Click "Sum Numbers" to calculate and display the total sum
4. **View Numbers**: The numbers list shows all added numbers with their IDs
5. **Session Persistence**: Open multiple tabs - numbers persist across all tabs

## Key Features

### Server-Side Operations
- All calculations and data manipulation happen on the server
- Client only displays data and sends requests
- No client-side data storage or calculations

### Session State Management
- Numbers stored in ASP.NET Core session state
- Persists across browser tabs and page refreshes
- Session timeout: 30 minutes

### Error Handling
- Comprehensive error handling in both backend and frontend
- User-friendly error messages
- Graceful degradation on failures

### Responsive Design
- Mobile-friendly interface
- Clean, professional appearance
- Grey buttons on white background as requested

### Testing Excellence
- 100% endpoint coverage
- Comprehensive unit test suite
- Integration testing for full workflows
- Error scenario testing
- Mock-based isolated testing

## Development Notes

### Backend Best Practices
- Dependency injection for loose coupling
- Async/await pattern for I/O operations
- Proper HTTP status codes and error responses
- Separation of concerns with service layer
- Interface-based programming
- Comprehensive unit testing

### Frontend Best Practices
- Modern ES6+ JavaScript with classes
- Event delegation and proper event handling
- Responsive CSS with mobile-first approach
- Clean separation of concerns
- Error handling and user feedback

### Testing Best Practices
- AAA pattern (Arrange, Act, Assert)
- Descriptive test names
- Single responsibility per test
- Comprehensive assertions
- Mock verification
- Integration testing

### Security Considerations
- CORS configured for frontend communication
- Session cookies with HttpOnly flag
- Input validation and sanitization
- Proper error handling without information leakage

## Quality Assurance

### Code Quality
- SOLID principles implementation
- Clean architecture patterns
- Comprehensive error handling
- Professional documentation

### Testing Quality
- High test coverage
- Multiple test types (unit, integration)
- Error scenario testing
- Performance considerations

### Documentation Quality
- Comprehensive README
- Detailed testing documentation
- Clear setup instructions
- Architecture explanations

## Future Enhancements

- **Performance Testing**: Load testing and optimization
- **Security Testing**: Penetration testing and security scanning
- **Contract Testing**: API contract validation
- **Visual Testing**: Frontend UI regression testing
- **Database Persistence**: Optional database storage
- **Authentication**: User authentication and authorization
- **Real-time Updates**: SignalR for live updates
- **Advanced Validation**: Input constraints and validation rules

## Troubleshooting

### Common Issues
1. **.NET Not Found**: Install .NET 8.0 SDK
2. **Port Conflicts**: Check if ports are available
3. **Session Issues**: Clear browser cookies and restart
4. **Test Failures**: Check test output for specific errors

### Getting Help
- Check the [TESTING.md](TESTING.md) for test-specific issues
- Review error messages in console output
- Ensure all dependencies are properly installed
- Verify .NET version compatibility
