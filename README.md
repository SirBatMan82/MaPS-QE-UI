# GOV.UK Holiday Entitlement Calculator — Playwright & C# (.NET 10) UI Automation Framework

Automated end-to-end, pre-flight, validation, and accessibility UI test suite built with **Playwright for .NET**, **NUnit**, and **C# (.NET 10)** targeting the [GOV.UK Holiday Entitlement Calculator](https://www.gov.uk/calculate-your-holiday-entitlement).

---

## Why This Suite Exists

Statutory holiday entitlement calculations directly affect pay accuracy and employment compliance for workers and employers across the UK. Because GOV.UK Smart Answers uses a dynamic, branching decision tree, a single broken radio label, routing change, or validation regression can block users from reaching an accurate entitlement figure.

This suite provides:
1. **Fast Pre-Flight Structural Verification:** Confirms critical DOM elements, headings, radios, and GOV.UK Design System landmarks exist and are visible before diagnosing deeper journey failures.
2. **Data-Driven Journey Coverage:** Separates test scenarios (`Data/HolidayTestCases.json`) from execution logic so new calculation paths (days worked per week, hours worked per week, part-year starters) can be added without modifying C# test code.
3. **Accessible Validation & Navigation Guardrails:** Verifies that empty submissions trigger GOV.UK's accessible `.govuk-error-summary` component and that "Change" / "Start again" links preserve expected Smart Answer state transitions.
4. **Zero-Guesswork Failure Diagnostics:** Automatically captures full-page screenshots and Playwright Trace Viewer archives (`*-trace.zip`) on test failure.

---

## What Is Being Tested

| Test Area | Test Class / Method | Purpose |
| :--- | :--- | :--- |
| **Start Page & Entry Flow** | `HolidayEntitlementTests.StartPage_DisplaysTitleAndStartsCalculatorFlow` | Confirms the landing page loads, renders the expected title, dismisses the GOV.UK cookie banner cleanly, and enters the Smart Answer flow via **Start now**. |
| **Data-Driven Entitlement Journeys** | `HolidayEntitlementTests.CalculatorFlow_CompletesJourneyAndDisplaysExpectedResult` | Executes end-to-end calculation scenarios loaded from `Data/HolidayTestCases.json` (e.g., 5 days/week full leave year returning `28 days`, part-leave-year hours worked per week) and verifies the calculated result summary. |
| **Regular-Hours Pre-Flight Check** | `EntitlementPageRegularTests.PreFlight_AllMappedRegularPageElementsExist` | Audits all mapped locators on `/calculate-your-holiday-entitlement/y/regular` (`QuestionHeading`, all 5 calculation basis radios, `ContinueButton`, `StartAgainLink`, `ChangeIrregularHoursLink`, feedback & footer controls). |
| **Regular Basis Selection** | `EntitlementPageRegularTests.RegularPage_SelectsDaysWorkedPerWeekAndContinues` | Verifies selecting a regular calculation basis advances to the next Smart Answer question step. |
| **Validation Error Summary** | `HolidayEntitlementTests.MissingSelection_ShowsAccessibleValidationErrorSummary`<br>`EntitlementBonusTests.RegularPage_MissingBasisSelection_DisplaysAccessibleErrorSummary` | Ensures submitting without selecting an option displays the accessible `.govuk-error-summary` banner (`"There is a problem"`). |
| **Journey State Navigation (Bonus)** | `EntitlementBonusTests.RegularPage_ChangeIrregularHoursLink_NavigatesBackToIrregularHoursQuestion`<br>`EntitlementBonusTests.RegularPage_StartAgainLink_ResetsJourneyToStartOrFirstQuestion` | Validates GOV.UK "Your answers" **Change** and **Start again** links navigate to the expected steps. |
| **Multi-Basis Statutory Calculations (Bonus)** | `EntitlementBonusTests.RegularPage_FullLeaveYear_CalculatesStatutoryEntitlementAcrossBases` | Parameterized verification across multiple regular-hours calculation bases (`DaysWorkedPerWeek` and `HoursWorkedPerWeek`). |
| **Accessibility Landmarks (Bonus)** | `EntitlementBonusTests.StartPage_AccessibilityLandmarksAndMainHeading_ArePresent` | Verifies semantic `<main id="content">`, primary `<h1>`, and the `"Skip to main content"` (`#content`) skip link. |

---

## Project Structure

