using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject panelRoot;      // TutorialWindow
    [SerializeField] private Button understoodButton;   // bottone

    public bool IsOpen => panelRoot != null && panelRoot.activeSelf;

    private void Awake()
    {
        if (panelRoot == null)
            Debug.LogError("[TutorialUI] Panel Root non assegnato (deve essere TutorialWindow).");

        if (understoodButton == null)
            Debug.LogError("[TutorialUI] Understood Button non assegnato.");

        if (understoodButton != null)
            understoodButton.onClick.AddListener(Close);

        // di default lo chiudiamo, poi lo apre il manager quando serve
        Close();
    }

    public void Open()
    {
        if (panelRoot != null) panelRoot.SetActive(true);
    }

    public void Close()
    {
        if (panelRoot != null) panelRoot.SetActive(false);
    }

    public void Toggle()
    {
        if (panelRoot == null) return;
        panelRoot.SetActive(!panelRoot.activeSelf);
    }
}