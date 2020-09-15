using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Helpers;
using MVC_Project.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Services
{
    public interface IOrderService : IService<Order>
    {
        IList<Order> FilterBy(string filtros, int? skip, int? take);
        IList<Store> FilterStore();
        IList<Staff> FilterStaff();
        IList<OrderItems> OrdenDetail(int orderId);
        int TotalFilterBy(string filtros, int? skip, int? take);
    }   
}