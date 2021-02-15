﻿using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Data.Services
{
    public class CedisService : ServiceBase<Cedis>, ICedisService
    {
        private IRepository<Cedis> _repository;
        public CedisService(IRepository<Cedis> baseRepository) : base(baseRepository)
        {
            _repository = baseRepository;
        }
        public IList<Cedis> ObtenerCedis(string filtros)
        {
            filtros = filtros.Replace("[", "").Replace("]", "").Replace("\\", "").Replace("\"", "");
            var filters = filtros.Split(',').ToList();

            var cedis = _repository.GetAll();
            if (!string.IsNullOrWhiteSpace(filters[0]))
            {
                string nombre = filters[0];
                cedis = cedis.Where(p => p.Name.ToLower().Contains(nombre.ToLower()));
            }
            return cedis.ToList();
        }
    }
}