# AI CV Parsing

## Decisions Log
This section records confirmed product and technical decisions.
It overrides any open questions or options described later in the document.
The agent must read this section first and apply these decisions throughout.

| # | Decision | Replaces / Overrides |
|---|---|---|
| D-01 | HR CV review module remains a staging area. Option A is confirmed. CV is parsed into CandidateModel first. Conversion to employee or engineer is a deliberate user-triggered action. | "Final recommendation" section - the decision is now made |
| D-02 | `.doc` support is deferred. The file picker keeps `.doc` but must show a user-facing message: "Legacy .doc format is not supported. Please save as .docx and retry." OpenXML remains unchanged for `.docx` extraction. | Gap item: "Either remove `.doc` from picker or add `.doc` extraction support" |
| D-03 | PDF extraction uses PdfPig. No change from current implementation. | Confirmed, no override needed |
| D-04 | CV pre-processing is user-triggered from the HR CV edit form. It is not a background job. | Phase 2 description |
| D-05 | Aspose.Words decision withdrawn. OpenXML is kept for `.docx`. `.doc` handling is covered by D-02. No new library is introduced. | Supersedes earlier draft suggestion to use Aspose.Words |
| D-06 | Phase 1 settings store AI provider configuration under a new nested settings object on `ConnectionModel`. Provider-specific API keys remain separately stored within that nested object and continue to use the existing encrypted save/load pattern. | Clarifies the Phase 1 persistence shape for provider settings |
| D-07 | Phase 2 pre-processing artifacts are stored on `CandidateModel` as a raw extracted text field and a normalized preprocessed JSON field. | Clarifies the Phase 2 artifact persistence target |
| D-08 | The existing **Parse with AI** button remains a single user-triggered orchestration action: run pre-processing first, then AI summarization. Saving remains a separate user decision (no auto-save on parse). | Replaces the temporary split-action finding from Phase 2 notes |
| D-09 | AI summary review output must be presented in a RichText/Word-friendly format with clear structure (headings, titles, and subtitles) before final save/approval. | Clarifies Phase 5 review UX requirement |

## Document Status

| Status | Value |
|---|---|
| Phase in progress | Phase 4.6 — HR CV binding synchronization and saved summary replay implemented |
| Last updated | 2026-05-26 |
| Updated by | GitHub Copilot |

