using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TuningGameManager : MonoBehaviour
{
    public static TuningGameManager Instance { get; private set; }
    public static bool HasInstance => Instance != null;

    [Header("In-Game UI")]
    public TMP_Text counterText; 

    [Header("Settings")]
    public int requiredCount = 6;
    
    // Variabile che contiene il punteggio finale
    public int finalScore { get; private set; } 

    private float currentTimer = 0f;
    public event Action OnVictory; 
    public bool IsGameActive { get; private set; } = false;

    private List<StringTuningConnector> connectors = new List<StringTuningConnector>();
    private HashSet<StringTuningConnector> tunedSet = new HashSet<StringTuningConnector>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (IsGameActive)
        {
            currentTimer += Time.deltaTime;
        }
    }

    public void SetGameActive(bool status)
    {
        IsGameActive = status;
        if (status) currentTimer = 0f;
        else
        {
            
            foreach (var s in FindObjectsByType<StringToggle>(FindObjectsSortMode.None))
                if (s.isOpen) s.Close();
        }
    }

    public void RegisterConnector(StringTuningConnector c)
    {
        if (!connectors.Contains(c)) connectors.Add(c);
        if (c.isTuned) tunedSet.Add(c);
        UpdateCounterUI();
    }

    public void UnregisterConnector(StringTuningConnector c)
    {
        connectors.Remove(c);
        tunedSet.Remove(c);
        UpdateCounterUI();
    }

    public void OnConnectorTunedChanged(StringTuningConnector c, bool isTuned)
    {
        if (isTuned) tunedSet.Add(c);
        else tunedSet.Remove(c);
        
        UpdateCounterUI();
        CheckVictory();
    }

    void UpdateCounterUI()
    {
        if (counterText != null) counterText.text = $"{tunedSet.Count}/{requiredCount}";
    }

    public void OpenString(StringToggle st)
    {
        if (!IsGameActive) return;
        foreach (var s in FindObjectsByType<StringToggle>(FindObjectsSortMode.None))
        {
            if (s != st && s.isOpen) s.Close();
        }
        st.Open();
    }

    void CheckVictory()
    {
        if (tunedSet.Count >= requiredCount)
        {
            CalculateScore();
            SetGameActive(false);
            Debug.Log($"Vittoria! Punteggio finale: {finalScore}/100");
            OnVictory?.Invoke();
        }
    }

    void CalculateScore()
    {
        
        int penalty = Mathf.FloorToInt(currentTimer); 
        
        
        finalScore = Mathf.Max(0, 100 - penalty);
    }
}