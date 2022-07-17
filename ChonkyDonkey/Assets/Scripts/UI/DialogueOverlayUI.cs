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
    private static readonly int OptionsShowing = Animator.StringToHash("OptionsShowing");

    //Affinity Bar
    private AffinityBar affinityBarScript;

    private void Awake()
    {
        affinityBarScript = FindObjectOfType<AffinityBar>();
    }


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
                Controller.SetBool(OptionsShowing, false);
                OnChoice(PlayerActionType.Leave);
                return;
            }

            if (InputHandler.Instance.interact.pressed)
            {
                if (typewriterDone)
                {
                    Controller.SetBool(OptionsShowing, true);
                }
                else
                {
                    typewriterDone = true;
                }
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
        Controller.SetBool(OptionsShowing, false);
        switch (actionType)
        {
            case PlayerActionType.Leave:
                break;
            case PlayerActionType.Awoo:
                Debug.Log("Awoo TODO");
                break;
            case PlayerActionType.Invite:
                Debug.Log("Invite TODO");
                break;
        }
        OnHide();
    }

    public void OnAwooPressed()
    {
        OnChoice(PlayerActionType.Awoo);
    }

    public void OnInvitePressed()
    {
        OnChoice(PlayerActionType.Invite);
    }
    
    public void OnLeavePressed()
    {
        OnChoice(PlayerActionType.Invite);
    }

    private void OnHide()
    {
        Controller.SetBool(IsShowing, false);
        cachedPlayer.IsFrozen = false;
        affinityBarScript.HideAffinity();
    }
}
