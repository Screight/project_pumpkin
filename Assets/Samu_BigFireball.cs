using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samu_BigFireball : MonoBehaviour
{

    AudioSource m_source;
    [SerializeField] AudioClip[] m_spawnSound;
    [SerializeField] AudioClip m_fireSound;
    [SerializeField] AudioClip m_impactSound;
    Rigidbody2D m_rb2d;
    private void Awake() {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_source = GetComponent<AudioSource>();
        
    }

    public void FireToPlayer(float p_speed){
        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        m_rb2d.velocity = p_speed * direction;
        
        Vector2 spiderToPlayer = Player.Instance.transform.position - transform.position;

        float angle = 360 / (2 * Mathf.PI) * Mathf.Atan(spiderToPlayer.y / spiderToPlayer.x);

        if (Player.Instance.transform.position.x < transform.position.x)
        {
            angle += -90;
        }
        else { angle += 90; }

        transform.eulerAngles = new Vector3(0, 0, angle);

        m_source.PlayOneShot(m_fireSound);
    }

    private void OnTriggerEnter2D(Collider2D p_collider) {
        if(p_collider.tag != "Player" && p_collider.tag != "floor"){ return;}
        if(p_collider.tag == "Player"){
            Player.Instance.HandleHostileCollision(Vector2.zero,Vector2.zero,0.5f,1,1);
        }
        m_source.PlayOneShot(m_impactSound);
        Destroy(gameObject);
    }

    public void PlaySound(){
        int number = Random.Range(0,m_spawnSound.Length);
        m_source.PlayOneShot(m_spawnSound[number]);
    }

}
