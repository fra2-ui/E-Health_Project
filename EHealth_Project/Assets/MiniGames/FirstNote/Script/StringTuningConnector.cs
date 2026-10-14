using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(StringToggle))]
public class StringTuningConnector : MonoBehaviour
{
    [Header("References")]
    public Slider slider;
    public RectTransform handleRect;
    public RectTransform arrowRect;
    public Image arrowImage;
    [Header("Distance thresholds (screen pixels)")]
    public float greenThreshold = 8f;
    public float orangeThreshold = 24f;
    [HideInInspector] public bool isTuned = false;
    private StringToggle toggle;

    void Awake()
    {
        toggle = GetComponent<StringToggle>();
        if (slider != null)
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        TuningGameManager.Instance.RegisterConnector(this);
    }
    void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        if (TuningGameManager.HasInstance)
            TuningGameManager.Instance.UnregisterConnector(this);
    }
    void OnSliderValueChanged(float v)
    {
        UpdateIndicator();
    }
    public void ForceRefresh()
    {
        UpdateIndicator();
    }
    void UpdateIndicator()
    {
        float dx = Mathf.Abs(handleRect.position.x - arrowRect.position.x);
        Color targetColor = Color.red;
        bool nowTuned = false;
        if (dx <= greenThreshold)
        {
            targetColor = Color.green;
            nowTuned = true;
        }
        else if (dx <= orangeThreshold)
        {
            targetColor = new Color(1f, 0.6f, 0f);
            nowTuned = false;
        }
        else
        {
            targetColor = Color.red;
            nowTuned = false;
        }
        arrowImage.color = targetColor;
        if (nowTuned != isTuned)
        {
            isTuned = nowTuned;
            TuningGameManager.Instance.OnConnectorTunedChanged(this, isTuned);
        }
    }
}
