using System;
using UnityEngine;

[Serializable]
public struct CharacterDialogueSpriteCollection
{
    public PetId Character;
    public Sprite Default;
    public Sprite Happy;
    public Sprite Angry;
    public AudioClip Bark;

    public Sprite Get(DogReactionType reactionType)
    {
        switch (reactionType)
        {
            case DogReactionType.Greeting:
                return Default;
            case DogReactionType.Happy:
                return Happy;
            case DogReactionType.Angry:
                return Angry;
            default:
                throw new ArgumentOutOfRangeException(nameof(reactionType), reactionType, null);
        }
    }
}