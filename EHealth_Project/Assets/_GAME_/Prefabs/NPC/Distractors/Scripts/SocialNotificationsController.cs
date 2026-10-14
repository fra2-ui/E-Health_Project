using System.Collections.Generic;
using UnityEngine;

public class SocialNotificationsController : MonoBehaviour
{
    [Header("UI inside THIS prefab")]
    [SerializeField] private Canvas socialCanvas;
    [SerializeField] private RectTransform notificationsRoot;

    [Header("Popup Prefab (drag SocialPopup.prefab here)")]
    [SerializeField] private GameObject popupPrefab;

    [Header("Sprites (drag ALL sliced sprites from your PNG here)")]
    [SerializeField] private Sprite[] notificationSprites;

    [Header("Burst")]
    public int defaultBurstCount = 12;

    [Header("Popup size")]
    public Vector2 popupSize = new Vector2(400, 160);

    [Header("Key rules")]
    public bool avoidWASDE = true;
    public bool avoidDuplicates = true;

    private readonly List<SocialPopupUI> _active = new();

    private void Awake()
    {
        if (socialCanvas == null) socialCanvas = GetComponentInChildren<Canvas>(true);

        if (notificationsRoot == null)
        {
            // prova a trovarlo con nome standard
            var t = transform.Find("SocialCanvas/NotificationsRoot");
            if (t != null) notificationsRoot = (RectTransform)t;
            else notificationsRoot = GetComponentInChildren<RectTransform>(true);
        }

        SetupCanvasSafe();
        ForceRootFullScreen();
    }

    private void Update()
    {
        // chiusura popup tramite lettera
        var pressed = GetPressedLetterKey();
        if (pressed != KeyCode.None)
            CloseTopmostMatching(pressed);
    }

    // ----------------------------------------------------
    // PUBLIC API
    // ----------------------------------------------------
    public void SpawnDefaultBurst() => SpawnBurst(defaultBurstCount);

    public void SpawnBurst(int count)
    {
        for (int i = 0; i < count; i++)
            SpawnPopup();
    }

    // ----------------------------------------------------
    // INTERNAL
    // ----------------------------------------------------
    private void SpawnPopup()
    {
        if (popupPrefab == null)
        {
            Debug.LogError("[SocialNotificationsController] popupPrefab NON assegnato (Inspector).");
            return;
        }

        if (notificationsRoot == null)
        {
            Debug.LogError("[SocialNotificationsController] notificationsRoot NON assegnato.");
            return;
        }

        if (notificationSprites == null || notificationSprites.Length == 0)
        {
            Debug.LogError("[SocialNotificationsController] notificationSprites vuoto.");
            return;
        }

        var go = Instantiate(popupPrefab, notificationsRoot);
        go.SetActive(true);
        go.transform.localScale = Vector3.one;

        var popup = go.GetComponent<SocialPopupUI>();
        if (popup == null)
        {
            Debug.LogError("[SocialNotificationsController] Il prefab popup NON ha SocialPopupUI sul root.");
            Destroy(go);
            return;
        }

        Sprite sprite = notificationSprites[Random.Range(0, notificationSprites.Length)];
        KeyCode key = PickRandomLetterKey();

        popup.Init(sprite, key);
        popup.Rect.sizeDelta = popupSize;

        PlaceRandom(popup.Rect);
        popup.Rect.SetAsLastSibling();

        _active.Add(popup);
    }

    private void PlaceRandom(RectTransform rt)
    {
        ForceRootFullScreen();

        float w = notificationsRoot.rect.width;
        float h = notificationsRoot.rect.height;

        float marginX = popupSize.x * 0.5f + 20f;
        float marginY = popupSize.y * 0.5f + 20f;

        float x = Random.Range(-w * 0.5f + marginX, w * 0.5f - marginX);
        float y = Random.Range(-h * 0.5f + marginY, h * 0.5f - marginY);

        rt.anchoredPosition = new Vector2(x, y);
    }

    private void ForceRootFullScreen()
    {
        if (notificationsRoot == null) return;

        notificationsRoot.anchorMin = Vector2.zero;
        notificationsRoot.anchorMax = Vector2.one;
        notificationsRoot.offsetMin = Vector2.zero;
        notificationsRoot.offsetMax = Vector2.zero;
    }

    private void CloseTopmostMatching(KeyCode key)
    {
        for (int i = _active.Count - 1; i >= 0; i--)
        {
            var p = _active[i];
            if (p == null) { _active.RemoveAt(i); continue; }

            if (p.RequiredKey == key)
            {
                _active.RemoveAt(i);
                Destroy(p.gameObject);
                return;
            }
        }
    }

    private KeyCode PickRandomLetterKey()
    {
        for (int tries = 0; tries < 200; tries++)
        {
            KeyCode k = (KeyCode)((int)KeyCode.A + Random.Range(0, 26));
            // T è riservato all'apertura del Tutorial
            if (k == KeyCode.T)
                continue;

            if (avoidWASDE && (k == KeyCode.W || k == KeyCode.A || k == KeyCode.S || k == KeyCode.D || k == KeyCode.E))
                continue;

            if (avoidDuplicates && IsKeyInUse(k))
                continue;

            return k;
        }

        // fallback: evita comunque T
        KeyCode fallback;
        do
        {
            fallback = (KeyCode)((int)KeyCode.A + Random.Range(0, 26));
        } while (fallback == KeyCode.T);
        return fallback;
    }

    private bool IsKeyInUse(KeyCode key)
    {
        for (int i = _active.Count - 1; i >= 0; i--)
        {
            var p = _active[i];
            if (p == null) { _active.RemoveAt(i); continue; }
            if (p.RequiredKey == key) return true;
        }
        return false;
    }

    private KeyCode GetPressedLetterKey()
    {
        for (int i = 0; i < 26; i++)
        {
            KeyCode k = (KeyCode)((int)KeyCode.A + i);
            // T è riservato al Tutorial, non deve chiudere popup
            if (k == KeyCode.T)
                continue;
            if (Input.GetKeyDown(k)) return k;
        }
        return KeyCode.None;
    }

    private void SetupCanvasSafe()
    {
        if (socialCanvas == null) return;

        socialCanvas.overrideSorting = true;

        int dialoghiId = SortingLayer.NameToID("Dialoghi");
        if (dialoghiId != 0)
            socialCanvas.sortingLayerID = dialoghiId;

        socialCanvas.sortingOrder = 32767;
        socialCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }
}