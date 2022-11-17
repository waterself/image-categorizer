using image_categorizer.Core;
using image_categorizer.MVVM.Model;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Windows;

namespace image_categorizer.MVVM.ViewModel
{
    internal class RunViewModel : ObservableObject
    {
        public RunViewModel()
        {
            SelectInputPathCommand = PathSelectCommand("input");
            SelectOutputPathCommand = PathSelectCommand("output");
            _runModel = new RunModel();
        }
        private RunModel _runModel = new();

        public RunModel RunModel
        {
            get { return _runModel; }
            set
            {
                _runModel = value;
                OnPropertyChanged();
            }
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
                    if (mode == "input")
                    {
                        RunModel.InputDirectorytPath = openFileDialog.FileName as string;
                        try
                        { 
                            string[] Files = Directory.GetFiles(openFileDialog.FileName, "*", SearchOption.AllDirectories);
                            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(RunModel.InputDirectorytPath);
                            RunModel.FileCount = directoryInfo.GetFiles("*.*", System.IO.SearchOption.AllDirectories).Length;
                            OnPropertyChanged();
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else if (mode == "output")
                    {
                        RunModel.OutputDirectorytPath = openFileDialog.FileName as string;
                    }
                }
            });
            return ret;
        }

        public RelayCommand SelectInputPathCommand { get; set; }
        public RelayCommand SelectOutputPathCommand { get; set; }


    }
}
