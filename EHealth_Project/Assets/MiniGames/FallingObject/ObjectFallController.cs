using UnityEngine;
using System.Collections;

public class ObjectFallController : MonoBehaviour
{
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    public float spawnRate1 = 1.5f;
    public float spawnRate2 = 2.3f;
    public float spawnRate3 = 0.9f;

    public float spawnY = 3.7f;
    public float spawnXRange = 9f;

    
    
    public void StartSpawning()
    {
        if (prefab1 != null) InvokeRepeating(nameof(Spawn1), spawnRate1, spawnRate1);
        if (prefab2 != null) InvokeRepeating(nameof(Spawn2), spawnRate2, spawnRate2);
        if (prefab3 != null) InvokeRepeating(nameof(Spawn3), spawnRate3, spawnRate3);
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(Spawn1));
        CancelInvoke(nameof(Spawn2));
        CancelInvoke(nameof(Spawn3));
    }

    private void OnDisable()
    {
        StopSpawning();
    }

    private void OnDestroy()
    {
        StopSpawning();
    }

    void Spawn1() => Spawn(prefab1);
    void Spawn2() => Spawn(prefab2);
    void Spawn3() => Spawn(prefab3);

    void Spawn(GameObject obj)
    {
        if (obj == null) return;
        Vector3 spawnPos = new Vector3(Random.Range(-spawnXRange, spawnXRange), spawnY, 0f);
        Instantiate(obj, spawnPos, Quaternion.identity);
    }
}
