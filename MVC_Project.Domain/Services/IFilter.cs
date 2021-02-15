using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Services
{
    public interface IFilter<T>
    {
        Tuple<IEnumerable<T>, int> FilterBy(NameValueCollection filtersValue, int? skip, int? take);
    }
}
