using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ObstacleData
    {
        public GameObject prefab;
        public float speed = 3f;
        public float spawnDelay = 2f; // ogni quanto spawnare questo oggetto

        [HideInInspector] public float timer = 0f;
    }

    public ObstacleData[] obstacles;

    private void Update()
    {
        foreach (var o in obstacles)
        {
            o.timer += Time.deltaTime;

            if (o.timer >= o.spawnDelay)
            {
                SpawnObstacle(o);
                o.timer = 0f;
            }
        }
    }

    private void SpawnObstacle(ObstacleData o)
    {
        float randomY = Random.Range(-2.4f, 2.4f);
        Vector3 spawnPos = new Vector3(10f, randomY, -1f);

        GameObject obj = Instantiate(o.prefab, spawnPos, Quaternion.identity);

        obj.GetComponent<ObstacleMover>().speed = o.speed;
    }
}



