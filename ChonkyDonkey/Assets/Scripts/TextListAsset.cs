using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TextListAsset")]
public class TextListAsset : ScriptableObject
{
    public string[] Entries;

    public bool TryGet(int i, out string res)
    {
        return Entries.TryGet(i, out res);
    }
}
