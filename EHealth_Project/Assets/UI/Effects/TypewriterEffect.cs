using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public float delay = 0.02f;
    private TextMeshProUGUI textMeshPro;

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }
    public void ShowText(string fullText)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(fullText));
    }
    private IEnumerator TypeText(string text)
    {
        textMeshPro.text = "";
        foreach (char c in text)
        {
            textMeshPro.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}
