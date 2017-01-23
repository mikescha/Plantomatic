using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using PlantMan.Plant;
using System.Runtime.CompilerServices;

namespace PlantomaticVM
{
    public class MyPlant : INotifyPropertyChanged
    {
        Plant plant;
        bool inCart;
        int count;
        string nameInCart;

        public MyPlant()
        {
            Plant = new Plant();
            InCart = false;
            Count = 1;
            NameInCart = "";
        }

        // Initialize a plant when one is passed in
        public MyPlant(Plant newPlant)
        {
            plant = newPlant;
            InCart = false;
            Count = 0;
            SetNameInCart();
        }

        public string NameInCart
        {
            set
            {
                if (nameInCart != value)
                {
                    nameInCart = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return nameInCart;
            }
        }

        // Display names for plants have two versions. 
        //    -- When count is greater then zero, display: "Sunflower (1)"
        //    -- If count is zero, then don't display the parentheses: "Sunflower"
        public string SetNameInCart()
        {
            NameInCart = Plant.Name + (Count > 0 ? (" (" + Count.ToString() + ")") : "");
            
            return NameInCart;
        }

        public Plant Plant
        {
            set {
                if (plant != value)
                {
                    plant = value;
                    OnPropertyChanged();
                }
            }
            get {
                return plant;
            }
        }

        // The state of whether a plant is in the shopping cart or not. This is independent of the count to 
        // simplify data binding.
        public bool InCart
        {
            set
            {
                if (inCart != value)
                {
                    inCart = value;

                    //It just turned true, so it used to be false. Make sure there is at least 1 plant in the cart. 
                    //Else zero it out
                    Count = (inCart) ? 1 : 0;

                    OnPropertyChanged();
                }
            }
            get { return inCart; }
        }

        // The count of how many plants the user wants in their shopping cart
        public int Count
        {
            set
            {
                if (count != value)
                {
                    count = (value >= 0) ? value : 0; //ensure that we don't decrement to negative numbers

                    //Since the name reflects the count, we need to update the name
                    SetNameInCart();

                    //Ensure the count and the cart flag stay in sync. Also, this lets the user set the
                    //count for a plant to zero and have the plant be removed from the cart.
                    inCart = (count > 0) ? true : false;
                    
                    //Tell the world that everything has changed
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(InCart));
                    OnPropertyChanged(nameof(NameInCart));
                }
            }
            get { return count; }
        }

        public bool TogglePlantCartStatus()
        {
            if (InCart)
            {
                InCart = false;
                Count = 0;
            }
            else
            {
                InCart = true;
                Count = 1;
            }
                
            return InCart;
        }

        public bool ClearCart()
        {
            InCart = false;
            Count = 0;

            return InCart;
        }

        //Command for clearing item from cart
        private Command _removeItem;
        public ICommand RemoveItem
        {
            get
            {
                if (_removeItem == null)
                {
                    _removeItem = new Command(() =>
                    {
                        ClearCart();
                    });
                }
                return _removeItem;
            }
        }

        //Command for adding one plant
        private Command _incrementPlantCount;
        public ICommand IncrementPlantCount
        {
            get
            {
                if (_incrementPlantCount == null)
                {
                    _incrementPlantCount = new Command(() =>
                    {
                        Count++;
                    });
                }
                return _incrementPlantCount;
            }
        }

        //Command for removing one plant
        private Command _decrementPlantCount;
        public ICommand DecrementPlantCount
        {
            get
            {
                if (_decrementPlantCount == null)
                {
                    _decrementPlantCount = new Command(() =>
                    {
                        Count--;
                    });
                }
                return _decrementPlantCount;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        //the CallerMemberName thing means that if I don't pass in the name of the property, then the
        //name of the calling function is used by default
        void OnPropertyChanged([CallerMemberName] string propertyName="")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
