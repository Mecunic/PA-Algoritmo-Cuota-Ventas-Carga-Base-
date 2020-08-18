using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Utils;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MVC_Project.Domain.Services
{
    #region Interfaces

    public interface IUserService : IService<User>
    {
        Tuple<IEnumerable<User>, int> FilterBy(NameValueCollection filtersValue, int? skip, int? take);
    }

    #endregion Interfaces
}