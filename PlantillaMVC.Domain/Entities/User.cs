using System;
using System.Collections.Generic;
using System.Text;

namespace PlantillaMVC.Domain.Entities {

    public class User : IEntity {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual DateTime RemovedAt { get; set; }
    }
}