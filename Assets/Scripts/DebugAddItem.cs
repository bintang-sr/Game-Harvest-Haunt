using UnityEngine;

public class DebugAddItem : MonoBehaviour
{
    public ScriptableObject item;

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.T)) return;
        if (item == null) return;

        if (item is CropsItem crop)
            crop.GenerateValue();

        InventoryManager.Instance.AddItem(item, 1);
    }
}
