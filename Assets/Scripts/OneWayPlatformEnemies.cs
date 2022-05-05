using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatformEnemies : MonoBehaviour
{
    [SerializeField] BoxCollider2D m_playerPlatform;
    BoxCollider2D m_enemyPlatform;
    // Start is called before the first frame update
    void Start()
    {
        m_enemyPlatform = GetComponent<BoxCollider2D>();

        m_enemyPlatform.size = new Vector2(m_playerPlatform.size.x, m_playerPlatform.size.y);

        m_enemyPlatform.offset = m_playerPlatform.offset;

        Physics2D.IgnoreLayerCollision(6,9, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
