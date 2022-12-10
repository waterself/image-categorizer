using image_categorizer.MVVM.Model;
using image_categorizer.Core;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;

namespace image_categorizer.MVVM.ViewModel
{
    class SettingViewModel : BaseViewModel
    {
        #region Constructor
        public SettingViewModel()
        {
            SaveButtonCommand = SaveSetting();
        }
        #endregion Constructor

        #region Model Property
        private static SettingModel _settingModel;
        public SettingModel SettingModel
        {
            get => _settingModel ?? (_settingModel = new SettingModel());
            set
            {
                _settingModel = value;
                OnPropertyChanged();
            }
        }
        #endregion Model Property

        #region RelayCommand
        private RelayCommand SaveSetting()
        {
            RelayCommand ret = new(o =>
            {
                RuleTest();
            });
            return ret;
        }
        private RelayCommand PathSelectCommand(string mode)
        {
            RelayCommand ret = new RelayCommand(o =>
            {
                CommonOpenFileDialog openFileDialog = new();
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.IsFolderPicker = true;
                if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string? fileName = openFileDialog.FileName as string;
                    if (fileName != null)
                    {
                        if (mode == "input")
                        {
                            SettingModel.InputDirectorytPath = fileName;
                            OnPropertyChanged();
                        }
                        else if (mode == "output")
                        {
                            SettingModel.OutputDirectorytPath = fileName;
                            OnPropertyChanged();
                        }
                    }
                    else
                    {
                        MessageBox.Show("incorrect directory");
                    }
                }
            });
            return ret;
        }
        public RelayCommand SaveButtonCommand { get; set; }
        public RelayCommand SelectInputPathCommand { get; set; }
        public RelayCommand SelectOutputPathCommand { get; set; }
        #endregion RelayCommand

        #region Static Data
        private readonly ObservableCollection<string> _rulesForComboBox = new() {
            "None",
            "Date",
            "CameraModel",
            "Format",
            "Location"
        };
        public ObservableCollection<string> RulesForComboBox
        {
            get { return _rulesForComboBox; }
        }
        #endregion Static Data

        #region Logical Function
        public string[] getDirectoryRules()
        {
            return SettingModel.DirectoryRules;
        }
        public string[] getFileNameRules()
        {
            return SettingModel.FileNameRules;
        }
        public void RuleTest()
        {
            for (int i = 0; i < 4; i++)
            {
                System.Diagnostics.Debug.WriteLine(SettingModel.ToString);
            }
        }
        #endregion Logical Function

        /*        public void SaveSatting(SettingModel model)
        {
            IsolateStorage.WriteStorageValue("inputDir", model.InputDirectorytPath);
        }*/

    }
}
