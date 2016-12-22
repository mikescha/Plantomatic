using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using PlantMan.Plant;
using Importer.CSV2toPlantV4;
using Xamarin.Forms;
using System.Windows.Input;


namespace PlantomaticVM
{
    public class PlantListViewModel : INotifyPropertyChanged
    {
        PlantList plantList;
        public bool showingWinterFlowers;
        public bool showingShadeAndDrought;
        public bool showingContainersAndHummingbirds;
        public bool showingBirds;
        public bool showingSmallYard;
        public bool showingPollenators;


        public PlantListViewModel()
        {
            ShowingWinterFlowers = false;
            ShowingShadeAndDrought = false;
            ShowingContainersAndHummingbirds = false;
            ShowingBirds = false;
            ShowingSmallYard = false;
            ShowingPollenators = false;

            //Initialize the list with test data. When we're ready to use real data then need to change the "true" to false
            //and add some other stuff that Joe's test app uses.
            PlantImporter myPI = new PlantImporter(true);
            List<Plant> somePlants = myPI.GetTestList();

            PlantList = new PlantList();
            PlantList.State = "California";

            foreach(Plant p in somePlants)
            {
                PlantList.AllPlants.Add(new MyPlant(p));
            }

            //Generate MyPlants
            FilterPlantList();
        }

        public PlantList PlantList
        {
            protected set
            {
                if (plantList != value)
                {
                    plantList = value;
                    OnPropertyChanged("PlantList");
                }
            }
            get { return plantList; }
        }

        
        /*
                public bool ShowingShoppingList
                {
                    set
                    {
                        if (showingShoppingList != value)
                        {
                            showingShoppingList = value;
                            OnPropertyChanged("ShowingShoppingList");
                        }
                    }

                    get { return showingShoppingList; }

                }
                */

        //Filter the list to show only plants that match Target, which is an element of PlantList
        public void FilterPlantList()
        {
            //Validate that fields contain valid values before doing the query; this is here (hopefully temporarily) to 
            //address the fact that using the "grouping" buttons results in invalid states in the filters, because grouping
            //allows multiple flags per property, whereas the UI doesn't yet allow that.
            if (PlantList.TargetPlant.SunRequirements == 0 || PlantList.TargetPlant.FloweringMonths == 0)
                return;

            //Since we can't cast a List to an ObservableCollection, make a temp list to operate on and then copy each item
            List<MyPlant> list = new List<MyPlant>();

            list = PlantList.AllPlants
                .Where(p => IncludesMonths(PlantList.TargetPlant.FloweringMonths, p.Plant.FloweringMonths))
                .Where(p => IncludesSun(PlantList.TargetPlant.SunRequirements, p.Plant.SunRequirements))
                .Where(p => (PlantList.TargetPlant.PlantTypes != PlantTypes.AllPlantTypes && p.Plant.PlantTypes.HasFlag(PlantList.TargetPlant.PlantTypes)) ||
                    (PlantList.TargetPlant.PlantTypes == PlantTypes.AllPlantTypes))
                .Where(p => (p.Plant.MinWinterTempF.Value <= PlantList.TargetPlant.MinWinterTempF.Value))
                .Where(p => (p.Plant.MaxHeight.Value <= PlantList.TargetPlant.MaxHeight.Value))
                .Where(p => (p.Plant.MaxWidth.Value <= PlantList.TargetPlant.MaxWidth.Value))
                .Where(p => CrittersMatch(PlantList.TargetPlant, p.Plant))
                .Where(p => CountiesMatch(PlantList.TargetPlant, p.Plant))

                .OrderBy(p => p.Plant.ScientificName)
                .ToList();

            //MyPlants is the field that stores the matching plants. Here, we take the list and convert it to an ObservableCollection for the UI 
            PlantList.MyPlants = new ObservableCollection<MyPlant>(list);
        }

        // Returns false if the user wants a particular critter and the plant DOES NOT have it.
        // Returns true if the user wants a particular critter and the plant has it. We are treating all the critter flags as "OR"s, so that 
        //     if the user says YES to both hummingbirds and bees, then they get plants that attract either instead of only getting plants that
        //     attract both
        // Returns true if the user didn't specify that they wanted a particular critter.
        private bool CrittersMatch(MyCriteria target, Plant candidate)
        {
            bool result = false;

            if ((target.AttractsBirds && candidate.AttractsBirds.HasFlag(YesNoMaybe.Yes)) ||
                (target.AttractsHummingbirds && candidate.AttractsHummingbirds.HasFlag(YesNoMaybe.Yes)) ||
                (target.AttractsButterflies && candidate.AttractsButterflies.HasFlag(YesNoMaybe.Yes)) ||
                (target.AttractsNativeBees && candidate.AttractsNativeBees.HasFlag(YesNoMaybe.Yes)) ||
                (!target.AttractsBirds && !target.AttractsButterflies && !target.AttractsHummingbirds && !target.AttractsNativeBees))
            {
                result = true;
            }

            return result;
        }

