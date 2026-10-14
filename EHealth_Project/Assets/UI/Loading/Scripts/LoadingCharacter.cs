using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingCharacter : MonoBehaviour
{
[Header("Movimento")]
public float speed = 300f;
public float startOffset = -200f;
public float endOffset = 200f;

    [Header("Animazione")]
    public float frameRate = 0.1f;
    public float characterScale = 0.5f;

    private RectTransform rect;
    private Image image;
    private Sprite[] loadingSprites;
    private int currentFrame;
    private float frameTimer;
    private Vector2 startPos;
    private float screenWidth;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        if (GameManager.Instance != null && GameManager.Instance.selectedCharacter != null)
        {
            loadingSprites = GameManager.Instance.selectedCharacter.loadingSprites;
            image.sprite = loadingSprites[0];
        }
        rect.localScale = Vector3.one * characterScale;
        screenWidth = Screen.width;
        startPos = new Vector2(-screenWidth / 2 - startOffset, rect.anchoredPosition.y);
        rect.anchoredPosition = startPos;
    }
    void Update()
    {
        if (loadingSprites == null || loadingSprites.Length == 0) return;
        rect.anchoredPosition += Vector2.right * speed * Time.deltaTime;
        if (rect.anchoredPosition.x > screenWidth / 2 + endOffset)
        {
            rect.anchoredPosition = startPos;
        }
        frameTimer += Time.deltaTime;
        if (frameTimer >= frameRate)
        {
            frameTimer = 0f;
            currentFrame = (currentFrame + 1) % loadingSprites.Length;
            image.sprite = loadingSprites[currentFrame];
        }
    }
}
