using MVC_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Services
{
    public interface IProductoService : IService<Producto>
    {
        IList<Producto> ObtenerProductos(string filtros);
    }
}
