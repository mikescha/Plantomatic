using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantomaticVM
{
    public class CartItem
    {
        public CartItem()
        {
            Name = "";
            Count = 0;
        }
         
        public string Name
        {
            set; get;
        }

        public int Count
        {
            set; get;
        }
    }
}
