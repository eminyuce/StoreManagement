using StoreManagement.Data;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Liquid.Constants;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Services;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(StoreManagement.Liquid.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(StoreManagement.Liquid.App_Start.NinjectWebCommon), "Stop")]

namespace StoreManagement.Liquid.App_Start
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

            var isAPIService = ProjectAppSettings.GetWebConfigBool("IsApiService", true);
            if (isAPIService)
            {
                var webServiceAddress = ProjectAppSettings.GetWebConfigString("WebServiceAddress", "localhost:8164");
                var service1 = kernel.Bind<IContentService>().To<ContentService>();
                service1.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service2 = kernel.Bind<IStoreService>().To<StoreService>();
                service2.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service3 = kernel.Bind<ISettingService>().To<SettingService>();
                service3.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service4 = kernel.Bind<IFileManagerService>().To<FileManagerService>();
                service4.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service5 = kernel.Bind<ICategoryService>().To<CategoryService>();
                service5.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service6 = kernel.Bind<IPageDesignService>().To<PageDesignService>();
                service6.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service7 = kernel.Bind<IContentFileService>().To<ContentFileService>();
                service7.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service8 = kernel.Bind<IStoreUserService>().To<StoreUserService>();
                service8.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service10 = kernel.Bind<INavigationService>().To<NavigationService>();
                service10.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service11 = kernel.Bind<IProductService>().To<ProductService>();
                service11.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service12 = kernel.Bind<IProductFileService>().To<ProductFileService>();
                service12.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service13 = kernel.Bind<IProductCategoryService>().To<ProductCategoryService>();
                service13.WithConstructorArgument("webServiceAddress", webServiceAddress);

            }
            else
            {

                kernel.Bind<IStoreContext>().To<StoreContext>().WithConstructorArgument("nameOrConnectionString", AppConstants.ConnectionStringName);
                kernel.Bind<IContentService>().To<ContentRepository>();
                var m = kernel.Bind<IStoreService>().To<StoreRepository>();
                kernel.Bind<ISettingService>().To<SettingRepository>();
                kernel.Bind<IFileManagerService>().To<FileManagerRepository>();
                kernel.Bind<ICategoryService>().To<CategoryRepository>();
                kernel.Bind<IPageDesignService>().To<PageDesignRepository>();
                kernel.Bind<IContentFileService>().To<ContentFileRepository>();
                kernel.Bind<IStoreUserService>().To<StoreUserRepository>();
 
                kernel.Bind<INavigationService>().To<NavigationRepository>();
                kernel.Bind<IProductService>().To<ProductRepository>();
                kernel.Bind<IProductFileService>().To<ProductFileRepository>();
                kernel.Bind<IProductCategoryService>().To<ProductCategoryRepository>();
                kernel.Bind<ILocationService>().To<LocationRepository>();
                kernel.Bind<IBrandService>().To<BrandRepository>();
            }
            kernel.Bind<IEmailSender>().To<EmailSender>();

        }        
    }
}
