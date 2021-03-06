using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneScript : MonoBehaviour
{
    float m_boneSpeed = 80.0f;
    private Rigidbody2D m_rb2D;

    private void Awake() {
        m_rb2D = GetComponent<Rigidbody2D>();
    }

    public void Shoot(int p_direction, AudioClipName p_audio)
    {
        m_rb2D.velocity = new Vector2(p_direction * m_boneSpeed, m_rb2D.velocity.y);
        //SoundManager.Instance.PlayOnce(p_audio, 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floor"|| collision.gameObject.tag == "Player") {
            Destroy(gameObject);
        }
    }
}