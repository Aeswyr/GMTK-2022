using UnityEngine;

[CreateAssetMenu]
public class CharacterSprites : ScriptableObject
{
    public const string Path = "CharacterSprites"; // Assets/Resources/CharacterSprites.asset
    public CharacterDialogueSpriteCollection[] Data;

    public static CharacterDialogueSpriteCollection Load(PetId id)
    {
        return Resources.Load<CharacterSprites>(Path).GetSprites(id);
    }
    
    private CharacterDialogueSpriteCollection GetSprites(PetId id)
    {
        if (Data.TryGet((int)id, out var collection))
        {
            if (collection.Character != id)
            {
                Debug.LogWarning("Character does not match id");
            }
            return collection;
        }
        
        // fallback case
        Debug.LogError("Character not found: " + id);
        return Data.Length > 0 ? Data[0] : default;
    }
}