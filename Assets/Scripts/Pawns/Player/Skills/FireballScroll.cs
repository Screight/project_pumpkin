using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScroll : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            SkillManager.Instance.UnlockFireball();
            gameObject.SetActive(false);
        }
    }
}