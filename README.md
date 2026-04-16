# PowerTradingExtractor
## Overview

This application generates intra-day power position reports by aggregating trade volumes per hour and exporting them into a CSV file.

It is implemented as a .NET 8 Worker Service and follows clean architecture principles.

---

## Features

* Aggregates power trades per hour
* Handles custom day boundary (starting at 23:00 previous day)
* Uses Europe/London timezone
* Generates CSV files with configurable interval
* Runs continuously as a background service
* Logging to console and rolling file logs
* Configurable via appsettings.json

---

## Configuration

Configured in `appsettings.json`:

* `OutputPath`: Folder where CSV files are generated
* `IntervalMinutes`: Execution interval

---

## Architecture

The project follows a layered architecture:

* **Domain**: Core business logic and interfaces
* **Application**: Use cases and services
* **Infrastructure**: External dependencies (file system, external DLL)
* **Worker**: Background execution and scheduling

Principles applied:

* SOLID
* Separation of concerns
* Dependency Injection

---

## Logging

Logging is implemented using Serilog:

* Console logging for real-time monitoring
* File logging with daily rolling files

---

## Scheduling

* Runs immediately on startup
* Executes every configured interval
* Ensures no executions are missed
* Logs execution drift if delays occur

---

## IMPROVEMENTS (FUTURE WORK)

With more time, the following improvements could be implemented:

* Encapsulation of the external PowerService.dll via a wrapper to improve decoupling
* Retry policies for external service calls
* Unit and integration testing
* More advanced scheduling (cron-like or distributed scheduler)
* Enhanced error handling strategies

---
