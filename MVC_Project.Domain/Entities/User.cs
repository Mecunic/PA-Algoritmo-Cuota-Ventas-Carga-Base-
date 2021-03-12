using System;
using System.Collections.Generic;
using System.Text;

namespace MVC_Project.Domain.Entities
{

    public class User : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Uuid { get; set; }
        public virtual string Nombre { get; set; }
        public virtual string Usuario { get; set; }
        public virtual Role Role { get; set; }
        public virtual IList<Permission> Permissions { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual DateTime? RemovedAt { get; set; }
        public virtual bool Status { get; set; }
        public virtual int IdCedis { get; set; }

        public User()
        {
            Permissions = new List<Permission>();
        }
    }
}