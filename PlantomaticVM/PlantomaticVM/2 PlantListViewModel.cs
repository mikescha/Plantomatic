using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using PlantMan.Plant;

namespace PlantomaticVM
{
    public class PlantList : INotifyPropertyChanged
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

        string state;  // The US state that the list of plants is associated with
        List<MyPlant> allPlants = new List<MyPlant>(); // All the plants in the database
        ObservableCollection<MyPlant> myPlants = new ObservableCollection<MyPlant>(); // The subset of plants that match the current target
        ObservableCollection<MyPlant> shoppingListPlants = new ObservableCollection<MyPlant>(); // The subset of plants that are in the list
        MyCriteria targetPlant = new MyCriteria(); // The criteria that the user has selected so far

        public ObservableCollection<MyPlant> MyPlants
        {
            set
            {
                if (myPlants != value)
                {
                    myPlants = value;
                    OnPropertyChanged("MyPlants");
                }
            }
            get { return myPlants; }
        }
        
        //Attempt to toggle the cart status of the item passed in
        public bool ToggleStatus(MyPlant p)
        {
            //First, find the plant that matches the one passed in (this assumes each plant's name is spelled consistently, that
            //the name is unique, and that it exists exactly once). And then, toggle the status of that plant.
            bool value = allPlants.Find(x => x.Name == p.Name).ToggleListStatus();

            //Now, regenerate the list so that it reflects the right set of plants and everybody knows it has changed
            RefreshShoppingListPlants();
            return value;
        }

        //Update the shopping list, and then generate a notification that it has changed.
        public void RefreshShoppingListPlants()
        {
            shoppingListPlants = new ObservableCollection<MyPlant>(allPlants.Where(i => i.InCart));

            //Generate notifications for everything that needs to know that the list has changed.
            OnPropertyChanged("ShoppingListPlants");
            OnPropertyChanged("MonthSummary");
            OnPropertyChanged("SunSummary");
        }

        // Generic set/get for the ShoppingList
        public ObservableCollection<MyPlant> ShoppingListPlants
        {
            set
            {
                if (shoppingListPlants != value)
                {
                    shoppingListPlants = value;
                    OnPropertyChanged("ShoppingListPlants");
                }
            }
            get
            {
                return shoppingListPlants;
            }
        }

        public List<MyPlant> AllPlants
        {
            set
            {
                if (allPlants != value)
                {
                    allPlants = value;
                    OnPropertyChanged("AllPlants");
                }
            }
            get { return allPlants; }
        }

        // This holds all the criteria used in filtering
        public MyCriteria TargetPlant
        {
            set
            {
                if (targetPlant != value)
                {
                    targetPlant = value;
                    OnPropertyChanged("TargetPlant");
                }
            }
            get { return targetPlant; }
        }

        public string State
        {
            set
            {
                if (state != value)
                {
                    state = value;
                    OnPropertyChanged("State");
                }
            }
            get { return state; }
        }

        private Command _clearCart;
        public ICommand ClearCart
        {
            get
            {
                if (_clearCart == null)
                {
                    _clearCart = new Command(() =>
                    {
                        foreach(MyPlant p in allPlants)
                        {
                            p.InCart = false;
                        }
                        RefreshShoppingListPlants();
                    });
                }
                return _clearCart;
            }
        }

        /* Analysis operations
         * These methods are used to summarize the contents of the shopping list. The listview controls in the view are bound to these lists.
         * 
         * To add a new summary, add the method below, and also add a PropertyChanged event to the RefreshShoppingListPlants method.
         */
        public List<string> MonthSummary
        {
            get
            {
                //Get the count of plants that flower in each month
                int maxMonths = 12;
                int[] monthCount = new int[maxMonths];
                int i;

                foreach (MyPlant p in shoppingListPlants)
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
                }//foreach

                //Add all the months to the list
                List<string> floweringResults = new List<string>();
                i = 0;
                foreach (string s in floweringMonthDict.Keys)
                {
                    floweringResults.Add(s + ": " + monthCount[i].ToString());
                    i++;
                }
                return floweringResults;
            }
        }

        public List<string> SunSummary
        {
            get {

                //Get the count of plants that flower in each month
                int maxSun = 4;
                int[] sunCount = new int[maxSun];
                int i;

                foreach (MyPlant p in shoppingListPlants)
                {
                    i = 0;
                    // if it's all sun types, then add 1 to everything
                    if (p.SunRequirements.HasFlag(SunRequirements.AllSunTypes))
                    {
                        for (i = 0; i < maxSun; i++)
                            sunCount[i]++;
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
                }

                List<string> sunResults = new List<string>();
                i = 0;

                foreach (string s in sunTypeDict.Keys)
                {
                    sunResults.Add(s + ": " + sunCount[i].ToString());
                    i++;
                }
                return sunResults;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
