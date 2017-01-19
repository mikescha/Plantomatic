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

                mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                mainGrid.RowDefinitions[2].Height = new GridLength(3, GridUnitType.Star);

                Grid.SetRow(summaryView, 2);
                Grid.SetColumn(summaryView, 0);
            }
            else //Landscape view
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                mainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Auto);

                Grid.SetRow(summaryView, 1);
                Grid.SetColumn(summaryView, 1);
            }
        }

        //TODO The "Refresh list" call is because I can't figure out how to get it to update from the Plant List page without
        //having a button that the user explicitly has to click. 
        protected override void OnAppearing()
        {
            base.OnAppearing();
            AppData appData = (AppData)BindingContext;
            appData.MasterViewModel.PlantList.RefreshShoppingListPlants();
        }

        private void shoppingListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            detailView.IsVisible = true;
            shoppingListView.IsVisible = false;
            labelTitle.IsVisible = false;
            buttonClearCart.IsVisible = false;
            buttonCloseDetail.IsVisible = true;
        }

        private void buttonCloseDetail_Clicked(object sender, EventArgs e)
        {
            detailView.IsVisible = false;
            shoppingListView.IsVisible = true;
            labelTitle.IsVisible = true;
            buttonClearCart.IsVisible = true;
            buttonCloseDetail.IsVisible = false;

            //refresh the list so that it shows the proper plants, in case the user removed one
            AppData appData = (AppData)BindingContext;
            appData.MasterViewModel.PlantList.RefreshShoppingListPlants();
        }

        private void OnMoreInfoTapped(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            MyPlant thePlant = (MyPlant)label.BindingContext;
            if (thePlant.Plant.URL != null)
            {
                Navigation.PushAsync(new MoreInfoPage(thePlant.Plant.URL));
            }
            else
            {
                //TODO: what to do if there is no URL for the plant
            }
        }
    }
}