## Changelog
| Version | Change |
|---|---|
| v1.12 | Fixed unrelated project-opening regression by hardening `ProjectEditForm` initialization: lookup loads now execute with per-source fault isolation and warning aggregation so one failing data source no longer blocks opening project records. Also fixed HR CV Education tab rendering by replacing runtime re-parenting with a dedicated Education grid bound to `bsEducationEntry` (Work tab behavior preserved). |
| v1.11 | Implemented HR CV persistence/UI synchronization refinements: added `CandidateModel.ReviewedSummaryTitle`, bound reviewed-title and years-of-experience fields explicitly in `HrCvEditForm`, added runtime Education/Work tab split with work-history grid binding, enabled `View AI Summary` to replay saved formatted summary content from `PreprocessedJson`, and replaced conversion action placeholders with SVG icon assignment + fallback URI behavior. |
| v1.10 | Expanded OpenAI model catalog in `ApiKeySettingForm` to include GPT-5.4 Nano and additional GPT/o-series options. Updated HR CV review apply flow to set a reviewed CV title in `txtSummary` and ensured parsed education output is explicitly bound to `gridControl1` (`bsEducationEntry`) after apply. |
| v1.9 | Implemented summary **approval/apply** behavior in AI review flow: parsed candidate values are now applied only when user confirms in `AiSummaryReviewForm` (`Apply` action). Added loading state feedback in `HrCvEditForm` during preprocess, AI summarize, preview preparation, and apply steps via `WaitForm1` (`SplashScreenManager`) so users see pending actions. |
| v1.8 | Implemented HR CV conversion actions baseline in `HrCvEditForm`: added **Convert To Employee** and **Convert To Engineer** ribbon actions, mapped candidate fields into `EmployeeModel`, resolved `EmployeeTypeModel` from `EmployeeType` enum via `EmployeeTypeRepository.ResolveEmployeeTypeModel`, and persisted converted records to `Employees` collection. |
| v1.7 | Resolved key blockers: implemented provider-aware AI parsing path (OpenAI/DeepSeek endpoint routing) and added RichText review surface (`AiSummaryReviewForm`) with structured headings/subheadings shown immediately after Parse-with-AI, while preserving D-08 no-auto-save behavior. |
| v1.6 | Optimized Parse-with-AI orchestration to remove duplicate extraction: pre-processing output raw text is now reused for AI summarization via `ICvParsingService.ParseCvTextAsync`. Preserved D-08 behavior (single user action, no auto-save). |
| v1.5 | Clarified behavior: `Parse with AI` is a single orchestration action (preprocess then summarize) with no auto-save (D-08). Added RichText/Word structured summary requirement (D-09). Added explicit red flags/blockers snapshot for next implementation steps. |
| v1.4 | Implemented core Phase 2 pre-processing flow: added `ICvParsingService.PreprocessCvAsync`, added raw/normalized artifact output in `CvParsingService`, added `CandidateModel.RawExtractedText` and `CandidateModel.PreprocessedJson`, and applied `.doc` deferral message in `HrCvEditForm` per D-02. Annotated resolved gaps and findings. |
| v1.3 | Started Phase 2 implementation. Added D-07 to define pre-processing artifact persistence on `CandidateModel` (raw extracted text + normalized preprocessed JSON). Updated Document Status to Phase 2. |
| v1.2 | Added D-06 to confirm that Phase 1 provider settings are grouped under a nested `ConnectionModel` settings object with provider-specific encrypted API keys. Updated document status for the current implementation session. |
| v1.1 | Added Decisions Log. Confirmed Option A (D-01). Confirmed OpenXML stays, `.doc` deferred with user message (D-02). Withdrawn Aspose.Words suggestion (D-05). Closed open final recommendation. Added agent maintenance instructions. |

## Purpose
This document captures the discovery findings for the current HR CV review module and its relationship to the employee and engineer modules.

The goal is to prepare the groundwork for a future feature that will:
- upload a CV file,
- pre-process it without AI,
- send the normalized output to an AI provider,
- generate a professional CV summary,
- archive the result against an employee or engineer,
- allow the end-user to review and adjust the generated output.

This document does **not** implement the feature. It documents the current state, gaps, constraints, and a recommended build sequence.

---

## Solution / project context
- Solution: `Spectrum.sln`
- UI project: `Spectrum.UI`
- Models project: `Spectrum.Models`
- Data access project: `Spectrum.DataLayers`
- Target framework: `.NET Framework 4.8`
- UI stack: WinForms + DevExpress
- Database: MongoDB via the official MongoDB C# driver

Relevant instruction already present in the repo:
- Use `EmployeeTypeModel` for `EmployeeModel.EmployeeType` and resolve persisted/default employee type values from the `EmployeeType` enum.

---

## Executive summary

### What currently exists
1. A dedicated HR CV review module already exists under:
   - `Spectrum.UI\Views\HumanResources\HRCVs\HrCvsListForm*`
   - `Spectrum.UI\Views\HumanResources\HRCVs\HrCvEditForm*`
2. The HR CV review module uses a separate MongoDB collection and model:
   - Model: `CandidateModel`
   - Repository: `CandidateRepository`
   - Collection name: `Candidates`
3. A CV parsing service already exists:
   - `Spectrum.DataLayers\HumanResources\Employees\Services\CvParsingService.cs`
4. The service already does non-AI extraction for:
   - `.pdf` via `UglyToad.PdfPig`
   - `.docx` via OpenXML
   - `.txt` directly