        // Returns true if the any of the flowering months contained in Wanted matches any of the flowering months in Test
        // e.g. if Wanted = "Jan or Feb" and Test = "Feb or Mar" then true
        //      if Wanted = "Jan or Feb" and Test = "Mar or Apr" then false
        //      if Wanted = "Jan or Feb" and Test = "AllMonths" then true
        private bool IncludesMonths(FloweringMonths wanted, FloweringMonths candidate)
        {
            
            //TODO how to enumerate through the flags?
            FloweringMonths[] target = {FloweringMonths.Jan, FloweringMonths.Feb, FloweringMonths.Mar, FloweringMonths.Apr,
                                        FloweringMonths.May, FloweringMonths.Jun, FloweringMonths.Jul, FloweringMonths.Aug,
                                        FloweringMonths.Sep, FloweringMonths.Oct, FloweringMonths.Nov, FloweringMonths.Dec};
            bool result = false;

            foreach (FloweringMonths f in target)
            {
                result = result || (candidate.HasFlag(f) && wanted.HasFlag(f));
            }

            return result || wanted.HasFlag(FloweringMonths.AllMonths);
        }

        // Returns true if the any of the sun types contained in Wanted matches any of the sun types in Test
        // e.g. if Wanted = "Full or Partial" and Test = "Partial" then true
        //      if Wanted = "Shade" and Test = "Full or Partial" then false
        private bool IncludesSun(SunRequirements wanted, SunRequirements candidate)
        {
            return (wanted.HasFlag(SunRequirements.Full) && candidate.HasFlag(SunRequirements.Full)) ||
                   (wanted.HasFlag(SunRequirements.Partial) && candidate.HasFlag(SunRequirements.Partial)) ||
                   (wanted.HasFlag(SunRequirements.Shade) && candidate.HasFlag(SunRequirements.Shade)) ||
                   (wanted.HasFlag(SunRequirements.AllSunTypes));
        }

        //TODO is there a logic bug here? This will return FALSE if...
        //   -- a plant is Unknown for Napa, and the user said they want plants that are Yes for Napa, then that plant
        //   -- a plant is No for Napa, and the user said they want plants NOT in Napa
        private bool CountiesMatch(MyCriteria target, Plant candidate)
        {
            bool result = false;

            if (target.NativeTo_Alameda && candidate.NativeTo_Alameda.HasFlag(YesNoMaybe.Yes) ||
                target.NativeTo_Contra_Costa && candidate.NativeTo_Contra_Costa.HasFlag(YesNoMaybe.Yes) ||
                target.NativeTo_Marin && candidate.NativeTo_Marin.HasFlag(YesNoMaybe.Yes) ||
                target.NativeTo_Napa && candidate.NativeTo_Napa.HasFlag(YesNoMaybe.Yes) ||
                target.NativeTo_Santa_Clara && candidate.NativeTo_Santa_Clara.HasFlag(YesNoMaybe.Yes) ||
                target.NativeTo_San_Francisco && candidate.NativeTo_San_Francisco.HasFlag(YesNoMaybe.Yes) ||
                target.NativeTo_San_Mateo && candidate.NativeTo_San_Mateo.HasFlag(YesNoMaybe.Yes) ||
                target.NativeTo_Solano && candidate.NativeTo_Solano.HasFlag(YesNoMaybe.Yes) ||
                target.NativeTo_Sonoma && candidate.NativeTo_Sonoma.HasFlag(YesNoMaybe.Yes))
            {
                result = true;
            }

            return result;
        }

        public bool ShowingWinterFlowers
        {
            set
            {
                if (showingWinterFlowers != value)
                {
                    showingWinterFlowers = value;
                    OnPropertyChanged("ShowingWinterFlowers");
                }
            }

            get { return showingWinterFlowers; }

        }

        public bool ShowingShadeAndDrought
        {
            set
            {
                if (showingShadeAndDrought != value)
                {
                    showingShadeAndDrought = value;
                    OnPropertyChanged("ShowingShadeAndDrought");
                }
            }

            get { return showingShadeAndDrought; }

        }

        public bool ShowingContainersAndHummingbirds
        {
            set
            {
                if (showingContainersAndHummingbirds != value)
                {
                    showingContainersAndHummingbirds = value;
                    OnPropertyChanged("ShowingContainersAndHummingbirds");
                }
            }

            get { return showingContainersAndHummingbirds; }

        }

        public bool ShowingBirds
        {
            set
            {
                if (showingBirds != value)
                {
                    showingBirds = value;
                    OnPropertyChanged("ShowingBirds");
                }
            }

            get { return showingBirds; }

        }

