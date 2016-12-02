using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using PlantMan.Plant;

namespace PlantomaticVM
{
    public class PlantList : INotifyPropertyChanged
    {
        string state;  // The US state that the list of plants is associated with
        List<MyPlant> allPlants = new List<MyPlant>(); // All the plants in the database
        ObservableCollection<MyPlant> myPlants = new ObservableCollection<MyPlant>(); // The subset of plants that match the current target
        ObservableCollection<MyPlant> shoppingListPlants = new ObservableCollection<MyPlant>(); // The subset of plants that are in the list
        MyPlant targetPlant = new MyPlant(); // The criteria that we're trying to match

        
        public string State
        {
            set
            {
                if (state != value)
                {
                    state = value;
                    OnPropertyChanged("State");
                }
            }
            get { return state; }
        }
        
        public ObservableCollection<MyPlant> MyPlants
        {
            set
            {
                if (myPlants != value)
                {
                    myPlants = value;
                    OnPropertyChanged("MyPlants");
                }
            }
            get { return myPlants; }
        }

        // When the UI needs the list of plants in the shopping list, we do a LINQ query to get the right set
        public ObservableCollection<MyPlant> ShoppingListPlants
        {
            set
            {
                if (shoppingListPlants != value)
                {
                    shoppingListPlants = value;
                    OnPropertyChanged("ShoppingListPlants");
                }
            }
            get
            {
                ObservableCollection<MyPlant> list = new ObservableCollection<MyPlant>(myPlants.Where(i => i.InCart));
                OnPropertyChanged("ShoppingListPlants");
                return list;
            }
        }

        public List<MyPlant> AllPlants
        {
            set
            {
                if (allPlants != value)
                {
                    allPlants = value;
                    OnPropertyChanged("AllPlants");
                }
            }
            get { return allPlants; }
        }

        // This holds all the criteria used in filtering
        // TODO how do i initialize this?
        public MyPlant TargetPlant
        {
            set
            {
                if (targetPlant != value)
                {
                    targetPlant = value;
                    OnPropertyChanged("TargetPlant");
                }
            }
            get { return targetPlant; }
        }
       
        private Command _clearCart;
        public ICommand ClearCart
        {
            get
            {
                if (_clearCart == null)
                {
                    _clearCart = new Command(() =>
                    {
                        foreach(MyPlant p in allPlants)
                        {
                            p.InCart = false;
                        }
                        OnPropertyChanged("ShoppingListPlants");
                    });
                }
                return _clearCart;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
