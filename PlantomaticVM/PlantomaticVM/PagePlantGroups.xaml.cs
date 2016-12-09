using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PlantomaticVM
{
    public partial class PlantGroupsPage : ContentPage
    {
        public PlantGroupsPage()
        {
            InitializeComponent();
        }

        void OnPageSizeChanged(object sender, EventArgs args)
        {
            //Portrait
            if (Width < Height)
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(0);
                
                Grid.SetRow(buttonWinterFlowers, 1);
                Grid.SetColumn(buttonWinterFlowers, 0);

                Grid.SetRow(buttonBirds, 2);
                Grid.SetColumn(buttonBirds, 0);

                Grid.SetRow(buttonShady, 3);
                Grid.SetColumn(buttonShady, 0);

                Grid.SetRow(buttonContainers, 4);
                Grid.SetColumn(buttonContainers, 0);
            }
            else //Landscape
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                Grid.SetRow(buttonWinterFlowers, 1);
                Grid.SetColumn(buttonWinterFlowers, 0);

                Grid.SetRow(buttonBirds, 2);
                Grid.SetColumn(buttonBirds, 0);

                Grid.SetRow(buttonShady, 1);
                Grid.SetColumn(buttonShady, 1);

                Grid.SetRow(buttonContainers, 2);
                Grid.SetColumn(buttonContainers, 1);
            }
        } //end OnPageSizeChanged

    }
}
