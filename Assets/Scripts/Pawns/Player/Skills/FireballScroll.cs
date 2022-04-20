using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScroll : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.Instance.SetIsSkillAvailable(SKILLS.FIRE_BALL, true);
            gameObject.SetActive(false);
        }
    }
}