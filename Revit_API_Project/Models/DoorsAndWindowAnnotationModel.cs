using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_Project.Models
{
    public class DoorsAndWindowAnnotationModel
    {
        #region Properties

        public string _categoryName { get; set; }
        public BuiltInCategory _builtInCategory { get; set; }

        #endregion


        #region Methods

        public override string ToString()
        {
            return _categoryName;
        }

        #endregion
    }
}
