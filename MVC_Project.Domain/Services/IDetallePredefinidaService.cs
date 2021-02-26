using MVC_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MVC_Project.Domain.Services
{
    public interface IDetallefinidaService : IService<DetallePredefinida>
    {
        void CreateAll(IList<DetallePredefinida> items);
        Tuple<IEnumerable<DetallePredefinida>, int> FilterBy(int masterListId, NameValueCollection filtersValue, int? skip, int? take);
    }
}