        public bool ShowingPollenators
        {
            set
            {
                if (showingPollenators != value)
                {
                    showingPollenators = value;
                    OnPropertyChanged("ShowingPollenators");
                }
            }

            get { return showingPollenators; }

        }

        public bool ShowingSmallYard
        {
            set
            {
                if (showingSmallYard != value)
                {
                    showingSmallYard = value;
                    OnPropertyChanged("ShowingSmallYard");
                }
            }

            get { return showingSmallYard; }

        }

        private void ClearButtons()
        {
            ShowingWinterFlowers = false;
            ShowingShadeAndDrought = false;
            ShowingContainersAndHummingbirds = false;
            ShowingBirds = false;
            ShowingPollenators = false;
            ShowingSmallYard = false;
        }

        //Command for choosing subset of plants
        private Command _setWinterFlowers;
        public ICommand SetWinterFlowers
        {
            get
            {
                if (_setWinterFlowers == null)
                {
                    _setWinterFlowers = new Command(() =>
                    {
                        ClearButtons();
                        ShowingWinterFlowers = true;

                        PlantList.TargetPlant.ResetCriteria();
                        PlantList.TargetPlant.FloweringMonths = FloweringMonths.Dec | FloweringMonths.Jan | FloweringMonths.Feb;
                        OnPropertyChanged("TargetPlant");

                        FilterPlantList();
                    });
                }
                return _setWinterFlowers;
            }
        }

        //Command for choosing subset of plants
        private Command _setShadeAndDrought;
        public ICommand SetShadeAndDrought
        {
            get
            {
                if (_setShadeAndDrought == null)
                {
                    _setShadeAndDrought = new Command(() =>
                    {
                        ClearButtons();
                        ShowingShadeAndDrought = true;

                        PlantList.TargetPlant.ResetCriteria();
                        PlantList.TargetPlant.SunRequirements = SunRequirements.Shade | SunRequirements.Partial;
                        OnPropertyChanged("TargetPlant");

                        FilterPlantList();
                    });
                }
                return _setShadeAndDrought;
            }
        }

        //Command for choosing subset of plants
        private Command _setAttractsBirds;
        public ICommand SetAttractsBirds
        {
            get
            {
                if (_setAttractsBirds == null)
                {
                    _setAttractsBirds = new Command(() =>
                    {
                        ClearButtons();
                        ShowingBirds = true;
                        
                        PlantList.TargetPlant.ResetCriteria();
                        PlantList.TargetPlant.AttractsBirds = true;
                        OnPropertyChanged("TargetPlant");

                        FilterPlantList();
                    });
                }
                return _setAttractsBirds;
            }
        }

        //Command for choosing subset of plants
        private Command _setContainersAndHummingbirds;
        public ICommand SetContainersAndHummingbirds
        {
            get
            {
                if (_setContainersAndHummingbirds == null)
                {
                    _setContainersAndHummingbirds = new Command(() =>
                    {
                        ClearButtons();
                        ShowingContainersAndHummingbirds = true;
                        
                        PlantList.TargetPlant.ResetCriteria();
                        PlantList.TargetPlant.MaxWidth.Value = 3;
                        PlantList.TargetPlant.AttractsHummingbirds = true;
                        OnPropertyChanged("TargetPlant");

                        FilterPlantList();
                    });
                }
                return _setContainersAndHummingbirds;
            }
        }

        //Command for choosing subset of plants
        private Command _setSmallYard;
        public ICommand SetSmallYard
        {
            get
            {
                if (_setSmallYard == null)
                {
                    _setSmallYard = new Command(() =>
                    {
                        ClearButtons();
                        ShowingSmallYard = true;

                        PlantList.TargetPlant.ResetCriteria();
                        PlantList.TargetPlant.MaxWidth.Value = 4;
                        PlantList.TargetPlant.MaxHeight.Value = 4;
                        OnPropertyChanged("TargetPlant");

                        FilterPlantList();
                    });
                }
                return _setSmallYard;
            }
        }

        //Command for choosing subset of plants
        private Command _setPollenators;
        public ICommand SetPollenators
        {
            get
            {
                if (_setPollenators == null)
                {
                    _setPollenators = new Command(() =>
                    {
                        ClearButtons();
                        ShowingPollenators = true;

                        PlantList.TargetPlant.ResetCriteria();
                        PlantList.TargetPlant.AttractsButterflies = true;
                        PlantList.TargetPlant.AttractsHummingbirds = true;
                        PlantList.TargetPlant.AttractsNativeBees = true;
                        OnPropertyChanged("TargetPlant");

                        FilterPlantList();
                    });
                }
                return _setPollenators;
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
