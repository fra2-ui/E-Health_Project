using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public GameObject panel;   
    public TextMeshProUGUI messageText;

    void Start()
    {
        panel.SetActive(false);
    }

    public void ShowMessage(string msg)
    {
        messageText.text = msg;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}