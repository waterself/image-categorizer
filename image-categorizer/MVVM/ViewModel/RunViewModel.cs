using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using image_categorizer.Core;
using Microsoft.WindowsAPICodePack.Dialogs;



namespace image_categorizer.MVVM.ViewModel
{
    internal class RunViewModel : ObservableObject
    {
        public RunViewModel()
        {
            InputPathSelectCommand = new RelayCommand(o =>
            {
                CommonOpenFileDialog openFileDialog = new();
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.IsFolderPicker = true;
                if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    MessageBox.Show(openFileDialog.FileName);
                }
            });
            /*SelectOutputPath = */
        }

        public RelayCommand InputPathSelectCommand { get; set; }
        public RelayCommand SelectOutputPath { get; set; }

    }
}
