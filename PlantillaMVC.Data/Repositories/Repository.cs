using NHibernate;
using PlantillaMVC.Data.Helpers;
using PlantillaMVC.Domain.Entities;
using PlantillaMVC.Domain.Helpers;
using PlantillaMVC.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PlantillaMVC.Data.Repositories {

    public class Repository<T> : IRepository<T> where T : IEntity {
        private UnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork) {
            _unitOfWork = (UnitOfWork)unitOfWork;
        }

        protected ISession Session { get { return _unitOfWork.Session; } }

        public void Create(T entity) {
            Session.Save(entity);
        }

        public void Delete(int id) {
            Session.Delete(Session.Load<T>(id));
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate) {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll() {
            return Session.Query<T>();
        }

        public T GetById(int id) {
            return Session.Get<T>(id);
        }

        public void Update(T entity) {
            Session.Update(entity);
        }
    }
}