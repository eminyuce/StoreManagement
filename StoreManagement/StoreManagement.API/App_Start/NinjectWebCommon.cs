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
            kernel.Bind<IStoreContext>().To<StoreContext>().WithConstructorArgument("nameOrConnectionString", "Stores");
            var storeContext = (StoreContext)kernel.Get<IStoreContext>();
            storeContext.Configuration.LazyLoadingEnabled = false;

            kernel.Bind<IRetailerRepository>().To<RetailerRepository>().InRequestScope();
            kernel.Bind<IContentRepository>().To<ContentRepository>().InRequestScope();
            kernel.Bind<IStoreRepository>().To<StoreRepository>().InRequestScope();
            kernel.Bind<ISettingRepository>().To<SettingRepository>().InRequestScope();
            kernel.Bind<IFileManagerRepository>().To<FileManagerRepository>().InRequestScope();
            kernel.Bind<INavigationRepository>().To<NavigationRepository>().InRequestScope();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>().InRequestScope();
            kernel.Bind<IPageDesignRepository>().To<PageDesignRepository>().InRequestScope();
            kernel.Bind<IContentFileRepository>().To<ContentFileRepository>().InRequestScope();

            kernel.Bind<IStoreUserRepository>().To<StoreUserRepository>().InRequestScope();

            kernel.Bind<ILabelRepository>().To<LabelRepository>().InRequestScope();
            kernel.Bind<ILabelLineRepository>().To<LabelLineRepository>().InRequestScope();

            kernel.Bind<IProductRepository>().To<ProductRepository>().InRequestScope();
            kernel.Bind<IProductFileRepository>().To<ProductFileRepository>().InRequestScope();
            kernel.Bind<IProductCategoryRepository>().To<ProductCategoryRepository>().InRequestScope();

            kernel.Bind<IEmailListRepository>().To<EmailListRepository>().InRequestScope();
            kernel.Bind<IContactRepository>().To<ContactRepository>().InRequestScope();
            kernel.Bind<ILocationRepository>().To<LocationRepository>().InRequestScope();
            kernel.Bind<IBrandRepository>().To<BrandRepository>().InRequestScope();
            kernel.Bind<IStoreLanguageRepository>().To<StoreLanguageRepository>().InRequestScope();
            kernel.Bind<ILogRepository>().To<LogRepository>().InRequestScope();
            kernel.Bind<IItemFileRepository>().To<ItemFileRepository>().InRequestScope();
            kernel.Bind<IActivityRepository>().To<ActivityRepository>().InRequestScope();
            kernel.Bind<ICommentRepository>().To<CommentRepository>().InRequestScope();
            kernel.Bind<IMessageRepository>().To<MessageRepository>().InRequestScope();
            kernel.Bind<IProductAttributeRepository>().To<ProductAttributeRepository>().InRequestScope();
            kernel.Bind<IProductAttributeRelationRepository>().To<ProductAttributeRelationRepository>().InRequestScope();
        }        
    }
}
