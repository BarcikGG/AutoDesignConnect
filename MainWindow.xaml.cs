using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;

namespace AutoDesignConnect
{
    public partial class MainWindow : Window
    {
        private string appXamlPath = "";
        private string appXamlDop = "\t\t xmlns:materialDesign=\"http://materialdesigninxaml.net/winfx/xaml/themes\"";
        private string MainWindowXamlPath = "";
        private string AppTextDesign = "         <ResourceDictionary>\r\n            <ResourceDictionary.MergedDictionaries>\r\n                " +
            "<materialDesign:BundledTheme BaseTheme=\"Light\" PrimaryColor=\"DeepPurple\" SecondaryColor=\"Lime\"/>\r\n                " +
            "<ResourceDictionary \r\nSource=\"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml\"/>\r\n" +
            "            </ResourceDictionary.MergedDictionaries>\r\n        " +
            "</ResourceDictionary>\r\n    </Application.Resources>\r\n</Application>";
        private string MainWindowText = "        xmlns:materialDesign=\"http://materialdesigninxaml.net/winfx/xaml/themes\"\n";
        List<string> lines = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string[] files = Directory.GetFiles(dialog.FileName);

                foreach (string file in files)
                {
                    try
                    {
                        if (file.EndsWith("App.xaml")) appXamlPath = file;
                        else if (file.EndsWith("MainWindow.xaml")) MainWindowXamlPath = file;
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show("Файлы App.xaml и MainWindow.xaml не найдены\n" + ex.Message);
                    }
                }

                try
                {
                    //Edit App.xaml
                    string[] appText = File.ReadAllLines(appXamlPath);
                    for (int i = 0; i < appText.Length - 2; i++)
                    {
                        lines.Add(appText[i]);
                        if (i == 3) lines.Add(appXamlDop);
                    }
                    lines.Add(AppTextDesign);
                    File.Delete(appXamlPath);
                    File.WriteAllLines(appXamlPath, lines);

                    //Edit MainWindow.xaml
                    string[] mainWindow = File.ReadAllLines(MainWindowXamlPath);
                    lines.Clear();
                    for (int j = 0; j < mainWindow.Length; j++)
                    {
                        lines.Add(mainWindow[j]);
                        if (j == 1) lines.Add(MainWindowText);
                    }
                    File.Delete(MainWindowXamlPath);
                    File.WriteAllLines(MainWindowXamlPath, lines);
                    Thread.Sleep(1000);
                    MessageBox.Show("Файлы успешно изменены!");
                }
                catch
                {
                    MessageBox.Show("Файлы App.xaml и MainWindow.xaml не найдены");
                }
            }
        }

    }
}
