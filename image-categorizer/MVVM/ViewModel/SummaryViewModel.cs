using System.Collections.Generic;
using System.Windows;
using image_categorizer.Core;
using image_categorizer.MVVM.Model;

namespace image_categorizer.MVVM.ViewModel
{
    class SummaryViewModel : BaseViewModel
    {
        public SummaryViewModel()
        { 
            _summaryModel = new SummaryModel();
            InitSummaryModel();
        }
        private SummaryModel? _summaryModel = new();

        public SummaryModel? SummaryModel
        {
            get { return _summaryModel; }
            set { _summaryModel = value; OnPropertyChanged(); }
        }

        public void InitSummaryModel()
        { 
            if (SummaryModel != null && SQLite.isInit == true)
            {
                string[] attributes = new[] { "file_path", "datetime", "format", "camera_model", "modified_date" }; 
                SummaryModel.SelectedDBData = SQLite.SelectQuery(attributes);
                if (SummaryModel.SelectedDBData != null)
                {
                    System.Diagnostics.Debug.WriteLine("SortStart");
                    //calc distinct cameraName, rate, rank
                    //.... year, month, location

                }
            }
        }
    }
}
