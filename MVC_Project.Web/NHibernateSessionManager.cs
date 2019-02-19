using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MVC_Project.Data.Mappings;
using MVC_Project.Domain.Entities;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Project.Web
{
    public class NHibernateSessionManager
    {
        public static ISession GetSession()
        {
            ISession session = (ISession)HttpContext.Current.Items["NHibernateSession"];
            if (session == null)
            {
                session = NHibernateHelper.OpenSession(); // Create session, like SessionFactory.createSession()...
                HttpContext.Current.Items.Add("NHibernateSession", session);
            }

            return session;
        }

        public static void CloseSession()
        {
            ISession session = (ISession)HttpContext.Current.Items["NHibernateSession"];
            if (session != null)
            {
                if (session.IsOpen)
                {
                    session.Close();
                }
                HttpContext.Current.Items.Remove("NHibernateSession");
            }
        }
    }

    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    string decryptedConnectionString = string.Empty;
                   
                    decryptedConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["testConectionString"].ConnectionString;
                    

                    _sessionFactory = Fluently.Configure()
                        .Database(MsSqlConfiguration.MsSql2012
                        //.ShowSql()
                        .ConnectionString(decryptedConnectionString))
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<RoleMap>())
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PermissionMap>())
                        .BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}