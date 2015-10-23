using System.Data.Entity;
using GenericRepository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LogEntities;

namespace StoreManagement.Service.DbContext
{
    public interface IStoreContext : IDisposable, IEntitiesContext 
    {
        DbSet<ContentFile> ContentFiles { get; set; }
        DbSet<Content> Contents { get; set; }
        DbSet<Store> Stores { get; set; }
        DbSet<Setting> Settings { get; set; } 
        DbSet<Category> Categories { get; set; }
        DbSet<Navigation> Navigations { get; set; }
        DbSet<FileManager> FileManagers { get; set; }
        DbSet<StoreUser> StoreUsers { get; set; }
        DbSet<UserProfile> UserProfiles { get; set; }
        DbSet<webpages_Membership> WebpagesMemberships { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<PageDesign> PageDesigns { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductFile> ProductFiles { get; set; }  
        DbSet<ProductCategory> ProductCategories { get; set; }
        DbSet<Label> Labels { get; set; }
        DbSet<LabelLine> LabelLines { get; set; }
        DbSet<EmailList> EmailLists { get; set; }
        DbSet<system_logging> system_loggings { get; set; }
        DbSet<Location> Locations { get; set; }
        DbSet<Contact> Contacts { get; set; }
        DbSet<Brand> Brands { get; set; }
        DbSet<StoreLanguage> StoreLanguages { get; set; }
        DbSet<ItemFile> ItemFiles { get; set; }
        DbSet<Activity> Activities { get; set; }
        DbSet<Message> Messages { get; set; }                
        DbSet<ProductAttributeRelation> ProductAttributeRelations { get; set; }
        DbSet<ProductAttribute> ProductAttributes { get; set; }
        DbSet<Retailer> Retailers { get; set; }
        DbSet<StorePageDesign> StorePageDesigns { get; set; }  
    }
}
