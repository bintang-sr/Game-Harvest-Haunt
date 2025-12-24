using TMPro;
using UnityEngine;

public class TotemSacrificeUI : MonoBehaviour
{
    public TextMeshProUGUI counterText;
    public SacrificeManager sacrificeManager;

    void Update()
    {
        counterText.text =
            sacrificeManager.currentValue + " / " +
            sacrificeManager.targetValue;
    }
}
