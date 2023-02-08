using System.IO;

namespace Note.Models
{
    public class NoteItem
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public NoteItem(string path)
        {
            Name = new FileInfo(path).Name;
            Description = File.ReadAllText(path);
        }
    }
}

