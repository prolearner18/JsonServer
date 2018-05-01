using JsonServer.Models;
using JsonServer.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using System;
using System.Data.Entity;
using System.Web;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;
using Unity.Lifetime;

namespace JsonServer
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterType<IDataContextAsync, JsonServer.Models.Appdata>(new PerRequestLifetimeManager());//数据上下文
           
            //container.RegisterInstance<HelpController>(new HelpController());//实例化HelpController



            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<ApplicationDbContext>(new HierarchicalLifetimeManager());

            container.RegisterType<IRoleStore<ApplicationRole, string>, RoleStore<ApplicationRole>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));
            //container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager());
            

            container.RegisterType<IRepositoryAsync<Product>, Repository<Product>>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IRepositoryAsync<Category>, Repository<Category>>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IRepositoryAsync<Order>, Repository<Order>>();
            container.RegisterType<IOrderService, OrderService>();

            container.RegisterType<IRepositoryAsync<Company>, Repository<Company>>();
            container.RegisterType<ICompanyService, CompanyService>();

            container.RegisterType<IRepositoryAsync<Department>, Repository<Department>>();
            container.RegisterType<IDepartmentService, DepartmentService>();

            container.RegisterType<IRepositoryAsync<Work>, Repository<Work>>();
            container.RegisterType<IWorkService, WorkService>();

            container.RegisterType<IRepositoryAsync<BaseCode>, Repository<BaseCode>>();
            container.RegisterType<IBaseCodeService, BaseCodeService>();
            container.RegisterType<IRepositoryAsync<CodeItem>, Repository<CodeItem>>();

        }
    }
}