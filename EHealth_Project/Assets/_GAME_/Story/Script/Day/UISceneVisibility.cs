using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(Canvas))]
public class UISceneVisibility : MonoBehaviour
{
    [Header("LISTA NERA SCENE")]
    [Tooltip("Scrivi qui i nomi esatti delle scene dove vuoi NASCONDERE questa Canvas")]
    public List<string> hiddenScenes;

    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        CheckVisibility(SceneManager.GetActiveScene().name);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckVisibility(scene.name);
    }

    private void CheckVisibility(string currentSceneName)
    {
        if (_canvas == null) return;

        if (hiddenScenes.Contains(currentSceneName))
        {
            _canvas.enabled = false;
        }
        else
        {
            _canvas.enabled = true;
        }
    }
}