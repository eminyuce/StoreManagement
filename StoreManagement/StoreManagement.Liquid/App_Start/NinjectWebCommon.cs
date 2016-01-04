using System.Web.Mvc;
using Quartz;
using StoreManagement.Data;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.LiquidHelpers;
using StoreManagement.Data.LiquidHelpers.Interfaces;
using StoreManagement.Liquid.Constants;
using StoreManagement.Liquid.ScheduledTasks;
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

            var isApiService = ProjectAppSettings.IsApiService;
            var webServiceAddress = ProjectAppSettings.WebServiceAddress; 

         
            kernel.Bind<IContentService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ContentService(webServiceAddress);
                }
                else
                {
                    return new ContentRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IStoreService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new StoreService(webServiceAddress);
                }
                else
                {
                    return new StoreRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<ISettingService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new SettingService(webServiceAddress);
                }
                else
                {
                    return new SettingRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IFileManagerService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new FileManagerService(webServiceAddress);
                }
                else
                {
                    return new FileManagerRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<ICategoryService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new CategoryService(webServiceAddress);
                }
                else
                {
                    return new CategoryRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IPageDesignService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new PageDesignService(webServiceAddress);
                }
                else
                {
                    return new PageDesignRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IContentFileService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ContentFileService(webServiceAddress);
                }
                else
                {
                    return new ContentFileRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IStoreUserService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new StoreUserService(webServiceAddress);
                }
                else
                {
                    return new StoreUserRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<INavigationService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new NavigationService(webServiceAddress);
                }
                else
                {
                    return new NavigationRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IProductService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ProductService(webServiceAddress);
                }
                else
                {
                    return new ProductRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IProductFileService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ProductFileService(webServiceAddress);
                }
                else
                {
                    return new ProductFileRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IProductCategoryService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ProductCategoryService(webServiceAddress);
                }
                else
                {
                    return new ProductCategoryRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<ILocationService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new LocationService(webServiceAddress);
                }
                else
                {
                    return new LocationRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IBrandService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new BrandService(webServiceAddress);
                }
                else
                {
                    return new BrandRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IStoreLanguageService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new StoreLanguageService(webServiceAddress);
                }
                else
                {
                    return new StoreLanguageRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IItemFileService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ItemFileService(webServiceAddress);
                }
                else
                {
                    return new ItemFileRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<ILabelService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new LabelService(webServiceAddress);
                }
                else
                {
                    return new LabelRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IContactService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ContactService(webServiceAddress);
                }
                else
                {
                    return new ContactRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IActivityService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ActivityService(webServiceAddress);
                }
                else
                {
                    return new ActivityRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();

            kernel.Bind<ICommentService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new CommentService(webServiceAddress);
                }
                else
                {
                    return new CommentRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();

            kernel.Bind<IMessageService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new MessageService(webServiceAddress);
                }
                else
                {
                    return new MessageRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();




            kernel.Bind<IProductAttributeService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ProductAttributeService(webServiceAddress);
                }
                else
                {
                    return new ProductAttributeRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();


            kernel.Bind<IProductAttributeRelationService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ProductAttributeRelationService(webServiceAddress);
                }
                else
                {
                    return new ProductAttributeRelationRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();

            kernel.Bind<IRetailerService>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new RetailerService(webServiceAddress);
                }
                else
                {
                    return new RetailerRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();


            kernel.Bind<IEmailSender>().To<EmailSender>().InRequestScope();
            kernel.Bind<ICategoryHelper>().To<CategoryHelper>().InRequestScope();

            kernel.Bind<IProductCategoryHelper>().To<ProductCategoryHelper>().InRequestScope();
            kernel.Bind<IProductHelper>().To<ProductHelper>().InRequestScope();
            kernel.Bind<IPhotoGalleryHelper>().To<PhotoGalleryHelper>().InRequestScope();
            kernel.Bind<IBrandHelper>().To<BrandHelper>().InRequestScope();
            kernel.Bind<IContentHelper>().To<ContentHelper>().InRequestScope();
            kernel.Bind<IHomePageHelper>().To<HomePageHelper>().InRequestScope();
            kernel.Bind<ILabelHelper>().To<LabelHelper>().InRequestScope();
            kernel.Bind<INavigationHelper>().To<NavigationHelper>().InRequestScope();
            kernel.Bind<IPagingHelper>().To<PagingHelper>().InRequestScope(); 
            kernel.Bind<ILocationHelper>().To<LocationHelper>().InRequestScope();
            kernel.Bind<IContactHelper>().To<ContactHelper>().InRequestScope();
            kernel.Bind<IActivityHelper>().To<ActivityHelper>().InRequestScope();
            kernel.Bind<ICommentHelper>().To<CommentHelper>().InRequestScope();
            kernel.Bind<IRetailerHelper>().To<RetailerHelper>().InRequestScope();
            kernel.Bind<IHttpContextFactory>().To<HttpContextFactory>().InRequestScope();

            kernel.Bind<HttpContext>().ToMethod(ctx => HttpContext.Current).InTransientScope();
            kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();

            kernel.Bind<ISchedulerFactory>().To<NinjectSchedulerFactory>();
            kernel.Bind<IScheduler>().ToMethod(ctx => ctx.Kernel.Get<ISchedulerFactory>().GetScheduler()).InSingletonScope();
            kernel.Bind<IBaseTasksScheduler>().To<LiquidTasksScheduler>().WithConstructorArgument("scheduler", kernel.Get<IScheduler>());
            kernel.Get<IBaseTasksScheduler>().Start();

        }           
    }
    
}
