using MVC_Project.Data.Helpers;
using MVC_Project.Data.Repositories;
using MVC_Project.Domain.Helpers;
using MVC_Project.Domain.Repositories;
using MVC_Project.Domain.Services;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace MVC_Project.Desktop
{
    static class Program
    {
        //#region Unity Container
        //private static Lazy<IUnityContainer> container =
        //  new Lazy<IUnityContainer>(() =>
        //  {
        //      var container = new UnityContainer();
        //      RegisterTypes(container);
        //      return container;
        //  });
        //public static IUnityContainer Container => container.Value;
        //#endregion

        static IUnityContainer container;

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RegisterTypes();
            //Application.Run(new LoginForm());
            Application.Run(new LoginForm(container.Resolve<IAuthService>()));
            //Application.Run(container.Resolve<LoginForm>());
            //Application.Run(container);
        }
        public static void RegisterTypes()
        {
            container = new UnityContainer();
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerThreadLifetimeManager());
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
            container.RegisterType(typeof(IService<>), typeof(ServiceBase<>));
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IAuthService, AuthService>();
            //container.RegisterType<IAuthService>(new InjectionProperty("AuthService", new AuthService()));
            container.RegisterType<IRoleService, RoleService>();
        }
        
        public static void RegisterTypes(IUnityContainer container)
        {
            //container = new UnityContainer();
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
            container.RegisterType(typeof(IService<>), typeof(ServiceBase<>));
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IAuthService, AuthService>();
            container.RegisterType<IRoleService, RoleService>();
        }
    }
}
