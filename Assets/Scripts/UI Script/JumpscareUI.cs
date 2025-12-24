using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpscareUI : MonoBehaviour
{
    public Sprite[] jumpscareImages;

    Image img;
    RectTransform rect;
    Coroutine routine;

    void Awake()
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;

        img.enabled = false;
    }

    public void Show(float duration)
    {
        if (jumpscareImages == null || jumpscareImages.Length == 0)
            return;

        if (routine != null)
            StopCoroutine(routine);

        img.sprite = jumpscareImages[Random.Range(0, jumpscareImages.Length)];
        routine = StartCoroutine(ShowRoutine(duration));
    }

    IEnumerator ShowRoutine(float duration)
    {
        img.enabled = true;
        yield return new WaitForSeconds(duration);
        img.enabled = false;
    }
}
