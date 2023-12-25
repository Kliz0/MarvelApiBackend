using BackEnd.Interfaces;
using BackEnd.Models;

public class MarvelApiService : IMarvelApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;
    private readonly string _apiUrl;

    private readonly Dictionary<string, List<int>> _userFavorites = new Dictionary<string, List<int>>();

    public MarvelApiService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _configuration = configuration;
        _apiKey = _configuration["MarvelApi:ApiKey"];
        _apiUrl = "https://gateway.marvel.com/v1/public/characters";
    }

    public async Task<(List<ICharacter> Characters, Exception Error)> GetCharacterNames()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<MarvelApiResponse>($"{_apiUrl}?apikey={_apiKey}");

            if (response?.Data?.Results == null)
            {
                return (new List<ICharacter>(), new Exception("Unexpected API response or data."));
            }

            var characterModels = response.Data.Results.Select(apiCharacter => MapApiCharacterToICharacter(apiCharacter)).ToList();

            var orderedCharacters = characterModels
                .OrderByDescending(c => c.IsFavorite)
                .ThenBy(c => c.Name)
                .ToList();

            return (orderedCharacters, null);
        }
        catch (Exception ex)
        {
            return (null, ex);
        }
    }

    public (bool IsFavorite, Exception Error) ToggleFavorite(int characterId, string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                return (false, new InvalidOperationException("User ID is required to toggle favorites."));
            }

            if (_userFavorites.ContainsKey(userId))
            {
                var userFavorites = _userFavorites[userId];

                if (userFavorites.Contains(characterId))
                {
                    userFavorites.Remove(characterId);
                }
                else
                {
                    if (userFavorites.Count < 5)
                    {
                        userFavorites.Add(characterId);
                    }
                    else
                    {
                        var errorMessage = "Cannot add more than 5 favorites.";
                        return (false, new InvalidOperationException(errorMessage));
                    }
                }
            }
            else
            {
                _userFavorites[userId] = new List<int> { characterId };
            }

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex);
        }
    }

    public async Task<ICharacter> GetCharacterDetails(int characterId)
    {
        var response = await _httpClient.GetFromJsonAsync<MarvelApiResponse>($"{_apiUrl}/{characterId}?apikey={_apiKey}");

        var characterDetails = response?.Data?.Results?.Select(apiCharacter => MapApiCharacterToICharacter(apiCharacter)).FirstOrDefault() as CharacterModel;

        return characterDetails;
    }

    private ICharacter MapApiCharacterToICharacter(ICharacter apiCharacter)
    {
        try
        {
            return new CharacterModel
            {
                Id = apiCharacter.Id,
                Name = apiCharacter.Name,
                Description = apiCharacter.Description,
                Thumbnail = new Image
                {
                    Path = apiCharacter.Thumbnail?.Path,
                    Extension = apiCharacter.Thumbnail?.Extension
                },
                Comics = new ComicList
                {
                    Available = apiCharacter.Comics?.Available ?? 0,
                    Returned = apiCharacter.Comics?.Returned ?? 0,
                    CollectionURI = apiCharacter.Comics?.CollectionURI,
                    Items = apiCharacter.Comics?.Items?.Select(apiComic => new ComicSummary
                    {
                        ResourceURI = apiComic.ResourceURI,
                        Name = apiComic.Name
                    }).ToList()
                },
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error in MapApiCharacterToICharacter: {ex.Message}");
        }
    }

}



public class MarvelApiResponse
{
    public Data? Data { get; set; }
}

public class Data
{
    public CharacterModel[]? Results { get; set; }
}
