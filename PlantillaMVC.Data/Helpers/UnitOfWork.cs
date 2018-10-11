using PlantillaMVC.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantillaMVC.Data.Helpers {

    public class UnitOfWork : IUnitOfWork {

        public void BeginTransaction() {
            throw new NotImplementedException();
        }

        public void Commit() {
            throw new NotImplementedException();
        }

        public void Rollback() {
            throw new NotImplementedException();
        }
    }
}