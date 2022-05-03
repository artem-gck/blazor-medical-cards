﻿using MedHelp.Access.Context;
using MedHelp.Access.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelp.Access.SqlServer
{
    public class DoctorAccess : IDoctorAccess
    {
        private readonly MedHelpContext _medHelpContext;

        public DoctorAccess(MedHelpContext medHelpContext)
        {
            _medHelpContext = medHelpContext;
        }

        public async Task<List<Doctor>> GetDoctors()
        {
            return await _medHelpContext.Doctors.Include(doc => doc.Tolons)
                                                .Include(doc => doc.User)
                                                .ToListAsync();
        }

        public async Task<int> UpdateDoctor(Doctor doctor)
        {
            var doc = await _medHelpContext.Doctors.Include(doc => doc.User)
                                                   .FirstOrDefaultAsync(doc => doc.DoctorId == doctor.DoctorId);

            doc.User.Login = doctor.User.Login;
            doc.User.Password = doctor.User.Password;
            doc.Name = doctor.Name;
            doc.FirstName = doctor.FirstName;
            doc.LastName = doctor.LastName;
            doc.NumberOfPhone = doctor.NumberOfPhone;
            doc.Specialization = doctor.Specialization;

            var id = await _medHelpContext.SaveChangesAsync();

            return id;
        }

        public async Task<int> DeleteDoctor(int id)
        {
            var doc = await _medHelpContext.Doctors.Include(doc => doc.Tolons)
                                                   .Include(doc => doc.User)
                                                   .FirstOrDefaultAsync(doc => doc.DoctorId == id);

            var docDb = _medHelpContext.Users.Remove(doc.User);
            await _medHelpContext.SaveChangesAsync();

            return docDb.Entity.UserId;
        }

        public async Task<int> AddDoctor(Doctor doctor)
        {
            var doc = await _medHelpContext.Doctors.AddAsync(doctor);

            await _medHelpContext.SaveChangesAsync();

            return doc.Entity.DoctorId;
        }

        public async Task<List<Tolon>> GetTolones(int doctorId)
        {
            return await _medHelpContext.Tolons.Include(t => t.Doctor)
                                               .Include(t => t.Patient)
                                               .ThenInclude(p => p.Sex)
                                               .Where(t => t.Doctor.DoctorId == doctorId)
                                               .ToListAsync();
        }

        public async Task<int> AddTolon(Tolon tolon)
        {
            var doctor = await _medHelpContext.Doctors.FindAsync(tolon.Doctor.DoctorId);
            var patient = await _medHelpContext.Patients.FindAsync(tolon.Patient.PatientId);

            tolon.Doctor = doctor;
            tolon.Patient = patient;

            var to = await _medHelpContext.Tolons.AddAsync(tolon);

            await _medHelpContext.SaveChangesAsync();

            return to.Entity.TolonId;
        }
    }
}
