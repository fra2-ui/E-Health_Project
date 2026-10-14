using UnityEngine;
using TMPro;

public class DayHUDDisplay : MonoBehaviour
{
    private TMP_Text myText;

    private void Awake()
    {
        myText = GetComponent<TMP_Text>();
    }

    
    private void OnEnable()
    {
        
        QuestManager.OnDayChanged += UpdateDayText;

        
        if (QuestManager.Instance != null)
        {
            UpdateDayText(QuestManager.Instance.currentDayIndex);
        }
    }

    
    private void OnDisable()
    {
        
        QuestManager.OnDayChanged -= UpdateDayText;
    }

    
    private void UpdateDayText(int dayIndex)
    {
        if (myText != null)
        {
            int dayNumber = dayIndex + 1;
            myText.text = $"Day_{dayNumber.ToString("D2")}";
        }
    }
}