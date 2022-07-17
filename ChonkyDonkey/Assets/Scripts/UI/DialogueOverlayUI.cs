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
    public GameObject AwooButton;
    public GameObject InviteButton;
    
    [Header("Assets")]
    public CharacterDialogueSpriteCollection[] CharacterSprites;

    [Header("Config")] 
    public float TypewriterSpeed;
    public float TypewriterDelay;
    
    // apparently, the new input system's "pressed" doesn't work like the old one, so tracking the pressed state here
    private bool inputFlag;

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


    public void OnGreetDog(int dogId)
    {
        int affinity = 0; // TODO get affinity
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
        CharacterIcon.sprite = GetSprites(dogId).Get(reactionType);
        // bounce
        Controller.SetTrigger(CharacterChanged);

        // queue the dog's name and text
        NameLabel.text = dog.DisplayName;

        // start the typewriter
        Typewriter.PlayTypewriter(dog.GetLine(reactionType, affinity), TypewriterSpeed, delay: TypewriterDelay);
        
        // show the appropriate buttons
        // note: will not be visible yet
        // invite and awoo are mutually exclusive by-design
        AwooButton.SetActiveFast(dog.CanAwoo);
        InviteButton.SetActiveFast(!dog.CanAwoo);
    }

    private CharacterDialogueSpriteCollection GetSprites(int id)
    {
        if (CharacterSprites.TryGet(id, out var collection))
        {
            if ((int)collection.Character != id)
            {
                Debug.LogWarning("Character does not match id");
            }
            return collection;
        }
        else
        {
            return CharacterSprites.Length > 0 ? CharacterSprites[0] : default;
        }
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

            // interact is contextual
            if (InputHandler.Instance.interact.pressed && !inputFlag)
            {
                // show options
                if (typewriterDone)
                {
                    Controller.SetBool( OptionsShowing, true);
                }
                // skip
                else
                {
                    typewriterDone = true;
                    Typewriter.Finish();
                }

                inputFlag = true;
            }

            if (InputHandler.Instance.interact.released)
            {
                inputFlag = false;
            }
        }

        bool shouldShowNextArrow = !Controller.GetBool(OptionsShowing) && typewriterDone;
        if (NextArrow.activeSelf != shouldShowNextArrow)
        {
            NextArrow.SetActiveFast(shouldShowNextArrow);
            NextArrow.GetComponent<Animation>()?.Play();
        }
    }

    public void OnChoice(PlayerActionType actionType)
    {
        Controller.SetBool(OptionsShowing, false);
        switch (actionType)
        {
            case PlayerActionType.Leave:
                Controller.SetBool(OptionsShowing, false);
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
