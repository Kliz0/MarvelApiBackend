using BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd.Interfaces
{
    public interface IMarvelApiService
    {
        Task<(List<ICharacter> Characters, Exception Error)> GetCharacterNames();

        (bool IsFavorite, Exception Error) ToggleFavorite(int characterId, string userId);

        Task<ICharacter> GetCharacterDetails(int characterId);
    }

}
