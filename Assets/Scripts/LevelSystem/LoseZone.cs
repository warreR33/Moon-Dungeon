using UnityEngine;

public class LoseZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            Debug.Log("Bloque entró en la zona de derrota.");
            GameManager.Instance.TriggerDefeat();
        }
    }
}
