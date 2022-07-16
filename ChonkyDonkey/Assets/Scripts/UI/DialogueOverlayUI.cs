using TMPro;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOverlayUI : MonoBehaviour
{
    [Header("Links")]
    public Image CharacterIcon;
    public Animator Controller;
    public TextMeshProUGUI NameLabel;
    public TypewriterText Typewriter;
    public GameObject NextArrow;
    
    [Header("Assets")]
    public CharacterDialogueSpriteCollection[] CharacterSprites;

    private PlayerController cachedPlayer;

    // animator triggers
    private static readonly int IsShowing = Animator.StringToHash("IsShowing");
    private static readonly int CharacterChanged = Animator.StringToHash("CharacterChanged");

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
        // ReSharper disable once Unity.NoNullCoalescing
        cachedPlayer = cachedPlayer ?? FindObjectOfType<PlayerController>();
        cachedPlayer.IsFrozen = true;

        // load the dialogue text
        var dog = StatsLoader.Get(dogId);

        // play the show animation
        Controller.SetBool(IsShowing, true);

        // set the character sprite
        CharacterIcon.sprite = CharacterSprites[dogId].Get(reactionType);
        // bounce
        Controller.SetTrigger(CharacterChanged);

        // queue the dog's name and text
        NameLabel.text = dog.DisplayName;

        // start the typewriter
        Typewriter.PlayTypewriter(dog.GetLine(reactionType, affinity), 10, delay: 1f);
    }

    private void Update()
    {
        bool typewriterDone = Typewriter.GetProgressPercent() >= 1;
        
        // handle interaction
        if (Controller.GetBool(IsShowing))
        {
            if (InputHandler.Instance.menu.pressed)
            {
                Debug.Log("esc press");
                OnChoice(PlayerActionType.Leave);
                return;
            }

            if (InputHandler.Instance.interact.pressed)
            {
                if (!typewriterDone) Typewriter.Finish();
                typewriterDone = true;
            }
        }

        if (NextArrow.activeSelf != typewriterDone)
        {
            NextArrow.SetActiveFast(typewriterDone);
            NextArrow.GetComponent<Animation>()?.Play();
        }
    }

    public void OnChoice(PlayerActionType actionType)
    {
        switch (actionType)
        {
            case PlayerActionType.Leave:
                OnHide();
                break;
        }
    }

    private void OnHide()
    {
        Controller.SetBool(IsShowing, false);
        cachedPlayer.IsFrozen = false;
    }
}
