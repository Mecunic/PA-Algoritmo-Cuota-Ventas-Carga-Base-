using NHibernate;
using MVC_Project.Data.Helpers;
using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Helpers;
using MVC_Project.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NHibernate.Criterion;

namespace MVC_Project.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : IEntity
    {
        private UnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
        }

        public ISession Session { get { return _unitOfWork.Session; } }

        public void Create(T entity)
        {
            Session.Save(entity);
            Session.Flush();
        }

        public void Delete(int id)
        {
            Session.Delete(Session.Load<T>(id));
            Session.Flush();
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return Session.Query<T>().Where(predicate).ToList<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return Session.Query<T>().ToList<T>();
        }

        public T GetById(int id)
        {
            return Session.Get<T>(id);
        }

        public T GetByUuid(string uuid)
        {
            ICriteria criteria = Session.CreateCriteria<IEntity>();
            IDictionary<string, object> attributes = new Dictionary<string, object>();
            attributes.Add("Uuid", uuid);
            return Find(attributes);
        }

        public void Update(T entity)
        {
            Session.Update(entity);
            Session.Flush();
        }

        public IEnumerable<T> FindAll(IDictionary<string, object> attributes)
        {
            ICriteria criteria = Session.CreateCriteria<IEntity>();
            Dictionary<string, string> aliasCreated = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> attribute in attributes)
            {
                string key = attribute.Key;
                IList<string> elements = key.Split('.').ToList();
                string mainAttribute;
                if (elements.Count > 1)
                {
                    mainAttribute = elements.Last();
                    elements.Remove(elements.Last());
                    StringBuilder alias = new StringBuilder();
                    StringBuilder innerAttr = new StringBuilder();
                    foreach (string element in elements)
                    {
                        innerAttr.Append(element);
                        alias.Append("_");
                        alias.Append(element);
                        if (!aliasCreated.ContainsKey(alias.ToString()))
                        {
                            aliasCreated.Add(alias.ToString(), innerAttr.ToString());
                            criteria.CreateAlias(innerAttr.ToString(), alias.ToString());
                        }

                        innerAttr.Clear();
                        innerAttr.Append(alias.ToString());
                        innerAttr.Append(".");
                    }
                    innerAttr.Append(mainAttribute);
                    criteria.Add(Restrictions.Eq(innerAttr.ToString(), attribute.Value));
                }
                else
                {
                    if (attribute.Value == null)
                    {
                        criteria.Add(NHibernate.Criterion.Expression.IsNull(attribute.Key));
                    }
                    else
                    {
                        criteria.Add(Restrictions.Eq(attribute.Key, attribute.Value));
                    }
                }
            }
            return criteria.List<T>();
        }

        public T Find(IDictionary<string, object> attributes)
        {
            IList<T> data = this.FindAll(attributes).ToList();
            if (data != null)
            {
                if (data.Count() > 0)
                {
                    return data.First();
                }
            }
            return default(T);
        }

    }
}