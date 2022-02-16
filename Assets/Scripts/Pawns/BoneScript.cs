using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneScript : MonoBehaviour
{
    public GameObject skeleton;
    [SerializeField] GameObject player;
    float m_boneSpeed = 80.0f;
    float m_skeletonPosX;
    float m_playerPosX;
    float m_playerBoneDist;
    float m_nextX;
    float m_baseY;
    float m_height;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //Bone Fire
        m_playerPosX = player.transform.position.x;
        m_skeletonPosX = skeleton.transform.position.x;
        m_playerBoneDist = m_playerPosX - m_skeletonPosX;

        m_nextX = Mathf.MoveTowards(transform.position.x, m_playerPosX, m_boneSpeed * Time.deltaTime);
        m_baseY = Mathf.Lerp(skeleton.transform.position.y, player.transform.position.y, (m_nextX - m_skeletonPosX) / m_playerBoneDist);
        m_height = 2 * (m_nextX - m_skeletonPosX) * (m_nextX - m_playerPosX) / (-0.25f * m_playerBoneDist * m_playerBoneDist);

        Vector3 movePosition = new Vector3(m_nextX, m_baseY + m_height, transform.position.z);
        transform.rotation = LookAtTarged(movePosition - transform.position);
        transform.position = movePosition;

        //Destroy Bone
        if (transform.position == player.transform.position)
        {
            Destroy(gameObject);
        }
    }

    public static Quaternion LookAtTarged(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }
}
