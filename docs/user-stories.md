# Prothia Platform — REST API Technical Stories

## Overview

This document specifies API-focused technical user stories for frontend, mobile, and IoT/telemetry integration with the **Prothia Platform REST API** (ASP.NET Core / .NET 10 / Entity Framework Core).

### Global Conventions
- **Base URL:** `/api/v1`
- **Default Content-Type:** `application/json; charset=utf-8`
- **Routing Style:** Kebab-case (`/api/v1/threshold-profiles`, `/api/v1/gait-sessions`, `/api/v1/maintenance-schedules`)
- **Error Format:** RFC 9457 `ProblemDetails` (`type`, `title`, `status`, `detail`, `instance`)
- **Authentication:** Standard HTTP Bearer Token (`Authorization: Bearer <token>`)

---

## 1. Bounded Context: Patients

The Patients context maintains amputee clinical demographic profiles, amputation level classification, and functional mobility (K-Level) tiering.

### TS-PAT-001: Register a New Patient
As a physiotherapist, I want to register an amputee patient via the API so that their clinical history, amputation details, and baseline biomechanics are available for follow-up and sensor tracking.

**Acceptance Criteria:**
- **Scenario: Successful Registration**
  - **Given** a valid `POST /api/v1/patients` payload containing `fullName`, `dni`, `age`, `amputation` (`TranstibialRight`, etc.), `kLevel` (`K1`-`K4`), and optional `prosthesisCode`
  - **When** the API validates the uniqueness of the DNI and persists the aggregate
  - **Then** the API responds with `201 Created`, a `Location` header (`/api/v1/patients/{id}`), and the created `PatientResource`
- **Scenario: Duplicate DNI**
  - **Given** a `POST /api/v1/patients` request with a DNI that is already registered
  - **Then** the API responds with `409 Conflict` or `400 Bad Request` with problem details
- **Scenario: Validation Error**
  - **Given** a request missing mandatory fields (`fullName`, `dni`, `age`)
  - **Then** the API responds with `400 Bad Request`

### TS-PAT-002: Query All Registered Patients
As a clinician, I want to retrieve all registered patients so that the clinical web directory lists the cohort with their current adherence, symmetry, and alert status.

**Acceptance Criteria:**
- **Given** a `GET /api/v1/patients` request
- **When** the query service executes a non-tracking read
- **Then** the API responds with `200 OK` and a JSON array of `PatientResource` objects

### TS-PAT-003: Query Patient by Identifier
As a frontend client, I want to fetch a single patient by ID so that detail views and report generators can populate demographic data.

**Acceptance Criteria:**
- **Given** a `GET /api/v1/patients/{id}` request
- **When** the patient with `{id}` exists
- **Then** the API responds with `200 OK` and the `PatientResource`
- **When** `{id}` does not exist
- **Then** the API responds with `404 Not Found`

---

## 2. Bounded Context: Prescriptions

The Prescriptions context manages structured physical rehabilitation regimens, exercise phases, repetitions, and rest periods.

### TS-PRE-001: Query Rehabilitation Exercise Catalog
As a physiotherapist, I want to query the library of physical therapy exercises so that I can select routines tailored to transtibial or transfemoral patients.

**Acceptance Criteria:**
- **Given** a `GET /api/v1/exercises` request
- **When** executed
- **Then** the API responds with `200 OK` and an array of `ExerciseResource` items (including `id`, `name`, `focus`)

### TS-PRE-002: Create Exercise Prescription for Patient
As a clinical specialist, I want to issue an exercise prescription with target phases and prescribed exercise sets so that the patient portal receives their home rehabilitation routine.

**Acceptance Criteria:**
- **Given** a `POST /api/v1/prescriptions` payload containing `patientId`, `phaseNumber`, `phaseName`, `durationWeeks`, `frequencyPerWeek`, and an array of `exercises`
- **When** the patient ID is valid and the prescription aggregate is persisted
- **Then** the API responds with `201 Created` with the `PrescriptionResource`
- **When** the referenced `patientId` does not exist
- **Then** the API responds with `404 Not Found`

