using System.Collections.Generic;
using System.Linq;
using CardEssential.Injector.Stat;
using CardEssential.Monitor.Stat.Data;

namespace CardEssential.Monitor.Stat;

public class StatFilter
{
    public static readonly Dictionary<string, HashSet<string>> NameFilterDefault = new()
    {
        {
            "Mental", new HashSet<string>()
            {
                "Morale", "Stress", "Wakefulness", "Appetite", "Entertainment", "Courage", "Loneliness",
                "Altered Mind State", "Mania", "Derealization", "Mental Structure", "Isolation",
            }
        },
        {
            "Physical", new HashSet<string>()
            {
                "Hydration ", "Satiation", "Stamina", "Filth", "Weight", "Tanning", "Foot Callouses", "Hand Callouses",
                "Eyesight", "Skin Integrity",
            }
        },
        {
            "Damage", new HashSet<string>()
            {
                "Pain", "Sunburn", "Back Pain", "Bug Bites", "Foot Damage", "Hand Damage", "Blood Loss", "Bruising",
                "Burns", "Eye Damage", "Lung Damage",
            }
        },
        {
            "Internal", new HashSet<string>()
            {
                "Hyperthermia", "Hypothermia", "Blood Pressure", "Fever", "Nausea", "Stool Liquidity", "Immune System",
                "Headache", "Stomach",
            }
        },
        {
            "Chemical", new HashSet<string>()
            {
                "Analgesia", "Spider Lily Effect", "Ginger", "Antibiotics", "Alcohol", "Sodium", "Quinine", "Caffeine",
                "Capsaicin", "Psylocibin", "Jasmine", "Food Poisoning", "Chine Rose Effect", "Rice Effect",
                "Venom Krait"
            }
        },
        {
            "Protection", new HashSet<string>()
            {
                "Heat Insulation", "Cold Insulation", "Sun Protection", "Rain Protection", "Bug Protection",
                "Foot Protection", "Armor",
            }
        },
        {
            "Saturation", new HashSet<string>()
            {
                "Saturation Coconuts", "Saturation Crustaceans", "Saturation Mollusks", "Saturation Fish",
                "Saturation Bird", "Saturation Meat", "Saturation Reptile", "Saturation Bananas", "Saturation Fruits",
                "Saturation Vegetables", "Saturation Sago", "Saturation Sugar", "Saturation Rice", "Saturation Nuts",
                "Saturation Rations", "Saturation Eggs", "Saturation Dairy", "Saturation Mushrooms", "Saturation Yam",
            }
        },
        {
            "Skills", new HashSet<string>()
            {
                "Climbing", "Swimming", "Herbology", "Cooking", "Crafting", "Tailoring", "Woodworking", "Knapping",
                "Metalworking", "Trapping", "Fishing", "Spear Fishing", "Spear Fighting", "Archery", "Rock Throwing",
                "Sling", "Percussion", "Stealth", "Insight",
            }
        },
        {
            "Animals", new HashSet<string>()
            {
                "Population Bees", "Population Boars", "Population Cobra", "Population Conch", "Population Crab",
                "Population Goats", "Population Sea Krait", "Population Lizard", "Population Macaques",
                "Population Monitors", "Population Mouse", "Population Mudskippers", "Oyster Population",
                "Population Partridges", "Population Prawn", "Population Seagulls", "Population Shark",
                "Urchin Population", "Bug Population",
            }
        },
        {
            "Plants", new HashSet<string>()
            {
                "Grasslands Almond Trees", "Highlands Almond Trees", "Bay Aloe Vera", "Beach Aloe Vera",
                "Highlands Aloe Vera", "Outskirts Aloe Vera", "Beach Palm Trees", "Outskirts Palm Trees",
                "Jungle Highlands China Rose", "Jungle Highlands Coffee", "Mangroves Ginger", "Wetlands Ginger",
                "Jungle Jasmine", "Jungle Kava", "Bay Lemon Grass", "Beach Lemon Grass", "Highlands Lemon Grass",
                "Outskirts Lemon Grass", "Jungle Mangos", "Mangroves Nipa Palms", "Bay Palm Trees", "Valley Rice",
                "Wetlands Sago", "Wetlands Spider Lily", "Jungle Molinaria", "Jungle Yam", "Wetlands Yam", 
                "Eastern Grasslands Wild Jujupe Population"
            }
        }
    };

    private StatFilter()
    {
        
    }
    private StatFilter(Dictionary<string, HashSet<string>> nameFilter)
    {
        NameFilter = nameFilter;
    }

    public static StatFilter Create(StatTabDataFile statTabDataFile)
    {
        Dictionary<string, HashSet<string>> res = new();
        foreach (var contain in statTabDataFile.tabContains)
        {
            res.Add(contain.tabName, new HashSet<string>(contain.statNames));
        }

        return new StatFilter(res);
    }

    public Dictionary<string, HashSet<string>> NameFilter { get; set; }
    public List<string> AllClassifications => NameFilter.Keys.ToList();

    public List<StatPack> Filter(string name, List<StatPack> statPacks)
    {
        if (AllClassifications.Contains(name))
        {
            return statPacks.Where(pack => NameFilter[name].Contains(pack.DefaultName)).ToList();
        }

        return new List<StatPack>();
    }
}