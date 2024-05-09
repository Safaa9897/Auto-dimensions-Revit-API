using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using Revit_API_Project.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_Project.ViewModels
{
    [Transaction(TransactionMode.Manual)]
    public class WallDimensionsViewModel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #region documents

            UIApplication _uIApplication = commandData.Application;

            UIDocument _Uidocument = commandData.Application.ActiveUIDocument;

            Document _document = _Uidocument.Document;

            #endregion


            try
            {
                //Select Walls 
                #region Select Walls 

                Reference _pickWallsRef = _uIApplication.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Select Walls To Create Dimensions");
                Element _selectedElement = _document.GetElement(_pickWallsRef);

                #endregion


                if (_selectedElement is Wall)
                {
                    //cast selected elements to wall
                    Wall _selectedWall = _selectedElement as Wall;

                    //References
                    ReferenceArray _referenceArray = new ReferenceArray();
                    ReferenceArray _referenceArray2 = new ReferenceArray();
                    Reference _reference1 = null, _reference2 = null;

                    //Get faces 
                    Face _wallFace = GetFacesClass.GetFace(_selectedWall, _selectedWall.Orientation);

                    //Get adges
                    EdgeArrayArray _edgeArrays = _wallFace.EdgeLoops;
                    EdgeArray _edges = _edgeArrays.get_Item(0);

                    //Edges list for lines
                    List<Edge> _edgeList = new List<Edge>();
                    foreach (Edge _edge in _edges)
                    {
                        Line _line = _edge.AsCurve() as Line;

                        if (LineVertical.IsLineVertical(_line) == true)
                        {
                            _edgeList.Add(_edge);
                        }
                    }


                    //dim referances
                    List<Edge> _sortedEdges = _edgeList.OrderByDescending(e => e.AsCurve().Length).ToList();
                    _reference1 = _sortedEdges[0].Reference;
                    _reference2 = _sortedEdges[1].Reference;

                    _referenceArray.Append(_reference1);

                    // reference wall ends for overall dim
                    _referenceArray2.Append(_reference1);
                    _referenceArray2.Append(_reference2);

                    //Windows and Doors Filter Elements
                    List<BuiltInCategory> _categoryList = new List<BuiltInCategory>() { BuiltInCategory.OST_Windows, BuiltInCategory.OST_Doors };
                    ElementMulticategoryFilter _wallFilter = new ElementMulticategoryFilter(_categoryList);

                    // get windows and doors from wall and create reference
                    List<ElementId> _wallElemsIds = _selectedWall.GetDependentElements(_wallFilter).ToList();

                    foreach (ElementId _elementId in _wallElemsIds)
                    {
                        FamilyInstance _currentFamilyInstance = _document.GetElement(_elementId) as FamilyInstance;
                        //Reference _currentRef = GetSpecialFamilyReference(_currentFamilyInstance, SpecialReferenceType.CenterLR);
                        Reference _currentRef = SpecialFamilyReference.GetSpecialFamilyReference(_currentFamilyInstance, SpecialReferenceType.Left);
                        Reference _currentRef2 = SpecialFamilyReference.GetSpecialFamilyReference(_currentFamilyInstance, SpecialReferenceType.Right);
                        _referenceArray.Append(_currentRef);
                        _referenceArray.Append(_currentRef2);
                    }

                    _referenceArray.Append(_reference2);

                    // create dimension line
                    LocationCurve _wallLocation = _selectedWall.Location as LocationCurve;
                    Line _wallLine = _wallLocation.Curve as Line;

                    XYZ _offset1 = GetOffset.GetOffsetByWallOrientation(_wallLine.GetEndPoint(0), _selectedWall.Orientation, 5);
                    XYZ _offset2 = GetOffset.GetOffsetByWallOrientation(_wallLine.GetEndPoint(1), _selectedWall.Orientation, 5);

                    XYZ _offset1b = GetOffset.GetOffsetByWallOrientation(_wallLine.GetEndPoint(0), _selectedWall.Orientation, 10);
                    XYZ _offset2b = GetOffset.GetOffsetByWallOrientation(_wallLine.GetEndPoint(1), _selectedWall.Orientation, 10);

                    Line _dimLine = Line.CreateBound(_offset1, _offset2);
                    Line _dimLine2 = Line.CreateBound(_offset1b, _offset2b);


                    //create dimension
                    using (Transaction _transaction = new Transaction(_document))
                    {
                        _transaction.Start("Create new dimension");

                        Dimension newDim = _document.Create.NewDimension(_document.ActiveView, _dimLine, _referenceArray);

                        if (_wallElemsIds.Count > 0)
                        {
                            Dimension newDim2 = _document.Create.NewDimension(_document.ActiveView, _dimLine2, _referenceArray2);
                        }

                        _transaction.Commit();
                    }

                }
                else
                {
                    return Result.Failed;
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }



        //Location
        #region Location

        public enum SpecialReferenceType
        {
            Left = 0,
            CenterLR = 1,
            Right = 2,
            Front = 3,
            CenterFB = 4,
            Back = 5,
            Bottom = 6,
            CenterElevation = 7,
            Top = 8
        }

        #endregion


    }
}

#region pick more than one wall
//using Autodesk.Revit.Attributes;
//using Autodesk.Revit.DB;
//using Autodesk.Revit.UI.Selection;
//using Autodesk.Revit.UI;
//using Revit_API_Project.Methods;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Revit_API_Project.ViewModels
//{
//    [Transaction(TransactionMode.Manual)]
//    public class WallDimensionsViewModel : IExternalCommand
//    {
//        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
//        {
//            #region documents

