using Note.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Note
{
    public partial class MainWindow : Window
    {
        private readonly List<NoteItem> items = new List<NoteItem>();

        public MainWindow()
        {
            InitializeComponent();

            string path = "Documents";
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                items.Add(new NoteItem(file));
            }
            MessageBox.Show($"{items.First().Name} {items.First().Description}");
        }
    }
}

