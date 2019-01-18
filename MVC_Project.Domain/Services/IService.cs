using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Services
{
    public interface IService<M>
    {
        IEnumerable<M> GetAll();

        M GetById(int id);

        void Create(M entity);

        void Update(M entity);

        void Delete(int id);
    }
}
