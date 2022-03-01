using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDust : MonoBehaviour
{
    Animator m_animator;
    SpriteRenderer m_spriteRenderer;
    string m_dashDustAnimationName = "dashDust";
    int m_dashDustHash;
    bool m_isFacingRight = true;

    void ChangeAnimationState(int p_newState)
    {
        m_animator.Play(p_newState);
    }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        m_dashDustHash = Animator.StringToHash(m_dashDustAnimationName);
    }

    public void ActivateDashDustAnimation(Vector3 p_positionToSpawn, bool p_isFacingRight)
    {
        transform.position = p_positionToSpawn;
        FlipX(p_isFacingRight);
        ChangeAnimationState(m_dashDustHash);
        
    }

    void FlipX(bool p_isFacingRight)
    {
        if(!m_isFacingRight && p_isFacingRight)
        {
            m_spriteRenderer.flipX = false;
            m_isFacingRight = true;
            
        }else if(m_isFacingRight && !p_isFacingRight)
        {
            m_spriteRenderer.flipX = true;
            m_isFacingRight = false;
        }
    }

}