```text
MaPS QE UI/
├── Configuration/
│   └── TestSettings.cs                    # Base URL, timeouts, and environment configuration
├── Data/
│   ├── HolidayTestCases.json              # Externalized JSON test scenarios & expected phrases
│   └── HolidayTestDataProvider.cs         # NUnit [TestCaseSource] loader for JSON cases
├── Pages/
│   ├── BasePage.cs                        # Shared IPage wrapper & GOV.UK cookie banner handler
│   ├── StartPage.cs                       # Start page actions (OpenAsync, ClickStartNowAsync)
│   ├── EntitlementPageStartLocators.cs    # Locator map & pre-flight dictionary for Start page
│   ├── EntitlementPageRegular.cs          # Regular-hours basis page actions & enum mapping
│   ├── EntitlementPageRegularLocators.cs  # Locator map & pre-flight dictionary for /y/regular
│   ├── SmartAnswerQuestionPage.cs         # Reusable radio, numeric, date, and Continue actions
│   └── EntitlementResultPage.cs           # Final entitlement summary & "Change" row actions
├── Tests/
│   ├── PlaywrightTestBase.cs              # NUnit PageTest base with auto-tracing & screenshots
│   ├── HolidayEntitlementTests.cs         # Core start, validation, and JSON-driven E2E tests
│   ├── EntitlementPageRegularTests.cs     # Pre-flight and interaction tests for /y/regular
│   └── EntitlementBonusTests.cs           # Bonus validation, navigation, multi-basis & a11y tests
├── playwright.runsettings                 # Default headless browser launch configuration
└── README.md


How to run Build and run tests
Powershell - dotnet restore
Powershell - dotnet build
Powershell - & .\bin\Debug\net10.0\playwright.ps1 install chromium
Powershell - Headed $env:HEADED = "0"

$env:HEADED = "1"
dotnet test --logger "console;verbosity=normal"
Powershell - Non headed mode $env:HEADED = "1"
                            dotnet test --logger "console;verbosity=normal"


Powershell - Run test - dotnet test --logger "console;verbosity=normal"

# Run only Pre-Flight structural checks
dotnet test --filter "Category=PreFlight" --logger "console;verbosity=normal"

# Run only Bonus tests
dotnet test --filter "Category=Bonus" --logger "console;verbosity=normal"

# Run a single test by name
dotnet test --filter "FullyQualifiedName~StartPage_DisplaysTitleAndStartsCalculatorFlow" --logger "console;verbosity=detailed"

Debugging outputs
& .\bin\Debug\net10.0\playwright.ps1 show-trace .\bin\Debug\net10.0\artifacts\<TestName>-trace.zip

Category	Test Class	Test Methods / Cases	What It Covers
PreFlight	EntitlementPageRegularTests	PreFlight_AllMappedRegularPageElementsExist	Verifies all mapped locators on /calculate-your-holiday-entitlement/y/regular exist in the DOM and are visible on load.
Bonus	EntitlementBonusTests	
RegularPage_MissingBasisSelection_DisplaysAccessibleErrorSummary


RegularPage_ChangeIrregularHoursLink_NavigatesBackToIrregularHoursQuestion


RegularPage_StartAgainLink_ResetsJourneyToStartOrFirstQuestion


RegularPage_FullLeaveYear_CalculatesStatutoryEntitlementAcrossBases (2 [TestCase] rows: DaysWorkedPerWeek and HoursWorkedPerWeek)


StartPage_AccessibilityLandmarksAndMainHeading_ArePresent

Validation error summary, "Change" and "Start again" navigation links, multi-basis full-leave-year calculations, and start page landmarks/skip link.
Uncategorized (Core / Data-Driven)	
HolidayEntitlementTests


EntitlementPageRegularTests

StartPage_DisplaysTitleAndStartsCalculatorFlow


MissingSelection_ShowsAccessibleValidationErrorSummary


CalculatorFlow_CompletesJourneyAndDisplaysExpectedResult (2 JSON-driven [TestCaseSource] cases)


RegularPage_SelectsDaysWorkedPerWeekAndContinues

Core start flow, missing-selection validation, JSON data-driven end-to-end journeys, and regular basis selection.

$env:HEADED = "0"

# 1. Run PreFlight category
dotnet test --filter "Category=PreFlight" --logger "console;verbosity=normal"

# 2. Run Bonus category
dotnet test --filter "Category=Bonus" --logger "console;verbosity=normal"

# 3. Run ALL tests across every category (and uncategorized tests)
dotnet test --logger "console;verbosity=normal"