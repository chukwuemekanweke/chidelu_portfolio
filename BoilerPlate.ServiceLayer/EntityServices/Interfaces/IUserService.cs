using BoilerPlate.ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoilerPlate.ServiceLayer.EntityServices.Interfaces
{
    public interface  IUserService
    {
        USER CreateUser(string email, string username, string phoneNumber, string identityUserId);
    }
}
