using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using Xunit;

namespace PokemanComparision.Tests
{
    public class PokemonTypeEffectivenessServiceTests
    {
        [Fact]
        public async Task GetPokemonTypeEffectiveness_ReturnsValidResult()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://pokeapi.co/api/v2/")
            };

            var service = new PokemonTypeEffectivenessService(httpClient);

            var pokemonResponse = new Pokemon
            {
                Types = new List<TypeInfo> {
                    new TypeInfo {
                        Type = new Type { Name = "fire", Url = "https://pokeapi.co/api/v2/type/10/" }
                    }
                }
            };

            var typeEffectivenessResponse = new TypeEffectiveness
            {
                Damage_relations = new Damage_relations
                {
                    double_damage_to = new List<Type> { new Type {  Name = "grass" }  },
                    double_damage_from = new List<Type> { new Type {  Name = "water" }  }
                }
            };

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    if (request.RequestUri.ToString().Contains("pokemon/"))
                    {
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(JsonConvert.SerializeObject(pokemonResponse))
                        };
                    }
                    else if (request.RequestUri.ToString().Contains("type/"))
                    {
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(JsonConvert.SerializeObject(typeEffectivenessResponse))
                        };
                    }
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                });

            var result = await service.GetPokemonTypeEffectiveness("charizard");

            Assert.NotNull(result);
            Assert.Contains("grass", result.StrongAgainst);
            Assert.Contains("water", result.WeakAgainst);
        }
    }
}
