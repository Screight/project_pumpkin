using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class weakPlatformDestroyer : MonoBehaviour
{
    private TilemapRenderer platformSprite;
    private Tilemap platform_render;
        
    private GameObject player;
    private Player playerScript;
    [SerializeField] Material shake_mat;
    [SerializeField] Material def_mat;
    

    private void Awake() 
    {
        platformSprite = GetComponent<TilemapRenderer>();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();

        platformSprite.material = def_mat;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "groundCheck" )  
        {
            platformSprite.material = shake_mat;

            if (playerScript.State == PLAYER_STATE.GROUNDBREAKER) {

                platform_render.color = Color.Lerp(Color.white, Color.red, 10.0f);
                Destroy(gameObject, 0.1f); }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
       
            platformSprite.material = def_mat;
        

    }
}

