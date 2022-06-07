using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samu_eye_script : MonoBehaviour
{
    [SerializeField] Samu_animation_script par_scr;

    public void Damage()
    {
        SoundManager.Instance.PlayOnce(AudioClipName.SAMAEL_LOSE_EYE);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D p_collider)
    {
        if (!p_collider.CompareTag("fireball")) { return; }
        Damage();
    }
}