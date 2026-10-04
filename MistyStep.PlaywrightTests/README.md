# MistyStep Playwright tests

The NUnit tests use Playwright's `PageTest` base class. Each test gets an isolated browser context, so IndexedDB and local storage do not leak between test cases.

## Run locally

Start the app in one terminal:

```sh
dotnet run --project MistyStep/MistyStep.csproj --urls http://127.0.0.1:5197
```

Build the solution, install Chromium once, and run the test project in another terminal:

```sh
dotnet build MistyStep.sln
pwsh MistyStep.PlaywrightTests/bin/Debug/net9.0/playwright.ps1 install --with-deps chromium
MISTYSTEP_BASE_URL=http://127.0.0.1:5197 dotnet test MistyStep.PlaywrightTests/MistyStep.PlaywrightTests.csproj
```

Set `MISTYSTEP_BASE_URL` to the test deployment URL in CI after the app is available. The GitHub Actions workflow installs Chromium and its Linux dependencies, starts the app, waits for its HTTP endpoint, and then runs the test suite. The tests cover language/theme persistence, responsive layout, FAB actions, exercise/program deletion and cascades, timer transitions, and backup import/export.