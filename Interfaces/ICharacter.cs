namespace BackEnd.Interfaces
{
    public interface ICharacter
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        Image Thumbnail { get; set; }
        ComicList Comics { get; set; }
        public bool IsFavorite { get; set; }
        public string FirstAppearance { get; set; }

    }

    public class Image
    {
        public string Path { get; set; }
        public string Extension { get; set; }

    }

    public class ComicList
    {
        public int Available { get; set; }
        public int Returned { get; set; }
        public string CollectionURI { get; set; }
        public List<ComicSummary> Items { get; set; }
    }
    public class ComicSummary
    {
        public string ResourceURI { get; set; }
        public string Name { get; set; }
    }

}
