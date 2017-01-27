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
                mainGrid.ColumnDefinitions[0].Width = new GridLength(2, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(0);

                mainGrid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Auto);
                mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                mainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Auto);
                mainGrid.RowDefinitions[3].Height = new GridLength(3, GridUnitType.Star);

                Grid.SetRow(summaryHeadingGrid, 2);
                Grid.SetColumn(summaryHeadingGrid, 0);
                Grid.SetRow(summaryView, 3);
                Grid.SetColumn(summaryView, 0);

                buttonHideSummary.IsVisible = true;
            }
            else //Landscape view
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                mainGrid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Auto);
                mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                mainGrid.RowDefinitions[2].Height = new GridLength(0);
                mainGrid.RowDefinitions[3].Height = new GridLength(0);

                Grid.SetRow(summaryHeadingGrid, 0);
                Grid.SetColumn(summaryHeadingGrid, 1);
                Grid.SetRow(summaryView, 1);
                Grid.SetColumn(summaryView, 1);

                buttonHideSummary.Text = "Close";
                buttonHideSummary.IsVisible = false;
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

        private void ShoppingListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            /*
            detailView.IsVisible = true;
            summaryView.IsVisible = false;
            shoppingListView.IsVisible = false;
            labelTitle.IsVisible = false;
            buttonClearCart.IsVisible = false;
            buttonCloseDetail.IsVisible = true;*/
            detailGrid.IsVisible = true;
            mainGrid.IsVisible = false;
        }

        private void ButtonCloseDetail_Clicked(object sender, EventArgs e)
        {
            /* detailView.IsVisible = false;
            summaryView.IsVisible = true;
            shoppingListView.IsVisible = true;
            labelTitle.IsVisible = true;
            buttonClearCart.IsVisible = true;
            buttonCloseDetail.IsVisible = false;
            */
            detailGrid.IsVisible = false;
            mainGrid.IsVisible = true;

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

        //used to hide the Summary panel in portrait view. I don't bother checking for screen dimensions before doing this
        //since the button is hidden by the page resize code when the window is too small
        private void buttonHideSummary_Clicked(object sender, EventArgs e)
        {
            var converter = new GridLengthTypeConverter();

            // if it's star then the area is expanded and we should collapse it
            if (mainGrid.RowDefinitions[3].Height.IsStar)
            {
                mainGrid.RowDefinitions[3].Height = new GridLength(0);
                buttonHideSummary.Text = "Open";
            }
            else
            {
                mainGrid.RowDefinitions[3].Height = new GridLength(3, GridUnitType.Star);
                buttonHideSummary.Text = "Close";
            }
        }
    }
}
