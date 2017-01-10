using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PlantomaticVM
{
    public partial class PlantCriteriaPage : ContentPage
    {
        public PlantCriteriaPage()
        {
            InitializeComponent();
        }

        void OnPageSizeChanged(object sender, EventArgs args)
        {
            
            //Portrait
            if (Width < Height)
            {
                //for the simple controls
                simpleGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                simpleGrid.ColumnDefinitions[1].Width = new GridLength(0);
                
                Grid.SetRow(buttonWinterFlowers, 1);
                Grid.SetColumn(buttonWinterFlowers, 0);

                Grid.SetRow(buttonBirds, 2);
                Grid.SetColumn(buttonBirds, 0);

                Grid.SetRow(buttonPollenators, 3);
                Grid.SetColumn(buttonPollenators, 0);

                Grid.SetRow(buttonShady, 4);
                Grid.SetColumn(buttonShady, 0);

                Grid.SetRow(buttonContainers, 5);
                Grid.SetColumn(buttonContainers, 0);

                Grid.SetRow(buttonSmallYard, 6);
                Grid.SetColumn(buttonSmallYard, 0);


                //for the advanced controls
                bodyGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                bodyGrid.ColumnDefinitions[1].Width = new GridLength(0);
                Grid.SetRow(filtersSet2, 1);
                Grid.SetColumn(filtersSet2, 0);
            }
            else //Landscape
            {
                simpleGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                simpleGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                Grid.SetRow(buttonWinterFlowers, 1);
                Grid.SetColumn(buttonWinterFlowers, 0);

                Grid.SetRow(buttonBirds, 2);
                Grid.SetColumn(buttonBirds, 0);

                Grid.SetRow(buttonPollenators, 3);
                Grid.SetColumn(buttonPollenators, 0);

                Grid.SetRow(buttonShady, 1);
                Grid.SetColumn(buttonShady, 1);

                Grid.SetRow(buttonContainers, 2);
                Grid.SetColumn(buttonContainers, 1);

                Grid.SetRow(buttonSmallYard, 3);
                Grid.SetColumn(buttonSmallYard, 1);


                //for the advanced controls
                bodyGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                bodyGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                Grid.SetRow(filtersSet2, 0);
                Grid.SetColumn(filtersSet2, 1);
            }
            
        } //end OnPageSizeChanged
        
        //These two methods are used to hide/show the appropriate controls
        private void buttonAdvanced_Clicked(object sender, EventArgs e)
        {
            simpleGrid.IsVisible = false;
            advancedGrid.IsVisible = true;
        }

        private void buttonSimple_Clicked(object sender, EventArgs e)
        {
            simpleGrid.IsVisible = true;
            advancedGrid.IsVisible = false;
        }

        //These methods are only used by the Advanced options
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

    }
}
