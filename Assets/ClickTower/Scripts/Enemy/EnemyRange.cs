using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    int demage;

    public int Demage { get { return transform.parent.GetComponent<Enemy>().Demage; } set => demage = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Tower tower = collision.GetComponent<Tower>();
            tower?.BeInjured(Demage);
        }
    }
}
