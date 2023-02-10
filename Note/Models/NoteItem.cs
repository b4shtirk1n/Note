using System.IO;

namespace Note.Models
{
    public class NoteItem
    {
        public string Path { get; private set; }

        public string Name { get; private set; }

        public int Length { get; private set; }

        public string Description { get; private set; }

        public NoteItem(string path)
        {
            Path = path;
            Name = new FileInfo(path).Name.Replace(".txt", "");
            Length = GetText().Length;
            Description = Length > 80 ? GetText().Substring(0, 80) : GetText();
        }

        public string GetText()
        {
            return File.ReadAllText(Path);
        }
    }
}

