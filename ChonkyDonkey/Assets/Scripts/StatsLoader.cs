
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class StatsLoader
{
    private const int
        ID = 0,
        Name = 1,
        CanAwoo = 2,
        Greeting0 = 3,
        Good0 = 4,
        Bad0 = 5,
        Greeting10 = 3,
        Good10 = 4,
        Bad10 = 5,
        Greeting20 = 3,
        Good20 = 4,
        Bad20 = 5,
        Greeting30 = 3,
        Good30 = 4,
        Bad30 = 5,
        Greeting40 = 3,
        Good40 = 4,
        Bad40 = 5;
    
    
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
        // different behavior for non romantic characters, use random line
        if (!CanAwoo)
        {
            
            return "";
        }
        
        int index = DogHelper.GetAffinityIndex(affinity);
        string[] category;
        switch (reactionType)
        {
            case DogReactionType.Greeting:
                category = Greetings;
                break;
            case DogReactionType.Happy:
                category = GoodAnswer;
                break;
            case DogReactionType.Angry:
                category = BadAnswer;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(reactionType), reactionType, null);
        }
        return category.TryGet(index, out string s) ? s : category[0];
        
    }
}

public enum IdentityType
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