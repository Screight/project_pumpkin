using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustFall : InteractiveItem
{
    AudioSource m_source;
    [SerializeField] AudioClip m_spawnSound;
    [SerializeField] ParticleSystem m_particles;

    protected override void Awake() {
        base.Awake();
        m_source = GetComponent<AudioSource>();
    }

    protected override void HandleInteraction()
    {
        base.HandleInteraction();
        m_source.PlayOneShot(m_spawnSound);
        m_particles.Play();
    }

}