5. Employee and engineer records already share the same underlying MongoDB object:
   - Model: `EmployeeModel`
   - Repository: `EmployeeRepository`
   - Collection name: `Employees`
   - Employee vs engineer distinction is done through `EmployeeModel.EmployeeType`

### What does not yet exist
1. No current link from `CandidateModel` to an `EmployeeModel` or engineer record.
2. No provider switch between OpenAI and DeepSeek.
3. No provider-specific model dropdown.
4. No structured archived AI result stored on employee/engineer records.
5. No CV summary rendering in DevExpress RichEdit / Word export flow.
6. No workflow that creates employee/engineer records from parsed CV output.

### Main architectural finding
The current HR CV review module is a **candidate intake / review surface**, not yet a bridge into the employee/engineer lifecycle. The employee and engineer modules already have richer business records and document archiving behavior, but the candidate module is currently isolated.

---

## Current module inventory

### HR CV review module
Files:
- `Spectrum.UI\Views\HumanResources\HRCVs\HrCvsListForm.cs`
- `Spectrum.UI\Views\HumanResources\HRCVs\HrCvsListForm.Designer.cs`
- `Spectrum.UI\Views\HumanResources\HRCVs\HrCvEditForm.cs`
- `Spectrum.UI\Views\HumanResources\HRCVs\HrCvEditForm.Designer.cs`

Supporting model / data layer:
- `Spectrum.Models\HumanResources\Candidates\CandidateModel.cs`
- `Spectrum.Models\HumanResources\Candidates\EducationEntryModel.cs`
- `Spectrum.Models\HumanResources\Candidates\WorkExperienceModel.cs`
- `Spectrum.DataLayers\HumanResources\Candidates\CandidateRepository.cs`
- `Spectrum.DataLayers\HumanResources\Candidates\ICandidateRepository.cs`
- `Spectrum.DataLayers\HumanResources\Employees\Services\CvParsingService.cs`
- `Spectrum.DataLayers\HumanResources\Employees\Services\ICvParsingService.cs`

### Employee module
Files:
- `Spectrum.UI\Views\HumanResources\Employees\EmployeesListForm.cs`
- `Spectrum.UI\Views\HumanResources\Employees\EmployeesListForm.Designer.cs`
- `Spectrum.UI\Views\HumanResources\Employees\EmployeeEditForm.cs`
- `Spectrum.UI\Views\HumanResources\Employees\EmployeeEditForm.Designer.cs`

Supporting model / data layer:
- `Spectrum.Models\HumanResources\Employees\EmployeeModel.cs`
- related nested models under `Spectrum.Models\HumanResources\Employees\*.cs`
- `Spectrum.Models\HumanResources\EmployeeTypes\EmployeeTypeModel.cs`
- `Spectrum.DataLayers\HumanResources\Employees\EmployeeRepository.cs`

### Engineer module
Files:
- `Spectrum.UI\Views\Members\Engineers\EngineersListForm.cs`
- `Spectrum.UI\Views\Members\Engineers\EngineersListForm.Designer.cs`
- `Spectrum.UI\Views\Members\Engineers\EngineerEditForm.cs`

Core finding:
- Engineers also use `EmployeeModel` and `EmployeeRepository`.
- The engineer list filters employees by `EmployeeType.Engineer`.
- The engineer edit form forces the type to `Engineer` when saving.

### AI/API settings module
Files:
- `Spectrum.UI\Views\Main\ApiKeySettingForm.cs`
- `Spectrum.UI\Views\Main\ApiKeySettingForm.Designer.cs`
- `Spectrum.Models\Administration\Connections\ConnectionModel.cs`
- `Spectrum.DataLayers\DataAccess\DatabaseFactory.cs`

---

## Underlying data layers and models

## 1. HR CV review data model

### `CandidateModel`
File:
- `Spectrum.Models\HumanResources\Candidates\CandidateModel.cs`

