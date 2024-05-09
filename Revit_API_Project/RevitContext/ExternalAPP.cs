using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_Project.RevitContext
{
    public class ExternalAPP : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab("Annotations");

            RibbonPanel _Panel1 = application.CreateRibbonPanel("Annotations", "Tags");
            RibbonPanel _Panel2 = application.CreateRibbonPanel("Annotations", "Dimensions");

            string _path = Assembly.GetExecutingAssembly().Location;

            PushButtonData _pushButtonData1 = new PushButtonData("BTN1", "Windows and Doors Auto Tagging", _path, "Revit_API_Project.RevitContext.OpenWindowCommand");
            _Panel1.AddItem(_pushButtonData1);

            PushButtonData _pushButtonData2 = new PushButtonData("BTN2", "Walls Dimensions by element", _path, "Revit_API_Project.ViewModels.WallDimensionsViewModel");
            _Panel2.AddItem(_pushButtonData2);

            PushButtonData _pushButtonData3 = new PushButtonData("BTN3", "Walls Dimensions by view", _path, "Revit_API_Project.ViewModels.WallDimPlanViewModel");
            _Panel2.AddItem(_pushButtonData3);

            PushButtonData _pushButtonData4 = new PushButtonData("BTN4", "Grids Dimensions", _path, "Revit_API_Project.ViewModels.GridDimensionsViewModel");
            _Panel2.AddItem(_pushButtonData4);


            return Result.Succeeded;
        }
    }
}
