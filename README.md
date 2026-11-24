# GOV.UK Holiday Entitlement UI Tests

Automated UI tests for the
[GOV.UK holiday entitlement calculator](https://www.gov.uk/calculate-your-holiday-entitlement),
built with C#, .NET 10, NUnit, and Microsoft Playwright.

The suite covers the calculator start page and calculation journeys, validation,
navigation, accessibility landmarks, and page structure.

## Requirements

- .NET 10 SDK
- Microsoft Playwright for .NET
- Chromium

Run the commands below from the repository root.

## Setup

### Restore dependencies

**Bash**

```bash
dotnet restore
```

**PowerShell**

```powershell
dotnet restore
```

### Build

**Bash**

```bash
dotnet build --configuration Release
```

**PowerShell**

```powershell
dotnet build --configuration Release
```

### Install Chromium

**Bash**

```bash
powershell.exe -NoProfile -File ./bin/Release/net10.0/playwright.ps1 install chromium
```

**PowerShell**

```powershell
& .\bin\Release\net10.0\playwright.ps1 install chromium
```

## Run tests

### Run the complete test suite

**Bash**

```bash
dotnet test --configuration Release --logger 'console;verbosity=normal'
```

**PowerShell**

```powershell
dotnet test --configuration Release --logger 'console;verbosity=normal'
```

### List discovered tests without running them

**Bash**

```bash
dotnet test --configuration Release --list-tests
```

**PowerShell**

```powershell
dotnet test --configuration Release --list-tests
```

### Run only the pre-flight test

**Bash**

```bash
dotnet test --configuration Release --filter 'Category=PreFlight' --logger 'console;verbosity=normal'
```

**PowerShell**

```powershell
dotnet test --configuration Release --filter 'Category=PreFlight' --logger 'console;verbosity=normal'
```

### Run only the bonus tests

**Bash**

```bash
dotnet test --configuration Release --filter 'Category=Bonus' --logger 'console;verbosity=normal'
```

**PowerShell**

```powershell
dotnet test --configuration Release --filter 'Category=Bonus' --logger 'console;verbosity=normal'
```

### Run one test by name

**Bash**

```bash
dotnet test --configuration Release --filter 'FullyQualifiedName~StartPage_DisplaysTitleAndStartsCalculatorFlow' --logger 'console;verbosity=detailed'
```

**PowerShell**

```powershell
dotnet test --configuration Release --filter 'FullyQualifiedName~StartPage_DisplaysTitleAndStartsCalculatorFlow' --logger 'console;verbosity=detailed'
```

## HTML test report

After a test run, the HTML report is written to:

```text
bin/Release/net10.0/artifacts/TestReport.html
```

Open this file in a browser to review the results.

## Test coverage

| Area | Coverage |
| --- | --- |
| Start page | Page title, main heading, skip link, cookie controls, and entry into the calculator |
| Data-driven journeys | JSON-defined full and part leave-year calculations |
| Regular-hours page | Calculation-basis selection and continuation |
| Validation | Accessible error summary shown after a missing selection |
| Navigation | Change and Start again links |
| Entitlement calculations | Days worked and hours worked per week |
| Accessibility | Main landmark and primary page heading |

## Project structure

```text
MaPS QE UI/
├── MaPS_QE_UI_Test.csproj
├── README.md
├── Data/
│   ├── HolidayTestCases.json
│   └── HolidayTestDataProvider.cs
├── Pages/
│   ├── BasePage.cs
│   ├── StartPage.cs
│   ├── EntitlementPageStartLocators.cs
│   ├── EntitlementPageRegular.cs
│   ├── EntitlementPageRegularLocators.cs
│   ├── SmartAnswerQuestionPage.cs
│   └── EntitlementResultPage.cs
├── Reporting/
│   └── HtmlTestReport.cs
└── Tests/
    ├── AssemblyHooks.cs
    ├── PlaywrightTestBase.cs
    ├── HolidayEntitlementTests.cs
    ├── EntitlementPageRegularTests.cs
    ├── EntitlementBonusTests.cs
    └── Configuration/
        └── TestSettings.cs
```

## Test data

Calculation scenarios are maintained in
[`Data/HolidayTestCases.json`](Data/HolidayTestCases.json).
`HolidayTestDataProvider` loads the scenarios for NUnit using `TestCaseSource`.
This lets you add or adjust calculation cases without changing the C# test
methods.

## Configuration

Test settings are defined in
[`Tests/Configuration/TestSettings.cs`](Tests/Configuration/TestSettings.cs).

| Setting | Environment variable | Default |
| --- | --- | --- |
| Base URL | `Holiday_calc_url` | `https://www.gov.uk/calculate-your-holiday-entitlement` |
| Browser | `BROWSER` | `chromium` |
| Headless mode | `HEADLESS` | `false` |
| Default timeout | None | `15000` ms |

To use a different base URL:

**Bash**

```bash
export Holiday_calc_url='https://example.gov.uk/calculate-your-holiday-entitlement'
dotnet test --configuration Release
```

**PowerShell**

```powershell
$env:Holiday_calc_url = 'https://example.gov.uk/calculate-your-holiday-entitlement'
dotnet test --configuration Release
```

## Failure diagnostics

When a test fails, the suite saves a full-page PNG screenshot and a Playwright
trace ZIP file in the test output's `artifacts` directory.

Open a trace in Playwright Trace Viewer:

**Bash**

```bash
powershell.exe -NoProfile -File ./bin/Release/net10.0/playwright.ps1 show-trace "./bin/Release/net10.0/artifacts/<TestName>-trace.zip"
```

**PowerShell**

```powershell
& .\bin\Release\net10.0\playwright.ps1 show-trace ".\bin\Release\net10.0\artifacts\<TestName>-trace.zip"
```
