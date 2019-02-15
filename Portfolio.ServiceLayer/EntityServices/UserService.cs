using BoilerPlate.DataLayer.Interface;
using BoilerPlate.ModelLayer.Entity;
using BoilerPlate.ServiceLayer.EntityServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoilerPlate.ServiceLayer.EntityServices
{
    public class UserService : IUserService
    {

        IUnitOfWork UnitOfWork { get; set; }
        public UserService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }


        public USER CreateUser(string email, string username, string phoneNumber, string identityUserId)
        {
            USER user = new USER
            {
                EmailAddress = email,
                IdentityUserId = identityUserId,
                PhoneNumber = phoneNumber,
                Username = username,
            };
            return UnitOfWork.GetRepository<USER>().Add(user);

        }
    }
}
