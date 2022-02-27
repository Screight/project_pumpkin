using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundbreakerScroll : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SkillManager.Instance.UnlockGroundbreaker();
            gameObject.SetActive(false);
        }
    }
}
