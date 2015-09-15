using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Liquid.Constants;
using StoreManagement.Liquid.Helper;
using StoreManagement.Liquid.Helper.Interfaces;
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
                var service1 = kernel.Bind<IContentService>().To<ContentService>().InRequestScope();
                service1.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service2 = kernel.Bind<IStoreService>().To<StoreService>().InRequestScope();
                service2.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service3 = kernel.Bind<ISettingService>().To<SettingService>().InRequestScope();
                service3.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service4 = kernel.Bind<IFileManagerService>().To<FileManagerService>().InRequestScope();
                service4.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service5 = kernel.Bind<ICategoryService>().To<CategoryService>().InRequestScope();
                service5.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service6 = kernel.Bind<IPageDesignService>().To<PageDesignService>().InRequestScope();
                service6.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service7 = kernel.Bind<IContentFileService>().To<ContentFileService>().InRequestScope();
                service7.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service8 = kernel.Bind<IStoreUserService>().To<StoreUserService>().InRequestScope();
                service8.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service10 = kernel.Bind<INavigationService>().To<NavigationService>().InRequestScope();
                service10.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service11 = kernel.Bind<IProductService>().To<ProductService>().InRequestScope();
                service11.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service12 = kernel.Bind<IProductFileService>().To<ProductFileService>().InRequestScope();
                service12.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service13 = kernel.Bind<IProductCategoryService>().To<ProductCategoryService>().InRequestScope();
                service13.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service14 = kernel.Bind<IBrandService>().To<BrandService>().InRequestScope();
                service14.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service15 = kernel.Bind<ILocationService>().To<LocationService>().InRequestScope();
                service15.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service16 = kernel.Bind<IStoreLanguageService>().To<StoreLanguageService>().InRequestScope();
                service16.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service17 = kernel.Bind<IItemFileService>().To<ItemFileService>().InRequestScope();
                service17.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service18 = kernel.Bind<IContactService>().To<ContactService>().InRequestScope();
                service18.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service19 = kernel.Bind<ILabelService>().To<LabelService>().InRequestScope();
                service19.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service20 = kernel.Bind<IActivityService>().To<ActivityService>().InRequestScope();
                service20.WithConstructorArgument("webServiceAddress", webServiceAddress);
                var service21 = kernel.Bind<ICommentService>().To<CommentService>().InRequestScope();
                service21.WithConstructorArgument("webServiceAddress", webServiceAddress);
            }
            else
            {

                //kernel.Bind<IStoreContext>().To<StoreContext>().InRequestScope().WithConstructorArgument("nameOrConnectionString", AppConstants.ConnectionStringName);
                //kernel.Bind<IContentService>().To<ContentRepository>().InRequestScope();
                //var m = kernel.Bind<IStoreService>().To<StoreRepository>().InRequestScope();
                //kernel.Bind<ISettingService>().To<SettingRepository>().InRequestScope();
                //kernel.Bind<IFileManagerService>().To<FileManagerRepository>().InRequestScope();
                //kernel.Bind<ICategoryService>().To<CategoryRepository>().InRequestScope();
                //kernel.Bind<IPageDesignService>().To<PageDesignRepository>().InRequestScope();
                //kernel.Bind<IContentFileService>().To<ContentFileRepository>().InRequestScope();
                //kernel.Bind<IStoreUserService>().To<StoreUserRepository>().InRequestScope();

                //kernel.Bind<INavigationService>().To<NavigationRepository>().InRequestScope();
                //kernel.Bind<IProductService>().To<ProductRepository>().InRequestScope();
                //kernel.Bind<IProductFileService>().To<ProductFileRepository>().InRequestScope();
                //kernel.Bind<IProductCategoryService>().To<ProductCategoryRepository>().InRequestScope();
                //kernel.Bind<ILocationService>().To<LocationRepository>().InRequestScope();
                //kernel.Bind<IBrandService>().To<BrandRepository>().InRequestScope();
                //kernel.Bind<IStoreLanguageService>().To<StoreLanguageRepository>().InRequestScope();
                //kernel.Bind<IItemFileService>().To<ItemFileRepository>().InRequestScope();
                //kernel.Bind<ILabelService>().To<LabelRepository>().InRequestScope();
                //kernel.Bind<IContactService>().To<ContactRepository>().InRequestScope();
                //kernel.Bind<IActivityService>().To<ActivityRepository>().InRequestScope();
                //kernel.Bind<ICommentService>().To<CommentRepository>().InRequestScope();





                //kernel.Bind<IStoreContext>().To<StoreContext>().InRequestScope().WithConstructorArgument("nameOrConnectionString", AppConstants.ConnectionStringName);
                kernel.Bind<IContentService>().To<ContentRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IStoreService>().To<StoreRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<ISettingService>().To<SettingRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IFileManagerService>().To<FileManagerRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<ICategoryService>().To<CategoryRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IPageDesignService>().To<PageDesignRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IContentFileService>().To<ContentFileRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IStoreUserService>().To<StoreUserRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<INavigationService>().To<NavigationRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IProductService>().To<ProductRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IProductFileService>().To<ProductFileRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IProductCategoryService>().To<ProductCategoryRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<ILocationService>().To<LocationRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IBrandService>().To<BrandRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IStoreLanguageService>().To<StoreLanguageRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IItemFileService>().To<ItemFileRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<ILabelService>().To<LabelRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IContactService>().To<ContactRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<IActivityService>().To<ActivityRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName));
                kernel.Bind<ICommentService>().To<CommentRepository>().InRequestScope().WithConstructorArgument("dbContext", new StoreContext(AppConstants.ConnectionStringName)); 
  
            }
            kernel.Bind<IEmailSender>().To<EmailSender>().InRequestScope();

            kernel.Bind<IProductCategoryHelper>().To<ProductCategoryHelper>().InRequestScope();
            kernel.Bind<IProductHelper>().To<ProductHelper>().InRequestScope();
            kernel.Bind<IPhotoGalleryHelper>().To<PhotoGalleryHelper>().InRequestScope();
            kernel.Bind<IBrandHelper>().To<BrandHelper>().InRequestScope();
            kernel.Bind<IContentHelper>().To<ContentHelper>().InRequestScope();
            kernel.Bind<IHomePageHelper>().To<HomePageHelper>().InRequestScope();
            kernel.Bind<ILabelHelper>().To<LabelHelper>().InRequestScope();
            kernel.Bind<INavigationHelper>().To<NavigationHelper>().InRequestScope();
            kernel.Bind<IPagingHelper>().To<PagingHelper>().InRequestScope(); 
            kernel.Bind<IStoreHelper>().To<StoreHelper>().InRequestScope();
            kernel.Bind<ILocationHelper>().To<LocationHelper>().InRequestScope();
            kernel.Bind<IContactHelper>().To<ContactHelper>().InRequestScope();
            kernel.Bind<IActivityHelper>().To<ActivityHelper>().InRequestScope();
            kernel.Bind<ICommentHelper>().To<CommentHelper>().InRequestScope();
        }           
    }
    
}
