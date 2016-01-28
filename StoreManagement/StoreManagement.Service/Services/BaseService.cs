using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Service.IGeneralRepositories;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public abstract class BaseService : IBaseService
    {
        [Inject]
        public IMessageGeneralRepository MessageRepository { set; get; }


        [Inject]
        public IStoreGeneralRepository StoreRepository { set; get; }

        [Inject]
        public IItemFileGeneralRepository ItemFileRepository { set; get; }

        [Inject]
        public ISettingGeneralRepository SettingRepository { set; get; }

        [Inject]
        public IFileManagerGeneralRepository FileManagerService { get; set; }

        [Inject]
        public IContentFileGeneralRepository ContentFileRepository { set; get; }

        [Inject]
        public ICommentGeneralRepository CommentRepository { set; get; }

        [Inject]
        public IContentGeneralRepository ContentRepository { set; get; }

        [Inject]
        public ICategoryGeneralRepository CategoryRepository { set; get; }

        [Inject]
        public INavigationGeneralRepository NavigationRepository { set; get; }

        [Inject]
        public IPageDesignGeneralRepository PageDesignRepository { set; get; }

        [Inject]
        public IStoreUserGeneralRepository StoreUserRepository { set; get; }

        [Inject]
        public IEmailSender EmailSender { set; get; }


        [Inject]
        public IActivityGeneralRepository ActivityRepository { set; get; }

        [Inject]
        public IRetailerGeneralRepository RetailerRepository { set; get; }

        [Inject]
        public IProductGeneralRepository ProductRepository { set; get; }

        [Inject]
        public IProductAttributeGeneralRepository ProductAttributeRepository { set; get; }

        [Inject]
        public IProductAttributeRelationGeneralRepository ProductAttributeRelationRepository { set; get; }

        [Inject]
        public IProductFileGeneralRepository ProductFileRepository { set; get; }

        [Inject]
        public IProductCategoryGeneralRepository ProductCategoryRepository { set; get; }

        [Inject]
        public IBrandGeneralRepository BrandRepository { set; get; }

        [Inject]
        public ILocationGeneralRepository LocationRepository { set; get; }

        [Inject]
        public IContactGeneralRepository ContactRepository { set; get; }


        [Inject]
        public ILabelGeneralRepository LabelRepository { set; get; }


    }
}
