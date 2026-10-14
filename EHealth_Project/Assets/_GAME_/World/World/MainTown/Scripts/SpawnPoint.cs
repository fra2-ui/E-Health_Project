using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Nota: scritto esattamente spawnId con la 'd' minuscola per non rompere l'altro script
    public string spawnId;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}