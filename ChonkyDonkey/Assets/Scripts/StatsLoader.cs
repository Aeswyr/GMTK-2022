
using System;
using System.Collections.Generic;
using UnityEngine;

public static class StatsLoader
{
    private const int
        ID = 0,
        Name = 1,
        CanAwoo = 2,
        Greeting0 = 3,
        Good0 = 4,
        Bad0 = 5,
        Greeting10 = 6,
        Good10 = 7,
        Bad10 = 8,
        Greeting20 = 9,
        Good20 = 10,
        Bad20 = 11,
        Greeting30 = 12,
        Good30 = 13,
        Bad30 = 14,
        Greeting40 = 15,
        Good40 = 16,
        Bad40 = 17;
    
    
    public static DogStatsBlock[] Stats { get; private set; }

    public static DogStatsBlock Get(int dogId)
    {
        // no stats
        if (Stats == null || Stats.Length == 0)
        {
            Debug.LogError($"Stats requested, but failed to initialize");
            return default;
        }

        // return stats
        if (Stats.TryGet(dogId, out DogStatsBlock stats)) return stats;
        
        // not found - fail quietly
        Debug.LogError($"Dog with id {dogId} not found");
        return Stats[0];

    }
    
    [RuntimeInitializeOnLoadMethod]
    public static void LoadStats()
    {
        // debug
        Debug.Log("Loading stats...");
        
        var statsAsset = Resources.Load<TextAsset>("DogStats");
        if (statsAsset == null)
        {
            Debug.LogError("Stats missing!");
            Stats = Array.Empty<DogStatsBlock>();
            return;
        }

        var statsCollector = new List<DogStatsBlock>();
        string statsString = statsAsset.text;
        string[] rows = statsString.Split('\n');
        for (var i = 0; i < rows.Length; i++)
        {
            string[] values = rows[i].Split('\t');
            
            // header row
            if (values[0] == "ID")
            {
                continue;
            }

            var newBlock = new DogStatsBlock()
            {
                ID = int.Parse(values[ID]),
                DisplayName = values[Name],
                CanAwoo = string.Compare(values[CanAwoo],  "TRUE", StringComparison.OrdinalIgnoreCase) == 0,
                Greetings = new string[]
                {
                    values[Greeting0], values[Greeting10], values[Greeting20], values[Greeting30], values[Greeting40]
                },
                GoodAnswer = new string[]
                {
                    values[Good0], values[Good10], values[Good20], values[Good30], values[Good40]
                },
                BadAnswer = new string[]
                {
                    values[Bad0], values[Bad10], values[Bad20], values[Bad30], values[Bad40]
                }
            };
            statsCollector.Add(newBlock);
        }

        Stats = statsCollector.ToArray();
        
        // debug
        string combined = "Stats read:";
        foreach (var stat in Stats)
        {
            combined += "\n" + stat;
        }
        Debug.Log(combined);
    }
}

public struct DogStatsBlock 
{
    public int ID;
    public string DisplayName;
    public bool CanAwoo;
    public string[] Greetings;
    public string[] GoodAnswer;
    public string[] BadAnswer;

    public override string ToString()
    {
        return $"[{ID}]{DisplayName} Greetings:{Greetings.Length} Good:{GoodAnswer.Length} Bad:{BadAnswer.Length}";
    }

    public string GetLine(DogReactionType reactionType, int affinity)
    {
        string[] category = GetCategory(reactionType);
        
        // different behavior for non romantic characters, use random line
        if (!CanAwoo)
        {
            // randomly select a line
            int randomIndex = UnityEngine.Random.Range(0, category.Length);
            // search for the first non-empty line
            for (int i = 0; i < category.Length; i++)
            {
                int idx = (randomIndex + i) % category.Length;
                if (string.IsNullOrEmpty(category[idx])) continue;
                if (string.CompareOrdinal(category[idx], "<Replace Me>") == 0) continue;
                return category[idx];
            }
            return "missing all entries for " + (PetId)ID + " " + reactionType;
        }
        
        int index = DogHelper.GetAffinityIndex(affinity);
        return category.TryGet(index, out string s) ? s : category[0];
        
    }

    private string[] GetCategory(DogReactionType reactionType)
    {
        switch (reactionType)
        {
            case DogReactionType.Greeting:
                return Greetings;
            case DogReactionType.Happy:
                return GoodAnswer;
            case DogReactionType.Angry:
                return BadAnswer;
            default:
                throw new ArgumentOutOfRangeException(nameof(reactionType), reactionType, null);
        }
    }
}

public enum PetId
{
    // ReSharper disable IdentifierTypo
    Default = 0,
    Kaiba = 1,
    Kiefy = 2,
    Leafeon = 3,
    Umbreon = 4,
    Haku = 5,
    KateAndOlive = 6,
    Sesame = 7,
    Riggs = 8,
    BeansAndKnuckels = 9,
    // ReSharper restore IdentifierTypo
}