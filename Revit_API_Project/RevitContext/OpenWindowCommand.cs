using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit_API_Project.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_Project.RevitContext
{
    [Transaction(TransactionMode.Manual)]
    public class OpenWindowCommand : IExternalCommand
    {
        #region Properties
        public static Document _document { get; set; }
        #endregion


        #region Methods
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIDocument _uIDocument = commandData.Application.ActiveUIDocument;
            _document = _uIDocument.Document;

            try
            {

                DoorsAndWindowAnnotationView _doorsAndWindowAnnotationView = new DoorsAndWindowAnnotationView();
                _doorsAndWindowAnnotationView.ShowDialog();

                return Result.Succeeded;
            }
            catch (Exception EX)
            {
                message = EX.Message;
                return Result.Failed;
            }

        }
        #endregion

    }
}
