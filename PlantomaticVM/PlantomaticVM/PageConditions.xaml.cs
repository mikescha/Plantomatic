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

namespace PlantomaticVM
{
    public partial class PageConditions : ContentPage
    {
        public PageConditions()
        {
            InitializeComponent();
        }

        void OnPageSizeChanged(object sender, EventArgs args)
        {
            //Portrait
            if (Width < Height)
            {
            }
            else //Landscape
            {
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

        private class AddressElement
        {
            public string long_name;
            public string short_name;
            public string type;
        }

        private void Button_Clicked(object sender, EventArgs e)
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
                            LabelCity.Text = element.First().long_name;
                            element = result.Where(p => p.type == "administrative_area_level_1");
                            LabelState.Text = element.First().long_name;
                            element = result.Where(p => p.type == "administrative_area_level_2");
                            LabelCounty.Text = element.First().long_name;
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

        //TEMP CODE: Do the reverse geocoding using Google Map API
        private void Old_Button_Clicked(object sender, EventArgs e)
        {
            string GoogleKey = "AIzaSyCTQFfvVgPT7Czy8ddpbAzVo1QB2y894ws";
            
            AppData appData = (AppData)BindingContext;
            double lat = appData.MasterViewModel.PlantList.TargetPlant.Lat;
            double lng = appData.MasterViewModel.PlantList.TargetPlant.Lng;

            string URL = "https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + lat.ToString() + "," + lng.ToString() + "&key=" + GoogleKey;
            WebRequest request = WebRequest.Create(URL);

            
            using (WebResponse response = request.GetResponse())
            {
                // Open the stream using a StreamReader for easy access.
                using (Stream dataStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();

                        // Parse this
                        //XmlReaderSettings settings = new XmlReaderSettings();
                        //settings.Async = false;
                        using (XmlReader xml = XmlReader.Create(new StringReader(responseFromServer)))
                        {
                            string s;
                            while (xml.Read())
                                switch (xml.NodeType)
                                {
                                    case XmlNodeType.Element:
                                       /* {
                                            if (xml.Name == "address_component")
                                            {
                                                //there are exactly four elements, so we'll parse through them
                                                for (int i = 0; i<4; i++)
                                                {
                                                    xml.Read();
                                                }
                                            }
                                        }*/
                                        s = xml.Name;
                                        break;
                                    case XmlNodeType.Text:
                                        s = xml.Value;
                                        break;
                                    case XmlNodeType.XmlDeclaration:
                                    case XmlNodeType.ProcessingInstruction:
                                    case XmlNodeType.Comment:
                                    case XmlNodeType.EndElement:
                                        break;
                                }
                            // Print to the UI for fun
                            LabelCity.Text = "";
                            LabelCounty.Text = "";
                            LabelState.Text = "";
                        }
                    }                  
                }
            }

            //Apparently the "using" clauses will properly dispose of everything, so do not need to call Dispose myself
        }
    }
}
