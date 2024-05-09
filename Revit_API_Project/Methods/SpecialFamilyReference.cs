using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Revit_API_Project.ViewModels.WallDimensionsViewModel;

namespace Revit_API_Project.Methods
{
    public static class SpecialFamilyReference
    {
        public static Reference GetSpecialFamilyReference(FamilyInstance _familyInstance, SpecialReferenceType _specialReferenceType)
        {
            Reference _indexRef = null;

            int idx = (int)_specialReferenceType;

            if (_familyInstance != null)
            {
                Document _document = _familyInstance.Document;

                Options _geomOptions = new Options();
                _geomOptions.ComputeReferences = true;
                _geomOptions.DetailLevel = ViewDetailLevel.Undefined;
                _geomOptions.IncludeNonVisibleObjects = true;

                GeometryElement _geometryElement = _familyInstance.get_Geometry(_geomOptions);
                GeometryInstance _geometryInstance = _geometryElement.First() as GeometryInstance;

                String _sampleStableRef = null;

                if (_geometryInstance != null)
                {
                    GeometryElement _geometrySymbol = _geometryInstance.GetSymbolGeometry();

                    if (_geometrySymbol != null)
                    {
                        foreach (GeometryObject _geometryObject in _geometrySymbol)
                        {
                            if (_geometryObject is Solid)
                            {
                                Solid _solid = _geometryObject as Solid;

                                if (_solid.Faces.Size > 0)
                                {
                                    Face _face = _solid.Faces.get_Item(0);
                                    _sampleStableRef = _face.Reference.ConvertToStableRepresentation(_document);
                                    break;
                                }
                            }
                            else if (_geometryObject is Curve)
                            {
                                Curve _curve = _geometryObject as Curve;
                                Reference _curveRef = _curve.Reference;
                                if (_curveRef != null)
                                {
                                    _sampleStableRef = _curve.Reference.ConvertToStableRepresentation(_document);
                                    break;
                                }

                            }
                            else if (_geometryObject is Point)
                            {
                                Point point = _geometryObject as Point;
                                _sampleStableRef = point.Reference.ConvertToStableRepresentation(_document);
                                break;
                            }
                        }
                    }

                    if (_sampleStableRef != null)
                    {
                        String[] _refTokens = _sampleStableRef.Split(new char[] { ':' });

                        String _customStableRef = _refTokens[0] + ":"
                          + _refTokens[1] + ":" + _refTokens[2] + ":"
                          + _refTokens[3] + ":" + idx.ToString();

                        _indexRef = Reference.ParseFromStableRepresentation(_document, _customStableRef);

                        GeometryObject _geometryObject = _familyInstance.GetGeometryObjectFromReference(_indexRef);

                        if (_geometryObject != null)
                        {
                            String _finalToken = "";
                            if (_geometryObject is Edge)
                            {
                                _finalToken = ":LINEAR";
                            }

                            if (_geometryObject is Face)
                            {
                                _finalToken = ":SURFACE";
                            }

                            _customStableRef += _finalToken;
                            _indexRef = Reference.ParseFromStableRepresentation(_document, _customStableRef);
                        }
                        else
                        {
                            _indexRef = null;
                        }
                    }
                }
                else
                {
                    throw new Exception("No Symbol Geometry found...");
                }
            }
            return _indexRef;
        }
    }
}
