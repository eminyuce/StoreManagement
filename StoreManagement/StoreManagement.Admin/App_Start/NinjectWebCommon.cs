using StoreManagement.Admin.Constants;
using StoreManagement.Data;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(StoreManagement.Admin.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(StoreManagement.Admin.App_Start.NinjectWebCommon), "Stop")]

namespace StoreManagement.Admin.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using GoogleDriveUploader;

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
        private const string clientId = "660481316212-ivbld0hjqll1k1u67l1l9g67cvd88gtc.apps.googleusercontent.com";
        private const string clientSecret = "30job5lDA-fzZNP2M7b0EQuA";
        private const string folderName = "MyStore";
        private const string applicationName = "MyStoreApplicationName";
        private const string folder = "MyStoreFolder";
        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IStoreContext>().To<StoreContext>().WithConstructorArgument("nameOrConnectionString", AppConstants.ConnectionStringName);
            kernel.Bind<IContentRepository>().To<ContentRepository>();
            kernel.Bind<IStoreRepository>().To<StoreRepository>();
            kernel.Bind<ISettingRepository>().To<SettingRepository>();
            kernel.Bind<IFileManagerRepository>().To<FileManagerRepository>();
            kernel.Bind<INavigationRepository>().To<NavigationRepository>();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            kernel.Bind<IPageDesignRepository>().To<PageDesignRepository>();
            kernel.Bind<IContentFileRepository>().To<ContentFileRepository>();
            kernel.Bind<IStoreUserRepository>().To<StoreUserRepository>();
            kernel.Bind<ICompanyRepository>().To<CompanyRepository>();
            var m = kernel.Bind<IUploadHelper>().To<UploadHelper>();
            m.InSingletonScope();
            m.WithConstructorArgument("folder", folder);
            m.WithConstructorArgument("clientId", ProjectAppSettings.GetWebConfigString("ClientId",clientId));
            m.WithConstructorArgument("clientSecret", ProjectAppSettings.GetWebConfigString("ClientSecret", clientSecret));

            //m.WithConstructorArgument("clientId", clientId);
            //m.WithConstructorArgument("clientSecret", clientSecret);
            m.WithConstructorArgument("applicationName", applicationName);
            m.WithConstructorArgument("folderName", folderName);
            

            

        }
    }
}
