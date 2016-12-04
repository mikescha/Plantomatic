using PlantMan.Plant;

namespace PlantomaticVM
{
    public class MyCriteria
    {
        decimal lowTemp;
        FloweringMonths floweringMonths;
        SunRequirements sunRequirements;
        YesNoMaybe attractsBirds;

        public MyCriteria()
        {
            lowTemp = 32;
            FloweringMonths = FloweringMonths.AllMonths;
            SunRequirements = SunRequirements.AllSunTypes;
            AttractsBirds = YesNoMaybe.Unassigned;
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

