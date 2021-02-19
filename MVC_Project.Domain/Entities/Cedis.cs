using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Entities
{
    public class Cedis : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<User> Users { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual DateTime? RemovedAt { get; set; }
        public virtual string Uuid { get; set; }
        public virtual Boolean Status { get; set; }
        public virtual User Manager { get; set; }

        public Cedis()
        {
            Users = new List<User>();
        }
    }
}
