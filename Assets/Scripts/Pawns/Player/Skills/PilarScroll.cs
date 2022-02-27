using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilarScroll : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SkillManager.Instance.UnlockPilar();
            gameObject.SetActive(false);
        }
    }
}