using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /// ABOUT TO BE DEPRECATED
    Player m_playerScript;
    CameraMovement m_cameraScript;

    // Positive means from left to right and from bottom to top
    [SerializeField] bool m_isHorizontal;
    bool m_isPositiveTransition;

    // positive transition
    [SerializeField] Transform m_leftLimitTransformPositive;
    [SerializeField] Transform m_rightLimitTransformPositive;
    [SerializeField] Transform m_topLimitTransformPositive;
    [SerializeField] Transform m_bottomLimitTransformPositive;
    [SerializeField] Transform m_targetPositive;
    [SerializeField] bool m_canMovePositive;

    // negative transition
    [SerializeField] Transform m_leftLimitTransformNegative;
    [SerializeField] Transform m_rightLimitTransformNegative;
    [SerializeField] Transform m_topLimitTransformNegative;
    [SerializeField] Transform m_bottomLimitTransformNegative;
    [SerializeField] Transform m_targetNegative;
    [SerializeField] bool m_canMoveNegative;

    

    private void Awake()
    {
        m_cameraScript = Camera.main.GetComponent<CameraMovement>();
        m_playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Player")
        {
            SetTransitionSign();

            if (m_isPositiveTransition)
            {
                if(m_leftLimitTransformPositive != null) { m_cameraScript.LeftLimit = m_leftLimitTransformPositive.position.x; }
                if(m_rightLimitTransformPositive != null) { m_cameraScript.RightLimit = m_rightLimitTransformPositive.position.x; ; }
                if(m_topLimitTransformPositive != null) { m_cameraScript.TopLimit = m_topLimitTransformPositive.position.y; }
                if(m_bottomLimitTransformPositive != null) { m_cameraScript.BottomLimit = m_bottomLimitTransformPositive.position.y; }

                if (m_targetPositive != null) { m_cameraScript.TargetPosition = m_targetPositive.position; }
                m_cameraScript.CanMove = m_canMovePositive;
            }
            else
            {
                if (m_leftLimitTransformNegative != null) { m_cameraScript.LeftLimit = m_leftLimitTransformNegative.position.x; }
                if (m_rightLimitTransformNegative != null) { m_cameraScript.RightLimit = m_rightLimitTransformNegative.position.x; }
                if (m_topLimitTransformNegative != null) { m_cameraScript.TopLimit = m_topLimitTransformNegative.position.y; }
                if (m_bottomLimitTransformNegative != null) { m_cameraScript.BottomLimit = m_bottomLimitTransformNegative.position.y; }
                
                if(m_targetNegative != null) { m_cameraScript.TargetPosition = m_targetNegative.position; }
                
                m_cameraScript.CanMove = m_canMoveNegative;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SetTransitionSign();

            if (m_isPositiveTransition)
            {
                if (m_leftLimitTransformPositive != null) { m_cameraScript.LeftLimit = m_leftLimitTransformPositive.position.x; }
                if (m_rightLimitTransformPositive != null) { m_cameraScript.RightLimit = m_rightLimitTransformPositive.position.x; ; }
                if (m_topLimitTransformPositive != null) { m_cameraScript.TopLimit = m_topLimitTransformPositive.position.y; }
                if (m_bottomLimitTransformPositive != null) { m_cameraScript.BottomLimit = m_bottomLimitTransformPositive.position.y; }

                if (m_targetPositive != null) { m_cameraScript.TargetPosition = m_targetPositive.position; }
                m_cameraScript.CanMove = m_canMovePositive;
            }
            else
            {
                if (m_leftLimitTransformNegative != null) { m_cameraScript.LeftLimit = m_leftLimitTransformNegative.position.x; }
                if (m_rightLimitTransformNegative != null) { m_cameraScript.RightLimit = m_rightLimitTransformNegative.position.x; }
                if (m_topLimitTransformNegative != null) { m_cameraScript.TopLimit = m_topLimitTransformNegative.position.y; }
                if (m_bottomLimitTransformNegative != null) { m_cameraScript.BottomLimit = m_bottomLimitTransformNegative.position.y; }

                if (m_targetNegative != null) { m_cameraScript.TargetPosition = m_targetNegative.position; }

                m_cameraScript.CanMove = m_canMoveNegative;
            }
        }
    }

    void SetTransitionSign()
    {
        if (m_isHorizontal)
        {
            if (m_playerScript.Speed.y > 0) { m_isPositiveTransition = true; }
            else { m_isPositiveTransition = false; }
        }
        else
        {
            if (m_playerScript.Speed.x > 0) { m_isPositiveTransition = true; }
            else { m_isPositiveTransition = false; }
        }
    }
}
