using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Data.Services
{
    public class CategoriaService : ServiceBase<Categoria>, ICategoriaService
    {
        private IRepository<Categoria> _repository;
        public CategoriaService(IRepository<Categoria> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }
        public IList<Categoria> ObtenerCategorias(string filtros)
        {
            filtros = filtros.Replace("[", "").Replace("]", "").Replace("\\", "").Replace("\"", "");
            var filters = filtros.Split(',').ToList();

            var result = _repository.GetAll();
            if (!string.IsNullOrWhiteSpace(filters[0]))
            {
                string nombre = filters[0];
                result = result.Where(p => p.Name.ToLower().Contains(nombre.ToLower()));
            }
            return result.ToList();
        }
    }
}
