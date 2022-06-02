using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] GameObject m_arrowPrefab;
    Timer m_shootCooldownTimer;
    [SerializeField] float m_cooldown = 1;
    [SerializeField] bool m_isFacingRight = true;

    private void Awake()
    {
        m_shootCooldownTimer = gameObject.AddComponent<Timer>();
    }

    private void Start()
    {
        m_shootCooldownTimer.Duration = m_cooldown;
        m_shootCooldownTimer.Run();
    }

    private void Update()
    {
        if (m_shootCooldownTimer.IsFinished)
        {
            BoneScript boneScript = Instantiate(m_arrowPrefab, new Vector3(transform.position.x + 3, transform.position.y - 1, transform.position.z), Quaternion.identity).GetComponent<BoneScript>();
            int direction;
            if (m_isFacingRight) { direction = 1; }else { direction = -1; }
            boneScript.Shoot(direction, AudioClipName.SKELLY_SHOOT);
            m_shootCooldownTimer.Stop();
            m_shootCooldownTimer.Run();
        }
    }

}
