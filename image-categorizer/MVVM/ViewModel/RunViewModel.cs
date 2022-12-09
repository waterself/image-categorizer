using image_categorizer.Core;
using image_categorizer.MVVM.Model;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;


namespace image_categorizer.MVVM.ViewModel
{
    class RunViewModel : ObservableObject
    {

        public RunViewModel()
        {
            SelectInputPathCommand = PathSelectCommand("input");
            SelectOutputPathCommand = PathSelectCommand("output");
            RunButtonCommand = Run();

            _runModel = new RunModel();
        }
        private static RunModel? _runModel;

        public RunModel? RunModel
        {
            get => _runModel ?? (_runModel = new RunModel());
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
            RelayCommand ret = new(o =>
            {
                ImageCategorize();
            });
            return ret;
        }

        public RelayCommand SelectInputPathCommand { get; set; }
        public RelayCommand SelectOutputPathCommand { get; set; }
        public RelayCommand RunButtonCommand { get; set; }
        public void ImageCategorize()
        {
            if (RunModel.InputDirectorytPath != null && RunModel.OutputDirectorytPath != null)
            {
                List<string> imageFiles = Utility.GetImageFiles(RunModel.InputDirectorytPath);
                List<string> dates = new();

                foreach (string file in imageFiles) //get metaData for Images
                {
                    FileInfo fileInfo = new(file);
                    using FileStream fs = new(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                    BitmapSource image = BitmapFrame.Create(fs);
                    BitmapMetadata? metaData = image.Metadata as BitmapMetadata;

                    ImageDetails imageDetails = new ImageDetails();
                    double[]? coordinate = Utility.GetCoordinate(metaData);
                    if (coordinate != null)
                    {
                        imageDetails.Latitude = coordinate[0];
                        imageDetails.Longtitude = coordinate[1];
                    }
                    imageDetails.Location = metaData.Location;
                    imageDetails.DateTaken = Utility.FormatDateTaken(metaData.DateTaken);
                    imageDetails.TimeTaken = Utility.FormatTimeTaken(metaData.DateTaken);
                    imageDetails.CameraModel = Utility.GetCameraModelWithCameraManufacturer(
                        metaData.CameraManufacturer, metaData.CameraModel);
                    imageDetails.Format = metaData.Format;
                    dates.Add(imageDetails.DateTaken);

                    if (!RunModel.FileWithDetails.ContainsKey(file))
                    {
                        RunModel.FileWithDetails.Add(file, imageDetails);
                    }//An item with the same key has already been added.'
                     //System.Diagnostics.Debug.WriteLine(imageDetails.CameraModel);
                }
                imageFiles.Clear();
                //make dir
                List<string>? distinctedDate = Utility.ListDistinct(dates);
                //to make directory with distinctedDate and rename and move by EXIF data
                foreach (string date in distinctedDate)
                {
                    string dir;
                    if (date == null) { dir = "ETC"; }
                    else { dir = date; }
                    string dirpath = String.Format($"{RunModel.OutputDirectorytPath}\\{dir}");
                    try
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(dirpath);
                        if (directoryInfo.Exists == false)
                        {
                            directoryInfo.Create();
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                foreach (string key in RunModel.FileWithDetails.Keys)
                {
                    ImageDetails file = RunModel.FileWithDetails[key];
                    string fileName = String.Format($"{file.DateTaken}_{file.TimeTaken}.{file.Format}");
                    string destPath = String.Format($"{RunModel.OutputDirectorytPath}\\{file.DateTaken}\\{fileName}");
                    try
                    {
                        File.Copy(key, destPath, true);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        continue;
                    }
                }
                //messageBox for check delete original files
            }
            else
            {
                MessageBox.Show("Please Select Input/Output Directory");
            }
        }
    }
}
