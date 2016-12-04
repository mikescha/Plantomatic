﻿using System;
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
            //test comment

            //Set up defaults
            //PlantList.TargetPlant.LowTemp = 32;
            //PlantList.TargetPlant.FloweringMonths = FloweringMonths.AllMonths;
            //PlantList.TargetPlant.SunRequirements = SunRequirements.AllSunTypes;
            //PlantList.TargetPlant.AttractsBirds = YesNoMaybe.Unassigned;
            
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

        //Filter the list
        public void FilterPlantList()
        {
            //Since we can't cast a List to an ObservableCollection, make a temp list to operate on and then copy each item
            List<MyPlant> list = new List<MyPlant>();

            list = PlantList.AllPlants
                .Where(p => (PlantList.TargetPlant.FloweringMonths != FloweringMonths.AllMonths && p.FloweringMonths.HasFlag(PlantList.TargetPlant.FloweringMonths)) ||
                    (PlantList.TargetPlant.FloweringMonths == FloweringMonths.AllMonths))
                .Where(p => (PlantList.TargetPlant.SunRequirements != SunRequirements.AllSunTypes && p.SunRequirements.HasFlag(PlantList.TargetPlant.SunRequirements)) ||
                    (PlantList.TargetPlant.SunRequirements == SunRequirements.AllSunTypes))
                .Where(p => (p.LowTemp <= PlantList.TargetPlant.LowTemp))
                .Where(p => (PlantList.TargetPlant.AttractsBirds == YesNoMaybe.Unassigned) ||
                    (p.AttractsBirds == PlantList.TargetPlant.AttractsBirds))
                .OrderBy(p => p.ScientificName)
                .ToList();

            //Clear the list if there is already something in it
            PlantList.MyPlants = new ObservableCollection<MyPlant>();

            foreach (MyPlant p in list)
                PlantList.MyPlants.Add(p);


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
