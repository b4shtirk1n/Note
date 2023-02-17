using AutoUpdaterDotNET;
using Note.Helpers;
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

        private readonly List<Theme> themes = new List<Theme>()
        {
            new Theme (0, "White", "\\Themes\\White.xaml"),
            new Theme (1, "Black", "\\Themes\\Black.xaml")
        };

        public MainWindow()
        {
            new DirectoryInfo(path).Create();

            ChangeSkin(Properties.Settings.Default.Theme);
            InitializeComponent();
            Fill();
            Update();
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

        private void OwnerScreenMouseDown(object sender, MouseButtonEventArgs e)
        {
            new HeightAnimation(0, 75, MoreCtx, 0.25).StartOut();
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
            Fill();
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

        private void OpenCreateDocClick(object sender, RoutedEventArgs e)
        {
            OpenDialog(BackCreateDoc, CreateDoc);

            FileName.Focus();
        }

        private void CreateDocMouseDown(object sender, MouseButtonEventArgs e) => CloseCreateDoc();

        private void BackgroundKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && CreateDoc.Visibility == Visibility.Visible)
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
            Fill();
            CloseCreateDoc();
        }

        private void SearchGotFocus(object sender, RoutedEventArgs e) =>
            TbxFocus(Search, "Поиск...", string.Empty);

        private void SearchLostFocus(object sender, RoutedEventArgs e) =>
            TbxFocus(Search, string.Empty, "Поиск...");

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            find = Search.Text;

            if (NoteList != null)
                Fill();
        }

        private void OpenMoreCtxClick(object sender, RoutedEventArgs e)
        {
            new HeightAnimation(0, 74, MoreCtx, 0.25).StartIn();
        }

        private void MoreCtxLostFocus(object sender, RoutedEventArgs e)
        {
            new HeightAnimation(0, 74, MoreCtx, 0.25).StartOut();
        }

        private void OpenAppInfoClick(object sender, RoutedEventArgs e)
        {
            OpenDialog(BackAppInfo, AppInfo);
        }

        private void BackAppInfoMouseDown(object sender, RoutedEventArgs e)
        {
            CloseDialog(BackAppInfo, AppInfo);
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            CloseDialog(BackAppInfo, AppInfo);
            Update(true);
        }

        public void ChangeSkin(int themeid)
        {
            Theme newTheme = themes.Find(x => x.Id == themeid);
            ResourceDictionary theme = Application.LoadComponent(new Uri(newTheme.Path,
                UriKind.Relative)) as ResourceDictionary;

            Resources.Clear();
            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add(theme);
        }

        private void Fill()
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

        private void Update(bool error = false)
        {
            try
            {
                var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);

                AutoUpdater.Mandatory = true;
                AutoUpdater.RunUpdateAsAdmin = false;
                AutoUpdater.InstallationPath = currentDirectory.FullName;
                AutoUpdater.ReportErrors = error;
                AutoUpdater.Start("https://raw.githubusercontent.com/b4shtirk1n/Note/Release/Version.xml");
            }
            catch
            {
                MessageBox.Show("Ошибка при обновлении");
            }
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

        private void TbxFocus(TextBox tbx, string check, string change)
        {
            if (tbx.Text == check)
                tbx.Text = change;
        }

        private void OpenDialog(FrameworkElement backElement, FrameworkElement element)
        {
            new OpacityAnimation(0, 0.5, backElement, 0.25).StartIn();
            new OpacityAnimation(0, 1, element, 0.25).StartIn();
        }

        private void CloseDialog(FrameworkElement backElement, FrameworkElement element)
        {
            new OpacityAnimation(0, 0.5, backElement, 0.25).StartOut();
            new OpacityAnimation(0, 1, element, 0.25).StartOut();
        }

        private void CloseCreateDoc()
        {
            CloseDialog(BackCreateDoc, CreateDoc);

            if (FileName.Text != string.Empty)
                NoteList.SelectedItem = items.First(x => x.Name == FileName.Text);

            FileName.Text = string.Empty;
            Editor.Focus();
        }

        private void Edit(DependencyProperty property, object value)
        {
            Editor.Selection.ApplyPropertyValue(property, value);
            isChange = true;
        }
    }
}