### TS-PRE-003: Get Prescription by Patient ID
As a patient or clinician, I want to retrieve the active prescription by patient ID so that the active workout plan is rendered on the portal.

**Acceptance Criteria:**
- **Given** a `GET /api/v1/prescriptions/patient/{patientId}` request
- **When** an active prescription exists
- **Then** the API responds with `200 OK` and the detailed `PrescriptionResource`

---

## 3. Bounded Context: Monitoring & Telemetry

The Monitoring context processes sensor gait telemetry, biomechanical asymmetry deviations, and threshold calibrations.

### TS-MON-001: Record Telemetry Gait Session
As an IoT sensor module or test runner, I want to post recorded gait session telemetry so that symmetry, cadence, steps, and impact peaks are tracked.

**Acceptance Criteria:**
- **Given** a `POST /api/v1/gait-sessions` payload with `patientId`, `durationMinutes`, `steps`, `cadence`, `symmetryPercent`, `alertsCount`, and `impactPeakNewtons`
- **When** persisted
- **Then** the API responds with `201 Created` and the `GaitSessionResource`

### TS-MON-002: Query Clinical Postural Alerts
As a clinician, I want to query pending and resolved postural alerts so that gait abnormalities are reviewed in the Clinical Alert Center.

**Acceptance Criteria:**
- **Given** a `GET /api/v1/alerts` request
- **When** executed
- **Then** the API responds with `200 OK` with an array of `AlertResource` items sorted by occurrence date

### TS-MON-003: Resolve Postural Alert
As a therapist, I want to resolve an alert via `PUT /api/v1/alerts/{id}/resolve` so that the incident is marked attended with a timestamp.

**Acceptance Criteria:**
- **Given** a `PUT /api/v1/alerts/{id}/resolve` request for a pending alert
- **When** executed
- **Then** the API responds with `200 OK`, marking `status = 'Resolved'` and recording `resolvedAt`

### TS-MON-004: Calibrate Patient Threshold Profile
As a specialist, I want to create or update baseline thresholds (`pelvicTilt`, `asymmetry`, `cadence`, `impactPeak`) for a patient via `POST` or `PUT /api/v1/threshold-profiles`.

**Acceptance Criteria:**
- **Given** a valid threshold payload for a patient
- **When** processed
- **Then** the API persists the profile and responds with `200 OK` or `201 Created`

---

## 4. Bounded Context: Workshop & Prosthesis Fleet

The Workshop context enables orthopedic technicians to monitor prosthetic hardware fatigue, schedule preventive maintenance, and manage device lifecycles.

### TS-WRK-001: Register / Associate a Prosthesis
As an orthopedic technician, I want to register a prosthesis and link it to an amputee patient so that cycle accumulation and socket/knee wear can be tracked.

**Acceptance Criteria:**
- **Given** a `POST /api/v1/prostheses` request containing `serialNumber`, `type`, `patientName`, `clinicName`, `kLevel`, `socketType`, etc.
- **When** persisted
- **Then** the API responds with `201 Created` and the `ProsthesisResource`

### TS-WRK-002: Schedule Preventive Maintenance
As a technician, I want to schedule a maintenance inspection via `POST /api/v1/maintenance-schedules` so that clinical workshops avoid mechanical failure before cycle limits are exceeded.

**Acceptance Criteria:**
- **Given** a payload with `serialNumber`, `patientName`, `scheduledDate`, `type`, `workshopName`, `technician`
- **When** executed
- **Then** the API responds with `201 Created` and the `MaintenanceScheduleResource`

### TS-WRK-003: Mechanical Alert Dispatch & Resolution
As an automated sensor system, I want to trigger mechanical overload alerts, and as a technician, resolve them via `PUT /api/v1/mechanical-alerts/{id}/resolve`.

**Acceptance Criteria:**
- **Given** a valid mechanical alert creation or resolution request
- **Then** the API responds with `201 Created` or `200 OK` reflecting updated alert lifecycle state

