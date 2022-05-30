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
        platform_render = GetComponent<Tilemap>();
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();

        platformSprite.material = def_mat;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("groundCheck"))
        {
            platformSprite.material = shake_mat;

            if (playerScript.State == PLAYER_STATE.GROUNDBREAKER || collision.name == "Samu5")
            {
                platform_render.color = Color.Lerp(Color.white, Color.red, 0.1f);
                Destroy(gameObject, 0.2f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D p_collision)
    {   if(p_collision.tag != "Player") { return;}
        platformSprite.material = def_mat;
    }

    private void OnDestroy() {
        Player.Instance.IsGrounded = false;
    }

}