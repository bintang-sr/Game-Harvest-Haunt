using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    public int maxHP = 3;
    public int hp = 3;
    public Transform campRespawnPoint;

    bool isGameOver;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (LoadContext.SelectedSlot == -1)
            hp = maxHP;
    }

    public void Heal(int amount)
    {
        if (isGameOver) return;

        hp += amount;
        hp = Mathf.Clamp(hp, 0, maxHP);
    }

    public void TakeJumpscareDamage(int damage)
    {
        if (isGameOver) return;

        hp -= damage;
        hp = Mathf.Clamp(hp, 0, maxHP);

        if (InventoryManager.Instance)
            InventoryManager.Instance.ClearInventory();
        if (DayCycle.Instance)
            DayCycle.Instance.ForceDay();
        if (campRespawnPoint)
            transform.position = campRespawnPoint.position;

        if (hp <= 0)
            GameOver();
    }

    public void TakeSacrificeDamage(int damage)
    {
        if (isGameOver) return;

        hp -= damage;
        hp = Mathf.Clamp(hp, 0, maxHP);

        if (hp <= 0)
        {
            if (InventoryManager.Instance)
                InventoryManager.Instance.ClearInventory();

            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER");

        if (GameOverUI.Instance)
            GameOverUI.Instance.ShowGameOver();
    }
}
