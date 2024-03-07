using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    SpriteRenderer[] spriteRenderers;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var item in spriteRenderers)
        {
            item.sprite = MainManager.Instance.GetFloorSprite();
        }
    }
}
