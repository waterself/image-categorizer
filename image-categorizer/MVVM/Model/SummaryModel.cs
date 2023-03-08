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
        private ObservableCollection<RankedDataModel> _yearMonthRankList = new();

        public ObservableCollection<RankedDataModel> YearMonthRankList
        {
            get { return _yearMonthRankList; }
            set { _yearMonthRankList = value; }
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
        private int _cameraModelListSum;

        public int CameraModelListSum
        {
            get { return _cameraModelListSum; }
            set { _cameraModelListSum = value; }
        }

        private int _yearMonthsListSum;

        public int YearMonthsListSum
        {
            get { return _yearMonthsListSum; }
            set { _yearMonthsListSum = value; }
        }

        private int _yearListSum;

        public int YearListSum
        {
            get { return _yearListSum; }
            set { _yearListSum = value; }
        }










    }
}
