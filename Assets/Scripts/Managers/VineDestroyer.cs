using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VineDestroyer : MonoBehaviour
{
    private Tilemap vineSprite;

    private void Awake() { vineSprite = GetComponent<Tilemap>(); }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("fireball")|| collision.CompareTag("Spirit"))
        {
            vineSprite.color = Color.Lerp(Color.white, Color.red, 10.0f);
            Destroy(gameObject, 0.5f);
        }
    }
}