namespace BackEnd.Interfaces
{
    public interface IMarvelApiService
    {
        Task<(List<ICharacter> Characters, Exception Error)> GetCharacterNames();

        (bool IsFavorite, Exception Error) ToggleFavorite(int characterId, string userId);

        Task<ICharacter> GetCharacterDetails(int characterId);
    }

}
