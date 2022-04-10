using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopSuction : MonoBehaviour
{
    [SerializeField] float p_suctionVelocity;
    bool m_isSuctionActive = true;

    private void OnTriggerEnter2D(Collider2D p_collider)
    {
        if (p_collider.tag != "Player") { return; }
        if (Player.Instance.Speed.y <= 0) { m_isSuctionActive = false; }
        else { m_isSuctionActive = true; }

        if (!m_isSuctionActive) { return; }
        Player.Instance.SetPlayerToScripted();
        Player.Instance.ScriptTopSuction(p_suctionVelocity);
    }
}