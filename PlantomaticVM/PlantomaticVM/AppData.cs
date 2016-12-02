using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace PlantomaticVM
{
    public class AppData
    {
        public AppData()
        {
            MasterViewModel = new PlantListViewModel();
        }

        //This is to make the PlantList be available at the highest level of the app, 
        //so all pages can access it.
        public PlantListViewModel MasterViewModel { set; get; }

        //Takes the current set of plants on the shopping list, and returns a string representing each scientific name stored as XML, or NULL if there is nothing in the list
        //This is to prevent having to write the entire object, so should be faster
        public string Serialize()
        {
            // if there is something in the cart, then write it to history
            if (this.MasterViewModel.PlantList.ShoppingListPlants.Count > 0)
            {
                // For each plant that is in the shopping list, add the scientific name to a smaller list. We'll use this to rehydrate later. 
                // TODO I should probably add a GUID or something to each plant and save that instead. That will prevent the list from breaking if the 
                // scientific names change later, which they could
                List<string> shoppingList = new List<string>();

                foreach (MyPlant p in this.MasterViewModel.PlantList.ShoppingListPlants)
                {
                    shoppingList.Add(p.ScientificName);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<string>)); //debug changed string to list<string>
                using (StringWriter stringWriter = new StringWriter())
                {
                    serializer.Serialize(stringWriter, shoppingList);
                    return stringWriter.GetStringBuilder().ToString();
                }
            }
            else
            {
                //hopefully this should clear the old value
                return null;
            }
        }

        //need to check whether, at the point this gets called, we already have loaded the basic data. if so, then we can change this to just 
        //set the proper fields instead of returning a list of plants (which then the caller is going to have to loop through)
        public static List<string> Deserialize(string strAppData)
        {
            if (strAppData != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                using (StringReader stringReader = new StringReader(strAppData))
                {
                    List<string> shoppingList = (List<string>)serializer.Deserialize(stringReader);
                    return shoppingList;
                }
            }
            else
            {
                return null;
            }
            
        }
    }
}
