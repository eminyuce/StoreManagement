using System.Web.Mvc;
using Quartz;
using StoreManagement.Data;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.LiquidHelpers;
using StoreManagement.Data.LiquidHelpers.Interfaces;
using StoreManagement.Liquid.Constants;
using StoreManagement.Liquid.ScheduledTasks;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.IGeneralRepositories;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.ApiRepositories;

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


            kernel.Bind<IContentGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ContentApiRepository(webServiceAddress);
                }
                else
                {
                    return new ContentRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IStoreGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new StoreApiRepository(webServiceAddress);
                }
                else
                {
                    return new StoreRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<ISettingGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new SettingApiRepository(webServiceAddress);
                }
                else
                {
                    return new SettingRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IFileManagerGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new FileManagerApiRepository(webServiceAddress);
                }
                else
                {
                    return new FileManagerRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<ICategoryGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new CategoryApiRepository(webServiceAddress);
                }
                else
                {
                    return new CategoryRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IPageDesignGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new PageDesignApiRepository(webServiceAddress);
                }
                else
                {
                    return new PageDesignRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IContentFileGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ContentFileApiRepository(webServiceAddress);
                }
                else
                {
                    return new ContentFileRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IStoreUserGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new StoreUserApiRepository(webServiceAddress);
                }
                else
                {
                    return new StoreUserRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<INavigationGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new NavigationApiRepository(webServiceAddress);
                }
                else
                {
                    return new NavigationRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IProductGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ProductApiRepository(webServiceAddress);
                }
                else
                {
                    return new ProductRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IProductFileGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ProductFileApiRepository(webServiceAddress);
                }
                else
                {
                    return new ProductFileRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IProductCategoryGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ProductCategoryApiRepository(webServiceAddress);
                }
                else
                {
                    return new ProductCategoryRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<ILocationGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new LocationApiRepository(webServiceAddress);
                }
                else
                {
                    return new LocationRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IBrandGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new BrandApiRepository(webServiceAddress);
                }
                else
                {
                    return new BrandRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IStoreLanguageGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new StoreLanguageApiRepository(webServiceAddress);
                }
                else
                {
                    return new StoreLanguageRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IItemFileGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ItemFileApiRepository(webServiceAddress);
                }
                else
                {
                    return new ItemFileRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<ILabelGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new LabelApiRepository(webServiceAddress);
                }
                else
                {
                    return new LabelRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IContactGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ContactApiRepository(webServiceAddress);
                }
                else
                {
                    return new ContactRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();
            kernel.Bind<IActivityGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ActivityApiRepository(webServiceAddress);
                }
                else
                {
                    return new ActivityRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();

            kernel.Bind<ICommentGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new CommentApiRepository(webServiceAddress);
                }
                else
                {
                    return new CommentRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();

            kernel.Bind<IMessageGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new MessageApiRepository(webServiceAddress);
                }
                else
                {
                    return new MessageRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();




            kernel.Bind<IProductAttributeGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ProductAttributeApiRepository(webServiceAddress);
                }
                else
                {
                    return new ProductAttributeRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();


            kernel.Bind<IProductAttributeRelationGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new ProductAttributeRelationApiRepository(webServiceAddress);
                }
                else
                {
                    return new ProductAttributeRelationRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();

            kernel.Bind<IRetailerGeneralRepository>().ToMethod(ctx =>
            {
                if (isApiService)
                {
                    return new RetailerApiRepository(webServiceAddress);
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
