using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEIS400_ECS
{
    public interface IHasBarcode
    {
        Barcode Barcode { get; }
        void GenerateBarcode();
    }
}
