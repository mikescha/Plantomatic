using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using PlantMan.Plant;

/* To add a property:
 * 1) Add a definition
 * 2) Add a constructor
 * 2.5) Add to the initializer in MyPlant
 * 3) Add to TargetPlant definition
 * 4) Add to filter query
 * 5) Add to filters page
*/
namespace PlantomaticVM
{
    public class MyPlant : INotifyPropertyChanged
    {
        string name;
        string scientificName;
        decimal lowTemp;
        FloweringMonths floweringMonths;
        SunRequirements sunRequirements;
        string moreInfoURL;
        YesNoMaybe attractsBirds;
        bool inCart;       
               
        public MyPlant()
        {
            //TODO is there anything i should do here?
            
        }

        // Initialize a plant with all the data I need.
        // TODO Should I just encapsulate the "Plant" inside this instead of breaking out each field? I have to know about the fields either way.
        public MyPlant(Plant newPlant)
        {
            name = newPlant.Name;
            scientificName = newPlant.ScientificName;
            lowTemp = newPlant.MinWinterTempF.Value;
            floweringMonths = newPlant.FloweringMonths;
            sunRequirements = newPlant.SunRequirements;
            attractsBirds = newPlant.AttractsBirds;
            moreInfoURL = newPlant.URL;
            inCart = false;
        }
 
        // The state of whether a plant is in the shopping cart or not
        public bool InCart
        {
            set
            {
                if (inCart != value)
                {
                    inCart = value;
                    OnPropertyChanged("InCart");
                }
            }
            get { return inCart; }
        }

        public string Name
        {
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
            get { return name; }
        }

        public string ScientificName
        {
            set
            {
                if (scientificName != value)
                {
                    scientificName = value;
                    OnPropertyChanged("ScientificName");
                }
            }
            get { return scientificName; }
        }

        public decimal LowTemp
        {
            set
            {
                if (lowTemp != value)
                {
                    lowTemp = value;
                    OnPropertyChanged("LowTemp");
                }
            }
            get { return lowTemp; }
        }

        public FloweringMonths FloweringMonths
        {
            set
            { 
                if (floweringMonths != value)
                {
                    floweringMonths = value;
                    OnPropertyChanged("FloweringMonths");
                }
            }
            get { return floweringMonths; }
        }

        public SunRequirements SunRequirements
        {
            set
            {
                if (sunRequirements != value)
                {
                    sunRequirements = value;
                    OnPropertyChanged("SunRequirements");
                }
            }
            get { return sunRequirements; }
        }

        public string MoreInfoURL
        {
            set
            {
                if (moreInfoURL != value)
                {
                    moreInfoURL = value;
                    OnPropertyChanged("MoreInfoURL");
                }
            }
            get { return moreInfoURL; }
        }

        public YesNoMaybe AttractsBirds
        {
            set
            {
                if (attractsBirds != value)
                {
                    attractsBirds = value;
                    OnPropertyChanged("AttractsBirds");
                }
            }
            get { return attractsBirds; }
        }

        public PlantList PlantList { set; get; }

        public bool ToggleListStatus()
        {
            InCart = !InCart;
            return InCart;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /* Should be fully deprecated, leaving it in but moving to a comment, just in case.
        private Command _toggleCartStatus;
        public ICommand ToggleCartStatus
        {
            get
            {
                if (_toggleCartStatus == null)
                {
                    _toggleCartStatus = new Command(() => {
                        InCart = !InCart;
                    });
                }
                return _toggleCartStatus;
            }
        }
        */
    }
}