//            UIApplication _uIApplication = commandData.Application;

//            UIDocument _Uidocument = commandData.Application.ActiveUIDocument;

//            Document _document = _Uidocument.Document;

//            #endregion


//            try
//            {
//                //Select Walls 
//                #region Select Walls 

//                ICollection<Reference> _pickWallsRefs = _uIApplication.ActiveUIDocument.Selection.PickObjects(ObjectType.Element, "Select Walls To Create Dimensions");


//                #endregion
//                foreach (Reference _pickWallsRef in _pickWallsRefs)
//                {
//                    Element _selectedElement = _document.GetElement(_pickWallsRef);


//                    if (_selectedElement is Wall)
//                    {
//                        //cast selected elements to wall
//                        Wall _selectedWall = _selectedElement as Wall;

//                        //References
//                        ReferenceArray _referenceArray = new ReferenceArray();
//                        ReferenceArray _referenceArray2 = new ReferenceArray();
//                        Reference _reference1 = null, _reference2 = null;

//                        //Get faces 
//                        Face _wallFace = GetFacesClass.GetFace(_selectedWall, _selectedWall.Orientation);

//                        //Get adges
//                        EdgeArrayArray _edgeArrays = _wallFace.EdgeLoops;
//                        EdgeArray _edges = _edgeArrays.get_Item(0);

//                        //Edges list for lines
//                        List<Edge> _edgeList = new List<Edge>();
//                        foreach (Edge _edge in _edges)
//                        {
//                            Line _line = _edge.AsCurve() as Line;

//                            if (LineVertical.IsLineVertical(_line) == true)
//                            {
//                                _edgeList.Add(_edge);
//                            }
//                        }


//                        //dim referances
//                        List<Edge> _sortedEdges = _edgeList.OrderByDescending(e => e.AsCurve().Length).ToList();
//                        _reference1 = _sortedEdges[0].Reference;
//                        _reference2 = _sortedEdges[1].Reference;

//                        _referenceArray.Append(_reference1);

//                        // reference wall ends for overall dim
//                        _referenceArray2.Append(_reference1);
//                        _referenceArray2.Append(_reference2);

//                        //Windows and Doors Filter Elements
//                        List<BuiltInCategory> _categoryList = new List<BuiltInCategory>() { BuiltInCategory.OST_Windows, BuiltInCategory.OST_Doors };
//                        ElementMulticategoryFilter _wallFilter = new ElementMulticategoryFilter(_categoryList);

//                        // get windows and doors from wall and create reference
//                        List<ElementId> _wallElemsIds = _selectedWall.GetDependentElements(_wallFilter).ToList();

//                        foreach (ElementId _elementId in _wallElemsIds)
//                        {
//                            FamilyInstance _currentFamilyInstance = _document.GetElement(_elementId) as FamilyInstance;
//                            //Reference _currentRef = GetSpecialFamilyReference(_currentFamilyInstance, SpecialReferenceType.CenterLR);
//                            Reference _currentRef = SpecialFamilyReference.GetSpecialFamilyReference(_currentFamilyInstance, SpecialReferenceType.Left);
//                            Reference _currentRef2 = SpecialFamilyReference.GetSpecialFamilyReference(_currentFamilyInstance, SpecialReferenceType.Right);
//                            _referenceArray.Append(_currentRef);
//                            _referenceArray.Append(_currentRef2);
//                        }

//                        _referenceArray.Append(_reference2);

//                        // create dimension line
//                        LocationCurve _wallLocation = _selectedWall.Location as LocationCurve;
//                        Line _wallLine = _wallLocation.Curve as Line;

//                        XYZ _offset1 = GetOffset.GetOffsetByWallOrientation(_wallLine.GetEndPoint(0), _selectedWall.Orientation, 5);
//                        XYZ _offset2 = GetOffset.GetOffsetByWallOrientation(_wallLine.GetEndPoint(1), _selectedWall.Orientation, 5);

//                        XYZ _offset1b = GetOffset.GetOffsetByWallOrientation(_wallLine.GetEndPoint(0), _selectedWall.Orientation, 10);
//                        XYZ _offset2b = GetOffset.GetOffsetByWallOrientation(_wallLine.GetEndPoint(1), _selectedWall.Orientation, 10);

//                        Line _dimLine = Line.CreateBound(_offset1, _offset2);
//                        Line _dimLine2 = Line.CreateBound(_offset1b, _offset2b);


//                        //create dimension
//                        using (Transaction _transaction = new Transaction(_document))
//                        {
//                            _transaction.Start("Create new dimension");

//                            Dimension newDim = _document.Create.NewDimension(_document.ActiveView, _dimLine, _referenceArray);

//                            if (_wallElemsIds.Count > 0)
//                            {
//                                Dimension newDim2 = _document.Create.NewDimension(_document.ActiveView, _dimLine2, _referenceArray2);
//                            }

//                            _transaction.Commit();
//                        }

//                    }
//                    else
//                    {
//                        return Result.Failed;
//                    }
//                }
//                return Result.Succeeded;
//            }
//            catch (Exception ex)
//            {
//                message = ex.Message;
//                return Result.Failed;
//            }
//        }



//        //Location
//        #region Location

//        public enum SpecialReferenceType
//        {
//            Left = 0,
//            CenterLR = 1,
//            Right = 2,
//            Front = 3,
//            CenterFB = 4,
//            Back = 5,
//            Bottom = 6,
//            CenterElevation = 7,
//            Top = 8
//        }

//        #endregion


//    }
//}
#endregion

