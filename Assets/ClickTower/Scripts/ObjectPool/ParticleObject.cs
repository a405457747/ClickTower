using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class ParticleObject : MonoBehaviour
{
    public float SaveTime = 1f;

    private void OnEnable()
    {
        Invoke("Recycle", SaveTime);
    }

    private void Recycle()
    {
        LeanPool.Despawn(this.gameObject);
    }
}
