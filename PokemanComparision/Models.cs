
namespace PokemanComparision
{
    public class Pokemon
    {
        public List<TypeInfo> Types { get; set; }
    }

    public class TypeInfo
    {
        public Type Type { get; set; }
    }

    public class Type
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class TypeEffectiveness
    {
        public Damage_relations Damage_relations { get; set; }
    }

    public class Damage_relations
    {
        public List<Type> double_damage_to { get; set; }
        public List<Type> double_damage_from { get; set; }
        public List<Type> half_damage_to { get; set; }
        public List<Type> half_damage_from { get; set; }
        public List<Type> no_damage_to { get; set; }
        public List<Type> no_damage_from { get; set; }
    }
}
