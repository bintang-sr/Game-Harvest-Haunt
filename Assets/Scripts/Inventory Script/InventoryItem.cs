using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ScriptableObject item;
    public int amount;
    public int fixedValue;

    public InventoryItem(ScriptableObject item, int amount)
    {
        this.item = item;
        this.amount = amount;

        if (item is CropsItem crop)
            fixedValue = Random.Range(crop.valueMin, crop.valueMax + 1);
        else
            fixedValue = 0;
    }
}
