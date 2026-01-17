using ClinicManagement.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Common.Errors
{

    public static class ApplicationErrors
    {
        // =========================
        // 👤 Users / Identity
        // =========================

        public static readonly Error UserNotFound =
            Error.NotFound(
                code: "User.NotFound",
                description: "User does not exist.");

        public static readonly Error UserAlreadyExists =
            Error.Conflict(
                code: "User.AlreadyExists",
                description: "A user with the same email already exists.");

        public static readonly Error UserInactive =
            Error.Conflict(
                code: "User.Inactive",
                description: "User account is inactive.");

        public static readonly Error InvalidCredentials =
            Error.Validation(
                code: "Auth.InvalidCredentials",
                description: "Invalid email or password.");

        // =========================
        // 🩺 Doctors
        // =========================

        public static readonly Error DoctorNotFound =
            Error.NotFound(
                code: "Doctor.NotFound",
                description: "Doctor does not exist.");

        public static readonly Error DoctorAlreadyExists =
            Error.Conflict(
                code: "Doctor.AlreadyExists",
                description: "Doctor profile already exists .");

        public static readonly Error DoctorInactive =
            Error.Conflict(
                code: "Doctor.Inactive",
                description: "Doctor is inactive.");

        public static readonly Error ScheduleNotFound =
           Error.Conflict(
               code: "Schedule_Not_Found",
               description: "Schedule Was Not Found.");
        


        // =========================
        // 🧑‍⚕️ Patients
        // =========================

        public static readonly Error PatientNotFound =
            Error.NotFound(
                code: "Patient.NotFound",
                description: "Patient does not exist.");

        
             public static readonly Error PatientAlreadyExists =
            Error.Conflict(
                code: "Patient.AlreadyExists",
                description: "Patient profile already exists.");

        public static readonly Error PatientInactive =
            Error.Conflict(
                code: "Patient.Inactive",
                description: "Patient is inactive.");




        // =========================
        // 📅 Appointments
        // =========================

        public static readonly Error AppointmentNotFound =
            Error.NotFound(
                code: "Appointment.NotFound",
                description: "Appointment does not exist.");

        public static readonly Error InvalidTime = Error.Failure(
       code: "Appointment.InvalidTime",
       description: "The appointment time is not valid."
   );

        public static readonly Error AppointmentInvalidStatus =
            Error.Conflict(
                code: "Appointment.InvalidStatus",
                description: "Appointment status does not allow this operation.");


        public static readonly Error InvalidAppointmentCancel =
           Error.Validation(
               code: "Appointment.Cancel.Invalid",
               description: "Appointment has session created cant cancel it.");


        public static Error DoctorNotAvailable =>
       Error.Conflict("Appointment.Doctor.NotAvailable",
           "Doctor is not available at this time");


        public static readonly Error PatientConflict = Error.Failure(
          code: "Appointment.PatientConflict",
          description: "The patient has a conflicting appointment at the same time."
      );

        public static readonly Error DoctorConflict = Error.Failure(
            code: "Appointment.DoctorConflict",
            description: "The doctor has a conflicting appointment at the same time."
        );

        public static Error AppointmentNotStartedYet =>
               Error.Validation(
        "Appointment.NotStartedYet",
        "The appointment time has not started yet.");

        public static Error AppointmentExpired =>
               Error.Validation(
                "Appointment.Expired",
                "The appointment time has already ended.");

        // =========================
        // 🧾 Sessions
        // =========================

        public static readonly Error SessionNotFound =
            Error.NotFound(
                code: "Session.NotFound",
                description: "Session does not exist.");

        public static readonly Error SessionAlreadyStarted =
            Error.Conflict(
                code: "Session.AlreadyStarted",
                description: "A session already exists for this appointment.");

        public static readonly Error SessionInvalidStartTime =
            Error.Conflict(
                code: "Session.InvalidStartTime",
                description: "Session can only start within 10 minutes of the scheduled appointment time.");

        public static Error AppointmentNotReadyForSession =>
             Error.Conflict(
                 "Appointment.NotReady",
                 "Appointment is not ready to start a session");

        public static Error SessionNotCompleted =>
            Error.Conflict(
                "Session_Not_Completed",
                "Session Not Completed Yet");
        

        // =========================
        // 🧍 Attendance
        // =========================

        public static readonly Error AttendanceAlreadyMarked =
            Error.Conflict(
                code: "Attendance.AlreadyMarked",
                description: "Attendance has already been recorded for this session.");

        public static readonly Error AttendanceInvalidSessionStatus =
            Error.Conflict(
                code: "Attendance.InvalidSessionStatus",
                description: "Attendance can only be marked for confirmed sessions.");

        public static readonly Error AttendanceNotFound =
           Error.Conflict(
               code: "Attendance.NotFound",
               description: "Attendance does not exist.");
        


        // =========================
        // 💊 Prescriptions
        // =========================

        public static readonly Error PrescriptionAlreadyExists =
            Error.Conflict(
                code: "Prescription.AlreadyExists",
                description: "A prescription already exists for this session.");

        public static readonly Error PrescriptionNotAllowed =
            Error.Conflict(
                code: "Prescription.NotAllowed",
                description: "Prescription can only be created for confirmed sessions with present patients.");

        // =========================
        // 📋 Medical Records
        // =========================

        public static readonly Error MedicalRecordNotFound =
            Error.NotFound(
                code: "MedicalRecord.NotFound",
                description: "Medical record does not exist.");


        public static readonly Error MedicalRecordAlreadyExists =
            Error.Conflict(
                code: "MedicalRecord_MedicalRecordAlreadyExists",
                description: "A MedicalRecord already exists for this session.");

        // =========================
        // 💰 Billing
        // =========================

        public static readonly Error BillAlreadyExists =
            Error.Conflict(
                code: "Billing.BillAlreadyExists",
                description: "A bill already exists for this session.");

        public static readonly Error BillNotFound =
            Error.NotFound(
                code: "Billing.BillNotFound",
                description: "Bill does not exist.");

        public static readonly Error InvalidPaymentAmount =
            Error.Validation(
                code: "Billing.InvalidPaymentAmount",
                description: "Payment amount does not match the bill amount.");

        // =========================
        // 🔐 Authorization
        // =========================

        public static readonly Error UnauthorizedAction =
            Error.Forbidden(
                code: "Authorization.Forbidden",
                description: "You are not authorized to perform this action.");
    }

}
