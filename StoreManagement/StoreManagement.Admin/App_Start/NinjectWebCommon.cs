using GenericRepository;
using GenericRepository.EntityFramework;
using Quartz;
using StoreManagement.Admin.ScheduledTasks;
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


        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IStoreContext>().To<StoreContext>().WithConstructorArgument("nameOrConnectionString", "Stores");
           
            kernel.Bind<IContentRepository>().To<ContentRepository>().InRequestScope(); 
            kernel.Bind<IStoreRepository>().To<StoreRepository>().InRequestScope(); 
            kernel.Bind<ISettingRepository>().To<SettingRepository>().InRequestScope(); 
            kernel.Bind<IFileManagerRepository>().To<FileManagerRepository>().InRequestScope(); 
            kernel.Bind<INavigationRepository>().To<NavigationRepository>().InRequestScope(); 
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>().InRequestScope(); 
            kernel.Bind<IPageDesignRepository>().To<PageDesignRepository>().InRequestScope(); 
            kernel.Bind<IContentFileRepository>().To<ContentFileRepository>().InRequestScope(); 
            kernel.Bind<IStoreUserRepository>().To<StoreUserRepository>().InRequestScope();

            kernel.Bind<IRetailerRepository>().To<RetailerRepository>().InRequestScope();
            kernel.Bind<IProductRepository>().To<ProductRepository>().InRequestScope();
            kernel.Bind<IProductFileRepository>().To<ProductFileRepository>().InRequestScope();
            kernel.Bind<IProductCategoryRepository>().To<ProductCategoryRepository>().InRequestScope(); 
            kernel.Bind<ILabelLineRepository>().To<LabelLineRepository>().InRequestScope(); 
            kernel.Bind<ILabelRepository>().To<LabelRepository>().InRequestScope(); 
            kernel.Bind<IEmailListRepository>().To<EmailListRepository>().InRequestScope(); 
            kernel.Bind<ILogRepository>().To<LogRepository>().InRequestScope(); 
            var m = kernel.Bind<IUploadHelper>().To<UploadHelper>().InRequestScope(); 
            kernel.Bind<IEmailSender>().To<EmailSender>().InRequestScope(); 
            kernel.Bind<IContactRepository>().To<ContactRepository>().InRequestScope(); 
            kernel.Bind<ILocationRepository>().To<LocationRepository>().InRequestScope(); 
            kernel.Bind<IBrandRepository>().To<BrandRepository>().InRequestScope(); 
            kernel.Bind<ISchedulerFactory>().To<NinjectSchedulerFactory>();
            kernel.Bind<IScheduler>().ToMethod(ctx => ctx.Kernel.Get<ISchedulerFactory>().GetScheduler()).InSingletonScope();
            kernel.Bind<IBaseTasksScheduler>().To<StoreTasksScheduler>().WithConstructorArgument("scheduler", kernel.Get<IScheduler>());
            kernel.Get<IBaseTasksScheduler>().Start();
            kernel.Bind<IStoreLanguageRepository>().To<StoreLanguageRepository>().InRequestScope(); 
            kernel.Bind<IItemFileRepository>().To<ItemFileRepository>().InRequestScope(); 
            kernel.Bind<IActivityRepository>().To<ActivityRepository>().InRequestScope();
            kernel.Bind<IMessageRepository>().To<MessageRepository>().InRequestScope();
            kernel.Bind<IProductAttributeRepository>().To<ProductAttributeRepository>().InRequestScope();
            kernel.Bind<IProductAttributeRelationRepository>().To<ProductAttributeRelationRepository>().InRequestScope();

        }
    }
}
