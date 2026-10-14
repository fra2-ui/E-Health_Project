using UnityEngine;

public class EnablePlayerMovementOnStart : MonoBehaviour
{
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("[EnablePlayerMovementOnStart] Nessun oggetto con Tag 'Player' trovato in scena.");
            return;
        }

        var controller = player.GetComponent<Player_Controller>();
        if (controller == null)
        {
            Debug.LogWarning("[EnablePlayerMovementOnStart] Nessun Player_Controller trovato sul Player.");
            return;
        }

        controller.canMove = true;
        player.SetActive(true);

        Debug.Log("[EnablePlayerMovementOnStart] Movimento abilitato nella scena " + gameObject.scene.name);
    }
}