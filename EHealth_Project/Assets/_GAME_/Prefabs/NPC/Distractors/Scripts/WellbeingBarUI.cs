using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WellbeingBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider slider;              // Slider principale
    [SerializeField] private TextMeshProUGUI valueText;  // opzionale

    [Header("Color thresholds")]
    [Range(0f, 1f)] public float yellowThreshold = 0.6f;
    [Range(0f, 1f)] public float redThreshold = 0.3f;

    public Color greenColor = new Color(0.2f, 0.8f, 0.3f, 1f);
    public Color yellowColor = new Color(0.95f, 0.85f, 0.25f, 1f);
    public Color redColor = new Color(0.9f, 0.25f, 0.25f, 1f);

    [Header("Smoothing")]
    public bool smooth = true;
    public float smoothSpeed = 10f;

    private Image _fillImage;
    private float _shownValue;

    // VERSIONING per update immediato
    private static int _version = 0;
    private int _seenVersion = -1;

    public static void NotifyWellbeingChanged()
    {
        _version++;
    }

    private void Awake()
    {
        if (slider == null)
            slider = GetComponentInChildren<Slider>(true);

        if (slider == null)
        {
            Debug.LogError("[WellbeingBarUI] Slider NON assegnato.");
            return;
        }

        if (slider.fillRect != null)
            _fillImage = slider.fillRect.GetComponent<Image>();

        slider.minValue = 0f;
        slider.maxValue = 100f;

        ForceRefresh();
    }

    public void ForceRefresh()
    {
        _shownValue = GetTargetValue();
        ApplyToUI(_shownValue);
        _seenVersion = _version;
    }

    private void Update()
    {
        float target = GetTargetValue();

        if (smooth)
            _shownValue = Mathf.Lerp(_shownValue, target, Time.deltaTime * smoothSpeed);
        else
            _shownValue = target;

        ApplyToUI(_shownValue);
        _seenVersion = _version;
    }

    private float GetTargetValue()
    {
        if (StatusManager.Instance == null) return 100f;
        return Mathf.Clamp(StatusManager.Instance.wellbeing, 0f, 100f);
    }

    private void ApplyToUI(float value)
    {
        if (slider != null)
            slider.value = value;

        if (valueText != null)
            valueText.text = Mathf.RoundToInt(value).ToString();

        if (_fillImage != null)
        {
            float v01 = value / 100f;

            if (v01 <= redThreshold)
                _fillImage.color = redColor;
            else if (v01 <= yellowThreshold)
                _fillImage.color = yellowColor;
            else
                _fillImage.color = greenColor;
        }
    }
}