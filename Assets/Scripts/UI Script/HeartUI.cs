using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Update()
    {
        if (PlayerHealth.Instance == null) return;

        int hp = PlayerHealth.Instance.hp;
        int maxHP = PlayerHealth.Instance.maxHP;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < hp)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
}
