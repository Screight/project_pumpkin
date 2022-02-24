using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CheckpointsManager.Instance.SetLocalCheckPoint(transform);
        }
    }
}
