using UnityEngine;

public class SpawnerTasti : MonoBehaviour
{
    public GameObject[] prefabsTasti;
    public float spawnInterval = 1.2f;
    public float tastoLifeTime = 1f;

    public GameManagerTasti gameManager;

    private float timer = 0f;

    void Update()
    {
        if (!gameManager.IsGameActive()) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnRandom();
        }
    }

    void SpawnRandom()
    {
        float x = Random.Range(-9f, 9f);
        float y = Random.Range(-4f, 4f);

        int index = Random.Range(0, prefabsTasti.Length);

        GameObject obj = Instantiate(prefabsTasti[index], new Vector3(x, y, 0), Quaternion.identity);
        Destroy(obj, tastoLifeTime);
    }
}

