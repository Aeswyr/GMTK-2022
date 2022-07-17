using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypewriterText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueLabel;
    [SerializeField] private TextMeshPro dialogueTextMesh;
    [SerializeField] private AudioSource soundSource;

    [Header("Auto config")]
    [SerializeField] private string textToAutoPlay;
    [SerializeField] private float clearAfterPause;

    private struct InputData
    {
        public string FullText;
        public AudioClip[] TypewriterSounds;
        public float TypewriterSpeed;
    }

    // animation
    private InputData inputData;
    private float textAppearingAnimationProgress = 0;
    private int prevTextAppearingAnimationCharacter;

    public void PlayTypewriter(string text, float lettersPerSecond, AudioClip[] sounds = null, float delay = 0f, int startingProgress = 0)
    {
        if (text == null) return;

        string prev = inputData.FullText ?? "";

        // params
        inputData = new InputData { FullText = text, TypewriterSounds = sounds ?? new AudioClip[] { soundSource.clip }, TypewriterSpeed = lettersPerSecond };
        
        VisualReset(startingProgress);
        
        textAppearingAnimationProgress -= (inputData.TypewriterSpeed * delay); 
    }

    public void Clear()
    {
        inputData = default;
        VisualReset();
    }

    private void VisualReset(int resetPoint=0)
    {
        // reset ui
        textAppearingAnimationProgress = resetPoint;
        SetText(""); // dialogueLabel.text handled by Update()
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(textToAutoPlay))
        {
            PlayTypewriter(textToAutoPlay, 10, new AudioClip[] { soundSource.clip });
            textToAutoPlay = null;
        }
    }

    private void Update()
    {
        if (inputData.FullText == null) return;
        textAppearingAnimationProgress += Time.deltaTime * inputData.TypewriterSpeed;

        if (clearAfterPause > 0 && (textAppearingAnimationProgress - inputData.FullText.Length) / inputData.TypewriterSpeed > clearAfterPause)
        {
            inputData.FullText = null;
            SetText(null);
            return;
        }

        // update text
        string fullText = inputData.FullText;
        if (Mathf.FloorToInt(textAppearingAnimationProgress) < fullText.Length)
        {
            int character = Mathf.FloorToInt(textAppearingAnimationProgress);
            if (character < 0) character = 0;
            if (character < fullText.Length && character > prevTextAppearingAnimationCharacter && !char.IsWhiteSpace(fullText[character]))
            {
                AudioClip[] audioClips = inputData.TypewriterSounds;
                if (audioClips.Length > 0)
                {
                    soundSource.PlayOneShot(audioClips[0]);
                }
            }
            prevTextAppearingAnimationCharacter = character;
            int split = Mathf.Min(character, fullText.Length);
            string visibleText = fullText.Substring(0, split);
            string invisibleText = fullText.Substring(split);
            SetText($"<alpha=#FF>{visibleText}<alpha=#00>{invisibleText}");
        }
        else
        {
            SetText($"<alpha=#FF>{fullText}<alpha=#00>{""}");
        }
    }

    private void SetText(string text)
    {
        if (ReferenceEquals(dialogueLabel, null))
        {
            // TextMesh
            dialogueTextMesh.text = text;
        }
        else
        {
            // UGUI
            dialogueLabel.text = text;
        }
    }

    private string GetText()
    {
        if (ReferenceEquals(dialogueLabel, null))
        {
            // TextMesh
            return dialogueTextMesh.text;
        }
        else
        {
            // UGUI
            return dialogueLabel.text;
        }
    }

    public float GetProgressPercent()
    {
        if (string.IsNullOrEmpty(inputData.FullText)) return 0;
        return textAppearingAnimationProgress / inputData.FullText.Length;
    }

    public void Finish()
    {
        textAppearingAnimationProgress = inputData.FullText.Length;
    }
}
