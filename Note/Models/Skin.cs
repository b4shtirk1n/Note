namespace Note.Models
{
    public class Theme
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public Theme(int id, string name, string path)
        {
            Id = id;
            Name = name;
            Path = path;
        }
    }
}
