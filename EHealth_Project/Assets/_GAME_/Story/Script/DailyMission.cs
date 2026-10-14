using UnityEngine;

[CreateAssetMenu(fileName = "NewDayMission", menuName = "Game/New Day Mission")]
public class DailyMission : ScriptableObject
{
    [Header("--- Day Settings ---")]
    public string dayName;          
    public AudioClip dayMusic;      

    [Header("--- Minigame Settings ---")]
    [Tooltip("The exact name of the Scene to load when the quest is finished.")]
    public string minigameSceneName; 

    [Header("--- Mission Steps ---")]
    public MissionStep[] steps; 
}

[System.Serializable]
public class MissionStep
{
    public string description; // Just for your reference in Editor
    public string targetNpcID; // MUST match the NPC ID in the scene
    [TextArea] public string[] dialogueLines; 
}