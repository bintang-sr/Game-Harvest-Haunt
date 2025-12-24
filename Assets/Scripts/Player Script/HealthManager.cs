using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    public int maxHealth = 3;
    public int health;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        health = maxHealth;
    }

    public void TakeDamage(int value)
    {
        health = Mathf.Clamp(health - value, 0, maxHealth);

        if (health <= 0)
        {
            Time.timeScale = 0f;
        }
    }

    public bool IsAggressive()
    {
        return health <= 1;
    }
}
