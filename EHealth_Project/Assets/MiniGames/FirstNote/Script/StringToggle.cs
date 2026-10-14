using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class StringToggle : MonoBehaviour
{
    [Header("References")]
    public Button fretButton;
    public Image buttonImage;
    public Slider tuningSlider;
    public TMP_Text noteText;
    public GameObject arrow;

    [Header("Audio")]
    public AudioClip noteClip;
    private AudioSource audioSource;

    [Header("Options")]
    public string noteName = "Mi";
    [HideInInspector] public bool isOpen = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        fretButton.onClick.AddListener(OnClick);
        Close();
    }

    void OnDestroy()
    {
        if (fretButton != null) fretButton.onClick.RemoveListener(OnClick);
    }

    void OnClick()
    {
        
        if (TuningGameManager.HasInstance)
            TuningGameManager.Instance.OpenString(this);
    }

    public void Open()
    {
        isOpen = true;
        // Grafica
        if (buttonImage != null) buttonImage.enabled = false;
        if (tuningSlider != null) tuningSlider.gameObject.SetActive(true);
        if (noteText != null) { noteText.gameObject.SetActive(true); noteText.text = noteName; }
        if (arrow != null) arrow.SetActive(true);

        // Audio
        if (audioSource != null && noteClip != null)
            audioSource.PlayOneShot(noteClip);

        StringTuningConnector sc = GetComponent<StringTuningConnector>();
        if (sc != null) sc.ForceRefresh();
    }

    public void Close()
    {
        isOpen = false;
        if (buttonImage != null) buttonImage.enabled = true;
        if (tuningSlider != null) tuningSlider.gameObject.SetActive(false);
        if (noteText != null) noteText.gameObject.SetActive(false);
        if (arrow != null) arrow.SetActive(false);
    }
}