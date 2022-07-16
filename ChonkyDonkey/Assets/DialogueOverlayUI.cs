using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOverlayUI : MonoBehaviour
{
    public Image CharacterIcon;
    public Animator Controller;
    public TextMeshProUGUI NameLabel;
    public TypewriterText Typewriter;

    public CharacterDialogueSpriteCollection[] CharacterSprites;
    
    // animator triggers
    private static readonly int Greet = Animator.StringToHash("Greet");
    private static readonly int Happy = Animator.StringToHash("Happy");
    private static readonly int Angry = Animator.StringToHash("Angry");

    public void OnGreetDog(int dogId, int affinity)
    {
        OnTalk(dogId, affinity, DogReactionType.Greeting);
    }
    
    public void OnSuccess(int dogId, int affinity)
    {
        OnTalk(dogId, affinity, DogReactionType.Happy);
    }
    
    public void OnFail(int dogId, int affinity)
    {
        OnTalk(dogId, affinity, DogReactionType.Angry);
    }

    private void OnTalk(int dogId, int affinity, DogReactionType reactionType)
    {
        // load the dialogue text
        var dog = StatsLoader.Get(dogId);

        // play the show animation
        Controller.SetTrigger(Greet);

        // set the character sprite
        CharacterIcon.sprite = CharacterSprites[dogId].Get(reactionType);

        // queue the dog's name and text
        NameLabel.text = dog.DisplayName;

        // start the typewriter
        Typewriter.PlayTypewriter(dog.GetLine(reactionType, affinity), 10);
    }
}
