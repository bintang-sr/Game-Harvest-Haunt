using UnityEngine;

public class PlayerSafeZone : MonoBehaviour
{
    public bool IsInSafeZone { get; private set; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Camp"))
        {
            IsInSafeZone = true;
            Debug.Log("[SafeZone] Player ENTER camp");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Camp"))
        {
            IsInSafeZone = false;
            Debug.Log("[SafeZone] Player EXIT camp");
        }
    }
}
