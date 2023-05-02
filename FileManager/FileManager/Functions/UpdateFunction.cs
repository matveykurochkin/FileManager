using NLog;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace FileManager.Include
{
    internal static class UpdateFunction
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private static string[] Drives = Environment.GetLogicalDrives();

        public static string LoadUpdate(bool isFile, string path, string selectedItemName, ListView listView, TextBox textBox)
        {
            try
            {

                path = textBox.Text;
                LoadFunction.LoadFilesAndDirectories(isFile, path, selectedItemName, listView);
                isFile = false;
                _logger.Info("Load and Update successfully");
                return path;
            }
            catch (Exception ex)
            {
                _logger.Info($"Load and Update not successfully. Error message: {ex.Message}");
            }
            return null;
        }

        public static void UpdateInfoDrive(ComboBox comboBox, Label freeSpace)
        {
            for (int i = 0; i < Drives.Length; i++)
            {
                if ((string)comboBox.SelectedItem == Drives[i])
                {
                    foreach (var drive in DriveInfo.GetDrives())
                    {
                        if (Drives[i] == drive.Name)
                        {
                            long freeSpaceOnDrive = drive.TotalFreeSpace / 1024, totalSizeOnDrive = drive.TotalSize / 1024;
                            freeSpace.Content = $"{freeSpaceOnDrive.ToString("#,#", new CultureInfo("ru-RU"))} k of {totalSizeOnDrive.ToString("#,#", new CultureInfo("ru-RU"))} k free";
                        }
                    }
                }
            }
        }

        public static string UpdatePath(bool isFile, string path, string selectedItemName, TextBox text, ListView listView, ComboBox comboBox)
        {
            try
            {
                if (text.Text != "")
                {
                    _logger.Info($"Following the next button click path on {listView.Name} window succes");
                    path = text.Text;

                    string copyPath = path;
                    TextBox textBox = new TextBox();
                    textBox.Text = copyPath;

                    string driveName = path;
                    driveName = driveName.Remove(3, driveName.Length - 3);

                    for (int i = 0; i < comboBox.Items.Count; i++)
                        if (comboBox.Items[i].ToString() == driveName)
                            comboBox.SelectedItem = comboBox.Items[i];

                    LoadUpdate(isFile, copyPath, selectedItemName, listView, textBox);
                    text.Text = copyPath;
                    path = copyPath;
                    return path;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error following the next button click path on {listView.Name} window. Error message: {ex.Message}");
            }
            return null;
        }
    }
}
