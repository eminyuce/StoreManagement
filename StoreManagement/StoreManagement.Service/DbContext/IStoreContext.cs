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
        IDbSet<ContentFile> ContentFiles { get; set; }
        IDbSet<Content> Contents { get; set; }
        IDbSet<Store> Stores { get; set; }
        IDbSet<Setting> Settings { get; set; } 
        IDbSet<Category> Categories { get; set; }
        IDbSet<Navigation> Navigations { get; set; }
        IDbSet<FileManager> FileManagers { get; set; }
        IDbSet<StoreUser> StoreUsers { get; set; }
        IDbSet<UserProfile> UserProfiles { get; set; }
        IDbSet<webpages_Membership> WebpagesMemberships { get; set; }
        IDbSet<Role> Roles { get; set; }
        IDbSet<PageDesign> PageDesigns { get; set; }
        IDbSet<Product> Products { get; set; }
        IDbSet<ProductFile> ProductFiles { get; set; }  
        IDbSet<ProductCategory> ProductCategories { get; set; }
        IDbSet<Label> Labels { get; set; }
        IDbSet<LabelLine> LabelLines { get; set; }
        IDbSet<EmailList> EmailLists { get; set; }
        IDbSet<system_logging> system_loggings { get; set; }
        IDbSet<Location> Locations { get; set; }
        IDbSet<Contact> Contacts { get; set; }
        IDbSet<Brand> Brands { get; set; }
        IDbSet<StoreLanguage> StoreLanguages { get; set; }
        IDbSet<ItemFile> ItemFiles { get; set; }
        IDbSet<Activity> Activities { get; set; }
        IDbSet<Message> Messages { get; set; }
    }
}
