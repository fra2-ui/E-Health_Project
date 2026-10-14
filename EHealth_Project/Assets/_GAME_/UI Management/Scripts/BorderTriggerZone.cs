using UnityEngine;

public class BorderTriggerZone : MonoBehaviour
{
    // CAMBIAMENTO: Non lo cerchiamo più in automatico.
    // Lo trascinerai tu a mano nell'Inspector.
    public BorderEventManager linkedManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Controlliamo se hai collegato il manager nell'Inspector
            if (linkedManager != null)
            {
                linkedManager.StartEasterEggEvent();
            }
            else
            {
                Debug.LogError($"[BorderTriggerZone] Attenzione! Non hai collegato il Manager nel trigger: {gameObject.name}");
            }
        }
    }
}