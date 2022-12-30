using image_categorizer.Core;
using image_categorizer.MVVM.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            //exception: KeyNotFoundException -> DataBase has no Data
            //exception: NullValueException
            if (SummaryModel != null && SQLite.isInit == true)
            {
                string[] attributes = new[] { "file_path", "datetime", "format", "camera_model","location", "modified_date" }; 
                SummaryModel.SelectedDBData = SQLite.SelectQuery(attributes);

                try
                {
                    Dictionary<string, List<string?>> CameraModels = Utility.GetSameValueList(SummaryModel.SelectedDBData["camera_model"]);
                    int CameraModelListSum = 0;
                    SummaryModel.CameraModelList = GetRankData(CameraModels, out CameraModelListSum);
                    CameraModels.Clear();


                    int YearMonthsListSum = 0;
                    int YearMonthsOtherSum = YearMonthsListSum;
                    Dictionary<string, List<string?>> YearMonths = Utility.GetSameValueList(SummaryModel.SelectedDBData["datetime"]);
                    SummaryModel.YearMonthRankList = GetRankData(GetYearMonthList(YearMonths), out YearMonthsListSum);

                    int YearListSum = 0;
                    SummaryModel.YearRankList = GetRankData(GetYearMonthList(YearMonths), out YearListSum);
                    YearMonths.Clear();

                    Dictionary<string, List<string?>> Locations = Utility.GetSameValueList(SummaryModel.SelectedDBData["location"]);
                    int LocationListSum = 0;
                    SummaryModel.LocationRankList = GetRankData(Locations, out LocationListSum);
                    Locations.Clear();
                    System.Diagnostics.Debug.WriteLine("initialized");

                }
                catch(KeyNotFoundException e)
                { 
                    ObservableCollection<RankedDataModel> None = new ObservableCollection<RankedDataModel>(new(6));
                    SummaryModel.CameraModelList = None;
                    SummaryModel.YearRankList = None;
                    SummaryModel.YearMonthRankList = None;
                    SummaryModel.LocationRankList = None;
                }
            }
        }


        #region init Logic
        private ObservableCollection<RankedDataModel> GetTopRank(ObservableCollection<RankedDataModel> ModelList, int size)
        {
            RankedDataModel[] High = new RankedDataModel[size];
            for (int i = 0; i < size; i++)
            {
                High[i] = new RankedDataModel();
            }
            foreach (RankedDataModel item in ModelList)
            {
                for (int i = High.Length - 1; i >= 0; i--)
                {
                    if (item.Count > High[i].Count && i > 0)
                    {
                        continue;
                    }
                    else if (item.Count > High[i].Count && i == 0)
                    {
                        if (High[i].Count > High[i + 1].Count)
                        {
                            High[i + 1] = High[i];
                        }
                        High[i] = item;
                    }
                    else if (item.Count < High[i].Count && i < High.Length - 1)
                    {
                        if (item.Count > High[i + 1].Count)
                        {
                            if (i + 2 < High.Length)
                            {
                                High[i + 2] = High[i + 1];
                            }
                            High[i + 1] = item;
                        }
                        else continue;
                    }
                }
            }
            ObservableCollection<RankedDataModel> ret = new ObservableCollection<RankedDataModel>();
            for (int i = 0; i < High.Length; i++)
            { 
                ret.Add(High[i]);
            }
            return ret;
        }
        private ObservableCollection<RankedDataModel> GetRankData(Dictionary<string, List<string?>> cameraModels, out int sum) 
        {
            int countSum = 0;
            ObservableCollection<RankedDataModel> Data = new();
            foreach (KeyValuePair<string, List<string?>> item in cameraModels)
            {
                RankedDataModel rankedDataModel = new RankedDataModel();
                rankedDataModel.Name = item.Key;
                rankedDataModel.Count = item.Value.Count;
                Data.Add(rankedDataModel);
                countSum += item.Value.Count;
            }
            sum = countSum;
            ObservableCollection<RankedDataModel> RankedList =  GetTopRank(Data, 5);
             ObservableCollection < RankedDataModel > result = new();
            int otherSum = countSum;
            int count = 0;
            for (int i = 0; i < RankedList.Count; i++)
            {
                RankedDataModel dataModel = new();
                dataModel.Count = RankedList[i].Count;
                dataModel.Index = i;
                dataModel.Name = RankedList[i].Name;
                dataModel.Rate = ((double)RankedList[i].Count / (double)countSum) * 100;
                otherSum -= RankedList[i].Count;
                result.Add(dataModel);
            }
            result.Add(new RankedDataModel(6, "Other", (otherSum / countSum) * 100, 0));
            return result;
        }

        private Dictionary<string, List<string?>> GetYearMonthList(Dictionary<string, List<string?>> dateTimes)
        {
            Dictionary<string, List<string?>> YearMonth = new();
            foreach (KeyValuePair<string, List<string?>> item in dateTimes)
            {
                string? formatedDate = Utility.FormatYearMonth(item.Key);
                if (!string.IsNullOrEmpty(formatedDate))
                {
                    if (YearMonth.ContainsKey(formatedDate))
                    {
                        YearMonth[formatedDate].Add(formatedDate);
                    }
                    else { YearMonth.Add(formatedDate, item.Value); }
                }
                
            }
            return YearMonth;
        }
        private Dictionary<string, List<string?>> GetYearList(Dictionary<string, List<string?>> dateTimes)
        {
            Dictionary<string, List<string?>> Year = new();
            foreach (KeyValuePair<string, List<string?>> item in dateTimes)
            {
                string? formatedDate = Utility.FormatYear(item.Key);
                if (!string.IsNullOrEmpty(formatedDate))
                {
                    if (Year.ContainsKey(formatedDate))
                    {
                        Year[formatedDate].Add(formatedDate);
                    }
                    else { Year.Add(formatedDate, item.Value); }
                }

            }
            return Year;
        }
        #endregion init Logic
    }
}

