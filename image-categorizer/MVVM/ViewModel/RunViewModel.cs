using image_categorizer.Core;
using image_categorizer.MVVM.Model;
using image_categorizer.Utility;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;


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
                        string? fileName = openFileDialog.FileName as string;
                        if (fileName != null)
                        {
                            RunModel.InputDirectorytPath = fileName;
                        }
                        else
                        {
                            MessageBox.Show("incorrect file path");
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

        private RelayCommand RunButtonCommand()
        {
            RelayCommand ret = new RelayCommand(o =>
            {
                if (RunModel.InputDirectorytPath != null)
                {
                    List<string> imageFiles = Utility.Utility.GetImageFiles(RunModel.InputDirectorytPath);
                    DirectoryInfo directoryInfo = new(RunModel.InputDirectorytPath);
                    RunModel.FileCount = directoryInfo.GetFiles("*.jpg|*.jpeg", System.IO.SearchOption.AllDirectories).Length;
                    foreach (string file in imageFiles)
                    {
                        FileInfo fileInfo = new(file);
                        using FileStream fs = new(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                        BitmapSource image = BitmapFrame.Create(fs);
                        BitmapMetadata? metaData = image.Metadata as BitmapMetadata;

                        ImageDetails imageDetails = new ImageDetails();
                        imageDetails.Location = metaData.Location;
                        imageDetails.DateTaken = metaData.DateTaken;
                        imageDetails.CameraModel = metaData.CameraModel;
                        imageDetails.Format = metaData.Format;
                        RunModel.FileWithDetails.Add(file, imageDetails);
                     }
                }
            });
            return ret;
        }

        public RelayCommand SelectInputPathCommand { get; set; }
        public RelayCommand SelectOutputPathCommand { get; set; }


    }
}
