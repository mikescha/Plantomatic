using PlantMan.Plant;
using System.Windows.Input;
using Xamarin.Forms;

namespace PlantomaticVM
{
    public class MyCriteria
    {
        Plant.AssignableDecimal minWinterTempF = new Plant.AssignableDecimal();
        FloweringMonths floweringMonths;
        SunRequirements sunRequirements;
        PlantTypes plantTypes;
        Plant.AssignableDecimal maxHeight = new Plant.AssignableDecimal();
        Plant.AssignableDecimal maxWidth = new Plant.AssignableDecimal();

        decimal defaultLowTemp = 40;
        FloweringMonths defaultFloweringMonths = FloweringMonths.AllMonths;
        SunRequirements defaultSunRequirements = SunRequirements.AllSunTypes;

        // We are using FALSE for the attracts to mean, "I don't care" instead of No, so that we don't have to support a 3-state switch and because
        // it seems unlikely that the user will want to choose plants that explicitly DO NOT attract wildlife. 
        bool defaultAttracts = false;

        PlantTypes defaultPlantType = PlantTypes.AllPlantTypes;
        decimal defaultMaxHeight = 300;
        decimal defaultMaxWidth = 300;
        bool defaultCounty = true;
       
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
        
        //TODO do I need to make this like the other properties, or should I make the other properties like this?
        public bool NativeTo_Alameda { get; set; }
        public bool NativeTo_Contra_Costa { get; set; }
        public bool NativeTo_Marin { get; set; }
        public bool NativeTo_Napa { get; set; }
        public bool NativeTo_San_Francisco { get; set; }
        public bool NativeTo_San_Mateo { get; set; }
        public bool NativeTo_Santa_Clara  { get; set; }
        public bool NativeTo_Solano { get; set; }
        public bool NativeTo_Sonoma { get; set; }

        public bool AttractsNativeBees { get; set; }
        public bool AttractsButterflies { get; set; }
        public bool AttractsHummingbirds { get; set; }
        public bool AttractsBirds { get; set; }
    }
}

