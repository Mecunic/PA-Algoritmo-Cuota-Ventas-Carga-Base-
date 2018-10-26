using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using PlantillaMVC.Data.Mappings;
using PlantillaMVC.Domain.Entities;
using PlantillaMVC.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Reflection;
using System.Text;

namespace PlantillaMVC.Data.Helpers {

    public class UnitOfWork : IUnitOfWork {
        private static readonly ISessionFactory _sessionFactory;
        private ITransaction _transaction;

        public ISession Session { get; set; }

        static UnitOfWork() {
            _sessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .BuildSessionFactory();
        }

        public UnitOfWork() {
            Session = _sessionFactory.OpenSession();
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