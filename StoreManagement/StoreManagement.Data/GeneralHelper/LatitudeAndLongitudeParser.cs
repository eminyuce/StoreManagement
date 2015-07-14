using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StoreManagement.Data.GeneralHelper
{
    public class LatitudeAndLongitudeParser
    {
        public class LocationGoogle
        {
            public string address { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public string country { get; set; }

        }
        /// <summary>
        ///  First value is Latitude, Second value is Longitude
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static List<float> GetLatitudeAndLongitude(String address)
        {
            var location = GetParseLocation(address, "", 0, false);
            var list = new List<float>();
            list.Add(location.latitude.ToFloat());
            list.Add(location.longitude.ToFloat());

            return list;
        }
        private static XDocument GetAddressResponseFromApi(string address, String proxyIp = "", int port = 0)
        {
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false",
                                           Uri.EscapeDataString(address));

            var request = WebRequest.Create(requestUri);
            if (!String.IsNullOrEmpty(proxyIp))
            {
                WebProxy proxyObject = new WebProxy(proxyIp, port);
                //// Disable proxy use when the host is local.
                //proxyObject.BypassProxyOnLocal = true;
                //// HTTP requests use this proxy information.
                GlobalProxySelection.Select = proxyObject;
                request.Proxy = proxyObject;
            }
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());
            if (xdoc == null)
                throw new Exception("Xml is not found");
            var result = xdoc.Element("GeocodeResponse").Element("status");
            if (result.Value.Equals("OVER_QUERY_LIMIT"))
            {
                throw new Exception("OVER_QUERY_LIMIT");
            }

            return xdoc;
        }
        public static LocationGoogle GetParseLocation(string address, String proxyIp = "", int port = 0, Boolean isLatitudeOnly = false)
        {
            address = Regex.Replace(address, @"\r\n?|\n", ", ");
            var loc = new LocationGoogle();
            var xdoc = GetAddressResponseFromApi(address, proxyIp, port);
            if (xdoc == null)
                return loc;
            var xElement1 = xdoc.Element("GeocodeResponse");
            if (xElement1 != null)
            {
                var result = xElement1.Element("result");
                if (result == null)
                    return loc;
                var element1 = result.Element("geometry");
                if (element1 != null)
                {
                    var locationElement = element1.Element("location");
                    if (locationElement != null)
                    {
                        var xElement = locationElement.Element("lat");
                        var element = locationElement.Element("lng");
                        if (element != null)
                        {
                            var lng = element.Value.Trim();
                            loc.longitude = lng.ToFloat();
                        }

                        if (xElement != null)
                        {
                            var lat = xElement.Value.Trim();
                            loc.latitude = lat.ToFloat();
                        }
                    }
                }
                if (isLatitudeOnly)
                {
                    return loc;
                }

                var addressComponent = result.Elements("address_component");
                String streetNumber = "";
                foreach (var xElement in addressComponent)
                {
                    var xElement2 = xElement.Element("type");
                    if (xElement2 != null)
                    {
                        var type = xElement2.Value.Trim();
                        if (type.ToLower().Equals("country"))
                        {
                            var element = xElement.Element("long_name");
                            if (element != null) loc.country = element.Value.Trim();
                        }
                        else if (type.ToLower().Equals("locality"))
                        {
                            var element = xElement.Element("long_name");
                            if (element != null) loc.city = element.Value.Trim();
                        }
                        else if (type.ToLower().Equals("postal_code"))
                        {
                            var element = xElement.Element("long_name");
                            if (element != null) loc.zip = element.Value.Trim();
                        }
                        else if (type.ToLower().Equals("administrative_area_level_1"))
                        {
                            var element = xElement.Element("short_name");
                            if (element != null) loc.state = element.Value.Trim();
                        }
                        else if (type.ToLower().Equals("route"))
                        {
                            var element = xElement.Element("long_name");
                            if (element != null)
                                loc.address = streetNumber + " " + element.Value.Trim();
                        }
                        else if (type.ToLower().Equals("street_number"))
                        {
                            var element = xElement.Element("long_name");
                            if (element != null)
                                streetNumber = element.Value.Trim();
                        }
                    }
                }
            }


            return loc;
        }
    }
}
