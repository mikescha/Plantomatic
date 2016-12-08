using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using PlantMan.Plant;
using Importer.CSV2toPlantV4;

namespace PlantomaticVM
{
    public class PlantListViewModel : INotifyPropertyChanged
    {
        PlantList plantList;
        bool showingShoppingList = false;
        
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

        //Filter the list to show only plants that match Target, which is an element of PlantList
        public void FilterPlantList()
        {
            //Since we can't cast a List to an ObservableCollection, make a temp list to operate on and then copy each item
            List<MyPlant> list = new List<MyPlant>();

            list = PlantList.AllPlants
                .Where(p => (PlantList.TargetPlant.FloweringMonths != FloweringMonths.AllMonths && p.Plant.FloweringMonths.HasFlag(PlantList.TargetPlant.FloweringMonths)) ||
                    (PlantList.TargetPlant.FloweringMonths == FloweringMonths.AllMonths))
                .Where(p => (PlantList.TargetPlant.SunRequirements != SunRequirements.AllSunTypes && p.Plant.SunRequirements.HasFlag(PlantList.TargetPlant.SunRequirements)) ||
                    (PlantList.TargetPlant.SunRequirements == SunRequirements.AllSunTypes))
                .Where(p => (p.Plant.MinWinterTempF.Value <= PlantList.TargetPlant.MinWinterTempF.Value))
                .Where(p => (PlantList.TargetPlant.AttractsBirds == YesNoMaybe.Unassigned) ||
                    (p.Plant.AttractsBirds == PlantList.TargetPlant.AttractsBirds))
                .OrderBy(p => p.Plant.ScientificName)
                .ToList();

            //MyPlants is the field that stores the matching plants. Here, we take the list and convert it to an ObservableCollection for the UI 
            PlantList.MyPlants = new ObservableCollection<MyPlant>(list);
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
