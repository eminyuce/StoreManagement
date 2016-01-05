using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    public class LocationLiquid : BaseDrop
    {
        public Location Location;

        public LocationLiquid(Location location, int imageWidth, int imageHeight)
        {
            this.Location = location;

            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        }

        public int Id
        {
            get { return Location.Id; }
        }
        public String Name
        {
            get { return Location.Name; }
        }

        public DateTime CreatedDate
        {
            get { return Location.CreatedDate.Value; }
        }
        public DateTime UpdatedDate
        {
            get { return Location.UpdatedDate.Value; }
        }
        public bool State
        {
            get { return Location.State; }
        }
        public string Address
        {
            get { return Location.Address; }
        }
        public string City
        {
            get { return Location.City; }
        }
        public string LocationState
        {
            get { return Location.LocationState; }
        }
        public string Postal
        {
            get { return Location.Postal; }
        }
        public string Country
        {
            get { return Location.Country; }
        }
        public double Latitude
        {
            get { return Location.Latitude; }
        }
        public double Longitude
        {
            get { return Location.Longitude; }
        }
    }
}
