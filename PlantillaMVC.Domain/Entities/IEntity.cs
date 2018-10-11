using System;
using System.Collections.Generic;
using System.Text;

namespace PlantillaMVC.Domain.Entities {

    public interface IEntity {
        int Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        DateTime RemovedAt { get; set; }
    }
}