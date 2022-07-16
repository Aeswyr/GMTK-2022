
using System;
using System.Collections.Generic;
using UnityEngine;

public static class StatsLoader
{
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
            string[] values = rows[i].Split(',');
            
            // header row
            if (values[0] == "ID")
            {
                continue;
            }

            var newBlock = new DogStatsBlock()
            {
                ID = int.Parse(values[0]),
                DisplayName = values[1],
                Greetings = new string[]
                {
                    values[2], values[5], values[8], values[11], values[14]
                },
                GoodAnswer = new string[]
                {
                    values[3], values[6], values[9], values[12], values[15]
                },
                BadAnswer = new string[]
                {
                    values[4], values[7], values[10], values[13], values[16]
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
    public string[] Greetings;
    public string[] GoodAnswer;
    public string[] BadAnswer;

    public override string ToString()
    {
        return $"[{ID}]{DisplayName} Greetings:{Greetings.Length} Good:{GoodAnswer.Length} Bad:{BadAnswer.Length}";
    }

    public string GetLine(DogReactionType reactionType, int affinity)
    {
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

public static class DogConstants
{
    // ReSharper disable IdentifierTypo
    public const int
        Kaiba = 1,
        Kiefy = 2,
        Leafeon = 3,
        Umbreon = 4,
        Haku = 5;
    // ReSharper restore IdentifierTypo
}