using System.Web.Mvc;
using StoreManagement.Constants;
using StoreManagement.Data;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.LiquidHelpers;
using StoreManagement.Data.LiquidHelpers.Interfaces;
using StoreManagement.Filters;
using StoreManagement.Helper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Service.ApiServices;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(StoreManagement.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(StoreManagement.App_Start.NinjectWebCommon), "Stop")]

namespace StoreManagement.App_Start
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
                    return new ContentApiService(webServiceAddress);
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
                    return new StoreApiService(webServiceAddress);
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
                    return new SettingApiService(webServiceAddress);
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
                    return new FileManagerApiService(webServiceAddress);
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
                    return new CategoryApiService(webServiceAddress);
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
                    return new PageDesignApiService(webServiceAddress);
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
                    return new ContentFileApiService(webServiceAddress);
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
                    return new StoreUserApiService(webServiceAddress);
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
                    return new NavigationApiService(webServiceAddress);
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
                    return new ProductApiService(webServiceAddress);
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
                    return new ProductFileApiService(webServiceAddress);
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
                    return new ProductCategoryApiService(webServiceAddress);
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
                    return new LocationApiService(webServiceAddress);
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
                    return new BrandApiService(webServiceAddress);
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
                    return new StoreLanguageApiService(webServiceAddress);
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
                    return new ItemFileApiService(webServiceAddress);
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
                    return new LabelApiService(webServiceAddress);
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
                    return new ContactApiService(webServiceAddress);
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
                    return new ActivityApiService(webServiceAddress);
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
                    return new CommentApiService(webServiceAddress);
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
                    return new MessageApiService(webServiceAddress);
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
                    return new ProductAttributeApiService(webServiceAddress);
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
                    return new ProductAttributeRelationApiService(webServiceAddress);
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
                    return new RetailerApiService(webServiceAddress);
                }
                else
                {
                    return new RetailerRepository(new StoreContext(AppConstants.ConnectionStringName));
                }
            }).InRequestScope();


            kernel.Bind<IEmailSender>().To<EmailSender>();
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
           

           
        }
    }
}
