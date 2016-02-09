using System.Web.Mvc;
using StoreManagement.Constants;
using StoreManagement.Data;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.LiquidHelpers;
using StoreManagement.Data.LiquidHelpers.Interfaces;
using StoreManagement.Filters;
using StoreManagement.Helper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.IGeneralRepositories;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Service.ApiRepositories;
using StoreManagement.Service.Services;
using StoreManagement.Service.Services.IServices;

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

            kernel.Bind<ISiteMapService>().To<SiteMapService>().InRequestScope();
            kernel.Bind<IFileManagerService>().To<FileManagerService>().InRequestScope();
            kernel.Bind<IProductService>().To<ProductService>().InRequestScope();
            kernel.Bind<ILocationService>().To<LocationService>().InRequestScope();
            kernel.Bind<IBrandService>().To<BrandService>().InRequestScope();
            kernel.Bind<IRetailerService>().To<RetailerService>().InRequestScope();
            kernel.Bind<IContentService>().To<ContentService>().InRequestScope();
            kernel.Bind<IContactService>().To<ContactService>().InRequestScope();
            kernel.Bind<INavigationService>().To<NavigationService>().InRequestScope();
            kernel.Bind<IStoreService>().To<StoreService>().InRequestScope();
            kernel.Bind<ICategoryService>().To<CategoryService>().InRequestScope();
            kernel.Bind<IProductCategoryService>().To<ProductCategoryService>().InRequestScope();
            kernel.Bind<ILabelService>().To<LabelService>().InRequestScope();

            kernel.Bind<IEmailSender>().To<EmailSender>();
             
           

           
        }
    }

     
}
