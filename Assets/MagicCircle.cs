using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    [SerializeField] AudioClip[] m_spawnSound;
    AudioSource m_source;
    private void Awake() {
        m_source = GetComponent<AudioSource>();
    }

    public void PlaySound(){
        int number = Random.Range(0,m_spawnSound.Length);
        m_source.PlayOneShot(m_spawnSound[number]);
    }

}
