using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePartycleSystemOnImpact : MonoBehaviour
{
    ParticleSystem m_partycleImpact;
    SpriteRenderer m_renderer;
    AudioSource m_source;
    [SerializeField] AudioClip m_dropImpact;

    private void Awake() {
        m_partycleImpact = GetComponent<ParticleSystem>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D p_collision) {
        if(p_collision.gameObject.tag != "floor") { return ;}
        m_partycleImpact.Play();
        m_renderer.enabled = false;
        m_source.PlayOneShot(m_dropImpact);
    }
}
