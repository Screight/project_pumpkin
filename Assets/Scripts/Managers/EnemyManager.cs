using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    static EnemyManager m_instance;

    private EnemyManager() { }

    Enemy[] enemies;

    private void Awake()
    {
        if(m_instance == null) { m_instance = this; }
        else { Destroy(this.gameObject); }
    }

    void Start()
    {
        GameObject[] enemiesObject = GameObject.FindGameObjectsWithTag("enemy");
        enemies = new Enemy[enemiesObject.Length];

        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = enemiesObject[i].GetComponent<Enemy>();
        }

    }

    public void ResetAllAliveEnemies()
    {
        foreach(Enemy enemy in enemies)
        {
            if(enemy != null) { enemy.Reset(); }
        }
    }

    static public EnemyManager Instance
    {
        get { return m_instance; }
        private set { }
    }

}
