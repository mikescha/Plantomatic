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

                Grid.SetRow(buttonStack1, 1);
                Grid.SetColumn(buttonStack1, 0);
                Grid.SetRow(buttonStack2, 2);
                Grid.SetColumn(buttonStack2, 0);

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

                Grid.SetRow(buttonStack1, 1);
                Grid.SetColumn(buttonStack1, 0);
                Grid.SetRow(buttonStack2, 1);
                Grid.SetColumn(buttonStack2, 1);

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
