using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager.Internal
{
    internal class Load
    {
        private bool isFile = false, isFirstUse = true;

        MainWindow _mainWindow = new MainWindow();

       public void LoadFilesAndDirectoriesOnFirstWindow(string filePath, string selectedItemName)
        {
            DirectoryInfo fileList;
            string tempFilePath = "";
            FileAttributes fileAttr;
            try
            {
                if (isFile)
                {
                    tempFilePath = filePath + "/" + selectedItemName;
                    FileInfo fileDetails = new FileInfo(tempFilePath);
                    fileAttr = File.GetAttributes(tempFilePath);
                    Process.Start(tempFilePath);
                }
                else
                {
                    fileList = new DirectoryInfo(filePath);
                    FileInfo[] fileInfo = fileList.GetFiles();
                    DirectoryInfo[] directoryInfo = fileList.GetDirectories();

                   _mainWindow.FirstWindowOnFileManager.Items.Clear();

                    for (int i = 0; i < fileInfo.Length; i++)
                    {
                        _mainWindow.FirstWindowOnFileManager.Items.Add(fileInfo[i]);
                    }
                    for (int i = 0; i < directoryInfo.Length; i++)
                    {
                        _mainWindow.FirstWindowOnFileManager.Items.Add(directoryInfo[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
