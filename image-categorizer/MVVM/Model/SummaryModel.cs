using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using image_categorizer.Core;
using image_categorizer.MVVM.Model;

namespace image_categorizer.MVVM.Model
{
    class SummaryModel : ObservableObject
    {
        private Dictionary<string, List<string?>>? _selectedDBData = new();

        public Dictionary<string, List<string?>>? SelectedDBData
        {
            get { return _selectedDBData; }
            set { _selectedDBData = value; }
        }
        private ObservableCollection<RankedDataModel> _monthRankList = new();

        public ObservableCollection<RankedDataModel> MonthRankList
        {
            get { return _monthRankList; }
            set { _monthRankList = value; }
        }

        private ObservableCollection<RankedDataModel> _yearRankList = new();

        public ObservableCollection<RankedDataModel> YearRankList
        {
            get { return _yearRankList; }
            set { _yearRankList = value; }
        }

        private ObservableCollection<RankedDataModel> _locationRankList = new();

        public ObservableCollection<RankedDataModel> LocationRankList
        {
            get { return _locationRankList; }
            set { _locationRankList = value; }
        }

        private ObservableCollection<RankedDataModel> _cameraModelList = new();

        public ObservableCollection<RankedDataModel> CameraModelList
        {
            get { return _cameraModelList; }
            set { _cameraModelList = value; }
        }









    }
}
