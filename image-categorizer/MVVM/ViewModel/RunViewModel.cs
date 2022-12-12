using image_categorizer.Core;
using image_categorizer.MVVM.Model;
using image_categorizer.MVVM.ViewModel;
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
                    //ReadSetting();
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
            Random rand = new Random();
            string[]? directoryRules = new[] { "Format", "CameraModel", "Date", "None" };
            string[]? fileNameRules = new[] { "Date", "None", "None", "None" };
            if (RunModel.InputDirectorytPath != null && RunModel.OutputDirectorytPath != null)
            {
                List<string> imageFiles = Utility.GetImageFiles(RunModel.InputDirectorytPath);
                Dictionary<string, List<string>>? TagList = new();
                for (int i = 0; i < directoryRules.Length; i++)
                {
                    TagList.Add(directoryRules[i], new List<string>());
                }

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
                        imageDetails.FileName = String.Join("_", fileBuf); // make setting seperator
                    }
                    if (!RunModel.FileWithDetails.ContainsKey(file))
                    {
                        RunModel.FileWithDetails.Add(file, imageDetails);
                    }//An item with the same key has already been added.'
                }
                imageFiles.Clear();

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
                        MessageBox.Show(e.Message);
                    }

                    try
                    {
                       File.Copy(item.Key, String.Format($"{destPath}\\{fileName}"), true);
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
            MessageBox.Show("Categorize Done!");
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
