using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlantMan.Plant;
using Xamarin.Forms;

/* What I'm trying to do next..
 * 
 * TODO Need to decide still what the overall UI is going to look like.
 * If this works, then get rid of the Shopping List page, and change the layout code to have a way to show the summary somewhere. OR, leave 
 * the separate page, but have it be all about the summary, e.g. which months you'll have flowers, how many square feet you'll need, etc. 
 * (and so should I let people type in the number of plants they want in their cart, so we can calculate sq ft needed? i.e. change the Add TO Cart from a BOOL to INT)
 * 
 * Then, I'd like to 
 * 5) add grouping to the list of plants https://mallibone.com/post/xamarin.forms-grouped-listview
 * 6) Add multiple selection to the list of flowering months http://thatcsharpguy.com/post/multiselect-listview-mvvm-en/
 */
namespace PlantomaticVM
{
    public partial class PlantListPage : ContentPage
    {
        public PlantListPage()
        {
            InitializeComponent();
        }

        //TODO For some reason, even though it shows up in the debugger as having a BindingContext, the line, "(MenuItem)sender.BindingContext" fails
        //to compile. Because of this, even though both a menuitem and a button have a BindingContext, I can't reference it with a single line
        //of code. So, I have these two methods that do exactly the same thing. Is there a better way?
        public void OnToggleCartMenuClicked(object sender, EventArgs args)
        {
            MenuItem m = (MenuItem)sender;
            MyPlant p = (MyPlant)m.BindingContext;
            AppData appData = (AppData)BindingContext;
            appData.MasterViewModel.PlantList.ToggleStatus(p);
        }

        public void OnToggleCartButtonClicked(object sender, EventArgs args)
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
                mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                mainGrid.RowDefinitions[2].Height = new GridLength(3, GridUnitType.Star);
                Grid.SetRow(detailLayout, 2);
                Grid.SetColumn(detailLayout, 0);
            }
            // Landscape mode. 
            else
            {
                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                mainGrid.RowDefinitions[2].Height = new GridLength(0);
                Grid.SetRow(detailLayout, 1);
                Grid.SetColumn(detailLayout, 1);
            }//end else
            
        }//end OnPageSizeChanged

        void OnPlantSelected(object sender, EventArgs args)
        {
            //Anything to do here?
        }

        // When the "More Info" link is tapped, then open a new pop-over window and load the URL of the selected plant into this
        async void OnMoreInfoTapped(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            MyPlant thePlant = (MyPlant) label.BindingContext ;
            if (thePlant.Plant.URL != null)
            {
                await Navigation.PushAsync(new MoreInfoPage(thePlant.Plant.URL));
            }
            else
            {
                //TODO: what to do if there is no URL for the plant
            }
        }
    
        //TODO do we want to do anything here?
        protected override void OnAppearing()
        {
            base.OnAppearing();
            listView.SelectedItem = null;
        }

    }
}
 