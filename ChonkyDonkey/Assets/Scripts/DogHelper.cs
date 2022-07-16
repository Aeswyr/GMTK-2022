using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for helper methods that are specific to this game
/// For generic functions, use the Util class instead
/// </summary>
public static class DogHelper
{
    public static int GetAffinityIndex(int affinity)
    {
        // 0-9 => 0
        // 10-19 => 1
        // etc.
        return affinity / 10; // floor
    }
}

public enum DogReactionType
{
    Greeting,
    Happy,
    Angry
}
