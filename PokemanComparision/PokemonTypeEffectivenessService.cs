using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PokemanComparision
{
    public class PokemonTypeEffectivenessService
    {
        private readonly HttpClient _httpClient;

        public PokemonTypeEffectivenessService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<PokemonTypeEffectivenessResult> GetPokemonTypeEffectiveness(string pokemonName)
        {
            try
            {
                var response = await _httpClient.GetAsync($"pokemon/{pokemonName}/");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error getting pokeman details. Check the pokeman name.");
                    return null;
                }

                var pokemonData = JsonConvert.DeserializeObject<Pokemon>(await response.Content.ReadAsStringAsync());
                var typeUrls = pokemonData.Types.Select(t => t.Type.Url).ToList();

                var result = new PokemonTypeEffectivenessResult();

                foreach (var typeUrl in typeUrls)
                {
                    var typeResponse = await _httpClient.GetAsync(typeUrl);
                    var typeData = JsonConvert.DeserializeObject<TypeEffectiveness>(await typeResponse.Content.ReadAsStringAsync());

                    result.AddEffectivenessData(typeData);
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
                return null;
            }
        }
    }

    public class PokemonTypeEffectivenessResult
    {
        public HashSet<string> StrongAgainst { get; set; } = new HashSet<string>();
        public HashSet<string> WeakAgainst { get; set; } = new HashSet<string>();

        public void AddEffectivenessData(TypeEffectiveness typeData)
        {


            if (typeData.Damage_relations.double_damage_to != null)
            {
                foreach (var type in typeData.Damage_relations.double_damage_to)

                    if (type != null) StrongAgainst.Add(type.Name);
            }

            if (typeData.Damage_relations.half_damage_from != null)
            {
                foreach (var type in typeData.Damage_relations.half_damage_from)
                {
                    if (type != null)
                    {
                        StrongAgainst.Add(type.Name);
                    }
                }
            }

            if (typeData.Damage_relations.no_damage_from != null)
            {
                foreach (var type in typeData.Damage_relations.no_damage_from)
                    if (type != null) StrongAgainst.Add(type.Name);
            }


            if (typeData.Damage_relations.double_damage_from != null)
            {
                foreach (var type in typeData.Damage_relations.double_damage_from)
                    if (type != null) WeakAgainst.Add(type.Name);
            }

            if (typeData.Damage_relations.half_damage_to != null)
            {
                foreach (var type in typeData.Damage_relations.half_damage_to)
                    if (type != null) WeakAgainst.Add(type.Name);
            }

            if (typeData.Damage_relations.no_damage_to != null)
            {
                foreach (var type in typeData.Damage_relations.no_damage_to)
                    if (type != null) WeakAgainst.Add(type.Name);
            }
        }

    }
}
