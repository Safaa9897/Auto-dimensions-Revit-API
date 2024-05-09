using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_Project.Methods
{
    public static class GetSolidsClass
    {
        public static List<Solid> GetSolids(Element _selectedElement)
        {
            List<Solid> _returnList = new List<Solid>();

            Options _options = new Options();
            _options.ComputeReferences = true;
            _options.DetailLevel = ViewDetailLevel.Fine;

            GeometryElement _geometryElement = _selectedElement.get_Geometry(_options);

            foreach (GeometryObject _geometryObject in _geometryElement)
            {
                if (_geometryObject is Solid)
                {
                    Solid _solid = (Solid)_geometryObject;
                    if (_solid.Faces.Size > 0 && _solid.Volume > 0.0)
                    {
                        _returnList.Add(_solid);
                    }
                }
            }

            return _returnList;
        }
    }
}
