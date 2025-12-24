using UnityEngine;

[CreateAssetMenu(fileName = "NewSeed")]
public class SeedItem : ScriptableObject
{
    public Sprite icon;
    public Sprite fullgrown;
    public string seedName;
    [TextArea] public string seedDescription;
    public float growTime;
    public CropsItem resultCrops;
}
