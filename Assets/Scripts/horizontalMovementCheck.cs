using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horizontalMovementCheck : MonoBehaviour
{
    Player m_playerScript;

    private void Start()
    {
        m_playerScript = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            m_playerScript.CanMoveHorizontal = false;
            Debug.Log("floor entered");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            m_playerScript.CanMoveHorizontal = true;
            Debug.Log("floor exited");
        }
    }
}
