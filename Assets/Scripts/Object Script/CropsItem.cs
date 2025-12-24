using UnityEngine;

[CreateAssetMenu(fileName = "NewCrop")]
public class CropsItem : ScriptableObject
{
    public Sprite icon;
    public string cropsName;
    [TextArea] public string cropsDescription;

    public int valueMin;
    public int valueMax;

    [HideInInspector] public int fixedValue;

    public void GenerateValue()
    {
        fixedValue = Random.Range(valueMin, valueMax + 1);
    }
}
