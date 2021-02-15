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
    public class PresentacionService : ServiceBase<Presentacion>, IPresentacionService
    {
        private IRepository<Presentacion> _repository;
        public PresentacionService(IRepository<Presentacion> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }
        public IList<Presentacion> ObtenerPresentaciones(string filtros)
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
