using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulGroundCheck : MonoBehaviour
{
    Ghoul ghoulScript;
    Rigidbody2D m_rb2D;

    void Start()
    {
        ghoulScript = GetComponentInParent<Ghoul>();
        m_rb2D = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floor") { ghoulScript.IsGrounded = false; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floor" && ghoulScript.State != ENEMY_STATE.DEATH)
        {
            ghoulScript.IsGrounded = true;
            m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, 0);
            //Physics2D.IgnoreLayerCollision(6, 7, false);
        }
    }
}