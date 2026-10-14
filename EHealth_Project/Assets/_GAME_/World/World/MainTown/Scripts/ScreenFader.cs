using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    void Awake()
    {
        Debug.Log("ScreenFader.Awake() chiamato su " + gameObject.name);

        if (fadeImage == null)
        {
            Debug.Log("fadeImage era NULL → prendo Image da questo GameObject");
            fadeImage = GetComponent<Image>();
        }

        if (fadeImage == null)
        {
            Debug.LogError("ERRORE: Nessuna Image trovata! Devi avere un componente Image nero sullo stesso GameObject.");
            return;
        }

        // assicuriamoci che sia attivo
        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = false;

        // nero, ma trasparente
        Color c = fadeImage.color;
        c.r = 0f;
        c.g = 0f;
        c.b = 0f;
        c.a = 0f;
        fadeImage.color = c;

        Debug.Log("ScreenFader: fadeImage inizializzato con alpha = 0");

        // ⚠️ IMPORTANTE: niente DontDestroyOnLoad.
        // Questo pannello esiste SOLO nella prima scena.
    }

    public IEnumerator FadeOutInAndLoad(string sceneName)
    {
        if (fadeImage == null)
        {
            Debug.LogError("FadeOutInAndLoad: fadeImage è NULL, esco");
            yield break;
        }

        Debug.Log("FadeOutInAndLoad → PARTITO per scena " + sceneName);

        // FASE 1: FADE OUT (0 → 1) nella scena attuale
        Debug.Log("FadeOutInAndLoad → FASE 1: Fade OUT (0 → 1)");
        yield return StartCoroutine(Fade(0f, 1f));
        Debug.Log("FadeOutInAndLoad → Fade OUT COMPLETATO");

        // FASE 2: CARICO SCENA
        Debug.Log("FadeOutInAndLoad → FASE 2: CARICO SCENA");
        SceneManager.LoadScene(sceneName);
        // Niente fade-in: il pannello viene distrutto col cambio scena
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade(): fadeImage è NULL, esco");
            yield break;
        }

        Debug.Log($"Fade() → da {startAlpha} a {endAlpha} in {fadeDuration} secondi");

        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;  // va bene così, è solo nella prima scena
            float a = Mathf.Lerp(startAlpha, endAlpha, t / fadeDuration);
            c.a = a;
            fadeImage.color = c;
            yield return null;
        }

        c.a = endAlpha;
        fadeImage.color = c;

        Debug.Log($"Fade() → COMPLETATO, alpha finale = {endAlpha}");
    }
}

