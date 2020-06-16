using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Project.Domain.Entities
{
    public class Document : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Uuid { get; set; }
        public virtual string URL { get; set; }

        public virtual string Type { get; set; }
        
        //public virtual string Comments { get; set; }
        public virtual string Name { get; set; }
        
        public virtual DateTime CreationDate { get; set; }
        
        public virtual User User { get; set; }
    }
}
