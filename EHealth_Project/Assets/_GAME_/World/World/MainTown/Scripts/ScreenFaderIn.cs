using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    void Awake()
    {
        if (fadeImage == null)
            fadeImage = GetComponent<Image>();

        if (fadeImage == null)
        {
            Debug.LogError("SceneFadeIn: nessuna Image trovata sul GameObject!");
            enabled = false;
            return;
        }

        // parto da nero pieno
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        // avvio il fade-in
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            c.a = a;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;

        // opzionale: disattivo il pannello per non tenerlo in giro
        gameObject.SetActive(false);
    }
}
