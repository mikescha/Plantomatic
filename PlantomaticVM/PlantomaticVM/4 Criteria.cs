using PlantMan.Plant;
using System.Windows.Input;
using Xamarin.Forms;

namespace PlantomaticVM
{
    public class MyCriteria
    {
        decimal lowTemp;
        FloweringMonths floweringMonths;
        SunRequirements sunRequirements;
        YesNoMaybe attractsBirds;

        decimal defaultLowTemp = 32;
        FloweringMonths defaultFloweringMonths = FloweringMonths.AllMonths;
        SunRequirements defaultSunRequirements = SunRequirements.AllSunTypes;
        YesNoMaybe defaultAttractsBirds = YesNoMaybe.Unassigned;

        // Constructor
        public MyCriteria()
        {
            lowTemp = defaultLowTemp;
            FloweringMonths = defaultFloweringMonths;
            SunRequirements = defaultSunRequirements;
            AttractsBirds = defaultAttractsBirds;
        }

        public decimal LowTemp
        {
            set
            {
                if (lowTemp != value)
                {
                    lowTemp = value;
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

        public YesNoMaybe AttractsBirds
        {
            set
            {
                if (attractsBirds != value)
                {
                    attractsBirds = value;
                }
            }
            get { return attractsBirds; }
        }

    }
}

