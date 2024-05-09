using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_Project.ViewModels
{

    [Transaction(TransactionMode.Manual)]
    public class GridDimensionsViewModel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #region Documents
            UIDocument _Uidocument = commandData.Application.ActiveUIDocument;
            Document _document = _Uidocument.Document;
            #endregion

            try
            {
                #region Filter
                //Select all grids and get only instances of an element
                var GridFilter = new ElementCategoryFilter(BuiltInCategory.OST_Grids);
                var GridList = new FilteredElementCollector(_document).WherePasses(GridFilter).WhereElementIsNotElementType();

                #endregion

                #region Create RefrenceArrays 
                //Split grids into Horizontal and Vertical through the digits and letters grids naming convention
                ReferenceArray GridColRef = new ReferenceArray();
                ReferenceArray GridRowRef = new ReferenceArray();
                var GridColElements = new List<Element>();
                var GridRowElements = new List<Element>();
                var EndPointColFirst = new XYZ();
                var EndPointColLast = new XYZ();
                var EndPointRowFirst = new XYZ();
                var EndPointRowLast = new XYZ();


                foreach (var grid in GridList)
                {
                    var GridName = grid.LookupParameter("Name").AsString();
                    var IsInt = int.TryParse(GridName, out var value);
                    var GridRef = new Reference(grid);
                    if (IsInt == true && GridName != null)
                    {
                        GridColRef.Append(GridRef);
                        GridColElements.Add(grid);

                        //Cast each element as a grid and gets its curve endpoints
                        //For Coloumns Grids
                        var GridLineCol = grid as Grid;
                        if (GridColElements[0] == grid)
                            EndPointColFirst = GridLineCol.Curve.GetEndPoint(0);
                        else if (GridColElements[(GridColElements.Count) - 1] == grid)
                            EndPointColLast = GridLineCol.Curve.GetEndPoint(0);

                    }
                    else
                    {
                        GridRowRef.Append(GridRef);
                        GridRowElements.Add(grid);

                        //Cast each element as a grid and gets its curve endpoints
                        //For Rows Grids
                        var GridLineRow = grid as Grid;
                        if (GridRowElements[0] == grid)
                            EndPointRowFirst = GridLineRow.Curve.GetEndPoint(0);
                        else if (GridRowElements[(GridRowElements.Count) - 1] == grid)
                            EndPointRowLast = GridLineRow.Curve.GetEndPoint(0);
                    }
                }
                #endregion

                #region Create Dimension Line
                var DimLineCol = Line.CreateBound(EndPointColFirst, EndPointColLast);
                var DimLineRow = Line.CreateBound(EndPointRowFirst, EndPointRowLast);
                #endregion

                using (Transaction _transaction = new Transaction(_document))
                {
                    _transaction.Start("Create Grid dimensions");
                    //Create Dimension
                    _document.Create.NewDimension(_Uidocument.ActiveView, DimLineCol, GridColRef);
                    _document.Create.NewDimension(_Uidocument.ActiveView, DimLineRow, GridRowRef);
                    _transaction.Commit();
                }
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}