using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float m_length;
    private float m_initialPosition;
    GameObject m_camera;
    public float m_parallaxEffect;

    private void Awake() { m_camera = GameObject.FindGameObjectWithTag("MainCamera"); }

    // Start is called before the first frame update
    void Start()
    {
        m_initialPosition = transform.position.x;
        m_length = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
    }
    private void LateUpdate()
    {
        float distance = -(m_initialPosition - m_camera.transform.position.x) * m_parallaxEffect;
        transform.position = new Vector3(m_initialPosition + distance, transform.position.y, transform.position.z);
    }
}