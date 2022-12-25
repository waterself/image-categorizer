using image_categorizer.Core;
using image_categorizer.MVVM.Model;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;


namespace image_categorizer.MVVM.ViewModel
{
    class RunViewModel : BaseViewModel
    {
        #region Constructor
        public RunViewModel()
        {

            SelectInputPathCommand = PathSelectCommand("input");
            SelectOutputPathCommand = PathSelectCommand("output");
            RunButtonCommand = Run();

        }
        #endregion Constructor

        #region Model Property
        private static RunModel? _runModel;

        public RunModel? RunModel
        {
            get
            {
                if (_runModel == null)
                {
                    _runModel = new RunModel();
                    ReadSetting();
                }
                return _runModel;
            }
            set
            {
                _runModel = value;
                OnPropertyChanged();
            }
        }
        #endregion Model Property

        #region RelayCommand
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
        #endregion RelayCommand

        #region Logical Function
        public void ImageCategorize()
        {
            Random rand = new();
            string[]? directoryRules = Properties.Settings.Default.DirectoryNameRule.Split(',');
            string[]? fileNameRules = Properties.Settings.Default.FileNameRule.Split(',');
            DirectoryInfo inputPathCheck = new(RunModel.InputDirectorytPath);
            if (RunModel.InputDirectorytPath != null || RunModel.OutputDirectorytPath != null || inputPathCheck.Exists)
            {
                List<string> imageFiles = Utility.GetImageFiles(RunModel.InputDirectorytPath);
                foreach (string file in imageFiles) //get metaData for Images
                {
                    ImageDetails imageDetails = new ImageDetails();
                    try
                    {
                        FileInfo fileInfo = new(file);
                        using FileStream fs = new(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                        BitmapSource image = BitmapFrame.Create(fs); //COMException
                        BitmapMetadata? metaData = image.Metadata as BitmapMetadata;

                        double[]? coordinate = Utility.GetCoordinate(metaData);
                        if (coordinate != null)
                        {
                            imageDetails.Latitude = coordinate[0];
                            imageDetails.longitude = coordinate[1];
                        }
                        if (coordinate != null)
                        {
                            imageDetails.Location = GeoCoding.GetLocation(coordinate[0], coordinate[1]);
                        }
                        else
                        {
                            imageDetails.Location = metaData.Location;
                        }
                        imageDetails.IsoDateTime = Utility.FormatIsoDateTime(metaData.DateTaken);
                        imageDetails.DateTaken = Utility.FormatDateTaken(metaData.DateTaken);
                        imageDetails.TimeTaken = Utility.FormatTimeTaken(metaData.DateTaken);
                        imageDetails.CameraModel = Utility.GetCameraModelWithCameraManufacturer(
                            metaData.CameraManufacturer, metaData.CameraModel);
                        imageDetails.Format = metaData.Format;

                        List<string?> pathBuf = new();
                        for (int i = 0; i < directoryRules.Length; i++)
                        {
                            switch (directoryRules[i])
                            {
                                case "Date":
                                    if (imageDetails.DateTaken != null)
                                        pathBuf.Add(imageDetails.DateTaken);
                                    else pathBuf.Add("ETC");
                                    break;
                                case "CameraModel":
                                    if (imageDetails.CameraModel != null)
                                        pathBuf.Add(imageDetails.CameraModel);
                                    else pathBuf.Add("ETC");
                                    break;
                                case "Format":
                                    if (imageDetails.Format != null)
                                        pathBuf.Add(imageDetails.Format);
                                    else pathBuf.Add("ETC");
                                    break;
                                case "Location":
                                    if (imageDetails.Location != null && imageDetails.Location != "/")
                                        pathBuf.Add(imageDetails.Location);
                                    else pathBuf.Add("ETC");
                                    break;
                                default:
                                    break;
                            }
                        }
                        imageDetails.FilePath = String.Join("\\", pathBuf);

                        List<string?> fileBuf = new();
                        for (int i = 0; i < fileNameRules.Length; i++)
                        {
                            switch (fileNameRules[i])
                            {
                                case "Date":
                                    if (imageDetails.DateTaken != null)
                                        fileBuf.Add(String.Format($"{imageDetails.DateTaken}_{imageDetails.TimeTaken}"));
                                    else fileBuf.Add(rand.Next(2147483647).ToString());
                                    break;
                                case "CameraModel":
                                    if (imageDetails.CameraModel != null)
                                        fileBuf.Add(imageDetails.CameraModel);
                                    else
                                        fileBuf.Add("ETC");
                                    break;
                                case "Location":
                                    if (imageDetails.Location != null && imageDetails.Location != "/")
                                        fileBuf.Add(imageDetails.Location);
                                    else
                                        fileBuf.Add("ETC");
                                    break;
                                default:
                                    break;
                            }
                        }
                        imageDetails.FileName = String.Join("_", fileBuf); // make setting seperator

                    }
                    catch (NotSupportedException) // no exif data in imagefile(ex. png)
                    {
                        FileInfo notSupported = new(file);
                        imageDetails.FilePath = "No_Data_To_Categorize";
                        imageDetails.FileName = notSupported.Name;
                    }
                    catch (FileFormatException e)
                    {
                        //error: file has damaged
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                    if (!RunModel.FileWithDetails.ContainsKey(file))
                    {
                        RunModel.FileWithDetails.Add(file, imageDetails);
                    }//An item with the same key has already been added.'
                }
                imageFiles.Clear();
                DateTime currentTime = DateTime.Now;
                foreach (KeyValuePair<string, ImageDetails> item in RunModel.FileWithDetails)
                {

                    string fileName = String.Format($"{item.Value.FileName}.{item.Value.Format}");
                    string destPath = String.Format($"{RunModel.OutputDirectorytPath}\\{item.Value.FilePath}");
                    try
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(destPath);
                        if (directoryInfo.Exists == false)
                        {
                            directoryInfo.Create();
                            SQLite.InsertQuery(fileName, item.Value.IsoDateTime, item.Value.Format, item.Value.CameraModel, currentTime.ToString("yyyy-MM-dd HH:MM:ss"));
                        }
                    }
                    catch (Exception e)
                    {
                        //write log file
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }

                    try
                    {
                        File.Copy(item.Key, String.Format($"{destPath}\\{fileName}"), true);
                    }
                    catch (Exception e)
                    {
                        //write log file
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        continue;
                    }
                }
                //messageBox for check delete original files
            }
            else
            {
                MessageBox.Show("Please Select Input/Output Directory");
                RunModel.FileWithDetails.Clear();
            }
            MessageBox.Show("Categorize Done!");
            RunModel.FileWithDetails.Clear();
        }


        public void ReadSetting()
        {
            if (RunModel != null)
            {
                RunModel.InputDirectorytPath = Properties.Settings.Default.InputDirectory;
                RunModel.OutputDirectorytPath = Properties.Settings.Default.OutputDirctory;
            }
        }
        #endregion Logical Function
    }
}
