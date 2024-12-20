﻿using Crematory.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Interfaces
{
    public interface IContactPersonRepository
    {
        Task<bool> InsertContactPersonAsync(ContactPersonModel contactPerson);
        Task<bool> DeleteContactPersonAsync(int id);
        Task<int> GetContactPersonIdAsync(ContactPersonModel contactPerson);
        Task<ContactPersonModel> GetContactPersonById(int contactPerson);
    }
}
