using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantMan.Plant;
using Xamarin.Forms;

/* What I'm trying to do next..
 * 3) When you toggle the Add/Remove state of the cart while the ShoppingLIst is showing, then I'd like that item to immediately be removed 
 * 
 * If this works, then get rid of the Shopping List page, and change the layout code to have a way to show the summary somewhere. OR, leave 
 * the separate page, but have it be all about the summary, e.g. which months you'll have flowers, how many square feet you'll need, etc. (and so should I 
 * let people type in the number of plants they want in their cart, so we can calculate sq ft needed?) i.e. change the Add TO Cart from a BOOL to INT
 * 
 * Then, I'd like to 
 * 3) add more filters
 * 5) add grouping to the list of plants https://mallibone.com/post/xamarin.forms-grouped-listview
 * 6) Add multiple selection to the list of flowering months http://thatcsharpguy.com/post/multiselect-listview-mvvm-en/
 * 7) why, when the app is resumed (alt tab away, then back), does the list not redraw?
 */
namespace PlantomaticVM
{
    public partial class PlantListPage : ContentPage
    {
        public PlantListPage()
        {
            InitializeComponent();
        }

        //TODO Either figure out how to tell in code if sender is a Button or a Menu, or make a separate function that does that
        public void OnContextMenuClicked(object sender, EventArgs args)
        {
            Button b = (Button)sender;
            MyPlant p = (MyPlant)b.BindingContext;
            AppData appData = (AppData)BindingContext;
            appData.MasterViewModel.PlantList.ToggleStatus(p);

        }


        void OnPageSizeChanged(object sender, EventArgs args)
        { 
            // Portrait mode. 
            if (Width < Height)
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(0);
                mainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                mainGrid.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
                Grid.SetRow(detailLayout, 3);
                Grid.SetColumn(detailLayout, 0);
            }
            // Landscape mode. 
            else
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
                mainGrid.RowDefinitions[3].Height = new GridLength(0);
                Grid.SetRow(detailLayout, 2);
                Grid.SetColumn(detailLayout, 1);
            }//end else
            
        }//end OnPageSizeChanged

        void OnPlantSelected(object sender, EventArgs args)
        {
            //These controls are hidden by default so that they don't show when the page first loads and there is no selection. So, make sure 
            //they get shown as visible when an item is selected
            MoreInfoURLLabel.IsVisible = true;
            AddToCartButton.IsVisible = true;
        }

        async void OnMoreInfoTapped(object sender, EventArgs e)
        {
            MyPlant thePlant = (MyPlant) detailLayout.BindingContext;
            if (thePlant.MoreInfoURL != null)
            {
                MoreInfoPage page = new MoreInfoPage(thePlant.MoreInfoURL);
                
                await Navigation.PushAsync(page);
            }
            else
            {
                //TODO: what to do if there is no URL for the plant
            }
        }

        void OnToggleViewButtonClicked(object sender, EventArgs e)
        {            
            //TODO Is this the way you're supposed to get the current state of the app? Or is it best to do it another way?
            AppData appData = (AppData)BindingContext;

            //Switch the source of the listview to be the proper list
            listView.ItemsSource = appData.MasterViewModel.ShowingShoppingList ? appData.MasterViewModel.PlantList.MyPlants : appData.MasterViewModel.PlantList.ShoppingListPlants;
            
            //toggle the state so we keep track of what we did for the next toggle
            appData.MasterViewModel.ShowingShoppingList = !appData.MasterViewModel.ShowingShoppingList;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Anything to do here?
        }
    }
}