using System;
using System.Collections.Generic;
using System.Text;

namespace MVC_Project.Domain.Helpers {

    public interface IUnitOfWork : IDisposable
    {

        void BeginTransaction();

        void Commit();

        void Rollback();

        void Dispose();
    }
}