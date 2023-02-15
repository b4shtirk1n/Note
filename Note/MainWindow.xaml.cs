using AutoUpdaterDotNET;
using Note.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Note
{
    public partial class MainWindow : Window
    {
        private List<NoteItem> items = new List<NoteItem>();
        private const string path = "Documents";
        private string find;
        private NoteItem currentItem;
        private TextRange textRange;
        private bool isChange;

        public MainWindow()
        {
            InitializeComponent();
            Update();

            try
            {
                var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);

                AutoUpdater.Mandatory = true;
                AutoUpdater.RunUpdateAsAdmin = false;
                AutoUpdater.InstallationPath = currentDirectory.FullName;
                AutoUpdater.Start("https://raw.githubusercontent.com/b4shtirk1n/Note/Release/Version.xml");
            }
            catch
            {
                MessageBox.Show("Ошибка при обновлении");
            }
        }

        private void NoteListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Editor.IsEnabled = true;

            if (NoteList.SelectedItem == currentItem || NoteList.SelectedItem == null)
                return;

            if (currentItem != null)
                Save();

            textRange = new TextRange(Editor.Document.ContentStart,
                Editor.Document.ContentEnd);

            currentItem = (NoteItem)NoteList.SelectedItem;
            FileStream stream = new FileStream(currentItem.Path, FileMode.Open);
            textRange.Load(stream, DataFormats.Rtf);
            stream.Close();
            Update();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            if (currentItem != null)
                Save();

            Application.Current.Shutdown();
        }

        private void FullscreenClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void MinimizeClick(object sender, RoutedEventArgs e) =>
            WindowState = WindowState.Minimized;

        private void OpenCreateClick(object sender, RoutedEventArgs e)
        {
            CreateDialog.Visibility = Visibility.Visible;
            FileName.Focus();
        }

        private void CreateDialogMouseDown(object sender, MouseButtonEventArgs e) => CloseDialog();

        private void CreateKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && CreateDialog.Visibility == Visibility.Visible)
                CreateClick(sender, null);
        }

        private void CreateClick(object sender, RoutedEventArgs e)
        {
            if (FileName.Text != string.Empty && items.Find(x => x.Name == FileName.Text) == null)
            {
                var doc = new FlowDocument();
                FileStream fileStream = new FileStream($"{path}/{FileName.Text}.rtf",
                    FileMode.Create);

                TextRange textRange = new TextRange(doc.ContentStart, doc.ContentEnd);
                textRange.Text = FileName.Text;
                textRange.Save(fileStream, DataFormats.Rtf);
                fileStream.Close();
            }
            Update();
            CloseDialog();
        }

        private void SearchGotFocus(object sender, RoutedEventArgs e) =>
            TbxFocus(Search, "Поиск...", string.Empty);

        private void SearchLostFocus(object sender, RoutedEventArgs e) =>
            TbxFocus(Search, string.Empty, "Поиск...");

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            find = Search.Text;

            if (NoteList != null)
                Update();
        }

        private void Update()
        {
            string[] files = Directory.GetFiles(path);

            items.Clear();

            foreach (string file in files)
            {
                items.Add(new NoteItem(file));
            }

            if (find != "Поиск..." && items != null)
                items = items.Where(x => x.Name.Contains(Search.Text) ||
                    x.Text.Contains(Search.Text)).ToList();

            NoteList.ItemsSource = items;
            NoteList.Items.Refresh();
        }

        private void Save()
        {
            TextRange textRange = new TextRange(Editor.Document.ContentStart,
                Editor.Document.ContentEnd);

            FileStream stream = new FileStream(currentItem.Path, FileMode.Create);
            textRange.Save(stream, DataFormats.Rtf);
            currentItem.Description = textRange.Text;
            stream.Close();
        }

        private void Edit(DependencyProperty property, object value)
        {
            Editor.Selection.ApplyPropertyValue(property, value);
            isChange = true;
        }

        private void TbxFocus(TextBox tbx, string check, string change)
        {
            if (tbx.Text == check)
                tbx.Text = change;
        }

        private void CloseDialog()
        {
            if (FileName.Text != string.Empty)
                NoteList.SelectedItem = items.First(x => x.Name == FileName.Text);

            CreateDialog.Visibility = Visibility.Hidden;
            FileName.Text = string.Empty;
            Editor.Focus();
        }

        private void WindowStateChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    OwnerScreen.Margin = new Thickness(0);
                    break;
                case WindowState.Maximized:
                    OwnerScreen.Margin = new Thickness(5);
                    break;
            }
        }
    }
}

