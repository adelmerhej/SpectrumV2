# Report Builder — Architecture, Findings & Refactor Plan

> **Living document.** Updated at the end of each iteration.  
> Repository: `SpectrumV1` | Branch: `spectrumv1/joseph_branchv2`  
> Target framework: .NET Framework 4.8 | UI: DevExpress WinForms + SpreadsheetControl

---

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [Architecture Overview](#architecture-overview)
3. [Component Inventory](#component-inventory)
4. [Functionality Catalogue](#functionality-catalogue)
5. [Known Issues & Technical Debt](#known-issues--technical-debt)
6. [Normalization Blockers](#normalization-blockers)
7. [Phased Refactor Roadmap](#phased-refactor-roadmap)
8. [Iteration Log](#iteration-log)

---

## Executive Summary

The Spectrum report builder is an **adapter-driven, spreadsheet-based** reporting infrastructure built around DevExpress `SpreadsheetControl`. It supports:

- Per-module domain adapters (`IReportAdapter`) that expose field descriptors, workbook templates, CSV/XLSX export, and round-trip spreadsheet editing.
- A generic designer UI (`ReportDesignerControl`) that consumes any adapter and provides layout management, field insertion, section mode, record selector, summary panel, and dirty-tracking.
- Disk-based template discovery via `ReportTemplateRegistry`.
- Module-level host forms (e.g., `ProjectReportDesignerForm`) that wire the adapter, designer, and repository persistence.

The foundation is solid, but the infrastructure has accumulated **schema debt** (trailing-space BSON names, legacy flat fields), **duplicated helper logic** across adapters, **hard-coded sheet assumptions** in the editor layer, and **missing model initializers** that require defensive null checks throughout.

---

## Architecture Overview

```
ProjectsListForm (launch point)
    └── ProjectReportDesignerForm (host form, persistence)
            ├── ProjectReportAdapter : IReportAdapter, IMultiRecordReportAdapter
            │       ├── ProjectModel (domain entity, MongoDB)
            │       │       ├── InvoiceModel[]
            │       │       ├── AddendumModel[]
            │       │       └── ContractDetailModel
            │       └── ReportSpreadsheetHelper (shared cell read/write)
            └── ReportDesignerControl (generic designer UI)
                    ├── ReportTemplateRegistry → IReportTemplateProvider
                    └── SpreadsheetReportEditorController
                            └── SpreadsheetReportEditorAdapterRegistry
                                    └── OrderSpreadsheetReportEditorAdapter (project-specific)

EmployeeReportAdapter : IReportAdapter
    └── EmployeeModel (domain entity, MongoDB)
            └── Nested subdocuments (ContactInfo, Education, Financial, …)
```

### Layer Map

| Layer | Key Types | Responsibility |
|-------|-----------|----------------|
| **Domain Models** | `ProjectModel`, `EmployeeModel`, `EntityObject` | Data persistence + BSON serialization |
| **Adapter Contract** | `IReportAdapter`, `IMultiRecordReportAdapter`, `IWorksheetRequirements` | Cross-module reporting interface |
| **Adapter Base** | `ReportAdapterBase` | Default no-op implementations |
| **Concrete Adapters** | `ProjectReportAdapter`, `EmployeeReportAdapter` | Domain-specific field mapping, workbook generation, CSV/XLSX export |
| **Designer UI** | `ReportDesignerControl` | Spreadsheet layout, field insertion, record selector, summary, dirty tracking |
| **Editor Infrastructure** | `SpreadsheetReportEditorController`, `SpreadsheetReportEditorAdapterRegistry`, `OrderSpreadsheetReportEditorAdapter` | Editable-sheet row operations, checkpoint/rollback |
| **Template Registry** | `ReportTemplateRegistry`, `ModelReportTemplateProvider` | Disk-based template resolution |
| **Helpers** | `ReportSpreadsheetHelper`, `AdapterFactory` | Shared cell utilities, model-to-adapter mapping |
| **Host Forms** | `ProjectReportDesignerForm` | Adapter wiring + MongoDB persistence |

---

## Component Inventory

### Domain Models

| File | Purpose | Issues |
|------|---------|--------|
| `Spectrum.Models/Projects/ProjectModel.cs` | Main project entity | 9 trailing-space BSON element names; legacy `LegacyArea/Country/City` not removed |
| `Spectrum.Models/Projects/ContractDetailModel.cs` | Contract/financial/warranty subdocument | Flat legacy addendum fields (`Addendum1Ref`, `Addendum1Amount`, etc.) should be removed |
| `Spectrum.Models/Projects/InvoiceModel.cs` | Invoice row model | Clean |
| `Spectrum.Models/Projects/AddendumModel.cs` | Addendum row model | Clean |
| `Spectrum.Models/HumanResources/Employees/EmployeeModel.cs` | Employee entity | Several nullable subdocuments not initialized in constructor (`ContactInfo`, `EmergencyContact`, `Cnss`, `Syndicat`, `WorkingPermit`, `Financial`, `WorkExperience`) |
| `Spectrum.Models/EntityObject.cs` | Base entity class | Dual notification helpers (`OnPropertyChanged`/`SendPropertyChanged`); redundancy should be resolved |
| `Spectrum.Models/Operations/Projects/FinancialSummaryModel.cs` | Financial summary | Not yet wired into report adapters |

### Report Adapter Layer

| File | Purpose | Issues |
|------|---------|--------|
| `Spectrum.Reports/Interfaces/IReportAdapter.cs` | Core adapter contract | Stable; descriptor types co-located in same file is acceptable |
| `Spectrum.Reports/Adapters/ReportAdapterBase.cs` | Default adapter implementation | Clean baseline |
| `Spectrum.Reports/Adapters/Projects/ProjectReportAdapter.cs` | Project adapter | Private cell-read helpers duplicated from `ReportSpreadsheetHelper`; hard-coded sheet names as string literals |
| `Spectrum.Reports/Adapters/HumanResources/Employees/EmployeeReportAdapter.cs` | Employee adapter | Similar duplicated cell-read helpers; defensive null checks for uninitialized subdocuments; no `IMultiRecordReportAdapter` support |
| `Spectrum.Reports/Helpers/AdapterFactory.cs` | Model-to-adapter factory | Manually hard-coded; not extensible |

### Designer & Editor Infrastructure

| File | Purpose | Issues |
|------|---------|--------|
| `Spectrum.Reports/UI/ReportDesignerControl.cs` | Generic designer UI | Uses `dynamic` in one section; some garbled/translated comments |
| `Spectrum.Reports/Editors/SpreadsheetReportEditorController.cs` | Edit checkpoint/rollback | Sheet-name validation is adapter-agnostic; clean |
| `Spectrum.Reports/Editors/SpreadsheetReportEditorAdapterRegistry.cs` | Editor adapter registry | Only registers `OrderSpreadsheetReportEditorAdapter`; no fallback/default for new modules |
| `Spectrum.Reports/Editors/OrderSpreadsheetReportEditorAdapter.cs` | Project row add/remove | Hard-coded to `ProjectReportAdapter`; `Invoices`/`Addendums` sheet names as literals |

### Template & Helper Layer

| File | Purpose | Issues |
|------|---------|--------|
| `Spectrum.Reports/Templates/ReportTemplateRegistry.cs` | Template discovery | Clean |
| `Spectrum.Reports/Templates/ModelReportTemplateProvider.cs` | Generic model template provider | Clean |
| `Spectrum.Reports/Common/ReportSpreadsheetHelper.cs` | Shared cell utilities | Missing key-value and section-write helpers that are duplicated in adapters |

---

## Functionality Catalogue

### Available Features (Project Report Adapter)

| Feature | Status | Location |
|---------|--------|----------|
| Multi-project record selector | ✅ Implemented | `IMultiRecordReportAdapter` in `ProjectReportAdapter` |
| Workbook template loading | ✅ Implemented | `ReportDesignerControl.LoadWorkbook` + `ReportTemplateRegistry` |
| Dynamic field list | ✅ Implemented | `GetFieldDescriptors` in `ProjectReportAdapter` |
| Section-mode field insertion | ✅ Implemented | `InsertCheckedFieldsAsSection` in `ReportDesignerControl` |
| Horizontal/vertical field insertion | ✅ Implemented | `InsertFieldHorizontal`/`InsertFieldVertical` |
| A4 print layout | ✅ Implemented | `ApplyA4LayoutToAllSheets` in editor controller |
| Layout save/load/reset | ✅ Implemented | `SaveLayoutTemplate`/`LoadLayoutTemplate`/`TryLoadDefaultStoredLayout` |
| Invoice row add/remove | ✅ Implemented | `OrderSpreadsheetReportEditorAdapter` |
| Addendum row add/remove | ✅ Implemented | `OrderSpreadsheetReportEditorAdapter` |
| CSV export | ✅ Implemented | `ExportToCsvString` via adapter |
| XLSX export | ✅ Implemented | `ExportToXlsxBytes` via adapter |
| Summary panel | ✅ Implemented | `GetSummaryFields` + `RefreshSummary` |
| Spreadsheet round-trip (apply back to model) | ✅ Implemented | `ApplySpreadsheetChanges` in `ProjectReportAdapter` |
| Format painter | ✅ Implemented | `ReportDesignerControl` ribbon |
| Dirty tracking & save prompt | ✅ Implemented | `MarkEditableSheetAsDirty` + `OnFormClosing` |

### Available Features (Employee Report Adapter)

| Feature | Status | Location |
|---------|--------|----------|
| Single-record selector | ✅ Implemented | Standard `IReportAdapter` |
| Workbook template loading | ✅ Implemented | Same infrastructure |
| Dynamic field list | ✅ Implemented | `GetFieldDescriptors` in `EmployeeReportAdapter` |
| Education worksheet editing | ✅ Implemented | `IWorksheetRequirements` + spreadsheet round-trip |
| CSV export | ✅ Implemented | `ExportToCsvString` via adapter |
| XLSX export | ✅ Implemented | `ExportToXlsxBytes` via adapter |
| Summary panel | ✅ Implemented | `GetSummaryFields` |
| Multi-record selector | ❌ Not implemented | Would need `IMultiRecordReportAdapter` |

---

## Known Issues & Technical Debt

### Schema / Model Layer

| ID | Severity | File | Description |
|----|----------|------|-------------|
| M-01 | **High** | `ProjectModel.cs` | 9 `[BsonElement]` values have trailing spaces (e.g., `"Reference "`, `"ClientName "`) — causes BSON deserialization drift on field rename or index operations |
| M-02 | **Medium** | `ContractDetailModel.cs` | Legacy flat addendum fields (`Addendum1Ref`, `Addendum1Amount`, `Addendum2Amount`, etc.) exist alongside the normalized `AddendumModel[]` list — schema duplication |
| M-03 | **Medium** | `EmployeeModel.cs` | Several nested subdocuments not initialized in constructor — forces null-guard chains throughout adapter and UI code |
| M-04 | **Low** | `EntityObject.cs` | Dual change-notification helpers (`OnPropertyChanged`/`SetField` and `SendPropertyChanging`/`SendPropertyChanged`) — redundancy and inconsistency |

### Adapter Layer

| ID | Severity | File | Description |
|----|----------|------|-------------|
| A-01 | **Medium** | `ProjectReportAdapter.cs` | Private `GetCellText`, `GetCellDate`, `GetCellDecimal` etc. duplicated locally instead of using `ReportSpreadsheetHelper` |
| A-02 | **Medium** | `EmployeeReportAdapter.cs` | Same cell-read helpers duplicated; also contains defensive null checks due to M-03 |
| A-03 | **Low** | `ProjectReportAdapter.cs` | Sheet names (`"Invoices"`, `"Addendums"`) as string literals in multiple places |
| A-04 | **Low** | `AdapterFactory.cs` | Manually hard-coded per model type; no reflection or DI-based registration |

### Designer / Editor Layer

| ID | Severity | File | Description |
|----|----------|------|-------------|
| D-01 | **Medium** | `ReportDesignerControl.cs` | `dynamic` keyword used in one member bind path — bypasses compile-time safety |
| D-02 | **Low** | `ReportDesignerControl.cs` | Some comments appear garbled or are in mixed language (French/English) |
| D-03 | **Low** | `SpreadsheetReportEditorAdapterRegistry.cs` | No fallback editor adapter registered for new module adapters |
| D-04 | **Low** | `OrderSpreadsheetReportEditorAdapter.cs` | Hard-coded to `ProjectReportAdapter` only; not extensible via interface |

---

## Normalization Blockers

The following issues prevent the report builder from being applied uniformly across new modules:

1. **No base spreadsheet editor adapter** — new modules cannot register row-add/remove behavior without creating a fully custom adapter class and registering it manually.
2. **Sheet name string literals** — `"Invoices"` and `"Addendums"` appear in both the adapter and the editor adapter; refactoring requires changing both.
3. **Legacy model fields** — `ContractDetailModel` flat addendum fields cause confusion about which data path is authoritative.
4. **Missing model initializers** — modules with optional subdocuments (Employee) require defensive null guards throughout the report stack.
5. **Hard-coded `AdapterFactory`** — adding a new module requires manual factory modification.
6. **Duplicated cell helpers** — adding a new type of cell parse logic requires editing multiple files.

---

## Phased Refactor Roadmap

### Phase 1 — Stabilize & Clean *(current)*

Focus: eliminate schema debt, remove duplicated helpers, improve model safety.

| Step | Target | Action |
|------|--------|--------|
| 1.1 | `ProjectModel.cs` | Remove trailing spaces from all `[BsonElement]` values |
| 1.2 | `ContractDetailModel.cs` | Remove legacy flat addendum fields (`Addendum1*`, `Addendum2*`) |
| 1.3 | `EmployeeModel.cs` | Initialize all nullable nested subdocuments in the constructor |
| 1.4 | `ReportSpreadsheetHelper.cs` | Add key-value and section-write helpers currently duplicated in adapters |
| 1.5 | `ProjectReportAdapter.cs` | Remove duplicated private cell-read methods; use `ReportSpreadsheetHelper` |
| 1.6 | `EmployeeReportAdapter.cs` | Remove duplicated private cell-read methods; use `ReportSpreadsheetHelper` |
| 1.7 | `ReportDesignerControl.cs` | Replace `dynamic` usage; fix garbled comments |
| 1.8 | `EntityObject.cs` | Remove or consolidate redundant notification helpers |
| 1.9 | Build verification | Confirm zero compiler errors after all changes |

### Phase 2 — Generalize Editor Infrastructure *(planned)*

- Extract `ISpreadsheetReportEditorAdapter` shared base or default fallback implementation.
- Define sheet-name constants on adapters (via interface property or adapter metadata).
- Register `OrderSpreadsheetReportEditorAdapter` as the default/fallback in the registry.

### Phase 3 — Normalize Across Modules *(planned)*

- Add `IMultiRecordReportAdapter` support to `EmployeeReportAdapter`.
- Introduce registration-based `AdapterFactory` (dictionary or DI).
- Wire `FinancialSummaryModel` into `ProjectReportAdapter` summary fields.
- Standardize `IWorksheetRequirements` usage across all adapters.

### Phase 4 — Scale & Extend *(planned)*

- Add a new module adapter (e.g., Contracts or HR Leave) using the normalized infrastructure.
- Validate that adding a module requires no changes to `ReportDesignerControl` or `SpreadsheetReportEditorController`.
- Add unit tests for adapter field descriptors and workbook round-trip logic.

---

## Iteration Log

### Iteration 0 — Analysis & Planning
**Date:** Phase 1 kickoff  
**Status:** ✅ Complete  

**Findings:**
- No dedicated project/report builder documentation existed in the repository.
- Report builder is adapter-driven and already has a solid contract in `IReportAdapter`.
- 9 trailing-space BSON names identified in `ProjectModel`.
- Legacy flat addendum fields in `ContractDetailModel` create schema duplication.
- Cell-read helpers duplicated across `ProjectReportAdapter` and `EmployeeReportAdapter`.
- `EmployeeModel` missing constructor initialization for 7 nested subdocuments.
- `dynamic` usage detected in `ReportDesignerControl`.
- No fallback editor adapter for new modules in the registry.
- `AdapterFactory` is manually hard-coded and not extensible.

**Deliverables:**
- This document (`REPORT_BUILDER.md`) created.
- Phase 1–4 roadmap established.

---

### Iteration 1 — Phase 1 Implementation *(completed)*
**Date:** Phase 1 execution  
**Status:** ✅ Complete — Build passes with 0 errors  

**Changes applied:**

| Step | File | Change |
|------|------|--------|
| 1.1 | `ProjectModel.cs` | Fixed 9 trailing-space `[BsonElement]` names (`Reference`, `TentativeReference`, `Contract`, `ClientName`, `ClientContact`, `EngineerInCharge`, `Username`, `Location`, `YearOfIssuance`) |
| 1.2 | `ContractDetailModel.cs` | Removed 8 legacy flat addendum fields (`Addendum1Ref`, `Addendum1Amount`, `Addendum1Vat`, `Addendum1Ttc`, `Addendum1BoardDate`, `Addendum2Amount`, `Addendum2Vat`, `Addendum2Ttc`) |
| 1.2b | `ProjectReportAdapter.cs` | Removed the legacy "Addendum Details (Fixed)" section from `BuildContractDetailsWorksheet` |
| 1.3 | `EmployeeModel.cs` | Added default initializers for all 7 previously-nullable subdocuments (`ContactInfo`, `EmergencyContact`, `Cnss`, `Syndicat`, `WorkingPermit`, `Financial`, `WorkExperience`) |
| 1.4–1.6 | `ProjectReportAdapter.cs` | Removed `private WriteDetailRow` wrapper (delegates to `ReportSpreadsheetHelper`); removed `private GetCell*` wrappers; all call sites now use `ReportSpreadsheetHelper.*` directly |
| 1.4–1.6 | `EmployeeReportAdapter.cs` | Removed `EnsureNestedInfo()` method and its call site (now redundant due to model initializers) |
| 1.7 | `ReportDesignerControl.cs` | Replaced `dynamic _formatSourceRange` and `ApplyFormatting(dynamic, dynamic)` with strongly-typed `CellRange`; removed un-callable `.Assign()` fallback in favour of clean `dstCell.Style = srcCell.Style` with alignment/format copy |
| 1.8 | `EntityObject.cs` | Made `_emptyChangingEventArgs` `readonly`; removed redundant `SendPropertyChanged(string)`; modernised `SendPropertyChanging` with null-conditional operator; removed stale comments |

**Bug fix (save-button glitch):**

| File | Change |
|------|--------|
| `SpreadsheetReportEditorController.cs` | Removed `IWorksheetRequirements` enforcement block from `ApplyChanges()` — save now always proceeds unconditionally; removed no-longer-needed `System.Linq` and `System.Collections.Generic` usings |
| `ProjectReportAdapter.cs` | Removed `IWorksheetRequirements` interface and `RequiredWorksheetNames()` implementation |
| `EmployeeReportAdapter.cs` | Removed `IWorksheetRequirements` interface and `RequiredWorksheetNames()` implementation |

**Root cause of save error:** `ApplyChanges()` checked for required sheet names (`Invoices`, `Addendums`) before persisting. When a user started from a blank sheet, the missing-sheets dialog appeared, blocking the save. `ApplySpreadsheetChanges` in the adapter already uses `FirstOrDefault` and null-guards, so it handles any sheet configuration safely without enforcement.

**Issues resolved (from Known Issues table):**

| ID | Status |
|----|--------|
| M-01 | ✅ Fixed — all BSON trailing spaces removed |
| M-02 | ✅ Fixed — legacy flat addendum fields removed |
| M-03 | ✅ Fixed — all EmployeeModel subdocuments initialised |
| M-04 | ✅ Fixed — EntityObject notification helpers consolidated |
| A-01 | ✅ Fixed — ProjectReportAdapter private helpers removed |
| A-02 | ✅ Fixed — EmployeeReportAdapter EnsureNestedInfo removed |
| D-01 | ✅ Fixed — dynamic replaced with CellRange |
| D-02 | ⬜ Deferred — garbled comments cosmetic, lower priority |

---

### Iteration 2 — Phase 2 Implementation *(completed)*
**Date:** Phase 2 execution  
**Status:** ✅ Complete — Build passes with 0 errors  

**Goal:** Generalize the spreadsheet editor infrastructure so any new report-adapter module automatically gets interactive row-editing without requiring a bespoke `ISpreadsheetReportEditorAdapter` implementation.

**Changes applied:**

| Step | File | Change |
|------|------|--------|
| 2.1 | `IReportAdapter.cs` | Added `IEditableSheetProvider` interface — adapters implement `GetEditableSheetNames()` to declare which sheets support row add/remove; `IWorksheetRequirements` kept as a tombstone but is semantically superseded |
| 2.2 | `ProjectReportAdapter.cs` | Implements `IEditableSheetProvider`, returning `"Invoices"` and `"Addendums"` |
| 2.3 | `DefaultSpreadsheetReportEditorAdapter.cs` | **New file.** Generic fallback editor adapter driven entirely by `IEditableSheetProvider`; handles any adapter that declares editable sheets |
| 2.4 | `OrderSpreadsheetReportEditorAdapter.cs` | Removed hard-coded `IsEditableDataSheet` helper and sheet name strings; `Initialize` builds `_editableSheets` `HashSet<string>` from `IEditableSheetProvider`; `CanHandle` checks `IEditableSheetProvider`; direct `Spectrum.Reports.Adapters.Projects` using removed |
| 2.5 | `SpreadsheetReportEditorAdapterRegistry.cs` | Registered `DefaultSpreadsheetReportEditorAdapter` after `OrderSpreadsheetReportEditorAdapter` as the generic fallback |

**Design decisions:**

- `OrderSpreadsheetReportEditorAdapter` still registered first (first-pick priority for projects).  
- Any new module adapter only needs `IEditableSheetProvider` for full row-edit UX — zero changes to `SpreadsheetReportEditorController` or `ReportDesignerControl`.  
- `IWorksheetRequirements` is retained as a tombstone; it is no longer enforced anywhere.

**Issues resolved:**

| ID | Status |
|----|--------|
| A-03 | ✅ Fixed — no per-module editor adapter required; `DefaultSpreadsheetReportEditorAdapter` covers any `IEditableSheetProvider` |
| A-04 | ✅ Fixed — `OrderSpreadsheetReportEditorAdapter` no longer imports `Spectrum.Reports.Adapters.Projects` |

---

---

### Iteration 3 — Insert-Value UX Fixes *(completed)*
**Date:** Post-Phase 2 stabilisation  
**Status:** ✅ Complete — Build passes with 0 errors

**Goal:** Fix two separate behavioural regressions in the "Insert Value" ribbon buttons:
1. All three Insert-Value buttons popped *"Select a field"* when fields were **checked** (not node-selected).
2. All three Insert-Value buttons wrote every checked field into the **same cell** — the cursor never advanced between fields.
3. Injected `DateTime` values were not formatted as date cells (Excel `"d"` is day-of-month, not a date format).

---

#### Root-cause analysis

| Bug | Root cause |
|-----|-----------|
| "Select a field" with checked fields | `OnInsertValue*Click` called `GetSelectedFieldDescriptor()` which only reads the highlighted tree node, not checked checkboxes |
| All values in the same cell | `foreach (fd) InsertMultiValue*(fd)` — each call reads `SelectedCell` independently; cursor never moves |
| Date cells unformatted | `SetCellValue` passed the raw `FormatString = "d"` directly to `NumberFormat`; `"d"` means *day digit* in Excel, not a date format |

---

#### Insertion layout model (reference for future scaling)

The designer uses two orthogonal dimensions: **field axis** (which field) and **record axis** (which record/row).  
Each Insert-Value button maps these dimensions as follows:

| Button | Field axis | Record axis (multi-record) | Record axis (row-level) |
|--------|-----------|---------------------------|------------------------|
| **Insert Value** (plain) | down (one row per field) | values go **right** (columns) | values stacked **down** within the field row |
| **Insert Value →** (horizontal) | down (one row per field) | values go **right** (columns) | values stacked **down** within the field row |
| **Insert Value ↓** (vertical) | right (one column per field) | values go **down** (rows) | values stacked **down** within the same column |

This mirrors the Label→Value section buttons but without the label column.

---

#### Cursor-advancement design rule *(important for future scaling)*

> **Any method that inserts multiple fields in a loop MUST use positional `At(row, col)` helpers — never read `SelectedCell` inside a loop.**

The two positional helpers introduced are:

```
InsertValueOnlyHorizontalAt(fd, row, col, recordCount) → returns int rowsUsed
InsertValueOnlyVerticalAt(fd, row, col, recordCount)   → returns int colsUsed
```

The caller maintains a `row` / `col` cursor and advances it by the returned extent after each field:

```csharp
// Horizontal layout example (one field per row):
foreach (var fd in fields)
{
    int usedRows = InsertValueOnlyHorizontalAt(fd, row, col, recordCount);
    row += usedRows;
}

// Vertical layout example (one field per column):
foreach (var fd in fields)
{
    int usedCols = InsertValueOnlyVerticalAt(fd, row, col, recordCount);
    col += usedCols;
}
```

This is the same pattern used by the working `InsertCheckedFieldsAsSection` method
(which calls `InsertFieldHorizontalAt` / `InsertFieldVerticalAt` and advances `row`/`col` after each field).

---

#### `ResolveTargetFields` helper — field selection contract

```
ResolveTargetFields()  →  IList<FieldDescriptor>
  1. Returns all CHECKED descriptors when any checkboxes are ticked (section / multi-field mode)
  2. Falls back to the SELECTED single node when nothing is checked
  3. Returns empty list (triggers info message) when neither is true
```

This is the standard entry point for all Insert-Value and future Insert-* operations. Single-field shortcodes (`InsertFieldValueOnly`) remain valid but must be called only when the list has exactly one element.

---

#### Date-format normalisation rule

`SetCellValue` now normalises the format string before assigning `cell.NumberFormat`:
- `null`, `""`, `"d"`, `"D"` → mapped to `"dd/mm/yyyy"`
- `"N2"` → mapped to `"#,##0.00"`
- Any other value passed through unchanged

Field descriptors in `ProjectReportAdapter` continue to carry `FormatString = "d"` for date fields — this is intentional so the adapter-side format string remains a *.NET* format specifier for display/grid use; only `SetCellValue` translates it to an Excel number-format code.

---

**Changes applied:**

| Step | File | Change |
|------|------|--------|
| 3.1 | `ReportDesignerControl.cs` | Added `ResolveTargetFields()` helper: checked fields → selected node → empty |
| 3.2 | `ReportDesignerControl.cs` | Added `InsertValueOnlyHorizontalAt(fd, row, col, recordCount)` positional helper |
| 3.3 | `ReportDesignerControl.cs` | Added `InsertValueOnlyVerticalAt(fd, row, col, recordCount)` positional helper |
| 3.4 | `ReportDesignerControl.cs` | `OnInsertValueClick`: single field → `InsertFieldValueOnly`; multi → horizontal cursor loop |
| 3.5 | `ReportDesignerControl.cs` | `OnInsertValueHorizontalClick`: replaced `foreach InsertMultiValueHorizontal` with horizontal cursor loop |
| 3.6 | `ReportDesignerControl.cs` | `OnInsertValueVerticalClick`: replaced `foreach InsertMultiValueVertical` with vertical cursor loop |
| 3.7 | `ReportDesignerControl.cs` | `SetCellValue`: normalises `"d"`/`"D"`/null → `"dd/mm/yyyy"` before assigning `NumberFormat`; `DateTime?` boxed as `object` handled automatically (null already guarded) |
| 3.8 | `ReportDesignerControl.cs` | `InsertFieldValueOnly`: multi-record dialog only shown when `fd.HasSelectedRecordValues` is true |
| 3.9 | `ReportDesignerControl.cs` | `InsertMultiValueHorizontal` / `InsertMultiValueVertical`: fall back to single-record path instead of `return` when `!HasSelectedRecordValues` |

---

### Next Iteration — Phase 3 *(pending)*

Planned work:
- Extend `EmployeeReportAdapter` to implement `IEditableSheetProvider` (expose Education / Work Experience sheets for row editing)
- Consider consolidating `OrderSpreadsheetReportEditorAdapter` into `DefaultSpreadsheetReportEditorAdapter` if no project-specific row logic remains
- Address D-02 (garbled comments cosmetic cleanup)
- Add field descriptor unit tests for workbook round-trip read/write
- **Intermediate phase** — ribbon icon polish, button categorisation, format-painter UX, and navigation improvements (to be started after all functional fixes are verified)

---

### Iteration 4 — Intermediate Phase: Ribbon & Navigation UI Redesign *(completed)*
**Date:** Post-Iteration-3 stabilisation
**Status:** ✅ Complete — Build passes with 0 errors

**Goal:** Make the Report Builder tab easier to access and navigate.
All changes are confined to `ReportDesignerControl.Designer.cs` (UI composition) and `ReportDesignerControl.cs` (toggle/state handlers).

---

#### What was wrong (before this iteration)

| Problem | Symptom |
|---------|---------|
| Field list and record selector were mutually exclusive panels hidden behind a single toggle button | Users had to know a button would swap the entire left panel; no visible label indicated what was currently shown |
| Toggle buttons (Field List, Section Mode, Record Selector) changed their **caption text** to signal state | Led to inconsistency; the button for "Records" was captioned "Field List" when records were visible — deeply confusing |
| `MarkEditableSheetAsDirty` had hard-coded `"Invoices"` / `"Addendums"` sheet names | Broke dirty-tracking for any non-project adapter |
| `ApplyRecordSelection` message used the word "project" | Not generic — broke the abstraction for employee or other module adapters |
| Insert Values and Label→Value buttons were in the same ribbon group | Two distinct operations with different semantics were visually indistinguishable |
| Ribbon buttons had no tooltip hints | First-time users had no contextual help |

---

#### Changes applied

**`Spectrum.Reports/UI/ReportDesignerControl.Designer.cs`**

| Change | Detail |
|--------|--------|
| Replaced dual-panel (`_fieldListPanel` / `_recordSelectorPanel`) with `XtraTabControl` (`_leftTabControl`) containing two persistent tab pages | **Fields** tab always visible; **Records** tab hidden until a multi-record adapter is loaded |
| `_fieldListPanel`, `_recordSelectorPanel`, `_fieldListLabel`, `_recordSelectorLabel`, `_leftPanel` retained as hidden stubs | Keeps backward-compatible field declarations so existing code compiles without change |
| Split panel widened from 250 px to 270 px; `Panel1MinSize` raised to 200 | Slightly more room for the field list tree and record combo |
| Ribbon reorganised into **5 focused groups** | `Insert Values` · `Fields` · `Records` · `Layout` · `Templates` |
| `_biToggleFieldList`, `_biSectionMode`, `_biFormatPainter`, `_biToggleRecordSelector` all use `ButtonStyle = Check` | Button visually presses in/out to reflect current state — no caption swap needed |
| `CreateReportCheckButton(...)` factory helper added | Thin wrapper around `CreateReportBarButton` that sets `ButtonStyle = Check` |
| `CreateReportBarButton` signature extended with optional `tipTitle` / `tipText` parameters | Wires a `SuperToolTip` if provided |
| `SetSuperTip` static helper added | Builds a `SuperToolTip` from title + body text |
| Every ribbon button now has a `SuperToolTip` | Describes what the button does and any prerequisites (e.g. "Select a field first") |
| `_biToggleFieldList.Down = true` set at init | Panel starts visible and button starts pressed |

**`Spectrum.Reports/UI/ReportDesignerControl.cs`**

| Change | Detail |
|--------|--------|
| `using DevExpress.XtraTab` added | Required for `XtraTabPage` reference in toggle handlers |
| `_recordSelectorVisible` field removed | State is now owned by the tab control (`_leftTabControl.SelectedTabPage == _tabRecords`) |
| `BuildRecordSelector()` — panel visibility replaced with `_tabRecords.PageVisible` | Records tab appears/disappears based on adapter capability |
| `OnToggleRecordSelectorClick` — replaces mutex panel-swap with tab navigation | Navigates to `_tabRecords` (or back to `_tabFields`); auto-expands the side panel if collapsed; syncs `_biToggleRecordSelector.Down` |
| `OnToggleSectionModeClick` — caption swap removed; `_biSectionMode.Down = _isSectionMode` | Button state reflects mode without text change |
| `OnToggleFieldListClick` — syncs `_biToggleFieldList.Down` after collapse/expand | Button visually reflects panel state |
| `MarkEditableSheetAsDirty` — hard-coded sheet names replaced with `IEditableSheetProvider` lookup | Dirty-tracking is now adapter-driven; falls back to "any edit = dirty" for adapters without the interface |
| `UpdateButtonStates` — syncs `Down` for all three toggle buttons on load/adapter-change | Ensures visual state is always in sync with actual state |
| `ClearPreviousAdapter` — resets tab to Fields tab; hides Records tab; clears `Down` on `_biSectionMode` and `_biToggleRecordSelector` | Clean slate after adapter unload |
| `ApplyRecordSelection` message — replaced "project"/"projects" wording with generic "record"/"records" | Correct for any module adapter |

---

#### Ribbon group reference (final layout)

| Group | Buttons | Purpose |
|-------|---------|---------|
| **Insert Values** | Insert Value · Insert Value → · Insert Value ↓ | Inject raw field values into cells; no label emitted |
| **Fields** | Label → Value · Label ↓ Value · Field List (check) · Section Mode (check) · Format Painter (check) | Field discovery and structured layout authoring |
| **Records** | Records Panel (check) · Apply Selection | Multi-record switching |
| **Layout** | Best Fit · Apply A4 Layout | Sheet formatting / page setup |
| **Templates** | Save Layout · Load Layout · Set Default · Save Data | Layout persistence and inline data save |

---

#### Navigation behaviour reference

| Action | What happens |
|--------|-------------|
| Click **Field List** (check button) | Side panel collapses/expands; button presses in/out |
| Click **Records Panel** (check button) | Side panel navigates to Records tab (or back to Fields tab); panel auto-expands if collapsed |
| Click a tab directly in the side panel | Direct tab navigation independent of ribbon button |
| Load a single-record adapter | Records tab is hidden; Records Panel button is disabled |
| Load a multi-record adapter | Records tab is shown; Records Panel button is enabled |
| Unload adapter (clear) | Records tab hidden; Section Mode and Records Panel buttons reset to "up" |

---

#### Key file summary (for future agents resuming this work)

| File | Role | Status after Iter 4 |
|------|------|---------------------|
| `Spectrum.Reports/UI/ReportDesignerControl.Designer.cs` | UI composition — ribbon, controls, layout | **Redesigned**: `XtraTabControl` left panel, 5 ribbon groups, `SuperToolTip` hints, Check-style toggle buttons |
| `Spectrum.Reports/UI/ReportDesignerControl.cs` | UI logic — toggle handlers, adapter wiring, insertion | **Updated**: toggle handlers use tab control + `Down` state; `_recordSelectorVisible` removed; `MarkEditableSheetAsDirty` is now adapter-driven |
| `Spectrum.Reports/Interfaces/IReportAdapter.cs` | Core contract (`IReportAdapter`, `IMultiRecordReportAdapter`, `IEditableSheetProvider`) | Unchanged this iteration |
| `Spectrum.Reports/Adapters/Projects/ProjectReportAdapter.cs` | Project-domain adapter | Unchanged this iteration |
| `Spectrum.Reports/Editors/SpreadsheetReportEditorController.cs` | Row-edit controller | Unchanged this iteration |
| `Spectrum.Reports/Editors/SpreadsheetReportEditorAdapterRegistry.cs` | Editor adapter registry | Unchanged this iteration |
| `Spectrum.Reports/Editors/DefaultSpreadsheetReportEditorAdapter.cs` | Generic fallback editor adapter | Unchanged this iteration |
| `Spectrum.Reports/Editors/OrderSpreadsheetReportEditorAdapter.cs` | Project-specific editor adapter | Unchanged this iteration |
| `Spectrum.Models/Projects/ContractDetailModel.cs` | BSON-safe project contract subdocument | Unchanged this iteration |
| `Spectrum.Models/Projects/Serializers/SafeObjectIdSerializer.cs` | Tolerant ObjectId BSON serializer | Unchanged this iteration |
| `Spectrum.Models/Spectrum.Models.csproj` | Model project file | Unchanged this iteration |

---

#### What's left after Iteration 4

| Priority | Work Item |
|----------|-----------|
| Phase 3 — High | Extend `EmployeeReportAdapter` to implement `IEditableSheetProvider` (expose Education / Work Experience sheets for row editing) |
| Phase 3 — Medium | Consolidate `OrderSpreadsheetReportEditorAdapter` into `DefaultSpreadsheetReportEditorAdapter` if no project-specific row logic remains |
| Phase 3 — Medium | Cosmetic cleanup of garbled/outdated comments (D-02) |
| Phase 3 — Low | Add field-descriptor unit tests for workbook round-trip |
| UI polish | Per-button large icon assignment — replace text `imageUri` placeholders with verified DX image keys for each action |
| UI polish | Summary panel: add a small "Info" icon and colour to distinguish empty vs. loaded state |
| UX | Double-click on a field tree node to insert label→value directly (shortcut for the most common action) |
| UX | Consider keyboard shortcuts for the three Insert Value modes |