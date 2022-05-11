using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBody : MonoBehaviour
{
    public GameObject[] bodyparts;
    private Rigidbody2D[] bodyparts_rb;
    public GameObject explosion;
    [SerializeField] Door[] m_doors;
    
    // Start is called before the first frame update
    void Start()
    {
        bodyparts_rb = new Rigidbody2D[bodyparts.Length];
        for (int i = 0; i < bodyparts.Length; i++)
        {
            bodyparts_rb[i] = bodyparts[i].GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Explode()
    {
        for (int i = 0; i < bodyparts.Length; i++)
        {
            float x = Random.Range(-1000, -3000);
            float y = Random.Range(5000, 6000);
            if (bodyparts[i].transform.localScale.x < 0) { x = -x; }
            if (bodyparts[i].tag == "spiderBoss") { x = x / 10; }
            Vector2 new_force = new Vector2(x, y);
            bodyparts_rb[i].AddForce(new_force);
        }

    }
    void Explode2() { 
        explosion.SetActive(true);
        SoundManager.Instance.PlayBackground(BACKGROUND_CLIP.BACKGROUND_1);
        for(int i  = 0; i < m_doors.Length; i++){
            m_doors[i].OpenDoor(true);
        }
    }
}
