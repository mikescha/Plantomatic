using Xamarin.Forms;
using System.Collections.Generic;

/* Update on current design
 * 
 * There is one view model that stores everything. It has the master list of all plants, the filtered list of plants, the criteria that we're using for 
 * filtering stored as a plant, and the name of the state in which these plants live.
 * 
 * In this file, we create a class called AppData. This class instantiates the view model that holds the data. Because the XML files have this line at the top:
 *
 *     BindingContext="{Binding Source={x:Static Application.Current}, Path=AppData}">
 * 
 * This allows all pages to reference this single data structure.
 * 
 */


namespace PlantomaticVM
{
    public class App : Application
    {
        public App()
        {
            //In theory, this should instantiate our view model and make it accessible to the other pages
            AppData = new AppData();

            // Load previous AppData if it exists.
            if (Properties.ContainsKey("appData"))
            {
                //Load the list of the scientific names of the plants the user wants 
                List<string> shoppingList = AppData.Deserialize((string)Properties["appData"]);

                if (shoppingList != null)
                {
                    //rehydrate the list
                    foreach (string sciName in shoppingList)
                    {
                        //Check that the plant exists before adding it to the cart. Store this in a variable so we don't do the query twice. The query
                        //returns a reference to the actual object, not a copy of it, so this works as expected.
                        MyPlant p = this.AppData.MasterViewModel.PlantList.AllPlants.Find(x => x.ScientificName == sciName);

                        if (p != null)
                            p.InCart = true;
                    }
                    this.AppData.MasterViewModel.PlantList.RefreshShoppingListPlants();
                }
            }//end if Properties

            MainPage = new NavigationPage(new TopLevelPage());
        }

        public AppData AppData { private set; get; }

        protected override void OnStart()
        {
            // Handle when the app starts
        }

        protected override void OnSleep()
        {
            // Handle when the app sleeps
            // Save the shopping list
            Properties["appData"] = AppData.Serialize();

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
