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
        PlantTypes plantTypes;
        Plant.AssignableDecimal maxHeight = new Plant.AssignableDecimal();
        Plant.AssignableDecimal maxWidth = new Plant.AssignableDecimal();

        decimal defaultLowTemp = 32;
        FloweringMonths defaultFloweringMonths = FloweringMonths.AllMonths;
        SunRequirements defaultSunRequirements = SunRequirements.AllSunTypes;
        YesNoMaybe defaultAttractsBirds = YesNoMaybe.Unassigned;
        PlantTypes defaultPlantType = PlantTypes.AllPlantTypes;
        decimal defaultMaxHeight = 6;
        decimal defaultMaxWidth = 6;

        // Constructor
        public MyCriteria()
        {
            MinWinterTempF.Value = defaultLowTemp;
            FloweringMonths = defaultFloweringMonths;
            SunRequirements = defaultSunRequirements;
            AttractsBirds = defaultAttractsBirds;
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

    }
}

