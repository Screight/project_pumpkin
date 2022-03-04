using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weakPlatformDestroyer : MonoBehaviour
{
    private SpriteRenderer platformSprite;
    private GameObject player;
    private Player playerScript;
    

    private void Awake() 
    {
        platformSprite = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "groundCheck" && playerScript.State == PLAYER_STATE.GROUNDBREAKER)
        {
            platformSprite.color = Color.Lerp(Color.white, Color.red, 10.0f);
            Destroy(gameObject, 0.1f);
        }
    }
}
