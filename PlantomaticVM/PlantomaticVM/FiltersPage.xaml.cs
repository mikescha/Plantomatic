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

        void OnPageSizeChanged(object sender, EventArgs args)
        {
            Grid.SetRow(filtersSet1, 1);
            Grid.SetColumn(filtersSet1, 0);

            // Portrait mode. 
            if (Width < Height)
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(0);
                Grid.SetRow(filtersSet2, 2);
                Grid.SetColumn(filtersSet2, 0);
            }
            // Landscape mode. 
            else
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                Grid.SetRow(filtersSet2, 1);
                Grid.SetColumn(filtersSet2, 1);
            }//end else

        }//end OnPageSizeChanged
    }
}
