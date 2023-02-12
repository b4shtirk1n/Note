using Note.Models;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Note
{
    public partial class MainWindow : Window
    {
        private readonly List<NoteItem> items = new List<NoteItem>();
        private const string path = "Documents";
        private NoteItem currentItem;
        private TextRange textRange;
        private bool isChange;

        public MainWindow()
        {
            InitializeComponent();
            Update();
        }

        private void NoteListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Editor.IsEnabled = true;

            if (NoteList.SelectedItem == currentItem || NoteList.ItemsSource == null)
                return;

            textRange = new TextRange(Editor.Document.ContentStart,
                Editor.Document.ContentEnd);

            if (IsEdit(textRange))
            {
                currentItem = (NoteItem)NoteList.SelectedItem;
                FileStream stream = new FileStream(currentItem.Path, FileMode.Open);
                textRange.Load(stream, DataFormats.Rtf);
                stream.Close();
            }
            else
            {
                NoteList.SelectedItem = currentItem;
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            if (IsEdit(textRange))
                Application.Current.Shutdown();
        }

        private void FullscreenClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void MinimizeClick(object sender, RoutedEventArgs e) =>
            WindowState = WindowState.Minimized;

        private void OpenCreatelick(object sender, RoutedEventArgs e) =>
            CreateWindow.Visibility = Visibility.Visible;

        private void CreateClick(object sender, RoutedEventArgs e)
        {
            if (FileName.Text != string.Empty)
            {
                var doc = new FlowDocument();
                FileStream fileStream = new FileStream($"{path}/{FileName.Text}.rtf",
                    FileMode.Create);

                TextRange textRange = new TextRange(doc.ContentStart, doc.ContentEnd);
                textRange.Save(fileStream, DataFormats.Rtf);
                fileStream.Close();
            }
            Update();
            CreateWindow.Visibility = Visibility.Hidden;
        }

        private void SearchGotFocus(object sender, RoutedEventArgs e) =>
            TbxFocus(Search, "Поиск...", string.Empty);

        private void SearchLostFocus(object sender, RoutedEventArgs e) =>
            TbxFocus(Search, string.Empty, "Поиск...");

        private void Update()
        {
            items.Clear();

            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                items.Add(new NoteItem(file));
            }
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

        private bool IsEdit(TextRange textRange)
        {
            if (currentItem == null)
                return true;

            if (textRange.Text != currentItem.Text || isChange)
            {
                var result = MessageBox.Show("Сохранить изменения ?", "Save",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                switch (result)
                {
                    default:
                    case MessageBoxResult.Cancel:
                        return false;
                    case MessageBoxResult.Yes:
                        Save();
                        NoteList.Items.Refresh();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
                isChange = false;
                return true;
            }
            return true;
        }

        private void TbxFocus(TextBox tbx, string check, string change)
        {
            if (tbx.Text == check)
                tbx.Text = change;
        }
    }
}

