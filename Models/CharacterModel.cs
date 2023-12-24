using BackEnd.Interfaces;

namespace BackEnd.Models
{

    public class CharacterModel : ICharacter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Image Thumbnail { get; set; }
        public ComicList Comics { get; set; }
        public bool IsFavorite { get; set; }
        public string FirstAppearance { get; set; }
    }
};