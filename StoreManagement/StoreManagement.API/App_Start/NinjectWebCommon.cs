using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(StoreManagement.API.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(StoreManagement.API.App_Start.NinjectWebCommon), "Stop")]

namespace StoreManagement.API.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IStoreContext>().To<StoreContext>().WithConstructorArgument("nameOrConnectionString","Stores");
            kernel.Bind<IContentRepository>().To<ContentRepository>();
            kernel.Bind<IStoreRepository>().To<StoreRepository>();
            kernel.Bind<ISettingRepository>().To<SettingRepository>();
            kernel.Bind<IFileManagerRepository>().To<FileManagerRepository>();
            kernel.Bind<INavigationRepository>().To<NavigationRepository>();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            kernel.Bind<IPageDesignRepository>().To<PageDesignRepository>();
            kernel.Bind<IContentFileRepository>().To<ContentFileRepository>();

            kernel.Bind<IStoreUserRepository>().To<StoreUserRepository>();

            kernel.Bind<ILabelRepository>().To<LabelRepository>();
            kernel.Bind<ILabelLineRepository>().To<LabelLineRepository>();

            kernel.Bind<IProductRepository>().To<ProductRepository>();
            kernel.Bind<IProductFileRepository>().To<ProductFileRepository>();
            kernel.Bind<IProductCategoryRepository>().To<ProductCategoryRepository>();

            kernel.Bind<IEmailListRepository>().To<EmailListRepository>();
            kernel.Bind<IContactRepository>().To<ContactRepository>();
            kernel.Bind<ILocationRepository>().To<LocationRepository>();
            kernel.Bind<IBrandRepository>().To<BrandRepository>();
            kernel.Bind<IStoreLanguageRepository>().To<StoreLanguageRepository>();
            kernel.Bind<ILogRepository>().To<LogRepository>();
            kernel.Bind<IItemFileRepository>().To<ItemFileRepository>();
 

        }        
    }
}
