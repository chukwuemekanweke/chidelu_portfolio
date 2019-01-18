using System;
using System.Collections.Generic;
using System.Text;

namespace BoilerPlate.DataLayer.Interface
{
    public interface IRepositoryReadOnly<T> : IReadRepository<T> where T : class
    {

    }
}
