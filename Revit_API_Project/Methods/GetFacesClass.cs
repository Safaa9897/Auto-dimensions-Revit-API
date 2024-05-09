using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_Project.Methods
{
    public static class GetFacesClass
    {
        public static Face GetFace(Element _selectedElement, XYZ _orientation)
        {
            //Get solids from faces
            PlanarFace _returnFace = null;
            List<Solid> _solids = GetSolidsClass.GetSolids(_selectedElement);

            foreach (Solid _solid in _solids)
            {
                foreach (Face _face in _solid.Faces)
                {
                    if (_face is PlanarFace)
                    {
                        PlanarFace _planarFace = _face as PlanarFace;

                        if (_planarFace.FaceNormal.IsAlmostEqualTo(_orientation))
                            _returnFace = _planarFace;
                    }
                }
            }

            return _returnFace;
        }
    }
}
