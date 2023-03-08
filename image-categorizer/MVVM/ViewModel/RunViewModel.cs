using image_categorizer.Core;
using image_categorizer.MVVM.Model;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;


namespace image_categorizer.MVVM.ViewModel
{
    class RunViewModel : BaseViewModel
    {
        private BackgroundWorker CategorizeThread;
        #region Constructor
        public RunViewModel()
        {
            SelectInputPathCommand = PathSelectCommand("input");
            SelectOutputPathCommand = PathSelectCommand("output");
            RunButtonCommand = Run();
            CategorizeThread = new();
            CategorizeThread.DoWork += new DoWorkEventHandler(ImageCategorize);
            CategorizeThread.WorkerReportsProgress = true;
            CategorizeThread.WorkerSupportsCancellation = true;
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
                CategorizeThread.RunWorkerAsync();
            });
            return ret;
        }

        public RelayCommand SelectInputPathCommand { get; set; }
        public RelayCommand SelectOutputPathCommand { get; set; }
        public RelayCommand RunButtonCommand { get; set; }
        #endregion RelayCommand

        #region Logical Function

        public void ImageCategorize(object? sender, DoWorkEventArgs doWorkEventArgs)
        {
            
            Random rand = new();
            GeoCoding geoCoding = new GeoCoding();
            geoCoding.GeoCodingInit();
            string[]? directoryRules = Properties.Settings.Default.DirectoryNameRule.Split(',');
            string[]? fileNameRules = Properties.Settings.Default.FileNameRule.Split(',');
            DirectoryInfo inputPathCheck = new(RunModel.InputDirectorytPath);
            RunModel.CategorizeProgress = 0;
            if (RunModel.InputDirectorytPath != null || RunModel.OutputDirectorytPath != null || inputPathCheck.Exists)
            {
                List<string> imageFiles = Utility.GetImageFiles(RunModel.InputDirectorytPath);
                
                Task dataExtractTask = Task.Run(() => Parallel.ForEach(imageFiles, file =>
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
                            imageDetails.Longitude = coordinate[1];
                        }
                        if (coordinate != null)
                        {
                            imageDetails.Location = geoCoding.GetLocation(coordinate[0], coordinate[1]);
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

                    RunModel.CategorizeProgress = RunModel.ProgressIncrement();
                }));
                dataExtractTask.Wait();
                imageFiles.Clear();
                DateTime currentTime = DateTime.Now;
                RunModel.CategorizeProgress = 0;
                List<InsertQueryModel> insertQueries = new();
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
                        //SQLite.InsertQuery(fileName, item.Value.IsoDateTime, item.Value.Format, item.Value.CameraModel,item.Value.Location , currentTime.ToString("yyyy-MM-dd HH:MM:ss"));
                        insertQueries.Add(new InsertQueryModel(fileName, item.Value.IsoDateTime, item.Value.Format, item.Value.CameraModel, item.Value.Location, currentTime.ToString("yyyy-MM-dd HH:MM:ss")));
                    }
                    catch (Exception e)
                    {
                        //write log file
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        continue;
                    }
                    RunModel.CategorizeProgress += 1;
                    
                }
                SQLite summarySQL = new();
                summarySQL.SQLiteinit();
                summarySQL.InsertQuery(insertQueries);
                //messageBox for check delete original files
            }
            else
            {
                MessageBox.Show("Please Select Input/Output Directory");
                RunModel.FileWithDetails.Clear();
            }
            RunModel.CategorizeProgress = RunModel.FileCount;
            
            MessageBox.Show("Categorize Done!");
            RunModel.FileWithDetails.Clear();
        }


        public void ReadSetting()
        {
            if (RunModel != null)
            {
                RunModel.InputDirectorytPath = Properties.Settings.Default.InputDirectory;
                RunModel.OutputDirectorytPath = Properties.Settings.Default.OutputDirctory;
                RunModel.FileCount = Utility.GetImageFiles(RunModel.InputDirectorytPath).Count;
            }
        }
        #endregion Logical Function
    }
}