Base class:
- Inherits `EntityObject`
- Therefore every candidate also carries common metadata such as:
  - `_id`
  - `Notes`
  - `Company`
  - `Branch`
  - `CreatedBy`
  - `CreatedAt`
  - `LastModifiedBy`
  - `LastModifiedDate`
  - `WorkingYear`
  - `IsProtected`
  - `IsDefault`
  - `Active`
  - `Locked`
  - `Deleted`

Candidate-specific fields:
- `FirstName`
- `LastName`
- `DateOfBirth`
- `Gender`
- `Email`
- `Phone`
- `Nationality`
- `City`
- `Address`
- `Position`
- `YearsOfExperience` (string)
- `Skills` (string)
- `Summary` (string)
- `RawExtractedText` (string)
- `PreprocessedJson` (string)
- `Education` (`List<EducationEntryModel>`)
- `History` (`List<WorkExperienceModel>`)
- `Confidence` (double)
- `RawInsights` (string)
- `DocumentLink` (string)

Related candidate sub-models:
- `EducationEntryModel`
  - `Degree`
  - `Institution`
  - `Year`
  - `Specialization`
- `WorkExperienceModel`
  - `Company`
  - `Position`
  - `Duration`

### `CandidateRepository`
File:
- `Spectrum.DataLayers\HumanResources\Candidates\CandidateRepository.cs`

Collection:
- `Candidates`

Available operations:
- `GetCandidatesAsync()`
- `GetCandidateByIdAsync(string id)`
- `GetCandidateByName(string name)`
- `AddNewCandidateAsync(CandidateModel candidate)`
- `UpdateCandidateAsync(CandidateModel candidate)`
- `DeleteCandidateAsync(string id)`
- `GetCountAsync()`

### Key finding
The HR CV review module is fully backed by its own candidate object and repository. It is **not** currently reusing `EmployeeModel`.

---

## 2. Employee / engineer data model

### `EmployeeModel`
File:
- `Spectrum.Models\HumanResources\Employees\EmployeeModel.cs`

Base class:
- Inherits `EntityObject`

Top-level fields:
- `EmployeeNo`
- `FirstName`
- `LastName`
- `Specialization`
- `FatherName`
- `MotherFullName`
- `BloodType`
- `DateOfBirth`
- `PlaceOfBirth`
- `ActualPosition`
- `Address`
- `RecordRegister`
- `Nationality`
- `IdCardOrPassportNo`
- `FamilyStatus`
- `EmployeeType` (`EmployeeTypeModel`)
- `HiredDate`
- `WorkingDate`
- `YearsOfExperience` (int)
- `LeftWork`
- `ContactInfo` (`EmployeeContactInfo`)
- `EmergencyContact` (`EmergencyContactInfo`)
- `Cnss` (`CnssInfo`)
- `Syndicat` (`SyndicatInfo`)
- `WorkingPermit` (`WorkingPermitInfo`)
- `Financial` (`FinancialInfo`)
- `Education` (`List<EducationInfo>`)
- `WorkExperience` (`WorkExperienceInfo`)
- `DocumentLink`
- computed property `FullName`

Related nested models:
- `EmployeeContactInfo`
  - `LocalFixPhone`
  - `LocalMobileNo`
  - `AbroadMobileNo`
  - `Email`
- `EmergencyContactInfo`
  - `ContactName`
  - `MobileNo`
- `CnssInfo`
  - `RegistrationDate`
  - `RegistrationNo`
  - `NoOfChildren`
  - `NoOfBeneficiary`
  - `SpouseRegistration`
  - `CnssLeaveDate`
- `WorkingPermitInfo`
  - `RegistrationNo`
  - `PermitDate`
- `FinancialInfo`
  - `FinancialNo`
- `EducationInfo`
  - `Degree`
  - `Specialization`
  - `SchoolOrUniversity`
  - `Place`
  - `GraduationYear`
  - `Specialization`
