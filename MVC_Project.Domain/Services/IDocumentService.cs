using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Repositories;
using MVC_Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MVC_Project.Domain.Services
{
    public interface IDocumentService : IService<Document>
    {
        Tuple<IEnumerable<Document>, int> FilterBy(NameValueCollection filtersValue, int? skip, int? take);
    }
    
}
