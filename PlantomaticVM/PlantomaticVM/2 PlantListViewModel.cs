using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using PlantMan.Plant;
using System.Runtime.CompilerServices;

namespace PlantomaticVM
{
    public class PlantList : INotifyPropertyChanged
    {
        #region Dictionaries
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
        #endregion Dictionaries

        #region Fields
        string state;  // The US state that the list of plants is associated with
        List<MyPlant> allPlants = new List<MyPlant>(); // All the plants in the database
        ObservableCollection<Grouping<PlantTypes, MyPlant>> myPlants = new ObservableCollection<Grouping<PlantTypes, MyPlant>>(); // The subset of plants that match the current target
        ObservableCollection<MyPlant> shoppingListPlants = new ObservableCollection<MyPlant>(); // The subset of plants that are in the list
        MyCriteria targetPlant = new MyCriteria(); // The criteria that the user has selected so far
        double[] summaryMonthArray = new double[13]; //used for summarizing the flowering months. 0 is for max value, 1-12 are for each month
        int matchingPlantCount;
        #endregion Fields

        #region Constructors
        //This is the list of plants that match the criteria
        public ObservableCollection<Grouping<PlantTypes, MyPlant>> MyPlants
        {
            set
            {
                if (myPlants != value)
                {
                    myPlants = value;
                    OnPropertyChanged();
                }
            }
            get { return myPlants; }
        }

        //The count of plants that match the criteria. Stored as a value so that we can bind to it and show in the UI. We can't just bind
        //to list.count because the list is grouped, so we would only get the number of groups, not the total number of elements.
        public int MatchingPlantCount
        {
            set
            {
                if (matchingPlantCount != value)
                {
                    matchingPlantCount = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return matchingPlantCount;
            }
        }

        // Generic set/get for the ShoppingList
        public ObservableCollection<MyPlant> ShoppingListPlants
        {
            set
            {
                if (shoppingListPlants != value)
                {
                    shoppingListPlants = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
            get { return state; }
        }
        #endregion Constructors

        #region KeyActions
        //Attempt to toggle the cart status of the item passed in
        public bool ToggleStatus(MyPlant p)
        {
            //First, find the plant that matches the one passed in (this assumes each plant's name is spelled consistently, that
            //the name is unique, and that it exists exactly once). And then, toggle the status of that plant.
            bool value = allPlants.Find(x => x.Plant.Name == p.Plant.Name).TogglePlantCartStatus();

            //Now, regenerate the list so that it reflects the right set of plants and everybody knows it has changed
            RefreshShoppingListPlants();
            return value;
        }

        //Update the shopping list, and then generate a notification that it has changed.
        public void RefreshShoppingListPlants()
        {
            shoppingListPlants = new ObservableCollection<MyPlant>(allPlants.Where(i => i.InCart));

            //Generate notifications for everything that needs to know that the list has changed.
            OnPropertyChanged(nameof(ShoppingListPlants));
            OnPropertyChanged(nameof(SummaryMonths));
            OnPropertyChanged(nameof(SummaryMonthArray));
            OnPropertyChanged(nameof(SummarySun));
            OnPropertyChanged(nameof(SummaryTemp));
            OnPropertyChanged(nameof(SummaryWidth));
        }

        #endregion KeyActions

        #region Commands
        /* 
         * Commands 
         * These are called by various bits of UI to manipulate the lists
         */

        //Resets the shopping cart state of all the plants by walking through the list and setting each InCart to false
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
                            p.Count = 0;
                        }
                        RefreshShoppingListPlants();
                    });
                }
                return _clearCart;
            }
        }

        //Command for restoring all defaults, called from Filters page
        private Command _resetCriteria;
        public ICommand ResetCriteria
        {
            get
            {
                if (_resetCriteria == null)
                {
                    _resetCriteria = new Command(() =>
                    {
                        //set everything back to defaults
                        targetPlant = new MyCriteria();
                        OnPropertyChanged(nameof(TargetPlant));
                    });
                }
                return _resetCriteria;
            }
        }

        #endregion Commands

        #region AnalysisOperations
        /* 
         * Analysis operations
         * These methods are used to summarize the contents of the shopping list. The listview controls in the view are bound to these lists.
         * 
         * To add a new summary, add the method below, and also add a PropertyChanged event to the RefreshShoppingListPlants method.
         */
        public double[] SummaryMonthArray
        {
            get
            {
                //Get the count of plants that flower in each month
                double[] monthCount = new double[13];
                int i;

                foreach (MyPlant p in shoppingListPlants)
                {
                    //if it's all months, then add 1 to everything
                    if (p.Plant.FloweringMonths.HasFlag(FloweringMonths.AllMonths))
                    {
                        for (i = 1; i <= 12; i++)
                        {
                            monthCount[i]++;
                        }
                    }
                    else //otherwise, go through all the months
                    {
                        i = 1;
                        foreach (FloweringMonths f in floweringMonthDict.Values)
                        {
                            if (p.Plant.FloweringMonths.HasFlag(f))
                                monthCount[i]++;
                            i++;
                        }
                    }
                }//foreach

                //Find the max
                double highCount = 0;
                for (i=1; i<=12; i++)
                {   
                    if (monthCount[i] > highCount)
                        highCount = monthCount[i];
                }
                //scale each value
                for (i = 1; i <= 12; i++)
                {
                    summaryMonthArray[i] = (highCount > 0) ? (monthCount[i] / highCount) : 0 ;
                }

                return summaryMonthArray;
            }

        }
    
        public List<string> SummaryMonths
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
                    if (p.Plant.FloweringMonths.HasFlag(FloweringMonths.AllMonths))
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
                            if (p.Plant.FloweringMonths.HasFlag(f))
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

        public List<string> SummarySun
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
                    if (p.Plant.SunRequirements.HasFlag(SunRequirements.AllSunTypes))
                    {
                        for (i = 0; i < maxSun; i++)
                            sunCount[i]++;
                    }
                    else
                    {
                        foreach (SunRequirements s in sunTypeDict.Values)
                        {
                            if (p.Plant.SunRequirements.HasFlag(s))
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

        public int SummaryTemp
        {
            get
            {
                int lowTemp = -99;

                foreach(MyPlant p in shoppingListPlants)
                {
                    if (lowTemp <= p.Plant.MinWinterTempF.Value)
                        lowTemp = (int)p.Plant.MinWinterTempF.Value;
                }
                return lowTemp;
            }
        }

        public int SummaryWidth
        {
            get
            {
                int totalWidth = 0;

                foreach (MyPlant p in shoppingListPlants)
                {
                    totalWidth += (int) (p.Plant.MaxWidth.Value * p.Plant.MaxWidth.Value * p.Count);
                }
                return totalWidth;
            }
        }
        #endregion AnalysisOperations

        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INPC
    }

}