- `WorkExperienceInfo`
  - `OfficialWorkingDay`
  - `WorkingPosition`
  - `TotalYearsOfExperience`
  - `SpectrumWorkingDate`
  - `YearsAtSpectrum`
  - `WorkLocation`
- `EmployeeTypeModel`
  - `TypeName`

### `EmployeeRepository`
File:
- `Spectrum.DataLayers\HumanResources\Employees\EmployeeRepository.cs`

Collection:
- `Employees`

Important operations:
- `GetEmployeesAsync()`
- `GetEmployeesAsync(EmployeeType employeeType)`
- `GetEmployeeByIdAsync(string id)`
- `GetEmployeeByName(string employeeName)`
- `AddNewEmployeeAsync(EmployeeModel employee)`
- `UpdateEmployeeAsync(EmployeeModel employee)`
- `DeleteEmployeeAsync(string id)`
- `GetLatestEmployeeNoAsync()`

### Key finding
Employee and engineer records already share one storage model and one MongoDB collection. The discriminator is:
- `EmployeeModel.EmployeeType.TypeName`
- resolved from enum `Spectrum.Utilities.Enums.EmployeeType`
  - `Employee`
  - `Engineer`

This directly supports your target statement that employee and engineer map to the same underlying object.

---

## Relationship between HR CV review and employee / engineer modules

## Current relationship: weak / indirect
There is currently **no persisted relational bridge** between:
- `CandidateModel` in `Candidates`
- `EmployeeModel` in `Employees`

There is also no field on `CandidateModel` such as:
- `EmployeeId`
- `EngineerId`
- `TargetEmployeeType`
- `ConversionStatus`
- `ProcessedCvJson`
- `AiSummaryDocument`

### What is shared conceptually
The candidate module and employee/engineer modules share overlapping personal and professional data:
- name
- date of birth
- nationality
- address/contact details
- position / specialization / experience
- document link

### What is not shared technically today
- No repository-level conversion flow
- No save-to-employee action from candidate screen
- No save-to-engineer action from candidate screen
- No common abstraction for CV-derived data
- No archive of AI output on employee/engineer records

### Important nuance in the employee list
`EmployeesListForm` does something extra:
- It loads actual employee records from `Employees`
- It also loads `PeopleDirectory.GetPeopleAsync()`
- It appends display-only engineer-like rows not already present in `_employees`

Implication:
- The employee list is not a pure view of the `Employees` collection.
- Some displayed rows may come from another source and may not represent full `EmployeeModel` records already persisted in MongoDB.
- This matters for future CV attachment / conversion flows because the selected list row may not always represent the same persistence state.

### Engineer module relationship
The engineer module is cleaner:
- It loads from `EmployeeRepository.GetEmployeesAsync(EmployeeType.Engineer)`
- It saves back to `Employees`
- It forces `EmployeeType = Engineer`

Implication:
- The engineer module is the more reliable target for a future parsed-CV-to-record flow.

---

## Current AI / CV parsing implementation

### Existing service
File:
- `Spectrum.DataLayers\HumanResources\Employees\Services\CvParsingService.cs`

### Current behavior
1. Reads file from disk
2. Extracts plain text:
   - `.txt` => `File.ReadAllText`
   - `.pdf` => PdfPig
   - `.docx` => OpenXML
3. Sends full extracted text to OpenAI chat completions API
4. Requests a JSON object response
5. Normalizes `DateOfBirth`
6. Deserializes into `CandidateModel`

> Resolved in [Phase 2] — Added a dedicated non-AI `PreprocessCvAsync` flow that extracts raw text and returns a normalized preprocessing artifact JSON before any AI call.

### Existing OpenAI-only assumptions
- Endpoint is hardcoded to `https://api.openai.com/v1/chat/completions`
- Authorization format assumes OpenAI bearer token flow
- The request shape is OpenAI-specific
- There is no provider abstraction
- There is no DeepSeek support

