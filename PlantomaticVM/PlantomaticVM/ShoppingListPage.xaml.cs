using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantMan.Plant;
using Xamarin.Forms;

namespace PlantomaticVM
{
    public partial class ShoppingListPage : ContentPage
    {
        Dictionary<String, FloweringMonths> floweringMonthDict = new Dictionary<string, FloweringMonths>
        {
            {"January", FloweringMonths.Jan }, {"February", FloweringMonths.Feb }, {"March", FloweringMonths.Mar },
            {"April", FloweringMonths.Apr }, {"May", FloweringMonths.May }, {"June", FloweringMonths.Jun },
            {"July", FloweringMonths.Jul }, {"August", FloweringMonths.Aug }, {"September", FloweringMonths.Sep },
            {"October", FloweringMonths.Oct }, {"November", FloweringMonths.Nov }, {"December", FloweringMonths.Dec }
           // , {"All", FloweringMonths.AllMonths}
        };

        Dictionary<String, SunRequirements> sunTypeDict = new Dictionary<string, SunRequirements>
        {
            {"Full sun", SunRequirements.Full }, {"Partial sun", SunRequirements.Partial}, {"Shade", SunRequirements.Shade}
            //, {"All", SunRequirements.AllSunTypes }
        };



        public ShoppingListPage()
        {
            InitializeComponent();
        }

        //TODO Move this into a class related to MyPlant and make it listen for changes to the shopping cart, so it can update automatically
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // Get AppData object (set to BindingContext in XAML file). 
            AppData appData = (AppData)BindingContext;
            countLabel.Text = appData.MasterViewModel.PlantList.ShoppingListPlants.Count.ToString() + " plants in list";
            shoppingListView.ItemsSource = appData.MasterViewModel.PlantList.ShoppingListPlants;

            //Get the count of plants that flower in each month
            int maxMonths = 12;
            int maxSun = 4;
            int[] monthCount = new int[maxMonths];
            int[] sunCount = new int[maxSun];

            int i;

            foreach (MyPlant p in appData.MasterViewModel.PlantList.ShoppingListPlants)
            {
                i = 0;
                //if it's all months, then add 1 to everything
                if (p.FloweringMonths.HasFlag(FloweringMonths.AllMonths))
                {
                    for (i = 0; i < maxMonths; i++)
                    {
                        monthCount[i]++;
                    }
                }
                else //otherwise, go through all the months
                {
                    foreach (FloweringMonths f in floweringMonthDict.Values)
                    {
                        if (p.FloweringMonths.HasFlag(f))
                            monthCount[i]++;
                        i++;
                    }
                }

                i=0;
                // if it's all sun types, then add 1 to everything
                if (p.SunRequirements.HasFlag(SunRequirements.AllSunTypes))
                {
                    for(i=0; i < maxSun; i++)
                    {
                        sunCount[i]++;
                    }
                }
                else
                {
                    foreach (SunRequirements s in sunTypeDict.Values)
                    {
                        if (p.SunRequirements.HasFlag(s))
                            sunCount[i]++;
                        i++;
                    }
                }
            }//foreach

            //Add all the months to the list
            List<string> floweringResults = new List<string>();
            i = 0;
            foreach (string s in floweringMonthDict.Keys)
            {
                floweringResults.Add(s + ": " + monthCount[i].ToString());
                i++;
            }
            floweringMonthView.ItemsSource = floweringResults;

            List<string> sunResults = new List<string>();
            i = 0;

            foreach (string s in sunTypeDict.Keys)
            {
                sunResults.Add(s + ": " + sunCount[i].ToString());
                i++;
            }
            sunRequirementsView.ItemsSource = sunResults;
        }

        
    }
}
