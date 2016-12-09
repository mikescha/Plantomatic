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
        bool showingShoppingList = false;
        bool showingWinterFlowers = false;
        bool showingShadeAndDrought = false;
        bool showingHummingbirds = false;
        bool showingBirds = false;

        public PlantListViewModel()
        {
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

        public bool ShowingHummingbirds
        {
            set
            {
                if (showingHummingbirds != value)
                {
                    showingHummingbirds = value;
                    OnPropertyChanged("ShowingHummingbirds");
                }
            }

            get { return showingHummingbirds; }

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
                .Where(p => (PlantList.TargetPlant.AttractsBirds == YesNoMaybe.Unassigned) ||
                    (p.Plant.AttractsBirds == PlantList.TargetPlant.AttractsBirds))
                .Where(p => (PlantList.TargetPlant.AttractsHummingbirds == YesNoMaybe.Unassigned) ||
                    (p.Plant.AttractsHummingbirds == PlantList.TargetPlant.AttractsHummingbirds))

                .OrderBy(p => p.Plant.ScientificName)
                .ToList();

            //MyPlants is the field that stores the matching plants. Here, we take the list and convert it to an ObservableCollection for the UI 
            PlantList.MyPlants = new ObservableCollection<MyPlant>(list);
        }

        // Returns true if the any of the flowering months contained in Wanted matches any of the flowering months in Test
        // e.g. if Wanted = "Jan or Feb" and Test = "Feb or Mar" then true
        //      if Wanted = "Jan or Feb" and Test = "Mar or Apr" then false
        //      if Wanted = "Jan or Feb" and Test = "AllMonths" then true
        private bool IncludesMonths(FloweringMonths wanted, FloweringMonths test)
        {
            return (wanted.HasFlag(FloweringMonths.Jan) && test.HasFlag(FloweringMonths.Jan)) ||
                   (wanted.HasFlag(FloweringMonths.Feb) && test.HasFlag(FloweringMonths.Feb)) ||
                   (wanted.HasFlag(FloweringMonths.Mar) && test.HasFlag(FloweringMonths.Mar)) ||
                   (wanted.HasFlag(FloweringMonths.Apr) && test.HasFlag(FloweringMonths.Apr)) ||
                   (wanted.HasFlag(FloweringMonths.May) && test.HasFlag(FloweringMonths.May)) ||
                   (wanted.HasFlag(FloweringMonths.Jun) && test.HasFlag(FloweringMonths.Jun)) ||
                   (wanted.HasFlag(FloweringMonths.Jul) && test.HasFlag(FloweringMonths.Jul)) ||
                   (wanted.HasFlag(FloweringMonths.Aug) && test.HasFlag(FloweringMonths.Aug)) ||
                   (wanted.HasFlag(FloweringMonths.Sep) && test.HasFlag(FloweringMonths.Sep)) ||
                   (wanted.HasFlag(FloweringMonths.Oct) && test.HasFlag(FloweringMonths.Oct)) ||
                   (wanted.HasFlag(FloweringMonths.Nov) && test.HasFlag(FloweringMonths.Nov)) ||
                   (wanted.HasFlag(FloweringMonths.Dec) && test.HasFlag(FloweringMonths.Dec)) ||
                   (wanted.HasFlag(FloweringMonths.AllMonths));
        }

        // Returns true if the any of the sun types contained in Wanted matches any of the sun types in Test
        // e.g. if Wanted = "Full or Partial" and Test = "Partial" then true
        //      if Wanted = "Shade" and Test = "Full or Partial" then false
        private bool IncludesSun(SunRequirements wanted, SunRequirements test)
        {
            return (wanted.HasFlag(SunRequirements.Full) && test.HasFlag(SunRequirements.Full)) ||
                   (wanted.HasFlag(SunRequirements.Partial) && test.HasFlag(SunRequirements.Partial)) ||
                   (wanted.HasFlag(SunRequirements.Shade) && test.HasFlag(SunRequirements.Shade)) ||
                   (wanted.HasFlag(SunRequirements.AllSunTypes));
        }


        private void ClearButtons()
        {
            ShowingWinterFlowers = false;
            ShowingShadeAndDrought = false;
            ShowingHummingbirds = false;
            ShowingBirds = false;
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
                        PlantList.TargetPlant = new MyCriteria();
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
                        PlantList.TargetPlant = new MyCriteria();
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
                        PlantList.TargetPlant = new MyCriteria();
                        PlantList.TargetPlant.AttractsBirds = YesNoMaybe.Yes;
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
                        ShowingHummingbirds = true;
                        PlantList.TargetPlant = new MyCriteria();
                        PlantList.TargetPlant.MaxWidth.Value = 2;
                        PlantList.TargetPlant.AttractsHummingbirds = YesNoMaybe.Yes;
                        OnPropertyChanged("TargetPlant");
                        FilterPlantList();
                    });
                }
                return _setContainersAndHummingbirds;
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
