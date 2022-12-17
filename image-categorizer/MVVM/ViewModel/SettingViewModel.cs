using image_categorizer.MVVM.Model;
using image_categorizer.Core;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System;

namespace image_categorizer.MVVM.ViewModel
{
    class SettingViewModel : BaseViewModel
    {
        #region Constructor
        public SettingViewModel()
        {
            SelectInputPathCommand = PathSelectCommand("input");
            SelectOutputPathCommand = PathSelectCommand("output");
            SaveButtonCommand = SaveSettingCommand();
        }
        #endregion Constructor

        #region Model Property
        private static SettingModel _settingModel;
        public SettingModel SettingModel
        {
            get
            {
                if (_settingModel == null)
                {
                    _settingModel = new SettingModel();
                    ReadSetting();
                }
                return _settingModel;
            }
            set
            {
                _settingModel = value;
                OnPropertyChanged();
            }
        }
        #endregion Model Property

        #region RelayCommand
        private RelayCommand SaveSettingCommand()
        {
            RelayCommand ret = new(o =>
            {
                SaveSetting(this.SettingModel);
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


        #region Logical Function
/*        public string[] getDirectoryRules()
        {
            return SettingModel.DirectoryRules;
        }
        public string[] getFileNameRules()
        {
            return SettingModel.FileNameRules;
        }*/
        public void RuleTest()
        {
            for (int i = 0; i < 4; i++)
            {
                System.Diagnostics.Debug.WriteLine(SettingModel.ToString);
            }
        }
        #endregion Logical Function
        #region Static Data
        private readonly ObservableCollection<string> _rulesForComboBox = new() {
            "None",
            "Date",
            "CameraModel",
            "Format",
            //"Location"
        };
        public ObservableCollection<string> RulesForComboBox
        {
            get { return _rulesForComboBox; }
        }
        #endregion Static Data
        public void SaveSetting(SettingModel model)
        {
            Properties.Settings.Default.InputDirectory = model.InputDirectorytPath;
            Properties.Settings.Default.OutputDirctory = model.OutputDirectorytPath;
            //it's length is not 4, add None
            Properties.Settings.Default.DirectoryNameRule = String.Join(",", model.DirectoryRules);
            Properties.Settings.Default.FileNameRule = String.Join(",", model.FileNameRules);
            Properties.Settings.Default.Save();

        }
        public void ReadSetting()
        {
            if (SettingModel != null)
            {
                SettingModel.InputDirectorytPath = Properties.Settings.Default.InputDirectory;
                SettingModel.OutputDirectorytPath = Properties.Settings.Default.OutputDirctory;
                string? directoryNameRule = Properties.Settings.Default.DirectoryNameRule;
                SettingModel.DirectoryRules = directoryNameRule.Split(",");
                string? fileNameRule = Properties.Settings.Default.FileNameRule;
                SettingModel.FileNameRules = fileNameRule.Split(",");
            }
        }

    }
}
