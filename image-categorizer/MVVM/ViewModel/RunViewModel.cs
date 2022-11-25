using image_categorizer;
using image_categorizer.Core;
using image_categorizer.MVVM.Model;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            RunButtonCommand = Run();
            
            _runModel = new RunModel();
        }
        private RunModel? _runModel = new();

        public RunModel? RunModel
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
                    string? fileName = openFileDialog.FileName as string;
                    if (fileName != null)
                    {
                        if (mode == "input")
                        {
                            RunModel.InputDirectorytPath = fileName;
                            List<string> imageFiles = Utility.GetImageFiles(RunModel.InputDirectorytPath);
                            RunModel.FileCount = imageFiles.Count;
                            OnPropertyChanged();
                        }
                        else if (mode == "output")
                        {
                            RunModel.OutputDirectorytPath = fileName;
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

        private RelayCommand Run()
        {
            RelayCommand ret = new RelayCommand(o =>
            {
                //will modulize
                if (RunModel.InputDirectorytPath != null) //will add && RunModel.OutputDirectorytPath != null
                {
                    List<string> imageFiles = Utility.GetImageFiles(RunModel.InputDirectorytPath);
                    List<string> dates = new();
                    
                    foreach (string file in imageFiles) //get metaData for Images
                    {
                        FileInfo fileInfo = new(file);
                        using FileStream fs = new(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                        BitmapSource image = BitmapFrame.Create(fs);
                        BitmapMetadata? metaData = image.Metadata as BitmapMetadata;
                        System.Diagnostics.Debug.WriteLine(Convert.ToDateTime(metaData.DateTaken));
                        ImageDetails imageDetails = new ImageDetails();
                        imageDetails.Location = metaData.Location;
                        imageDetails.DateTaken = Utility.FormatDateTaken(metaData.DateTaken);    
                        imageDetails.TimeTaken = Utility.FormatTimeTaken(metaData.DateTaken);
                        dates.Add(metaData.DateTaken);
                        imageDetails.CameraModel = Utility.GetCameraModelWithCameraManufacturer(
                            metaData.CameraManufacturer, metaData.CameraModel);
                        imageDetails.Format = metaData.Format;
                        if (!RunModel.FileWithDetails.ContainsKey(file))
                        {
                            RunModel.FileWithDetails.Add(file, imageDetails);
                        }//An item with the same key has already been added.'
                        //System.Diagnostics.Debug.WriteLine(imageDetails.CameraModel);
                    }
                    
                    dates.Sort();
                    List<string>? distinctedDate = Utility.ListDistinct(dates);
                    //to make directory with distinctedDate and rename and move by EXIF data
                    //Guid? guid = null;  


                }
                else {
                    MessageBox.Show("Please Select Input/Output Directory");
                }
            });
            return ret;
        }

        public RelayCommand SelectInputPathCommand { get; set; }
        public RelayCommand SelectOutputPathCommand { get; set; }
        public RelayCommand RunButtonCommand { get; set; }
        

        

    }
}
