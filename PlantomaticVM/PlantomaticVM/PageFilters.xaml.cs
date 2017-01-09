using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PlantomaticVM
{
    public partial class FiltersPage : ContentPage
    {
        public FiltersPage()
        {
            InitializeComponent();
        }

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

        void OnPageSizeChanged(object sender, EventArgs args)
        {
            //Put filtersSet1 into R0C0
            Grid.SetRow(filtersSet1, 0);
            Grid.SetColumn(filtersSet1, 0);
            
            // Portrait mode. 
            if (Width < Height)
            {
                //Make the page skinnier, so set the second column to 0 width and move filtersSet2 into R2C0
                bodyGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                bodyGrid.ColumnDefinitions[1].Width = new GridLength(0);
                Grid.SetRow(filtersSet2, 1);
                Grid.SetColumn(filtersSet2, 0);
            }
            // Landscape mode. 
            else
            {
                //Make the page wider, so make the second column wide and move filtersSet2 into R1C1
                bodyGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                bodyGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                Grid.SetRow(filtersSet2, 0);
                Grid.SetColumn(filtersSet2, 1);
            }//end else

        }//end OnPageSizeChanged
    }
}
