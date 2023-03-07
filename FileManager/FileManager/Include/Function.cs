using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FileManager.Include
{
    public static class Function
    {
         static string[] Drives = Environment.GetLogicalDrives();
        public static void CopyPath(string path)
        {
            Clipboard.SetText($"{path}");
        }

        public static void removeBackSlash(TextBox textBox)
        {
            string path = textBox.Text;
            if (path.LastIndexOf("\\") == path.Length - 1)
            {
                textBox.Text = path.Substring(0, path.Length - 1);
            }
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
            }
            goBack(isFile, textBox);
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
                    {
                        listView.Items.Add(directoryInfo[i]);
                    }

                    for (int i = 0; i < fileInfo.Length; i++)
                    {
                        listView.Items.Add(fileInfo[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void AddFolder(string path)
        {
            string pathString = System.IO.Path.Combine(path, "New Folder");
            Directory.CreateDirectory(pathString);
        }
    }
}