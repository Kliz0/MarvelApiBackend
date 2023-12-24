using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

[TestClass]
public class MarvelApiServiceTests
{
    private MarvelApiService _marvelApiService;
    private Mock<HttpClient> _httpClientMock;
    private Mock<IConfiguration> _configurationMock;

    [TestInitialize]
    public void Initialize()
    {
        _httpClientMock = new Mock<HttpClient>();
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(x => x["MarvelApi:ApiKey"]).Returns("your_api_key_here");

        _marvelApiService = new MarvelApiService(_configurationMock.Object);
    }

    [TestMethod]
    public void ToggleFavorite_ShouldToggleFavorite()
    {
        const int characterId = 1;
        const string userId = "testUser";

        var result1 = _marvelApiService.ToggleFavorite(characterId, userId);
        var result2 = _marvelApiService.ToggleFavorite(characterId, userId);

        Assert.IsTrue(result1.IsFavorite);
        Assert.IsTrue(result2.IsFavorite);
        Assert.IsNull(result1.Error);
        Assert.IsNull(result2.Error);
    }

    [TestMethod]
    public void ToggleFavorite_ShouldHandleInvalidUserId()
    {
        const int characterId = 1;
        const string userId = "";

        var result = _marvelApiService.ToggleFavorite(characterId, userId);

        Assert.IsFalse(result.IsFavorite);
        Assert.IsNotNull(result.Error);
    }

    [TestMethod]
    public void ToggleFavorite_ShouldHandleMaxFavorites()
    {
        const int characterId = 1;
        const string userId = "testUser";

        for (int i = 0; i < 5; i++)
        {
            _marvelApiService.ToggleFavorite(characterId + i, userId);
        }

        var result = _marvelApiService.ToggleFavorite(characterId + 5, userId);

        Assert.IsFalse(result.IsFavorite);
        Assert.IsNotNull(result.Error);
    }

}
