using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CartLine
    {
        public MedicinalProduct MedicinalProduct { get; set; }
        public int Quantity { get; set; }
    }
}
