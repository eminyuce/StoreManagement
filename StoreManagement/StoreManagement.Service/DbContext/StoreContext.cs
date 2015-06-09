using GenericRepository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.DbContext
{
    public class StoreContext : EntitiesContext, IStoreContext
    {

        public StoreContext(String nameOrConnectionString) : base(nameOrConnectionString) { }

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
        public IDbSet<Company> Companies { get; set; }
        public IDbSet<PageDesign> PageDesigns { get; set; }
    }
}
