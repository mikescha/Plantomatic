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

        }

        //TODO Anything that should happen when this page appears?
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
