using UnityEngine;

[CreateAssetMenu(fileName = "NewWateringCan")]
public class WateringCanItem : ScriptableObject
{
    public Sprite fullSprite;
    public Sprite halfSprite;
    public Sprite emptySprite;
    public int maxWater = 5;
}
