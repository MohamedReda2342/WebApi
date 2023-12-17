﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Services
{

    public interface IPatientService
    {
        // Existing patient-related methods
        List<PatientResponse> GetPatientsByUserId(int userId);
        PatientResponse GetPatientById(int userId, int patientId);
        void AddPatient(int userId, PatientAddRequest model);
        void UpdatePatient(int userId, int patientId, PatientUpdateRequest model);
        void DeletePatient(int userId, int patientId);

        // Medicine-related methods
        List<MedicineResponse> GetMedicinesByPatientId(int userId, int patientId);
        MedicineResponse GetMedicineById(int userId, int patientId, int medicineId);
        void AddMedicine(int userId, int patientId, MedicineAddRequest model);
        void UpdateMedicine(int userId, int patientId, int medicineId, MedicineUpdateRequest model);
        void DeleteMedicine(int userId, int patientId, int medicineId);
        void UpdateBand(int userId, int patientId, BandData model);
    }


    public class PatientServices : IPatientService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PatientServices(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<PatientResponse> GetPatientsByUserId(int userId)
        {
            var user = getUser(userId);

            // Map Patient entities to PatientResponse objects
            var patientResponses = _mapper.Map<List<PatientResponse>>(user.Patients);

            return patientResponses;
        }

        public PatientResponse GetPatientById(int userId, int patientId)
        {
            var user = getUser(userId);
            var patient = getPatient(user, patientId);
            // Map Patient entity to PatientResponse object
            var patientResponse = _mapper.Map<PatientResponse>(patient);

            return patientResponse;
        }

        public void AddPatient(int userId, PatientAddRequest model)
        {

            var user = getUser(userId);
            //Check if the patient with the same details already exists
            if (user.Patients.Any(p => p.PhoneNumber == model.PhoneNumber))
            {
                // Patient with the same details already exists
                throw new ApplicationException("Patient with the same details already exists.");
            }
            var patient = _mapper.Map<Patient>(model);
            user.Patients.Add(patient);
            _context.SaveChanges();
        }

        public void UpdatePatient(int userId, int patientId, PatientUpdateRequest model)
        {
            var user = getUser(userId);
            var patient = getPatient(user, patientId);

            // Update patient properties if provided, otherwise keep the existing values
            patient.Name = model.Name ?? patient.Name;
            patient.Age = model.Age ?? patient.Age;
            patient.Gender = model.Gender ?? patient.Gender;
            patient.PhoneNumber = model.PhoneNumber ?? patient.PhoneNumber;
            patient.Illness = model.Illness ?? patient.Illness;

            _context.SaveChanges();
        } 

        public void UpdateBand(int userId, int patientId, BandData model)
        {
            var user = getUser(userId);
            var patient = getPatient(user, patientId);
            // New properties in the PatientUpdateRequest
            patient.Temperature = model.Temperature ?? patient.Temperature;
            patient.O2 = model.O2 ?? patient.O2;
            patient.HeartRate = model.HeartRate ?? patient.HeartRate;
            patient.Longitude = model.Longitude ?? patient.Longitude;
            patient.Latitude = model.Latitude ?? patient.Latitude;

            _context.SaveChanges();
        }

        public void DeletePatient(int userId, int patientId)
        {
            var user = getUser(userId);
            var patient = getPatient(user, patientId);

            user.Patients.Remove(patient);
            _context.SaveChanges();

        }

        //------------------------------------------------ ...CRUD operations for Medicine... ------------------------------------------------

        public List<MedicineResponse> GetMedicinesByPatientId(int userId, int patientId)
        {
            var user = getUser(userId);
            var patient = getPatient(user, patientId);
            var medicineResponses = _mapper.Map<List<MedicineResponse>>(patient.Medicines);

            return medicineResponses;
        }

        public MedicineResponse GetMedicineById(int userId, int patientId, int medicineId)
        {
            var user = getUser(userId);
            var patient = getPatient(user, patientId);
            var medicineResponses = _mapper.Map<MedicineResponse>(getMedicine(patient, medicineId));
            return medicineResponses;
        }

        public void AddMedicine(int userId, int patientId, MedicineAddRequest model)
        {
            var user = getUser(userId);
            var patient = getPatient(user, patientId);

            if (patient != null)
            {
                var medicine = _mapper.Map<Medicine>(model);
                patient.Medicines.Add(medicine);
            }
            else
            {
                // Handle the case where patient is null
                throw new ArgumentNullException(nameof(patient));
            }
                _context.SaveChanges();
        }
        

        public void UpdateMedicine(int userId, int patientId, int medicineId, MedicineUpdateRequest model)
        {
            var user = getUser(userId);
            var patient = getPatient(user, patientId);
            var medicine = getMedicine(patient, medicineId);

            //// Use AutoMapper to map the properties from the DTO to the entity
            //_mapper.Map(model, medicine);
            if (medicine == null)
            {
                throw new ArgumentNullException(nameof(patient));

            }
            medicine.MedicineName = model.MedicineName ?? medicine.MedicineName;
            medicine.Date = model.Date ?? medicine.Date;
            medicine.Time = model.Time ?? medicine.Time;
            medicine.Repeat = model.Repeat ?? medicine.Repeat;
            medicine.Reminder = model.Reminder ?? medicine.Reminder; 
            _context.SaveChanges();
        }

        public void DeleteMedicine(int userId, int patientId, int medicineId)
        {
            var user = getUser(userId);
            var patient = getPatient(user, patientId);
            var medicine = getMedicine(patient, medicineId);

            patient.Medicines.Remove(medicine);
            _context.SaveChanges();
        }

        // Helper method for getting Medicine
        private Medicine getMedicine(Patient patient, int medicineId)
        {
            var medicine = patient.Medicines.FirstOrDefault(m => m.MedicineId == medicineId);
            if (medicine == null) throw new KeyNotFoundException("Medicine not found");
            return medicine;
        }



        //------------------------------------------------ ...Helper method... ------------------------------------------------

        private User getUser(int userId)
        {
            var user = _context.Users
                .Include(u => u.Patients).ThenInclude(p => p.Medicines) // Make sure Patients are loaded
                .FirstOrDefault(u => u.Id == userId);

            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

        private Patient getPatient(User user, int patientId)
        {
            //var patient = user.Patients.FirstOrDefault(p => p.PatientId == patientId);

            var patient = _context.Users
                .Where(u => u.Id == user.Id)  // Ensure we are working with the correct user
                .SelectMany(u => u.Patients)  // Flatten the Patients list
                .Include(p => p.Medicines)    // Eager load the Medicines property
                .FirstOrDefault(p => p.PatientId == patientId);

            if (patient == null) throw new KeyNotFoundException("Patient not found");
            return patient;
        }


//-------------------------------------------------------------------------------------


    }
}
