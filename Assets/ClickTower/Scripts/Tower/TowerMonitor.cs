using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMonitor : MonoBehaviour
{
    CircleCollider2D CircleCollider2D;
    float radius;

    public float Radius { get { return transform.parent.GetComponent<Tower>().TowerMonitorDetectRange; } set => radius = value; }
    public List<Enemy> EnemyRange = new List<Enemy>();

    private void Awake()
    {
        CircleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = Radius;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemyCS = collision.GetComponent<Enemy>();
            EnemyRange.Add(enemyCS);
            enemyCS.DieHandler += RemoveEnemy;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemyCS = collision.GetComponent<Enemy>();
            EnemyRange.Remove(enemyCS);
            enemyCS.DieHandler -= RemoveEnemy;
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        EnemyRange.Remove(enemy);
        enemy.DieHandler -= RemoveEnemy;
    }
}
