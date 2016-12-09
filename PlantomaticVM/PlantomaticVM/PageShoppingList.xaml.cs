using System;
using Xamarin.Forms;

namespace PlantomaticVM
{
    public partial class ShoppingListPage : ContentPage
    {
        public ShoppingListPage()
        {
            InitializeComponent();
        }

        // Handles user wanting to remove an item from the shopping list
        public void OnContextMenuClicked(object sender, EventArgs args)
        {
            MenuItem m = (MenuItem)sender;
            MyPlant p = (MyPlant) m.BindingContext;
            AppData appData = (AppData)BindingContext;
            appData.MasterViewModel.PlantList.ToggleStatus(p);

        }

        //TODO What happens when page size changes
        void OnPageSizeChanged(object sender, EventArgs args)
        {
            //Portrait view
            if (Width < Height)
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(0);

                mainGrid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);

                Grid.SetRow(summaryView, 1);
                Grid.SetColumn(summaryView, 0);
            }
            else //Landscape view
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                mainGrid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Auto);
                mainGrid.RowDefinitions[1].Height = new GridLength(0);

                Grid.SetRow(summaryView, 0);
                Grid.SetColumn(summaryView, 1);
            }
        }

        //TODO The "Update list" call is because I can't figure out how to get it to update from the Plant List page without
        //having a button that the user explicitly has to click. 
        protected override void OnAppearing()
        {
            base.OnAppearing();
            AppData appData = (AppData)BindingContext;
            appData.MasterViewModel.PlantList.RefreshShoppingListPlants();
        }
    }
}
