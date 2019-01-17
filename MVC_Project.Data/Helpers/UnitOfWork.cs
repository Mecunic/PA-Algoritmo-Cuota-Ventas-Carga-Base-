using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using MVC_Project.Data.Mappings;
using MVC_Project.Domain.Helpers;

namespace MVC_Project.Data.Helpers {

    public class UnitOfWork : IUnitOfWork {
        private static readonly ISessionFactory _sessionFactory;
        private static Configuration configuration;
        private ITransaction _transaction;

        public ISession Session { get; set; }

        static UnitOfWork() {
            _sessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.ConnectionString(conection => conection.FromConnectionStringWithKey("testConectionString")))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .ExposeConfiguration(cfg => configuration = cfg)
                .BuildSessionFactory();
        }

        public UnitOfWork() {
            Session = _sessionFactory.OpenSession();
             var export = new SchemaExport(configuration);
            export.Execute(true, true, false, Session.Connection, null);
        }

        public void BeginTransaction() {
            _transaction = Session.BeginTransaction();
        }

        public void Commit() {
            try {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Commit();
            } catch {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();

                throw;
            } finally {
                Session.Dispose();
            }
        }

        public void Rollback() {
            try {
                if (_transaction != null && _transaction.IsActive)
                    _transaction.Rollback();
            } finally {
                Session.Dispose();
            }
        }
    }
}