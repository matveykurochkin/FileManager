using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FileManager.Include
{
    public static class Function
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        static string[] Drives = Environment.GetLogicalDrives();
        public static void CopyPath(string path)
        {
            Clipboard.SetText($"{path}");
            _logger.Info("Command copy path to buffer success!");
        }

        public static string MSWord = "docx";
        public static string pathOnTCWindow = "";

        public static void removeBackSlash(TextBox textBox)
        {
            string path = textBox.Text;
            if (path.LastIndexOf("\\") == path.Length - 1)
                textBox.Text = path.Substring(0, path.Length - 1);
        }

        public static void goBack(bool isFile, TextBox textBox)
        {
            try
            {
                removeBackSlash(textBox);
                string path = textBox.Text;
                path = path.Substring(0, path.LastIndexOf("\\"));
                isFile = false;
                textBox.Text = path;
                removeBackSlash(textBox);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void Remove(string path, ListView listView, TextBox textBox, bool isFile)
        {
            var result = MessageBox.Show("Внимание: программа удалит папку в которой вы находитесь и все ее содержимое!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Stop);
            if (result == MessageBoxResult.Yes)
            {
                Directory.Delete(path, true);
                listView.Items.Remove(listView.SelectedItem);
                goBack(isFile, textBox);
                _logger.Info("Delete folder success");
            }
            else
                _logger.Info("Folder not deleted");
        }

        public static string ViewDirectoryAndFileOnWindow(bool isWindowFile, string path, string selectedItemName, ListView listView, ComboBox comboBox, TextBox textBox, Label freeSpace, Label formatDrive, Label typeDrive)
        {
            listView.Items.Clear();

            for (int i = 0; i < Drives.Length; i++)
            {
                if ((string)comboBox.SelectedItem == Drives[i])
                {

                    path = Convert.ToString(Drives[i]);

                    foreach (var drive in DriveInfo.GetDrives())
                    {
                        if (Drives[i] == drive.Name)
                        {
                            freeSpace.Content = $"{drive.TotalFreeSpace} b of {drive.TotalSize} b";
                            formatDrive.Content = $"{drive.DriveFormat}";
                            typeDrive.Content = $"{drive.VolumeLabel}";
                        }
                    }
                }
            }

            LoadFilesAndDirectories(isWindowFile, path, selectedItemName, listView);
            path = path.Trim(new[] { '\\' });
            textBox.Text = path;
            _logger.Info("Files and Directories view successfully");
            return path;
        }
        public static void LoadFilesAndDirectories(bool isFile, string filePath, string selectedItemName, ListView listView)
        {
            DirectoryInfo fileList;
            string tempFilePath = "";
            FileAttributes fileAttr;
            try
            {
                if (isFile)
                {
                    tempFilePath = filePath + "\\" + selectedItemName;
                    FileInfo fileDetails = new FileInfo(tempFilePath);
                    fileAttr = File.GetAttributes(tempFilePath);
                    Process.Start(tempFilePath);
                }
                else
                {
                    fileList = new DirectoryInfo(filePath);
                    FileInfo[] fileInfo = fileList.GetFiles();
                    DirectoryInfo[] directoryInfo = fileList.GetDirectories();

                    listView.Items.Clear();

                    for (int i = 0; i < directoryInfo.Length; i++)
                        listView.Items.Add(directoryInfo[i]);


                    for (int i = 0; i < fileInfo.Length; i++)
                        listView.Items.Add(fileInfo[i]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.Error("Files and Directories not uploaded");
            }
        }
        public static void AddFolder(string path)
        {
            try
            {
                string pathString = Path.Combine(path, "New Folder");
                Directory.CreateDirectory(pathString);
                _logger.Info("Folder create successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.Error("Folder create not successfully");
            }
        }

        public static void AddFolder(string path, string folderName)
        {
            try
            {
                string pathString = Path.Combine(path, folderName);
                Directory.CreateDirectory(pathString);
                _logger.Info("Folder create successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.Error("Folder create not successfully");
            }
        }

        public static void AddFile(string path)
        {
            try
            {
                if (File.Exists($"{path}\\FileManager.txt"))
                    File.Delete($"{path}\\FileManager.txt");

                FileStream fileStream = File.Create($"{path}\\FileManager.txt");
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file created by File Manager!");
                fileStream.Write(info, 0, info.Length);
                fileStream.Close();
                _logger.Info("File create successfully");
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
                    File.Delete($"{path}\\{fileName}.txt");

                FileStream fileStream = File.Create($"{path}\\{fileName}.txt");
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file created by File Manager!");
                fileStream.Write(info, 0, info.Length);
                fileStream.Close();
                _logger.Info("File create successfully");
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
                    File.Delete($"{path}\\{fileName}.{fileType}");

                FileStream fileStream = File.Create($"{path}\\{fileName}.{fileType}");
                _logger.Info("File create successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.Error("File create not successfully");
            }
        }
        public static void CreationMenuView(string path, TextBox textBox)
        {
            if (textBox.Text != "")
            {
                Create create = new Create();
                pathOnTCWindow = path;
                create.Show();
                _logger.Info("Creation Menu loaded");
            }
            _logger.Info("Creation Menu not loaded");
        }

        public static void CopyFileAndDerictories(string sourcePath, string targetPath)
        {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                try
                {
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("File and Directories copy not successfully");
                }
            }

            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("File and Directories copy not successfully");
                }
            }
            _logger.Info("File and Directories copy successfully");
        }
    }
}