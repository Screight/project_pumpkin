using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samu_BigFireball : MonoBehaviour
{
    AudioSource m_audioSrc;
    Rigidbody2D m_rb2d;
    private void Awake()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_audioSrc = GetComponent<AudioSource>();
    }

    public void FireToPlayer(float p_speed)
    {
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

        m_audioSrc.PlayOneShot(SoundManager.Instance.ClipToPlay(AudioClipName.SAMAEL_FB_NOISE));
    }

    public void Fire(Vector2 p_direction, float p_speed){
         m_rb2d.velocity = p_speed * p_direction;

        Vector2 spiderToPlayer = p_direction;

        float angle = 360 / (2 * Mathf.PI) * Mathf.Atan(spiderToPlayer.y / spiderToPlayer.x);

        if (p_direction.x < 0)
        {
            angle += -90;
        }
        else { angle += 90; }

        transform.eulerAngles = new Vector3(0, 0, angle);

    }

    private void OnTriggerEnter2D(Collider2D p_collider)
    {
        if (!p_collider.CompareTag("Player") && !p_collider.CompareTag("floor")) { return; }
        if (p_collider.CompareTag("Player"))
        {
            Player.Instance.HandleHostileCollision(Vector2.zero, Vector2.zero, 0.5f, 1, 1);
        }

        m_audioSrc.PlayOneShot(SoundManager.Instance.ClipToPlay(AudioClipName.SAMAEL_FB_HIT));
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 1.0f);
    }

    public void PlaySound()
    {
        int randNum = Random.Range(0, 2);
        if (randNum == 0) { m_audioSrc.PlayOneShot(SoundManager.Instance.ClipToPlay(AudioClipName.SAMAEL_FB_SPAWN_1)); }
        else if (randNum == 1) { m_audioSrc.PlayOneShot(SoundManager.Instance.ClipToPlay(AudioClipName.SAMAEL_FB_SPAWN_2)); }
    }
}