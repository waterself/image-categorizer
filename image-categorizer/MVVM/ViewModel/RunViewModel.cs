using image_categorizer.Core;
using image_categorizer.MVVM.Model;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Microsoft.Win32;
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
            ReadSetting();
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
                Logger PathSelectLogger = new Logger(base.ProgramDir, "PathSelect");
                IUtility utility = new Utility(ref PathSelectLogger);
                CommonOpenFileDialog openFileDialog = new();
                //TODO:이니셜 디렉토리를 전에 선택한 디렉토리로 수정 
                openFileDialog.InitialDirectory = RunModel.InputDirectorytPath ?? "C:\\";
                openFileDialog.IsFolderPicker = true;
                if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string? fileName = openFileDialog.FileName as string;
                    if (fileName != null)
                    {
                        if (mode == "input")
                        {
                            RunModel.InputDirectorytPath = fileName;
                            RunModel.FileCount = utility.GetImageFiles(RunModel.InputDirectorytPath).Count + utility.GetVideoFiles(RunModel.InputDirectorytPath).Count;
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
            RunModel.MaxProgress = RunModel.FileCount;
            Logger RunLogger = new Logger(base.ProgramDir, "Categorize");
            IUtility _utility = new Utility(ref RunLogger);
            IGeoCoding geoCoding = new GeoCoding(base.ProgramDir, ref RunLogger);
            geoCoding.GeoCodingInit();
            string[]? directoryRules = Properties.Settings.Default.DirectoryNameRule.Split(',');
            string[]? fileNameRules = Properties.Settings.Default.FileNameRule.Split(',');
            DirectoryInfo inputPathCheck = new(RunModel.InputDirectorytPath);
            DirectoryInfo outputPathCheck = new(RunModel.OutputDirectorytPath);
            if (!outputPathCheck.Exists)
            {
                try
                {
                    outputPathCheck.Create();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    RunLogger.WriteLog(e.Message, true);
                    return;           
                }
            }
     
            RunModel.CategorizeProgress = 0;

            if (RunModel.InputDirectorytPath != null || RunModel.OutputDirectorytPath != null || inputPathCheck.Exists)
            {
                RunModel.IsIdle = false;
                List<string> imageFiles = _utility.GetImageFiles(RunModel.InputDirectorytPath);
                List<string> videoFiles = _utility.GetVideoFiles(RunModel.InputDirectorytPath);

                Task dataExtractTask = Task.Run(() => Parallel.ForEach(imageFiles, file =>
                {
                    ImageDetails imageDetails = new ImageDetails();
                    try
                    {
                        FileInfo fileInfo = new(file);
                        using FileStream fs = new(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                        BitmapSource image = BitmapFrame.Create(fs); //COMException
                        BitmapMetadata? metaData = image.Metadata as BitmapMetadata;

                        imageDetails.IsoDateTime = _utility.FormatIsoDateTime(metaData.DateTaken);
                        imageDetails.DateTaken = _utility.FormatDateTaken(metaData.DateTaken);
                        imageDetails.TimeTaken = _utility.FormatTimeTaken(metaData.DateTaken);
                        imageDetails.CameraModel = _utility.GetCameraModelWithCameraManufacturer(
                            metaData.CameraManufacturer, metaData.CameraModel);
                        imageDetails.Format = metaData.Format;

                        double[]? coordinate = _utility.GetCoordinate(metaData);
                        if (coordinate != null)
                        {
                            imageDetails.Latitude = coordinate[0];
                            imageDetails.Longitude = coordinate[1];
                        }
                        if (coordinate != null)
                        {
                            lock (geoCoding) { imageDetails.Location = geoCoding.GetLocation(coordinate[0], coordinate[1]); }
                        }
                        else
                        {
                            imageDetails.Location = metaData.Location;
                        }

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
                        Random rand = new();
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
                        string message = $"File : {file}, Error : Not Supported File";
                        RunLogger.WriteLog(message, isError:true);
                    }
                    catch (FileFormatException e)
                    {
                        //error: file has damaged
                        string message = $"File : {file}, Error : Corrupted File";
                        RunLogger.WriteLog(message, isError: true);
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                    lock (RunModel.FileWithDetails) {
                        if (!RunModel.FileWithDetails.ContainsKey(file))
                        {
                            RunModel.FileWithDetails.Add(file, imageDetails);
                        }//An item with the same key has already been added.'
                    }

                    RunModel.CategorizeProgress = RunModel.ProgressIncrement();
                }));
                RunLogger.WriteLog("extract end", false, true);
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
                        string message = "directory create error";
                        RunLogger.WriteLog(message, isError: true);
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        MessageBox.Show("Error! Please Check Log file");
                        return;
                    }

                    try
                    {
                        string outputPath = String.Format($"{destPath}\\{fileName}");
                        File.Copy(item.Key, outputPath, true);
                        //IcTagSql.InsertQuery(fileName, item.Value.IsoDateTime, item.Value.Format, item.Value.CameraModel,item.Value.Location , currentTime.ToString("yyyy-MM-dd HH:MM:ss"));
                        //Insert OutputPath
                        insertQueries.Add(new InsertQueryModel(outputPath, item.Value.IsoDateTime, item.Value.Format, item.Value.CameraModel, item.Value.Location, currentTime.ToString("yyyy-MM-dd HH:MM:ss")));
                    }
                    catch (Exception e)
                    {
                        //write log file
                        string message = $"File : {fileName}, Error : {e.Message}";
                        RunLogger.WriteLog(message, isError: true);
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        continue;
                    }
                    RunModel.CategorizeProgress += 1;

                }
                foreach (string videoFile in videoFiles)
                {
                    string destPath = String.Format($"{RunModel.OutputDirectorytPath}\\VideoFolder");
                    string destFile = Path.GetFileName(videoFile);
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
                        string message = "directory create error";
                        RunLogger.WriteLog(message, isError: true, true);
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                    try
                    {
                        string outputPath = String.Format($"{destPath}\\{destFile}");
                        File.Copy(videoFile, outputPath, false);
                    }
                    catch (Exception e)
                    {
                        string message = $"File : {videoFile}, Error : {e.Message}";
                        RunLogger.WriteLog(message, isError: true);
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        continue;
                    }
                    RunModel.CategorizeProgress += 1;
                }
                IIcTagSql summarySQL = new IcTagSql(base.ProgramDir, ref RunLogger);
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
            RunLogger.WriteLog("Categorize Done", isError: false, true);
            RunModel.IsIdle = true;
            RunModel.FileWithDetails.Clear();
            if (!RunLogger.ShowLogFile()) {
                MessageBox.Show("Categorize Done!");
            }
        }


        public void ReadSetting()
        {
            Logger ReadSettingLogger = new Logger(base.ProgramDir, "RunTab ReadSetting");
            IUtility utility = new Utility(ref ReadSettingLogger);
            try
            {
                if (RunModel != null)
                {
                    RunModel.InputDirectorytPath = Properties.Settings.Default.InputDirectory;
                    RunModel.OutputDirectorytPath = Properties.Settings.Default.OutputDirctory;
                    RunModel.FileCount = utility.GetImageFiles(RunModel.InputDirectorytPath).Count + utility.GetVideoFiles(RunModel.InputDirectorytPath).Count;
                }
            }
            catch (Exception e)
            {
                ReadSettingLogger.WriteLog(e.Message, true);
                return;
            }
            
        }
        #endregion Logical Function
    }
}
