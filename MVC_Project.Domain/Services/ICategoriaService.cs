using MVC_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Services
{
    public interface ICategoriaService : IService<Categoria>
    {
        IList<Categoria> ObtenerCategorias(string filtros);
    }
}