---

## 5. Bounded Context: Communication

The Communication context handles inter-professional messaging and technical case file transmissions between clinics and orthopedic workshops.

### TS-COM-001: Transmit Clinical File to Workshop
As a physiotherapist, I want to transmit an authorized clinical summary to an external orthopedic workshop via `POST /api/v1/clinical-transmissions`.

**Acceptance Criteria:**
- **Given** a payload with `patientId`, `patientName`, `patientDni`, `workshopName`, `sharedModules`, and `technicalNotes`
- **When** persisted
- **Then** the API responds with `201 Created` and returns `ClinicalTransmissionResource` with `status: 'Transmitted'`

### TS-COM-002: Exchange Clinical Coordination Messages
As a patient or clinician, I want to send and retrieve direct clinical messages via `/api/v1/clinical-messages`.

**Acceptance Criteria:**
- **Given** `GET` or `POST` requests to `/api/v1/clinical-messages`
- **Then** the API persists and returns message threads with timestamps and sender metadata

---

## 6. Bounded Context: Analytics

The Analytics context compiles consolidated biomechanical reports for clinical auditing and longitudinal progress evaluation.

### TS-ANA-001: Generate and Issue Clinical Report
As a clinical director, I want to issue an official biomechanical progress report with quantitative gait indicators via `POST /api/v1/reports`.

**Acceptance Criteria:**
- **Given** a payload with `reportCode`, `patientId`, `indicators` array, `diagnosis`, and `medicalRecommendation`
- **When** created
- **Then** the API responds with `201 Created` and the full `ClinicalReportResource`

### TS-ANA-002: Query Reports by ID or Patient
As a frontend user, I want to query reports via `GET /api/v1/reports` and `GET /api/v1/reports/{id}` so that clinical sheets can be rendered, printed, or exported to CSV.

**Acceptance Criteria:**
- **Given** a `GET /api/v1/reports` request
- **Then** the API returns `200 OK` with all issued reports

---

## 7. Bounded Context: Billing

The Billing context handles institutional clinic subscriptions, patient quota allocations, and electronic tax invoices.

### TS-BIL-001: Manage Clinic Subscription
As an account administrator, I want to query and update clinic subscription tier and quota allocation via `GET` and `PUT /api/v1/clinic-subscriptions/{id}`.

**Acceptance Criteria:**
- **Given** a `GET /api/v1/clinic-subscriptions` or `PUT /api/v1/clinic-subscriptions/{id}` request
- **Then** the API responds with `200 OK` returning `ClinicSubscriptionResource` (plan, usedQuota, totalQuota, fee)

### TS-BIL-002: Query Electronic Invoices
As a clinic finance manager, I want to query issued electronic invoices via `GET /api/v1/invoices`.

**Acceptance Criteria:**
- **Given** a `GET /api/v1/invoices` request
- **Then** the API returns `200 OK` with an array of `ElectronicInvoiceResource` items

---

## 8. Bounded Context: IAM (Identity & Access Management)

The IAM context provides user account registration, authentication, credential validation, and JWT token issuance.

### TS-IAM-001: User Sign-Up
As a new clinician or workshop technician, I want to register a user account via `POST /api/v1/authentication/sign-up`.

**Acceptance Criteria:**
- **Given** a payload containing `username` and `password`
- **When** the username is available and password meets security policies
- **Then** the API creates the account with a BCrypt hash and responds with `201 Created` (or `200 OK`) and `UserResource`
- **When** the username is already taken
- **Then** the API responds with `409 Conflict`

### TS-IAM-002: User Sign-In & Token Issuance
As a registered user, I want to sign in via `POST /api/v1/authentication/sign-in` to receive a signed JWT bearer token.

**Acceptance Criteria:**
- **Given** a payload containing matching credentials
- **When** authenticated
- **Then** the API returns `200 OK` and `AuthenticatedUserResource` containing the JWT token
- **When** credentials are invalid
- **Then** the API returns `401 Unauthorized` or `400 Bad Request`
