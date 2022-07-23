using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
    public AudioSource BarkSource;
    public bool PrintDebug;
    
    [FormerlySerializedAs("CharacterSprites")] [Header("Assets")]
    public CharacterDialogueSpriteCollection[] CharacterSpritesOld;

    [Header("Config")] 
    public float TypewriterSpeed;
    public float TypewriterDelay;
    
    // apparently, the new input system's "pressed" doesn't work like the old one, so tracking the pressed state here
    private float inputCooldown;
    private const float InputInterval = 0.2f;

    private PlayerController cachedPlayer;

    // animator triggers
    private static readonly int IsShowing = Animator.StringToHash("IsShowing");
    private static readonly int CharacterChanged = Animator.StringToHash("CharacterChanged");
    private static readonly int OptionsShowing = Animator.StringToHash("OptionsShowing");

    //Affinity Bar
    private AffinityBar affinityBarScript;

    private PetId prevPet;
    private DogReactionType prevReaction;

    private void Awake()
    {
        affinityBarScript = FindObjectOfType<AffinityBar>();
    }

    public void Dbg(string log)
    {
        if (PrintDebug) Debug.Log(log);
    }

    public void OnGreetDog(PetId dogId)
    {
        Dbg("OnGreetDog " + dogId);
        int affinity = affinityBarScript.ShowThisDogsAffinity((int)dogId);
        AudioClip bark = GetSprites(dogId).Bark;
        if (bark != null)
        {
            SFXHelper.PlaySound(bark);
        }
        OnTalk(dogId, affinity, DogReactionType.Greeting);
    }
    
    public void OnSuccess(PetId dogId, int affinity)
    {
        Dbg("OnSuccess " + dogId);
        affinityBarScript.ShowThisDogsAffinity((int)dogId);
        OnTalk(dogId, affinity, DogReactionType.Happy);
    }
    
    public void OnFail(PetId dogId, int affinity)
    {
        Dbg("OnFail " + dogId);
        affinityBarScript.ShowThisDogsAffinity((int)dogId);
        OnTalk(dogId, affinity, DogReactionType.Angry);
    }

    private void OnTalk(PetId pet, int affinity, DogReactionType reactionType)
    {
        Dbg("OnTalk " + pet);
        if (inputCooldown > 0) return;
        Dbg("OnTalk condition passed");
        if (ModeManager.Instance.Mode != GameMode.Dialogue)
            ModeManager.Instance.ChangeMode(GameMode.Dialogue);

        prevReaction = reactionType;
        
        // ReSharper disable once Unity.NoNullCoalescing
        cachedPlayer = cachedPlayer ?? FindObjectOfType<PlayerController>();

        int dogId = (int)pet;

        // load the dialogue text
        var dog = StatsLoader.Get(dogId);

        // play the show animation
        Controller.SetBool(IsShowing, true);
        Dbg("IsShowing true");

        // set the character sprite
        CharacterIcon.sprite = GetSprites(pet).Get(reactionType);
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
        
        // store for events
        prevPet = pet;
    }

    private CharacterDialogueSpriteCollection GetSprites(PetId id)
    {
        return CharacterSprites.Load(id);
    }

    private void Update()
    {
        inputCooldown -= Time.deltaTime;
        if (ModeManager.Instance.Mode != GameMode.Dialogue) return;
        
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
            if (InputHandler.Instance.interact.pressed && inputCooldown < 0)
            {
                // show options
                if (typewriterDone)
                {
                    if (prevReaction == DogReactionType.Greeting)
                        Controller.SetBool( OptionsShowing, true);
                    else
                        OnChoice(PlayerActionType.Leave);
                }
                // skip
                else
                {
                    typewriterDone = true;
                    Typewriter.Finish();
                }

                inputCooldown = InputInterval;
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
        Dbg("OnChoice " + actionType);
        Controller.SetBool(OptionsShowing, false);
        OnHide();
        switch (actionType)
        {
            case PlayerActionType.Leave:
                affinityBarScript.HideAffinity();
                break;
            case PlayerActionType.Awoo:
                Debug.Log("Awoo " + prevPet);
                ModeManager.Instance.ChangeMode(GameMode.AwooDice);
                break;
            case PlayerActionType.Invite:
                Debug.Log("Invited " + prevPet);
                FlipCupGameStats.opponentId = prevPet;
                ModeManager.Instance.ChangeMode(GameMode.CupDice);
                break;
        }
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
        OnChoice(PlayerActionType.Leave);
    }

    private void OnHide()
    {
        Dbg("OnHide");
        Controller.SetBool(IsShowing, false);
        
        if (ModeManager.Instance.Mode != GameMode.Bar)
        {
            ModeManager.Instance.ChangeMode(GameMode.Bar);
        }
    }
}
