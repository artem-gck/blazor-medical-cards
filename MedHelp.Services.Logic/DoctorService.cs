﻿using MedHelp.Access;
using MedHelp.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelp.Services.Logic
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorAccess _doctorAccess;

        public DoctorService(IDoctorAccess doctorAccess)
        {
            _doctorAccess = doctorAccess;
        }

        public async Task<List<Doctor>> GetDoctors()
        {
            var doctors = await _doctorAccess.GetDoctors();

            return doctors.Select(doc => MapDoctorToMod(doc)).ToList();
        }

        public async Task<int> UpdateDoctor(Doctor doctor)
        {
            return await _doctorAccess.UpdateDoctor(MapDoctorToEnt(doctor));
        }

        public async Task<int> DeleteDoctor(int id)
        {
            return await _doctorAccess.DeleteDoctor(id);
        }

        public Task<int> AddDoctor(Doctor doctor)
        {
            return _doctorAccess.AddDoctor(MapDoctorToEnt(doctor));
        }

        public async Task<List<Tolon>> GetTolones(int doctorId)
        {
            var tolones = await _doctorAccess.GetTolones(doctorId);

            return tolones.Select(t => MapTolonToMod(t)).ToList();
        }

        public async Task<int> AddTolon(Tolon tolon)
        {
            return await _doctorAccess.AddTolon(MapTolonToEnt(tolon));
        }

        private Doctor MapDoctorToMod(Access.Entity.Doctor doctor)
        {
            var tolons = doctor.Tolons.Select(t => new Tolon()
            {
                TolonId = t.TolonId
            }).ToList();

            var user = new User()
            {
                UserId = doctor.User.UserId,
                Login = doctor.User.Login,
                Password = doctor.User.Password,
            };

            var doctorMod = new Doctor()
            {
                DoctorId = doctor.DoctorId,
                Name = doctor.Name,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                NumberOfPhone = doctor.NumberOfPhone,
                Specialization = doctor.Specialization,
                Tolons = tolons,
                User = user
            };

            return doctorMod;
        }

        private Access.Entity.Doctor MapDoctorToEnt(Doctor doctor)
        {
            var tolons = doctor.Tolons.Select(t => new Access.Entity.Tolon()
            {
                TolonId = t.TolonId
            }).ToList();

            var user = new Access.Entity.User()
            {
                UserId = doctor.User.UserId,
                Login = doctor.User.Login,
                Password = doctor.User.Password,
            };

            var doctorEnt = new Access.Entity.Doctor()
            {
                DoctorId = doctor.DoctorId,
                Name = doctor.Name,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                NumberOfPhone = doctor.NumberOfPhone,
                Specialization = doctor.Specialization,
                Tolons = tolons,
                User = user
            };

            return doctorEnt;
        }

        private Tolon MapTolonToMod(Access.Entity.Tolon tolon)
        {
            var doctorMod = new Doctor()
            {
                DoctorId = tolon.Doctor.DoctorId,
                Name = tolon.Doctor.Name,
                FirstName = tolon.Doctor.FirstName,
                LastName = tolon.Doctor.LastName,
                NumberOfPhone = tolon.Doctor.NumberOfPhone,
                Specialization = tolon.Doctor.Specialization,
            };

            var patientMod = new Patient()
            {
                PatientId = tolon.Patient.PatientId,
                Name = tolon.Patient.Name,
                FirstName = tolon.Patient.FirstName,
                LastName = tolon.Patient.LastName,
                NumberOfPhone = tolon.Patient.NumberOfPhone,
                DateOfDirth = tolon.Patient.DateOfDirth,
            };

            var tolonMod = new Tolon()
            {
                TolonId = tolon.TolonId,
                Patient = patientMod,
                Doctor = doctorMod,
                Time = tolon.Time,
            };

            return tolonMod;
        }

        private Access.Entity.Tolon MapTolonToEnt(Tolon tolon)
        {
            var doctorMod = new Access.Entity.Doctor()
            {
                DoctorId = tolon.Doctor.DoctorId,
                Name = tolon.Doctor.Name,
                FirstName = tolon.Doctor.FirstName,
                LastName = tolon.Doctor.LastName,
                NumberOfPhone = tolon.Doctor.NumberOfPhone,
                Specialization = tolon.Doctor.Specialization,
            };

            var patientMod = new Access.Entity.Patient()
            {
                PatientId = tolon.Patient.PatientId,
                Name = tolon.Patient.Name,
                FirstName = tolon.Patient.FirstName,
                LastName = tolon.Patient.LastName,
                NumberOfPhone = tolon.Patient.NumberOfPhone,
                DateOfDirth = tolon.Patient.DateOfDirth
            };

            var tolonMod = new Access.Entity.Tolon()
            {
                TolonId = tolon.TolonId,
                Patient = patientMod,
                Doctor = doctorMod,
                Time = tolon.Time,
            };

            return tolonMod;
        }
    }
}