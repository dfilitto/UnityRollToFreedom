using UnityEngine;

public class RespawnZone : MonoBehaviour
{

    [Header("Respawn Settings")]
    [SerializeField, Tooltip("Ponto de respawn do player")]
    private Transform respawnPoint; // The point where the player will respawn

    [SerializeField, Tooltip("Meu Player")]
    private string playerTag = "Player"; // The tag of the player object

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the player tag
        if (other.CompareTag(playerTag))
        {
            // Move the player to the respawn point
            GameObject player = other.gameObject;
            player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            player.transform.position = respawnPoint.position;
            player.transform.rotation = Quaternion.identity;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null)
        {
            Gizmos.DrawCube(transform.position + box.center, box.size);
        }
    }
}
