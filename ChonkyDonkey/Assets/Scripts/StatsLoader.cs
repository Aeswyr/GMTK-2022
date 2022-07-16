
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class StatsLoader
{
    public static DogStatsBlock[] Stats { get; private set; } 
    
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
}