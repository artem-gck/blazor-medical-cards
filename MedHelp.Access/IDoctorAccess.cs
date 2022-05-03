﻿using MedHelp.Access.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelp.Access
{
    public interface IDoctorAccess
    {
        public Task<List<Doctor>> GetDoctors();
        public Task<int> UpdateDoctor(Doctor doctor);
        public Task<int> DeleteDoctor(int id);
        public Task<int> AddDoctor(Doctor doctor);
        public Task<List<Tolon>> GetTolones(int doctorId);
        public Task<int> AddTolon(Tolon tolon);
    }
}
