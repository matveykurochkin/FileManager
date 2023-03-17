using FileManager.Include;
using NLog;
using System;
using System.Windows;

namespace FileManager
{
    public partial class Create : Window
    {
        public Create()
        {
            InitializeComponent();
            _logger.Info($"Creation Menu running. Time: {DateTime.Now}");
        }

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfFileOrFolder.Text != "")
            {
                _logger.Info("Click button add folder on Creation menu, process success");
                Function.AddFolder(Function.pathOnTCWindow, NameOfFileOrFolder.Text);
                Close();
            }
            else
                _logger.Error("Click button add folder on Creation menu, process not success. Error: folder name not specified");
        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfFileOrFolder.Text != "")
            {
                _logger.Info("Click button add file on Creation menu, process success");
                Function.AddFile(Function.pathOnTCWindow, NameOfFileOrFolder.Text);
                Close();
            }
            else
                _logger.Error("Click button add file on Creation menu, process not success. Error: file name not specified");
        }

        private void AddWordDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfFileOrFolder.Text != "")
            {
                _logger.Info("Click button add MS Word document on Creation menu, process success");
                Function.AddFile(Function.pathOnTCWindow, NameOfFileOrFolder.Text, Function.MSWord);
                Close();
            }
            else
                _logger.Error("Click button add MS Word document on Creation menu, process not success. Error: MS Word document name not specified");
        }

        private void CreatePPTFile_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfFileOrFolder.Text != "")
            {
                _logger.Info("Click button add MS Power Point document on Creation menu, process success");
                Function.AddFile(Function.pathOnTCWindow, NameOfFileOrFolder.Text, Function.MSPP);
                Close();
            }
            else
                _logger.Error("Click button add MS Power Point document on Creation menu, process not success. Error: MS Power Point document name not specified");
        }
    }
}