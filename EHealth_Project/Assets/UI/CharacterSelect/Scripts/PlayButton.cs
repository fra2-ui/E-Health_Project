using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayButton : MonoBehaviour
{
    public string sceneToLoad = "GameScene";
    private Button button;
    private RectTransform rect;

    private void Awake()
    {
        button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
        button.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        StartCoroutine(ClickEffect());
    }
    private IEnumerator ClickEffect()
    {
        Vector3 originalScale = rect.localScale;
        Color originalColor = button.image.color;
        button.image.color = new Color(1f, 1f, 1f, 0.6f);
        rect.localScale = originalScale * 1.1f;
        yield return new WaitForSeconds(0.2f);
        rect.localScale = originalScale;
        button.image.color = originalColor;
        LoadingManager.nextScene = sceneToLoad;
        SceneManager.LoadScene("OutWorld");
    }
}
