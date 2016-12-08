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
        YesNoMaybe attractsBirds;

        decimal defaultLowTemp = 32;
        FloweringMonths defaultFloweringMonths = FloweringMonths.AllMonths;
        SunRequirements defaultSunRequirements = SunRequirements.AllSunTypes;
        YesNoMaybe defaultAttractsBirds = YesNoMaybe.Unassigned;

        // Constructor
        public MyCriteria()
        {
            MinWinterTempF.Value = defaultLowTemp;
            FloweringMonths = defaultFloweringMonths;
            SunRequirements = defaultSunRequirements;
            AttractsBirds = defaultAttractsBirds;
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

