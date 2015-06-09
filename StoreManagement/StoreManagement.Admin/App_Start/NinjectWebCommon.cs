using StoreManagement.Data;
using StoreManagement.Data.EmailHelper;
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
    using System.Web.Hosting;
    using StoreManagement.Data.GeneralHelper;

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
        private const string clientId = "660481316212-aietulh54ei2eqsi1gdvl0g7s12ohf70.apps.googleusercontent.com";
        private const string serviceAccountPkCs12FilePath = @"~\Content\Google Drive File Upload-2db8e7cde3bb.p12";
        private const string serviceAccountEmail = "660481316212-aietulh54ei2eqsi1gdvl0g7s12ohf70@developer.gserviceaccount.com";
        private const string folder = "MyStoreFolder";
        private const String password = "notasecret";
        private const String userEmail = "eminyuce@gmail.com";
        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IStoreContext>().To<StoreContext>().WithConstructorArgument("nameOrConnectionString", "Stores");
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
            m.WithConstructorArgument("clientId", ProjectAppSettings.GetWebConfigString("ClientId", clientId));
            m.WithConstructorArgument("userEmail", ProjectAppSettings.GetWebConfigString("UserEmail",userEmail));
            m.WithConstructorArgument("serviceAccountEmail",  ProjectAppSettings.GetWebConfigString("ServiceAccountEmail",serviceAccountEmail));
            m.WithConstructorArgument("certificate", GeneralHelper.CreateCert(HostingEnvironment.MapPath(serviceAccountPkCs12FilePath), password));
            m.WithConstructorArgument("folderName", ProjectAppSettings.GetWebConfigString("GoogleDriveFolder",folder));
            m.WithConstructorArgument("password",  ProjectAppSettings.GetWebConfigString("password",password));




            kernel.Bind<IEmailSender>().To<EmailSender>();

        }
    }
}
