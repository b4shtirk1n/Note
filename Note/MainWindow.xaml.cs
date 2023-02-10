﻿using Note.Models;
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

        public MainWindow()
        {
            InitializeComponent();
            Update();
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

        private bool IsEdit(TextRange textRange)
        {
            if (textRange.Text != "\r\n" &&
                textRange.Text.Substring(0, currentItem.Length) != currentItem.GetText())
            {
                var result = MessageBox.Show("Сохранить изменения ?", "Save",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                switch (result)
                {
                    default:
                    case MessageBoxResult.Cancel:
                        return false;
                    case MessageBoxResult.Yes:
                        break;
                    case MessageBoxResult.No:
                        break;
                }
                return true;
            }
            return true;
        }

        private void NoteListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NoteList.SelectedItem == currentItem)
                return;

            TextRange textRange = new TextRange(Editor.Document.ContentStart,
                Editor.Document.ContentEnd);

            if (IsEdit(textRange))
            {
                currentItem = (NoteItem)NoteList.SelectedItem;
                textRange.Text = currentItem.GetText();
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
    }
}