> Resolved in [Phase 3] — `CvParsingService` now routes chat completion calls by provider (`OpenAI` or `DeepSeek`) behind a common parse path.

### Important implementation gaps in the current parser
1. File filter in `HrCvEditForm` allows `.doc`, but `CvParsingService` does **not** support `.doc`.
   > Decision D-02: `.doc` remains in the picker but triggers a user-facing message. Gap is acknowledged and deferred by design.
   > Resolved in [Phase 2] — `HrCvEditForm` now blocks `.doc` parsing with the exact user-facing message from D-02.
2. The current service sends raw extracted text directly to AI. It does **not** create an intermediate JSON/raw extraction artifact that is stored or inspectable.
   > Resolved in [Phase 2] — Added pre-processing artifact creation (`RawExtractedText` + normalized JSON) and persisted it on `CandidateModel`.
3. The parser result is loaded into `CandidateModel`, not `EmployeeModel`.
4. No provider abstraction exists.
   > Resolved in [Phase 3] — Added provider-aware parsing route in `CvParsingService` and provider-aware settings usage in `HrCvEditForm`.
5. No model catalog exists.
6. No archive/version history exists for parsed outputs.
7. No result is written into a rich text or Word-editable document.
   > Resolved in [Phase 3] — Added a RichText review form and generated structured heading-based summary output shown immediately after Parse-with-AI.

---

## Red flags / blockers snapshot (current)

1. **Model catalog remains static/manual**
   - Impact: Provider/model values come from configured settings and hardcoded defaults; dynamic model discovery is not implemented.
   - Blocker type: Enhancement (not blocking core parse flow).

2. **Formatted summary persistence currently stored inside preprocessing artifact JSON**
   - Impact: Rich text output is available for review, but dedicated first-class fields/versioning are still pending.
   - Blocker type: Data model maturity gap.

3. **Runtime debug-session limitation when adding interface members (ENC0023)**
   - Impact: During active debugging, interface contract changes may not apply via hot reload and require restart/rebuild.
   - Blocker type: Environment/runtime workflow constraint.

---

## Restored baseline discovery findings (preserved)

> This section restores the original discovery-level findings that must remain in the document for cross-session continuity.

### HR CV review edit form - available fields
File:
- `Spectrum.UI\Views\HumanResources\HRCVs\HrCvEditForm.Designer.cs`

Bound/visible groups:
- Personal: `DocumentLink`, `FirstName`, `LastName`, `DateOfBirth`, `Gender`, `Nationality`
- Contact: `Email`, `Phone`, `Address`, `City`
- Experience: `Position`, `Skills`, `Summary`
- Education grid: `Degree`, `Institution`, `Year`, `Specialization`

Important original findings kept:
1. `txtYearsOfExperience` existed but was not data-bound.
2. `txtSummary` existed but was not data-bound.
3. `txtSummaryDetails` was data-bound to `Summary`.
4. Education binding hookup to `_candidateModel.Education` was unclear/incomplete.
5. Hidden `simpleButton1` appeared leftover/test UI.

### HR CV review list form - available fields
File:
- `Spectrum.UI\Views\HumanResources\HRCVs\HrCvsListForm.Designer.cs`

Visible:
- `FirstName`, `LastName`, `DateOfBirth`, `Email`, `Phone`, `Confidence`, `RawInsights`, `Notes`, `IsDefault`, `Active`

Model fields not shown in grid:
- `Gender`, `Nationality`, `City`, `Address`, `Position`, `YearsOfExperience`, `Skills`, `Summary`, `Education`, `History`, `DocumentLink`

### Employee list/edit and engineer list findings (kept)
- Employee and Engineer still share `EmployeeModel` + `EmployeeRepository` + `Employees` collection.
- Engineer distinction remains via `EmployeeTypeModel` resolved from enum `EmployeeType`.
- Employee list may include appended non-persisted rows via `PeopleDirectory`, so list row origin is not always equal to persisted `EmployeeModel`.

