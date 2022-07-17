using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingCharacterBehavior : MonoBehaviour
{
    public int CharacterId;

    public void ShowDialogueForCharacter()
    {
        FindObjectOfType<DialogueOverlayUI>().OnGreetDog(CharacterId);
    }
}
