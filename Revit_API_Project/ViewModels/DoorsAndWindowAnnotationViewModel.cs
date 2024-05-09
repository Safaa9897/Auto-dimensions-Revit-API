using Autodesk.Revit.DB;
using Revit_API_Project.Command;
using Revit_API_Project.Models;
using Revit_API_Project.RevitContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_Project.ViewModels
{
    public class DoorsAndWindowAnnotationViewModel : INotifyPropertyChanged
    {


        #region Fields

        private DoorsAndWindowAnnotationModel _selectedCategoryObj;
        private bool _hasLeader;

        #endregion


        #region Constructor

        public DoorsAndWindowAnnotationViewModel()
        {
            DoneCommand = new RelayCommand(ExcuteDoneCommand);
        }

        #endregion


        #region Properties

        public event PropertyChangedEventHandler PropertyChanged;

        public List<DoorsAndWindowAnnotationModel> _doorsAndWindowAnnotationModel { get; set; } = new List<DoorsAndWindowAnnotationModel>()
        {
            new DoorsAndWindowAnnotationModel() {_categoryName="Doors",_builtInCategory=Autodesk.Revit.DB.BuiltInCategory.OST_Doors},

            new DoorsAndWindowAnnotationModel() {_categoryName="Windows",_builtInCategory=Autodesk.Revit.DB.BuiltInCategory.OST_Windows},

        };

        /// <summary>
        /// this property shows the selected item from the combox with a data type CategoryObj
        /// </summary>
        public DoorsAndWindowAnnotationModel SelectedCategoryObj
        {

            get
            {
                return _selectedCategoryObj;
            }
            set
            {
                _selectedCategoryObj = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// this property shows the selected value from the check box (true or false),if the user needs 
        /// Leader or not 
        /// </summary>
        public bool HasLeader
        {
            get { return _hasLeader; }
            set
            {

                _hasLeader = value;
                OnPropertyChanged();

            }
        }
        public RelayCommand DoneCommand { get; set; }

        #endregion


        #region Methods

        public void ExcuteDoneCommand()
        {
            Document _document = OpenWindowCommand._document;

            IList<Element> _elements = new FilteredElementCollector(_document).OfCategory(SelectedCategoryObj._builtInCategory)
              .WhereElementIsNotElementType().ToElements();


            foreach (Element _element in _elements)


            {
                //Reference
                Reference _reference = new Reference(_element);

                //location point
                LocationPoint _locationPoint = _element.Location as LocationPoint;

                XYZ _point = _locationPoint.Point;

                //create Independed tags
                using (Transaction _transaction = new Transaction(_document, "Tag Element"))
                {
                    _transaction.Start();

                    IndependentTag _independentTag = IndependentTag.Create(_document, _document.ActiveView.Id, _reference, HasLeader, TagMode.TM_ADDBY_CATEGORY, TagOrientation.Horizontal, _point);

                    _transaction.Commit();
                }
            }




        }

        public void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));

        }
        #endregion

    }
}