### Field mapping opportunities (Candidate -> Employee/Engineer)
Key preserved mapping candidates:
- `FirstName`, `LastName`, `DateOfBirth`, `Nationality`, `Address` direct
- `Email` -> `ContactInfo.Email`
- `Phone` -> `ContactInfo.LocalMobileNo` (business rule pending)
- `Position` -> `ActualPosition`
- `YearsOfExperience` requires string->int conversion
- `Skills/Summary` destination still not first-class structured fields on `EmployeeModel`

### Archive behavior (preserved)
- Candidate side: `DocumentLink` exists, no mature versioned archive workflow yet.
- Employee/Engineer side: document-folder copy conventions and semicolon-delimited `DocumentLink` are mature and reusable.

---

## Implementation trace (executed updates)

### Phase 1 executed
- Added nested AI settings on `ConnectionModel` (`AiSettingsModel`).
- Kept legacy flat fields for compatibility.
- Updated `DatabaseFactory` encryption/decryption flow for provider-specific API keys.
- Updated `ApiKeySettingForm` for provider + model + provider-key editing.

### Phase 2 executed
- Added preprocessing service path: `PreprocessCvAsync`.
- Added `CandidateModel.RawExtractedText` and `CandidateModel.PreprocessedJson`.
- Enforced `.doc` deferral message in parse flow per D-02.
- Kept D-08 single-button orchestration behavior.
- Removed duplicate extraction by reusing preprocessed text in summarize call.

### Phase 3 baseline executed
- Added provider-aware routing in `CvParsingService` for OpenAI/DeepSeek chat endpoints.
- Wired provider-specific model/key resolution from nested settings in `HrCvEditForm`.
- Added RichText review form (`AiSummaryReviewForm`) and structured heading-based summary output displayed post-parse.
- Preserved no-auto-save rule (user decides whether to save).

### Phase 4 baseline executed
- Added candidate conversion actions in `HrCvEditForm`:
  - **Convert To Employee** ribbon action
  - **Convert To Engineer** ribbon action
- Mapped candidate fields into `EmployeeModel` during conversion
- Resolved `EmployeeTypeModel` from `EmployeeType` enum via `EmployeeTypeRepository.ResolveEmployeeTypeModel`
- Persisted converted records to `Employees` collection

### Phase 4.5 updates
- Updated AI review flow to require user **approval/apply** action to commit parsed values to employee/engineer records.
- Integrated loading state feedback using `SplashScreenManager` to indicate pending operations during CV processing and apply stages.

### Phase 4.6 updates
- Implemented HR CV persistence/UI synchronization refinements: added `CandidateModel.ReviewedSummaryTitle`, bound reviewed-title and years-of-experience fields explicitly in `HrCvEditForm`, added runtime Education/Work tab split with work-history grid binding, enabled `View AI Summary` to replay saved formatted summary content from `PreprocessedJson`, and replaced conversion action placeholders with SVG icon assignment + fallback URI behavior.

---

## Remaining gaps / worklist

### Functional gaps still open
1. Candidate-to-employee conversion workflow is not implemented.
   > Resolved in [Phase 4 baseline] — Added `Convert To Employee` action in `HrCvEditForm` and persisted mapped `EmployeeModel` records via `EmployeeRepository`.
2. Candidate-to-engineer conversion workflow is not implemented.
   > Resolved in [Phase 4 baseline] — Added `Convert To Engineer` action in `HrCvEditForm` and persisted mapped `EmployeeModel` records with `EmployeeType.Engineer`.
3. Structured archive/version history for repeated parses is not implemented.
4. Final approval/apply-to-target-record flow is not implemented.
   > Resolved in [Phase 4.5] — Added explicit summary review approval gate (`Apply`) before parsed AI output is applied back to candidate record fields.

### Data model maturity gaps
1. Formatted summary currently stored in preprocessing artifact JSON; no dedicated first-class versioned model yet.
2. Full audit metadata model (provider/model/prompt version/parse history per run) is still incomplete.
3. Employee-side dedicated CV artifact structure remains pending.

