
using BackEnd.Interfaces;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class CharactersController : ControllerBase
{
    private readonly IMarvelApiService _marvelApiService;

    public CharactersController(IMarvelApiService marvelApiService)
    {
        _marvelApiService = marvelApiService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCharacterList()
    {
        try
        {
            var characters = await _marvelApiService.GetCharacterNames();
            return Ok(characters);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{characterId}/toggleFavorite")]
    public IActionResult ToggleFavorite(int characterId, string userId)
    {
        try
        {
            _marvelApiService.ToggleFavorite(characterId, userId);
            return Ok(new { message = "Favorite toggled successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{characterId}")]
    public async Task<IActionResult> GetCharacterDetails(int characterId)
    {
        try
        {
            var characterDetails = await _marvelApiService.GetCharacterDetails(characterId);
            return Ok(characterDetails);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}