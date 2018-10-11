using System;
using System.Collections.Generic;
using System.Text;

namespace PlantillaMVC.Domain.Helpers {

    public interface IUnitOfWork {

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}