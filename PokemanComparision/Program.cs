// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using PokemanComparision;

var serviceProvider = new ServiceCollection()
    .AddHttpClient()
    .AddSingleton<PokemonTypeEffectivenessService>()
    .BuildServiceProvider();

var service = serviceProvider.GetService<PokemonTypeEffectivenessService>();

Console.WriteLine("Enter a pokeman name:");
string pokemonName = Console.ReadLine().ToLower();

if (string.IsNullOrEmpty(pokemonName))
{
    Console.WriteLine("Pokean Name cannot be blank!");
    return;
}

try
{
    var result = await service.GetPokemonTypeEffectiveness(pokemonName);

    if (result != null)
    {
        Console.WriteLine("Strong against: " + string.Join(", ", result.StrongAgainst));
        Console.WriteLine("Weak against: " + string.Join(", ", result.WeakAgainst));
    }
    else
    {
        Console.WriteLine(" pokeman not found or error occurred.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}