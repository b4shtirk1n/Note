using Note.Models;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Note
{
    public partial class MainWindow : Window
    {
        private readonly List<NoteItem> items = new List<NoteItem>();
        private NoteItem currentItem;
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

            TextRange textRange = new TextRange(Editor.Document.ContentStart,
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

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void FullscreenClick(object sender, RoutedEventArgs e)
        {

        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {

        }

        private void Update()
        {
            items.Clear();

            string path = "Documents";
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                items.Add(new NoteItem(file));
            }
            NoteList.ItemsSource = items;
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
    }
}

