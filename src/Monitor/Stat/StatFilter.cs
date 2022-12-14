using System.Collections.Generic;
using System.Linq;
using CardEssential.Injector.Stat;

namespace CardEssential.Monitor.Stat;

public class StatFilter
{
    public static readonly Dictionary<string, HashSet<string>> NameFilter = new ()
    {
        { "Mental", new HashSet<string>()
        {
            "Morale", "Stress", "Wakefulness", "Appetite", "Entertainment", "Courage", "Loneliness", "Altered Mind State", "Mania", "Derealization", "Mental Structure", "Isolation",
        } },
        { "Physical", new HashSet<string>()
        {
            "Hydration ", "Satiation", "Stamina", "Filth", "Weight", "Tanning", "Foot Callouses", "Hand Callouses", "Eyesight", "Skin Integrity", 
        } },
        { "Damage", new HashSet<string>()
        {
            "Pain", "Sunburn", "Back Pain", "Bug Bites", "Foot Damage", "Hand Damage", "Blood Loss", "Bruising", "Burns", "Eye Damage", "Lung Damage",
        } },
        { "Internal", new HashSet<string>()
        {
            "Hyperthermia", "Hypothermia", "Blood Pressure", "Fever", "Nausea", "Stool Liquidity", "Immune System", "Headache",
        } },
        { "Chemical", new HashSet<string>()
        {
            "Analgesia", "Spider Lily Effect", "Ginger", "Antibiotics", "Alcohol", "Sodium", "Quinine", "Caffeine", "Capsaicin", "Psylocibin", "Jasmine", "Food Poisoning", "Chine Rose Effect", "Rice Effect", "Venom Krait"
        } },
        { "Protection", new HashSet<string>()
        {
            "Heat Insulation", "Cold Insulation", "Sun Protection", "Rain Protection", "Bug Protection", "Foot Protection", "Armor",
        } },
        { "Saturation", new HashSet<string>()
        {
            "Saturation Coconuts", "Saturation Crustaceans", "Saturation Mollusks", "Saturation Fish", "Saturation Bird", "Saturation Meat", "Saturation Reptile", "Saturation Bananas", "Saturation Fruits", "Saturation Vegetables", "Saturation Sago", "Saturation Sugar", "Saturation Rice", "Saturation Nuts", "Saturation Rations", "Saturation Eggs", "Saturation Dairy", "Saturation Mushrooms", "Saturation Yam",
        } },
        { "Skills", new HashSet<string>()
        {
            "Climbing", "Swimming", "Herbology", "Cooking", "Crafting", "Tailoring", "Woodworking", "Knapping", "Metalworking", "Trapping", "Fishing", "Spear Fishing", "Spear Fighting", "Archery", "Rock Throwing", "Sling", "Percussion", "Stealth", "Insight",
        } }
    };

    public static readonly HashSet<string> PlantNames = new ()
    {
        "Lily", "Kava", "Jasmine", "Ginger", "Bananas", "Yam", "Mango", "Aloe Vera", "Lemon", "Tree", "Chilly", "Coffee", "Almonds", "China Rose", "Rice", "Puffballs", "Mushrooms", "Snake Grass", "Palm", "Wild Jujube", "Sago", "Molinaria"
    };
    
    public static readonly HashSet<string> OfficialClassifications = new ()
    {
        "Mental", "Physical", "Damage", "Internal", "Chemical", "Protection", "Saturation", "Skills"
    };
    
    public static readonly List<string> AllClassifications = new ()
    {
        "Mental", "Physical", "Damage", "Internal", "Chemical", "Protection", "Saturation", "Skills", "Population", "Plant", "Other"
    };

    public static List<StatPack> Filter(string name, List<StatPack> statPacks)
    {
        if (OfficialClassifications.Contains(name))
        {
            return statPacks.Where(pack => NameFilter[name].Contains(pack.DefaultName)).ToList();
        }
        else
        {
            switch (name)
            {
                case "Population":
                    return statPacks.Where(pack => pack.DefaultName.Contains("Population")).ToList();
                case "Plant":
                    return statPacks.Where(pack => PlantNames.Any(plantName => pack.DefaultName.Contains(plantName))).ToList();
            }
        }

        return null;
    }
}