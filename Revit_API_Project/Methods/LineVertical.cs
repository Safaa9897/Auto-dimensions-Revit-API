using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_Project.Methods
{
    public static class LineVertical
    {
        public static bool IsLineVertical(Line _line)
        {
            if (_line.Direction.IsAlmostEqualTo(XYZ.BasisZ) || _line.Direction.IsAlmostEqualTo(-XYZ.BasisZ))
                return true;
            else
                return false;
        }
    }
}
