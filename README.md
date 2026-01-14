# Clinic Management System (Backend)

## 📌 Overview

The **Clinic Management System** is a real-world backend project designed to solve actual problems faced by medical clinics — not just a simple CRUD application.

The system is built with scalability, maintainability, and real business rules in mind using **ASP.NET Core** and **Clean Architecture**.

The project is developed in clear phases, each phase targeting a specific business goal.

---

## 🎯 Project Goals

### 🎯 Goal 1: Smart Time Management (✅ Completed)

Efficiently manage clinic time and prevent conflicts between:

* Doctors
* Patients
* Sessions

**What has been achieved:**

* Prevent overlapping appointments
* Validate appointment date and time before starting a session
* Link sessions only to confirmed appointments
* Full control over the appointment lifecycle

---

### 🎯 Goal 2: Notification System (🚧 In Progress)

Deliver smart notifications to doctors and patients based on clear **business rules**.

**Examples:**

* Appointment reminders for patients
* Doctor notification when a patient arrives
* Notifications for appointment cancellation or rescheduling
* Session start and completion notifications

Planned support:

* SMS notifications (e.g., Twilio)
* Extensible notification system for future channels

---

### 🎯 Goal 3: Payment Gateway Integration (🔜 Planned)

Integrate an online payment gateway to manage clinic payments.

> This goal requires significant effort and time because it targets a real and common problem in clinics.

**Purpose:**

* Organize payment operations
* Link payments to sessions and services
* Support multiple payment methods in the future

---

## ✅ Implemented Features

The following core features have been completed:

* 👨‍⚕️ Doctor Management
* 👤 Patient Management
* 📅 Appointment Booking
* ✅ Appointment Confirmation
* 🧾 Session Management

  * Create Session
  * Start Session
  * Complete Session

---

## 🔄 Workflow Example

1. Patient books an appointment
2. Reception confirms the appointment
3. Patient arrives at the clinic
4. Session is created
5. Session starts
6. Session is completed

---

## ⏳ Pending Features

* 🗂 Medical Records
* 💊 Prescriptions
* 🔔 Full Notification System
* 💳 Payment Gateway Integration

---

## 🏗 Architecture

The project follows **Clean Architecture**, using the same approach previously applied in the:

### 🧱 Architecture Layers

* **Domain Layer**

  * Entities
  * Value Objects
  * Business Rules

* **Application Layer**

  * Use Cases (CQRS: Commands & Queries)
  * DTOs
  * Interfaces

* **Infrastructure Layer**

  * Entity Framework Core
  * Repository Implementations
  * External Services (SMS, Payment Gateways)

* **Presentation Layer (API)**

  * Controllers
  * API Versioning
  * Request & Response Handling

---

## 🗄 Database Design

The database is designed to support real-world relationships and future scalability.

### Main Tables

* Doctors
* Patients
* Appointments
* Sessions
* MedicalRecords (Planned)
* Prescriptions (Planned)
* Payments (Planned)

---

## 🛠 Technologies Used

* ASP.NET Core Web API
* Clean Architecture
* CQRS with MediatR
* Entity Framework Core
* SQL Server
* API Versioning
* Logging & Validation

---

## 🚀 Project Status

* ✅ Goal 1: Completed (Time Management)
* 🚧 Goal 2: In Progress (Notifications)
* 🔜 Goal 3: Planned (Payments)

---

## 📌 Notes

This project is **real-world oriented**, focusing on:

* Proper architectural design
* Real business rules
* Scalability and maintainability

---

## 👨‍💻 Author

**Moaz Ashour**
Backend Developer (.NET)
Clean Architecture | Real-world Systems