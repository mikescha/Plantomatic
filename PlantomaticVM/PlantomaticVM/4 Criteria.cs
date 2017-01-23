using PlantMan.Plant;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PlantomaticVM
{
    public class MyCriteria : INotifyPropertyChanged
    {
        Plant.AssignableDecimal minWinterTempF = new Plant.AssignableDecimal();
        FloweringMonths floweringMonths;
        SunRequirements sunRequirements;
        PlantTypes plantTypes;
        Plant.AssignableDecimal maxHeight = new Plant.AssignableDecimal();
        Plant.AssignableDecimal maxWidth = new Plant.AssignableDecimal();
        FlowerColor flowerColors;

        public Dictionary<FlowerColor, string> FlowerColorDict = new Dictionary<FlowerColor, string>
        {
            {FlowerColor.AnyColor, "" }, {FlowerColor.Red, "Red" },{FlowerColor.Orange, "Orange" },
            {FlowerColor.Yellow, "Yellow" }, {FlowerColor.Green, "Green" }, {FlowerColor.Blue, "Blue" },
            {FlowerColor.Purple, "Purple" }, {FlowerColor.White, "White" }, {FlowerColor.Brown, "Brown" }
        };

        // We are using FALSE for the attracts to mean, "I don't care" instead of No, so that we don't have to support a 3-state switch and because
        // it seems unlikely that the user will want to choose plants that explicitly DO NOT attract wildlife. 
        bool defaultAttracts = false;
        decimal defaultLowTemp = 99;
        FloweringMonths defaultFloweringMonths = FloweringMonths.AllMonths;
        SunRequirements defaultSunRequirements = SunRequirements.AllSunTypes;
        PlantTypes defaultPlantType = PlantTypes.AllPlantTypes;
        decimal defaultMaxHeight = 300;
        decimal defaultMaxWidth = 300;
        bool defaultCounty = false;
        FlowerColor defaultFlowerColors = FlowerColor.AnyColor;
       
        // Constructor
        public MyCriteria()
        {
            //desired values
            FloweringMonths = defaultFloweringMonths;
            SunRequirements = defaultSunRequirements;
            PlantTypes = defaultPlantType;
            maxHeight.Value = defaultMaxHeight;
            maxWidth.Value = defaultMaxWidth;
                
            AttractsBirds = defaultAttracts;
            AttractsHummingbirds = defaultAttracts;
            AttractsNativeBees = defaultAttracts;
            AttractsButterflies = defaultAttracts;

            FlowerColors = defaultFlowerColors;

            //local conditions
            MinWinterTempF.Value = defaultLowTemp;
            NativeTo_Alameda = defaultCounty;
            NativeTo_Contra_Costa = defaultCounty;
            NativeTo_Marin = defaultCounty;
            NativeTo_Napa = defaultCounty;
            NativeTo_San_Francisco = defaultCounty;
            NativeTo_San_Mateo = defaultCounty;
            NativeTo_Santa_Clara = defaultCounty;
            NativeTo_Solano = defaultCounty;
            NativeTo_Sonoma = defaultCounty;

            Lat = 0;
            Lng = 0;
            UserCounty = "";

        }

        //Resets everything but location and other environmental variables to the default values
        public void ResetCriteria()
        {
            FloweringMonths = defaultFloweringMonths;
            SunRequirements = defaultSunRequirements;
            AttractsBirds = defaultAttracts;
            AttractsHummingbirds = defaultAttracts;
            AttractsNativeBees = defaultAttracts;
            AttractsButterflies = defaultAttracts;

            PlantTypes = defaultPlantType;
            maxHeight.Value = defaultMaxHeight;
            maxWidth.Value = defaultMaxWidth;

            FlowerColors = defaultFlowerColors;
        }

        public Plant.AssignableDecimal MinWinterTempF
        {
            set
            {
                if (minWinterTempF != value)
                {
                    minWinterTempF = value;
                }
            }
            get { return minWinterTempF; }
        }

        public FloweringMonths FloweringMonths
        {
            set
            {
                if (floweringMonths != value)
                {
                    floweringMonths = value;
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
                }
            }
            get { return sunRequirements; }
        }

        public PlantTypes PlantTypes
        {
            set
            {
                if (plantTypes != value)
                {
                    plantTypes = value;
                }
            }
            get { return plantTypes; }
        }

        public Plant.AssignableDecimal MaxHeight
        {
            set
            {
                if (maxHeight != value)
                {
                    maxHeight = value;
                }
            }

            get { return maxHeight; }
        }

        public Plant.AssignableDecimal MaxWidth
        {
            set
            {
                if (maxWidth != value)
                {
                    maxWidth = value;
                }
            }

            get { return maxWidth; }
        }

        public FlowerColor FlowerColors
        {
            set
            {
                if (flowerColors != value)
                {
                    flowerColors = value;
                }
            }

            get { return flowerColors; }
        }



        //TODO do I need to make this like the other properties, or should I make the other properties like this?
        bool nativeTo_Alameda;
        public bool NativeTo_Alameda
        {
            set
            {
                if (nativeTo_Alameda != value)
                {
                    nativeTo_Alameda = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return nativeTo_Alameda;
            }
        }

        bool nativeTo_Contra_Costa;
        public bool NativeTo_Contra_Costa
        {
            set
            {
                if (nativeTo_Contra_Costa != value)
                {
                    nativeTo_Contra_Costa = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return nativeTo_Contra_Costa;
            }
        }

        bool nativeTo_Marin;
        public bool NativeTo_Marin
        {
            set
            {
                if (nativeTo_Marin != value)
                {
                    nativeTo_Marin = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return nativeTo_Marin;
            }
        }

        bool nativeTo_Napa;
        public bool NativeTo_Napa
        {
            set
            {
                if (nativeTo_Napa != value)
                {
                    nativeTo_Napa = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return nativeTo_Napa;
            }
        }

        bool nativeTo_San_Francisco;
        public bool NativeTo_San_Francisco
        {
            set
            {
                if (nativeTo_San_Francisco != value)
                {
                    nativeTo_San_Francisco = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return nativeTo_San_Francisco;
            }
        }

        bool nativeTo_San_Mateo;
        public bool NativeTo_San_Mateo
        {
            set
            {
                if (nativeTo_San_Mateo != value)
                {
                    nativeTo_San_Mateo = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return nativeTo_San_Mateo;
            }
        }

        bool nativeTo_Santa_Clara;
        public bool NativeTo_Santa_Clara
        {
            set
            {
                if (nativeTo_Santa_Clara != value)
                {
                    nativeTo_Santa_Clara = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return nativeTo_Santa_Clara;
            }
        }

        bool nativeTo_Solano;
        public bool NativeTo_Solano
        {
            set
            {
                if (nativeTo_Solano != value)
                {
                    nativeTo_Solano = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return nativeTo_Solano;
            }
        }

        bool nativeTo_Sonoma;
        public bool NativeTo_Sonoma
        {
            set
            {
                if (nativeTo_Sonoma != value)
                {
                    nativeTo_Sonoma = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return nativeTo_Sonoma;
            }
        }

        public bool AttractsNativeBees { get; set; }
        public bool AttractsButterflies { get; set; }
        public bool AttractsHummingbirds { get; set; }
        public bool AttractsBirds { get; set; }

        double lat;
        public double Lat { 
            set
            {
                if (Lat != value)
                {
                    lat = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return lat;
            }
        }
        public double Lng { set; get; }

        public string UserCounty { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        //the CallerMemberName thing means that if I don't pass in the name of the property, then the
        //name of the calling function is used by default
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}