### UX gaps still open
1. RichText review baseline exists, but not yet integrated as a full editing/approval workflow with commit semantics.
2. HR CV form legacy binding inconsistencies remain (years-of-experience/summary/education wiring cleanup still pending).
3. No loading feedback exists during parse/preview operations.
   > Resolved in [Phase 4.5] — Added wait/loading state messaging for preprocess, AI summarize, summary preview preparation, and apply operations.

### Technical/operational gaps
1. Model catalog is static/manual (no provider model discovery).
2. Debug-session ENC0023 limitation remains an environment workflow constraint.

---

## Recommended next sequence (from current state)
1. Introduce first-class parse artifact/version model (instead of JSON-only payload growth).
2. Add audit metadata fields for each parse run.
3. Resolve remaining HR CV form binding inconsistencies.
4. Add full commit semantics from RichText review output to employee/engineer target records.
5. Optionally add provider model catalog discovery.

---

## Current state summary
- Decision framework is active through D-09.
- Phase 1 is complete.
- Phase 2 is complete and optimized.
- Phase 3 baseline (provider routing + rich text review baseline) is implemented.
- Phase 4 baseline (candidate conversion actions to employee/engineer) is implemented.
- Phase 4.5 baseline (approval/apply gate + loading-state feedback) is implemented.
- Phase 4.6 updates (UI/synchronization refinements) are implemented.
- Remaining work is mainly artifact/versioning, audit metadata maturity, and deeper review-to-target commit semantics.

---

## Agent instructions for maintaining this document
When implementing any phase:
1. Read Decisions Log first and apply decisions directly.
2. Do not delete original findings; annotate with `Resolved in [Phase X]` or `Finding [date]`.
3. Update Document Status and Changelog every session.
4. Keep this file as the single continuity source for new chats/sessions.
5. Record unresolved blockers with current impact and next action.

## Resume handoff checklist (latest)

Use this checklist when resuming work in the next branch/session.

1. **Approval/apply workflow**
   - Add explicit user action to apply reviewed RichText summary output into target persisted records.
   - Keep user-controlled save semantics (no auto-save during parse).

2. **Parse artifact versioning**
   - Introduce first-class versioned parse artifacts instead of growing JSON payload-only storage.
   - Preserve backward compatibility for existing `PreprocessedJson` records.

3. **Audit metadata completeness**
   - Persist per-run metadata for provider, model, prompt/version stamp, and parse timestamp/history.
   - Ensure metadata is visible for troubleshooting and reproducibility.

4. **HR CV form binding cleanup**
   - Resolve legacy binding inconsistencies in `HrCvEditForm` (`YearsOfExperience`, `Summary`, and education-related wiring).

5. **Optional catalog enhancement**
   - Add provider model discovery/catalog flow if needed after core persistence and approval workflow are finalized.

### Current verified baseline before branch handoff
- Parse with AI orchestration is single-action and ordered: preprocess -> summarize.
- `.doc` deferral message is enforced exactly per D-02.
- Provider-aware OpenAI/DeepSeek routing is implemented.
- RichText review surface is implemented and displayed post-parse.
- Candidate conversion baseline to Employee/Engineer is implemented.

---

## Session update - Project Types report preview binding

- File updated: `Spectrum.UI\Views\Projects\Settings\ProjectTypes\ProjectTypesListForm.cs`
- Issue addressed: `btnPrint_ItemClick` already awaited `GetProjectTypesAsync()`, but the fetched `_dataReportModels` list was never assigned to `ProjectTypeReport`, so the preview could open without displaying rows.
- Change made: assigned `report.DataSource = _dataReportModels;` before setting the viewer document source and showing the modal preview.
- Result: the report preview now opens after the async fetch completes and renders the retrieved project type data correctly.
- Full solution build succeeded after latest fixes.