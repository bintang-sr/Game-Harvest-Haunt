using UnityEngine;
using UnityEngine.UI;

public class WaterUI : MonoBehaviour
{
    public Image wateringCanImage;
    public Sprite fullSprite;
    public Sprite halfSprite;
    public Sprite emptySprite;

    public void UpdateWaterUI(int currentWater, int maxWater)
    {
        if (currentWater >= maxWater)
            wateringCanImage.sprite = fullSprite;
        else if (currentWater >= 1)
            wateringCanImage.sprite = halfSprite;
        else
            wateringCanImage.sprite = emptySprite;
    }
}
