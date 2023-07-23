using image_categorizer.MVVM.Model;
using image_categorizer.Core;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System;
using System.Windows.Controls;
using image_categorizer.MVVM.State;

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
            DirectoryRuleSelectorCommand = PathExampleSetterCommand();
            FileNameRuleSelectorCommand = FileNameExampleSetterCommand();
            _settingModel = new();
            ReadSetting();
        }
        #endregion Constructor

        #region Model Property
        private static SettingViewState _settingModel;
        public SettingViewState SettingModel
        {
            get
            {
                if (_settingModel == null)
                { 
                _settingModel = new SettingViewState();
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
        private RelayCommand PathExampleSetterCommand()
        {
            RelayCommand ret = new RelayCommand(o =>
               {
                   PathExampleSetter();

               });
            return ret;
        }
        private RelayCommand FileNameExampleSetterCommand()
        {
            RelayCommand ret = new RelayCommand(o =>
            {
                FileNameExampleSetter();
            });
            return ret;
        }
        public RelayCommand SaveButtonCommand { get; set; }
        public RelayCommand SelectInputPathCommand { get; set; }
        public RelayCommand SelectOutputPathCommand { get; set; }
        public RelayCommand DirectoryRuleSelectorCommand { get; set; }
        public RelayCommand FileNameRuleSelectorCommand { get; set; }
        #endregion RelayCommand


        #region Logical Function
        public void SaveSetting(SettingViewState model)
        {
            Logger logger = new Logger(base.ProgramDir, "SettingTab SaveSetting");
            IUtility utility = new Utility( ref logger);
            try
            {
                Properties.Settings.Default.InputDirectory = model.InputDirectorytPath;
                Properties.Settings.Default.OutputDirctory = model.OutputDirectorytPath;
                Properties.Settings.Default.DirectoryNameRule = String.Join(",", utility.ArrayLengthCheck(model.DirectoryRules, 4));
                Properties.Settings.Default.FileNameRule = String.Join(",", utility.ArrayLengthCheck(model.FileNameRules, 4));
                Properties.Settings.Default.Save();
                MessageBox.Show("Setting Saved", "Setting", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                logger.WriteLog(e.Message, true);
                return;
            }
        }
        public void ReadSetting()
        {
            Logger logger = new Logger(ProgramDir, "SettingTab ReadSetting");
            IUtility utility = new Utility(ref logger);
            try
            {
                if (SettingModel != null)
                {
                    SettingModel.InputDirectorytPath = Properties.Settings.Default.InputDirectory;
                    SettingModel.OutputDirectorytPath = Properties.Settings.Default.OutputDirctory;
                    string? directoryNameRule = Properties.Settings.Default.DirectoryNameRule;
                    SettingModel.DirectoryRules = utility.ArrayLengthCheck(directoryNameRule.Split(","), 4);
                    string? fileNameRule = Properties.Settings.Default.FileNameRule;
                    SettingModel.FileNameRules = utility.ArrayLengthCheck(fileNameRule.Split(","), 4);
                    //string[]? directoryNameRuleIndexes = Properties.Settings.Default.DirectoryRuleIndexes.Split(",");
                    SettingModel.DirectoryRulesIndexes = Array.ConvertAll(SettingModel.DirectoryRules, s => ComboBoxIndexConverter(s));
                    //string[]? fileNameRuleIndexes = Properties.Settings.Default.FileNameRuleIndexes.Split(",");
                    SettingModel.FileNameRulesIndexes = Array.ConvertAll(SettingModel.FileNameRules, s => ComboBoxIndexConverter(s));
                }
            }
            catch (Exception e)
            {
                logger.WriteLog(e.Message, true);
                return;
            }

        }

        public void PathExampleSetter()
        {
            ImageDetailsModel exImage = new ImageDetailsModel("20050813", "094723", "Thika", "KODAK CX7530", "jpg", null, null, null, "Kodak_CX7530.jpg", null);

            List<string?> pathBuf = new();
            for (int i = 0; i < SettingModel.DirectoryRules.Length; i++)
            {
                switch (SettingModel.DirectoryRules[i])
                {
                    case "Date":
                        if (exImage.DateTaken != null)
                            pathBuf.Add(exImage.DateTaken);
                        else pathBuf.Add("ETC");
                        break;
                    case "CameraModel":
                        if (exImage.CameraModel != null)
                            pathBuf.Add(exImage.CameraModel);
                        else pathBuf.Add("ETC");
                        break;
                    case "Format":
                        if (exImage.Format != null)
                            pathBuf.Add(exImage.Format);
                        else pathBuf.Add("ETC");
                        break;
                    case "Location":
                        if (exImage.Location != null && exImage.Location != "/")
                            pathBuf.Add(exImage.Location);
                        else pathBuf.Add("ETC");
                        break;
                    default:
                        break;
                }
            }
            string pathName = String.Join("\\", pathBuf);
            SettingModel.NewPathExample = String.Format($"{SettingModel.OutputDirectorytPath}\\{pathName}");
        }

        public void FileNameExampleSetter()
        {
            ImageDetailsModel exImage = new ImageDetailsModel("20050813", "094723", "Thika", "KODAK CX7530", "jpg", null, null, null, "Kodak_CX7530.jpg", null);
            List<string?> fileBuf = new();
                for (int i = 0; i < SettingModel.FileNameRules.Length; i++)
                {
                    switch (SettingModel.FileNameRules[i])
                    {
                        case "Date":
                            if (exImage.DateTaken != null)
                                fileBuf.Add(String.Format($"{exImage.DateTaken}_{exImage.TimeTaken}"));
                            else fileBuf.Add("ETC");
                            break;
                        case "CameraModel":
                            if (exImage.CameraModel != null)
                                fileBuf.Add(exImage.CameraModel);
                            else
                                fileBuf.Add("ETC");
                            break;
                        case "Location":
                            if (exImage.Location != null && exImage.Location != "/")
                                fileBuf.Add(exImage.Location);
                            else
                                fileBuf.Add("ETC");
                            break;
                        default:
                            break;
                    }
            }
            string filename = String.Join("_", fileBuf);
            SettingModel.NewFileNameExample = String.Format($"{filename}.{exImage.Format}");
        }

        private int ComboBoxIndexConverter(string s)
        {
            int ret = 0;
            switch (s)
            {
                case "None":
                    ret = 0;
                    break;
                case "Date":
                    ret = 1;
                    break;
                case "CameraModel":
                    ret = 2;
                    break;
                case "Format":
                    ret = 3;
                    break;
                case "Location":
                    ret = 4;
                    break;
            }
            return ret;
                
        }
        #endregion Logical Function

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


    }
}
