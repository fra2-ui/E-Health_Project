using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        // 1. IMPORTANT FIX: Detach from parent to become a Root GameObject.
        // DontDestroyOnLoad only works on root objects. 
        // If this object is a child of a Canvas or another Manager, we must unparent it first.
        transform.SetParent(null);

        // Debug.Log($"[MusicManager] Awake called. Object detached from parent. ID: {gameObject.GetInstanceID()}");

        // --- SINGLETON PATTERN ---
        if (instance != null && instance != this)
        {
            // Debug.Log("[MusicManager] Duplicate detected. Destroying new instance.");
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Now this will work because the object is a root object (has no parent)
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        // --- MUSIC START LOGIC ---
        // Play only if not already playing (avoids restart on scene load)
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            // Debug.Log("[MusicManager] Music started.");
        }
    }
}