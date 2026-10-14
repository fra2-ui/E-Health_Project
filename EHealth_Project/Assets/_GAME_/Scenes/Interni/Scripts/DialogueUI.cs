using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private enum Mode { None, Lines, Prompt }
    private Mode _mode = Mode.None;

    private string[] _lines;
    private int _index;

    private bool _waitReleaseE;
    private bool _isQuestDialogue;

    // anti-riapertura immediata (E dentro trigger NPC)
    private float _reopenBlockTimer = 0f;
    [SerializeField] private float reopenBlockSeconds = 0.20f;

    private Player_Controller _player;

    // Prompt
    private Action _onYes;
    private Action _onNo;
    private GameObject _owner; // chi ha aperto il prompt/dialogo

    public bool IsActive => dialoguePanel != null && dialoguePanel.activeSelf;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (dialoguePanel == null) Debug.LogError("[DialogueUI] dialoguePanel NON assegnato");
        if (dialogueText == null) Debug.LogError("[DialogueUI] dialogueText NON assegnato");

        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        _player = FindFirstObjectByType<Player_Controller>();
    }

    private void Update()
    {
        if (_reopenBlockTimer > 0f)
            _reopenBlockTimer -= Time.deltaTime;

        if (!IsActive) return;

        // aspetta rilascio E (evita che l’E che ha aperto venga usato subito)
        if (_waitReleaseE)
        {
            if (!IsEPressed())
                _waitReleaseE = false;
            return;
        }

        // Input
        if (_mode == Mode.Lines)
        {
            if (IsEJustPressed())
                AdvanceLines();
        }
        else if (_mode == Mode.Prompt)
        {
            // Prompt: E = yes, Q = no
            if (IsEJustPressed())
            {
                var yes = _onYes;
                CloseInternal();
                yes?.Invoke();
            }
            else if (IsQJustPressed())
            {
                var no = _onNo;
                CloseInternal();
                no?.Invoke();
            }
        }
    }

    // =========================================================
    // DIALOGO A RIGHE (usato da QuestNPC)
    // =========================================================
    public void StartDialogue(string[] lines, bool isQuest = false)
    {
        if (IsActive) return;
        if (_reopenBlockTimer > 0f) return;

        if (lines == null || lines.Length == 0)
            return;

        _mode = Mode.Lines;
        _owner = null;

        _lines = lines;
        _index = 0;
        _isQuestDialogue = isQuest;

        dialoguePanel.SetActive(true);
        dialogueText.text = _lines[0];

        LockPlayer();

        _waitReleaseE = IsEPressed();
    }

    private void AdvanceLines()
    {
        _index++;

        if (_index < _lines.Length)
        {
            dialogueText.text = _lines[_index];
            return;
        }

        // fine dialogo
        CloseInternal();

        if (_isQuestDialogue && QuestManager.Instance != null)
            QuestManager.Instance.AdvanceQuest();
    }

    // =========================================================
    // PROMPT YES/NO (per tappeto uscita)
    // =========================================================
    public bool TryShowPrompt(GameObject owner, string text, Action onYes, Action onNo)
    {
        if (IsActive) return false;
        if (_reopenBlockTimer > 0f) return false;

        _mode = Mode.Prompt;
        _owner = owner;

        _onYes = onYes;
        _onNo = onNo;
        _isQuestDialogue = false;

        dialoguePanel.SetActive(true);
        dialogueText.text = text;

        LockPlayer();

        _waitReleaseE = IsEPressed();
        return true;
    }

    // chiudi solo se l’ha aperto quell’owner (serve per OnTriggerExit)
    public void CloseIfOwner(GameObject owner)
    {
        if (!IsActive) return;
        if (_mode != Mode.Prompt) return;
        if (_owner != owner) return;

        var no = _onNo; // opzionale: quando esci dal tappeto puoi considerarlo "no"
        CloseInternal();
        no?.Invoke();
    }

    // =========================================================
    // CLOSE / LOCK
    // =========================================================
    private void CloseInternal()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        UnlockPlayer();

        _mode = Mode.None;
        _lines = null;
        _index = 0;

        _onYes = null;
        _onNo = null;
        _owner = null;

        _reopenBlockTimer = reopenBlockSeconds;
    }

    private void LockPlayer()
    {
        if (_player == null) _player = FindFirstObjectByType<Player_Controller>();
        if (_player != null) _player.canMove = false;
    }

    private void UnlockPlayer()
    {
        if (_player == null) _player = FindFirstObjectByType<Player_Controller>();
        if (_player != null) _player.canMove = true;
    }

    // =========================================================
    // INPUT (Input System)
    // =========================================================
    private bool IsEPressed()
    {
        return Keyboard.current != null && Keyboard.current.eKey.isPressed;
    }

    private bool IsEJustPressed()
    {
        return Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
    }

    private bool IsQJustPressed()
    {
        return Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame;
    }
}