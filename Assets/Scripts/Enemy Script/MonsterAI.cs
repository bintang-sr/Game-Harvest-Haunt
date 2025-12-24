using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public bool PlayerDetected;

    MonsterMovement movement;
    PlayerSafeZone playerSafe;

    bool wasInSafeZone;

    void Awake()
    {
        movement = GetComponent<MonsterMovement>();
        playerSafe = FindObjectOfType<PlayerSafeZone>();
    }

    void Update()
    {
        if (!movement || !playerSafe || !PlayerHealth.Instance) return;

        if (playerSafe.IsInSafeZone)
        {
            if (!wasInSafeZone)
            {
                movement.SetChase(false, true);
                wasInSafeZone = true;
            }
            return;
        }

        wasInSafeZone = false;

        int hp = PlayerHealth.Instance.hp;
        bool night = DayCycle.Instance && DayCycle.Instance.IsNight;

        bool chase = false;

        if (hp == 1)
            chase = true;
        else if (hp == 2)
            chase = night || PlayerDetected;
        else if (hp == 3)
            chase = night || PlayerDetected;

        movement.SetChase(chase, false);
        movement.UpdateChaseSpeed(hp);
    }
}
