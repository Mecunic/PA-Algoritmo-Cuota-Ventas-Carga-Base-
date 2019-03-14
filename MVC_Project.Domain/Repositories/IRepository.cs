using MVC_Project.Domain.Entities;
using NHibernate;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace MVC_Project.Domain.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        ISession Session { get; }

        IQueryable<T> GetAll();

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        T GetById(int id);

        void Create(T entity);

        void Update(T entity);

        void Delete(int id);
    }
}