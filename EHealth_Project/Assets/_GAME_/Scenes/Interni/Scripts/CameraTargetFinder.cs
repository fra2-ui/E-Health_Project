using UnityEngine;
using Unity.Cinemachine;

public class CameraTargetFinder : MonoBehaviour
{
    public bool updateLookAt = true; 

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        var vcam = GetComponent<CinemachineCamera>(); 
        if (player != null && vcam != null)
        {
            vcam.Follow = player.transform;
            if (updateLookAt) vcam.LookAt = player.transform;
            Debug.Log("Cinemachine: Player trovato!");
        }
    }
}