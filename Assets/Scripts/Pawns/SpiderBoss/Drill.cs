using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    [SerializeField] SPIDER_BOSS_DAMAGEABLE_PARTS m_part;
    SpiderBoss m_spiderBoss;
    [SerializeField] Vector2 m_pushAwayVelocity = new Vector2(50.0f,0);
    [SerializeField] float m_noControlDuration = 0.4f;
    [SerializeField] float m_invulnerableDuration = 0.2f;
    [SerializeField] int m_damage;

    Collider2D m_collider;
    bool m_canDamagePlayer = false;
    bool m_canBeDamaged = false;

    private void Awake() {
        m_collider = GetComponent<Collider2D>();
        m_spiderBoss = GameObject.FindObjectOfType<SpiderBoss>();
    }

    private void OnTriggerStay2D(Collider2D p_collider) {
        if( p_collider.tag != "Player" || !m_canDamagePlayer){ return ;}
        m_canDamagePlayer = false;
        Vector2 direction = new Vector2(1,1);
        if(Player.Instance.transform.position.x < transform.position.x){
            direction.x = -1;
        }
        else{ direction.x = 1; }
        Player.Instance.HandleHostileCollision(m_pushAwayVelocity, direction,m_noControlDuration, m_invulnerableDuration, m_damage);

    }

    public void Damage(float p_damage){
        if(!m_canBeDamaged){ return ;}
        m_spiderBoss.Damage(p_damage, m_part);
        Debug.Log(m_part + "DAMAGED");
    }

    public bool CanDamagePlayer {
        set { m_canDamagePlayer = value;}
    }

    public bool CanBeDamaged {
        set { m_canBeDamaged = value; }
    }

    public void InitializeExplosion(){
        this.gameObject.SetActive(false);
        Debug.Log("INITIALIZING EXPLOSION");
    }

}
