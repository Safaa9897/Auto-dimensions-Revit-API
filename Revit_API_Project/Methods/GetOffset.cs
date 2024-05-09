using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_Project.Methods
{
    public static class GetOffset
    {
        public static XYZ GetOffsetByWallOrientation(XYZ _point, XYZ _orientation, int _value)
        {
            XYZ _newVector = _orientation.Multiply(_value);
            XYZ _returnPoint = _point.Add(_newVector);

            return _returnPoint;
        }
    }
}
