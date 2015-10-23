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

        public DbSet<ContentFile> ContentFiles { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Navigation> Navigations { get; set; }
        public DbSet<FileManager> FileManagers { get; set; }
        public DbSet<StoreUser> StoreUsers { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<webpages_Membership> WebpagesMemberships { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PageDesign> PageDesigns { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFile> ProductFiles { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<LabelLine> LabelLines { get; set; }        
        public DbSet<EmailList> EmailLists { get; set; }
        public DbSet<system_logging> system_loggings { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<StoreLanguage> StoreLanguages { get; set; }
        public DbSet<ItemFile> ItemFiles { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ProductAttributeRelation> ProductAttributeRelations { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<Retailer> Retailers { get; set; }
        public DbSet<StorePageDesign> StorePageDesigns { get; set; }
    }
}
