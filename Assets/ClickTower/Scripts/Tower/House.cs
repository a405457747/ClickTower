using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using xmaolol.com;
using QFramework;

public class House : MonoSingleton<House>
{
    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void BeInjured()
    {
        sr.color = Color.red;
        this.Delay(1f, () => { sr.color = Color.white; });
    }
}
