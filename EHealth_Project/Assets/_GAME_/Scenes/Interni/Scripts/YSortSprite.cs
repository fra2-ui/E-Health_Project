using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSortSprite : MonoBehaviour
{
    public int sortingOffset = 0;   // per alzare/abbassare manualmente se serve

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        // più in basso (y piccolo) → sortingOrder più alto → disegnato sopra
        sr.sortingOrder = sortingOffset + Mathf.RoundToInt(-transform.position.y * 100);
    }
}