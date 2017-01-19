using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebUtility;
using Xamarin.Forms;
using System.Xml;
using System.Xml.Linq;
using Plugin.Geolocator; // From https://github.com/jamesmontemagno/GeolocatorPlugin/blob/master/README.md

namespace PlantomaticVM
{
    public partial class PageConditions : ContentPage
    {
        public PageConditions()
        {
            InitializeComponent();

            GetLocation();
        }

        void OnPageSizeChanged(object sender, EventArgs args)
        {
            //Portrait
            if (Width < Height)
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(0);

                Grid.SetRow(customStack, 2);
                Grid.SetColumn(customStack, 0);
            }
            else //Landscape
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                Grid.SetRow(customStack, 1);
                Grid.SetColumn(customStack, 1);
            }
        } //end OnPageSizeChanged

        void OnPickerSelectedIndexChanged(object sender, EventArgs args)
        {
            // Get AppData object (set to BindingContext in XAML file). 
            AppData appData = (AppData)BindingContext;
            appData.MasterViewModel.FilterPlantList();
        }

        void OnSwitchToggled(object sender, EventArgs args)
        {
            // Get AppData object (set to BindingContext in XAML file). 
            AppData appData = (AppData)BindingContext;
            appData.MasterViewModel.FilterPlantList();
        }

        async void GetLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(10000);

            AppData appData = (AppData)BindingContext;
            
            labelStatus.Text = "Using this location:";

            appData.MasterViewModel.PlantList.TargetPlant.Lat = position.Latitude;
            appData.MasterViewModel.PlantList.TargetPlant.Lng = position.Longitude;

            labelHelpText.IsVisible = true;
            
            ReverseGeocode();

            EnableUserCounty();
        }

        private class AddressElement
        {
            public string long_name;
            public string short_name;
            public string type;
        }

        private void ReverseGeocode()
        {
            string GoogleKey = "AIzaSyCTQFfvVgPT7Czy8ddpbAzVo1QB2y894ws";

            AppData appData = (AppData)BindingContext;
            double lat = appData.MasterViewModel.PlantList.TargetPlant.Lat;
            double lng = appData.MasterViewModel.PlantList.TargetPlant.Lng;

            var request = HttpWebRequest.Create(string.Format(@"https://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&key={2}",
                lat.ToString(), lng.ToString(), GoogleKey));
            request.Method = "GET";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string content = reader.ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            //parse XML
                            var doc = XDocument.Parse(content);
                            var result = doc.Root.Descendants("address_component").Select(x => new AddressElement
                            {
                                long_name = x.Element("long_name").Value,
                                short_name = x.Element("short_name").Value,
                                type = x.Element("type").Value
                            });

                            //In the Google Maps API, City is type=="political", Count is type=="administrative_area_level_2", 
                            //State is type=="administrative_area_level_1"
                            //For some reason there are duplicate results in the response, so select just the first one
                            IEnumerable<AddressElement> element = result.Where(p => p.type == "political");
                            string city = element.Any() ? element.First().long_name : "";

                            element = result.Where(p => p.type == "administrative_area_level_1");
                            string state = element.Any() ? element.First().long_name : "";

                            labelCityState.Text = city + ", " + state;

                            //Get the county and write it to both the UI and to the data structure
                            element = result.Where(p => p.type == "administrative_area_level_2");
                            labelCounty.Text = element.Any() ? element.First().long_name : "";
                            appData.MasterViewModel.PlantList.TargetPlant.UserCounty = labelCounty.Text;
                        }
                        else
                        {
                            //TODO String was empty. What now?
                            ;
                        }
                    }
                }
                else
                {
                    // TODO Got some kind of error, should check it out. Put this here so we can put a breakpoint on it
                    string error = response.StatusDescription;
                }
            }
        }

        private void EnableUserCounty()
        {
            AppData appData = (AppData)BindingContext;

            var p = appData.MasterViewModel.PlantList.TargetPlant;
            p.NativeTo_Alameda = appData.MasterViewModel.PlantList.TargetPlant.UserCounty.Contains("Alameda");
            p.NativeTo_Contra_Costa = appData.MasterViewModel.PlantList.TargetPlant.UserCounty.Contains("Contra");
            p.NativeTo_Marin = appData.MasterViewModel.PlantList.TargetPlant.UserCounty.Contains("Marin");
            p.NativeTo_Napa = appData.MasterViewModel.PlantList.TargetPlant.UserCounty.Contains("Napa");
            p.NativeTo_San_Francisco = appData.MasterViewModel.PlantList.TargetPlant.UserCounty.Contains("Francisco");
            p.NativeTo_San_Mateo = appData.MasterViewModel.PlantList.TargetPlant.UserCounty.Contains("Mateo");
            p.NativeTo_Santa_Clara = appData.MasterViewModel.PlantList.TargetPlant.UserCounty.Contains("Clara");
            p.NativeTo_Solano = appData.MasterViewModel.PlantList.TargetPlant.UserCounty.Contains("Solano");
            p.NativeTo_Sonoma = appData.MasterViewModel.PlantList.TargetPlant.UserCounty.Contains("Sonoma");

        }
    }
}
   
