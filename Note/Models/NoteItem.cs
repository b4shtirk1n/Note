using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace Note.Models
{
    public class NoteItem
    {
        public string Path { get; private set; }

        public string Name { get; private set; }

        public string Text { get; private set; }

        public string Description { get; set; }

        public NoteItem(string path)
        {
            Path = path;
            Name = new FileInfo(path).Name.Replace(".rtf", "");

            var doc = new FlowDocument();
            FileStream fileStream = new FileStream(Path, FileMode.OpenOrCreate);
            TextRange textRange = new TextRange(doc.ContentStart, doc.ContentEnd);
            textRange.Load(fileStream, DataFormats.Rtf);
            fileStream.Close();

            Text = textRange.Text;
            Description = Text.Length > 80 ? Text.Substring(0, 80) : Text;
        }
    }
}

