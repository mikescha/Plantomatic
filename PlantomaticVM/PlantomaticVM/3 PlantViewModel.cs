using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using PlantMan.Plant;

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
                    OnPropertyChanged("NameInCart");
                }
            }
            get
            {
                return nameInCart;
            }
        }

        public string SetNameInCart()
        {
            if (Count > 0)
                NameInCart = Plant.Name + " (" + Count.ToString() + ")";
            else
                NameInCart = Plant.Name;

            return NameInCart;
        }

        public Plant Plant
        {
            set {
                if (plant != value)
                {
                    plant = value;
                    OnPropertyChanged("Plant");
                }
            }
            get {
                return plant;
            }
        }

        // The state of whether a plant is in the shopping cart or not
        public bool InCart
        {
            set
            {
                if (inCart != value)
                {
                    inCart = value;

                    if (inCart)
                    {
                        //it just turned true, so it used to be false. Make sure there is at least 1 plant in the cart
                        Count = 1;
                    }
                    else
                    {
                        //it is false, so it used to be true. Make sure the cart is empty.
                        Count = 0;
                    }
                    OnPropertyChanged("InCart");
                }
            }
            get { return inCart; }
        }

        // The state of whether a plant is in the shopping cart or not
        public int Count
        {
            set
            {
                if (count != value)
                {
                    count = value;
                    SetNameInCart();

                    //ensure the count and the cart flag stay in sync
                    if (count > 0)
                        inCart = true;
                    else
                        inCart = false;

                    OnPropertyChanged("Count");
                    OnPropertyChanged("InCart");
                    OnPropertyChanged("NameInCart");
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

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
