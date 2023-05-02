using NLog;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileManager.Include
{
    internal static class CreateFunction
    {
        public static string MSWord = "docx", MSPP = "ppt";
        public static string pathOnTCWindow = "", selectedonTCItemName = "";
        public static bool isFileTCwindow;
        private static ListView listViewOnTC = new ListView();
        private static TextBox textBoxOnTC = new TextBox();
        public static Label labelOnTC = new Label();
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void CreationMenuView(bool isFile, string path, string selectedItemName, ListView listView, TextBox textBox, Label label)
        {
            bool isWindowOpen = false;

            if (textBox.Text != "")
            {

                foreach (Window window in Application.Current.Windows)
                {
                    if (window is Create)
                    {
                        isWindowOpen = true;
                        window.Activate();
                    }
                }

                if (!isWindowOpen)
                {
                    Create create = new Create();
                    pathOnTCWindow = path;
                    selectedonTCItemName = selectedItemName;
                    isFileTCwindow = isFile;
                    listViewOnTC = listView;
                    textBoxOnTC = textBox;
                    labelOnTC = label;
                    create.Show();
                    _logger.Info("Creation Menu loaded");
                }
            }
            else
                _logger.Error("Creation Menu not loaded");
        }

        public async static void AddFolder(string path)
        {
            try
            {
                string pathString = Path.Combine(path, "New Folder");
                await Task.Run(() =>
                {
                    if (Directory.Exists(pathString))
                    {
                        var result = MessageBox.Show("Папка с таким названием уже существует, заменить ее?",
                            "Информация",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Information);
                        if (result == MessageBoxResult.Yes)
                            Directory.Delete(pathString, true);
                        else
                            return;
                    }
                    Directory.CreateDirectory(pathString);
                });
                _logger.Info($"Folder create successfully. Folder name: New Folder. Folder path: {pathString}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Folder create not successfully. Error message: {ex.Message}");
            }
        }
        public async static void AddFolder(string path, string folderName)
        {
            try
            {
                string pathString = Path.Combine(path, folderName);
                await Task.Run(() =>
                {
                    if (Directory.Exists(pathString))
                    {
                        var result = MessageBox.Show("Папка с таким названием уже существует, заменить ее?",
                            "Информация",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Information);
                        if (result == MessageBoxResult.Yes)
                            Directory.Delete(pathString, true);
                        else
                            return;
                    }
                    Directory.CreateDirectory(pathString);
                });
                UpdateFunction.LoadUpdate(isFileTCwindow, pathOnTCWindow, selectedonTCItemName, listViewOnTC, textBoxOnTC);
                _logger.Info($"Folder create successfully. Folder name: {folderName}. Folder path: {pathString}");

            }
            catch (Exception ex)
            {
                _logger.Error($"Folder create not successfully. Error message: {ex.Message}");
            }
        }

        public static void AddFile(string path)
        {
            try
            {
                if (File.Exists($"{path}\\FileManager.txt"))
                {
                    var result = MessageBox.Show("Файл с таким названием уже существует, заменить его?",
                        "Информация",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                        File.Delete($"{path}\\FileManager.txt");
                    else
                        return;
                }

                FileStream fileStream = File.Create($"{path}\\FileManager.txt");
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file created by File Manager!");
                fileStream.Write(info, 0, info.Length);
                fileStream.Close();
                _logger.Info($"File create successfully. File name: FileManager.txt. File path: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.Error("File create not successfully");
            }
        }

        public static void AddFile(string path, string fileName)
        {
            try
            {
                if (File.Exists($"{path}\\{fileName}.txt"))
                {
                    var result = MessageBox.Show("Файл с таким названием уже существует, заменить его?",
                        "Информация",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                        File.Delete($"{path}\\{fileName}.txt");
                    else
                        return;
                }

                FileStream fileStream = File.Create($"{path}\\{fileName}.txt");
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file created by File Manager!");
                fileStream.Write(info, 0, info.Length);
                fileStream.Close();
                UpdateFunction.LoadUpdate(isFileTCwindow, pathOnTCWindow, selectedonTCItemName, listViewOnTC, textBoxOnTC);
                _logger.Info($"File create successfully. File name: {fileName}.txt. File path: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.Error("File create not successfully");
            }
        }

        public static void AddFile(string path, string fileName, string fileType)
        {
            try
            {
                if (File.Exists($"{path}\\{fileName}.{fileType}"))
                {
                    var result = MessageBox.Show("Файл с таким названием уже существует, заменить его?",
                        "Информация",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                        File.Delete($"{path}\\{fileName}.{fileType}");
                    else
                        return;
                }
                FileStream fileStream = File.Create($"{path}\\{fileName}.{fileType}");
                fileStream.Close();
                UpdateFunction.LoadUpdate(isFileTCwindow, pathOnTCWindow, selectedonTCItemName, listViewOnTC, textBoxOnTC);
                _logger.Info($"File create successfully. File name: {fileName}.{fileType}. File path: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.Error("File create not successfully");
            }
        }

    }
}
