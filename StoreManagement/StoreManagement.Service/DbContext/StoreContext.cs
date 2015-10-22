using GenericRepository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LogEntities;

namespace StoreManagement.Service.DbContext
{
    public class StoreContext : EntitiesContext, IStoreContext
    {

        public StoreContext(String nameOrConnectionString) : base(nameOrConnectionString)
        {
            this.Database.CommandTimeout = int.MaxValue;
        }

        public IDbSet<ContentFile> ContentFiles { get; set; }
        public IDbSet<Content> Contents { get; set; }
        public IDbSet<Store> Stores { get; set; }
        public IDbSet<Setting> Settings { get; set; }
        public IDbSet<Category> Categories { get; set; }
        public IDbSet<Navigation> Navigations { get; set; }
        public IDbSet<FileManager> FileManagers { get; set; }
        public IDbSet<StoreUser> StoreUsers { get; set; }
        public IDbSet<UserProfile> UserProfiles { get; set; }
        public IDbSet<webpages_Membership> WebpagesMemberships { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<PageDesign> PageDesigns { get; set; }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<ProductFile> ProductFiles { get; set; }
        public IDbSet<ProductCategory> ProductCategories { get; set; }
        public IDbSet<Label> Labels { get; set; }
        public IDbSet<LabelLine> LabelLines { get; set; }        
        public IDbSet<EmailList> EmailLists { get; set; }
        public IDbSet<system_logging> system_loggings { get; set; }
        public IDbSet<Location> Locations { get; set; }
        public IDbSet<Contact> Contacts { get; set; }
        public IDbSet<Brand> Brands { get; set; }
        public IDbSet<StoreLanguage> StoreLanguages { get; set; }
        public IDbSet<ItemFile> ItemFiles { get; set; }
        public IDbSet<Activity> Activities { get; set; }
        public IDbSet<Message> Messages { get; set; }
        public IDbSet<ProductAttributeRelation> ProductAttributeRelations { get; set; }
        public IDbSet<ProductAttribute> ProductAttributes { get; set; }
        public IDbSet<Retailer> Retailers { get; set; }
    }
}
