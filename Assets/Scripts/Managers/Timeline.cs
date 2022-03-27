using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Timeline : MonoBehaviour
{
    public PlayableDirector m_director;
    private BoxCollider2D m_collider;
    private void Awake() { m_collider = GetComponent<BoxCollider2D>(); }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            startTimeline();
        }
    }

    public void startTimeline()
    {
        m_director.Play();
        gameObject.SetActive(false);
    }
}
