using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace MVC_Project.Data.Services
{
    public class ServiceBase<M> : IService<M> where M : IEntity
    {
        private IRepository<M> _baseRepository;

        public ServiceBase(IRepository<M> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public void Create(M entity)
        {
            _baseRepository.Create(entity);
        }

        public void Delete(int id)
        {
            _baseRepository.Delete(id);
        }

        public virtual Tuple<IEnumerable<M>, int> FilterBy(NameValueCollection filtersValue, int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<M> FindBy(Expression<Func<M, bool>> predicate)
        {
            return _baseRepository.FindBy(predicate);
        }

        public IEnumerable<M> GetAll()
        {
            return _baseRepository.GetAll().ToList();
        }

        public M GetById(int id)
        {
            return _baseRepository.GetById(id);
        }

        public M GetByUuid(string uuid)
        {
            return _baseRepository.GetByUuid(uuid);
        }

        public void Update(M entity)
        {
            _baseRepository.Update(entity);
        }

        
    }
}
