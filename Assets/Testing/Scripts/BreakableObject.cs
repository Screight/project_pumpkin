using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    // IT IS ASSUMED THAT ALL THE PARTS OF THE OBJECT HAVE THE SAME MASS (1)
    [SerializeField] GameObject m_object;
    [SerializeField] Rigidbody2D[] m_objectParts;
    Collider2D[] m_colliders;
    [Tooltip("If set to 0 the explosion will be calculated from the center of the object.")]
    [SerializeField] Vector2 m_offSet;
    [Tooltip("Intensity of the explosion.")]
    [SerializeField] float m_intensity;
    [SerializeField] float m_friction;
    Vector2 m_workVector;
    [SerializeField] float m_gravityScale = 1;
    [Tooltip("True if the parts ob the object collide between them.")]
    [SerializeField] bool m_collision;
    bool m_isBroken = false;
    private void Awake() {
        if(m_objectParts.Length > 0){
            m_objectParts[0].transform.parent.gameObject.SetActive(false);

            m_colliders = new Collider2D[m_objectParts.Length];
            for(int i = 0; i < m_colliders.Length; i++){
                m_colliders[i] = m_objectParts[i].gameObject.GetComponent<Collider2D>();
                m_objectParts[i].gravityScale = m_gravityScale;
            }
            if(!m_collision){
                
                for(int i = 0; i < m_colliders.Length; i++){
                    for(int j = 0; j < m_colliders.Length && i != j; j++){
                        Physics2D.IgnoreCollision(m_colliders[i], m_colliders[j], true);
                    }
                }
            }
            

        }

    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.H)){
            Break();
        }
    }
    
    private void FixedUpdate() {
        if(m_isBroken){
            for(int i = 0; i < m_objectParts.Length; i++){
                m_objectParts[i].velocity -= m_objectParts[i].velocity * (m_friction/1 * Time.fixedDeltaTime);
            }
        }
    }

    public void Break(){
        Debug.Log("EXPLOOOOOOOOOOOOOOOOOOOOOOSION!");
        m_objectParts[0].transform.parent.gameObject.SetActive(true);
        m_objectParts[0].transform.parent.transform.position = m_object.transform.position;
        m_object.SetActive(false);
        m_isBroken = true;
        
        for(int i = 0;i < m_objectParts.Length; i++){
            m_workVector = (m_colliders[i].offset - m_offSet).normalized;
            Debug.Log(m_workVector);
            m_objectParts[i].velocity = m_intensity * m_workVector;
        }
    }

}
