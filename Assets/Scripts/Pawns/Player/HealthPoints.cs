using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPoints : MonoBehaviour
{
    [SerializeField] Sprite m_fullHeart;
    [SerializeField] Sprite m_emptyHeart;
    [SerializeField] GameObject m_life;
    List<GameObject> m_hearts;

    int m_numberofHearts;
    [SerializeField] float m_cameraSeparation = 1;
    float m_separationBetweenHearts;

    private void Start()
    {
        float cameraSize = Camera.main.scaledPixelWidth;

        m_separationBetweenHearts = cameraSize * (m_cameraSeparation/100)
        ;
        m_numberofHearts = GameManager.Instance.PlayerMaxHealth;
        m_hearts = new List<GameObject>(m_numberofHearts);
        for(int i = 0; i < m_numberofHearts; i++){
            GameObject gameobject = Instantiate(m_life);
            gameobject.transform.SetParent(this.gameObject.transform);
            gameobject.transform.localScale = new Vector3(1,1,1);
            gameobject.transform.position = transform.position;
            gameobject.transform.position += new Vector3(m_separationBetweenHearts * i, 0, 0);
            m_hearts.Add(gameobject);

        }

        for(int i = 0; i < GameManager.Instance.PlayerHealth; i++){
            m_hearts[i].GetComponent<LifeUra>().GainHeart();
        }
    }

    public void RestoreHeart(){
        m_hearts[GameManager.Instance.PlayerHealth - 1].GetComponent<LifeUra>().GainHeart();
    }

    public void RestoreAllHearts(){
        for(int i =0; i < GameManager.Instance.PlayerMaxHealth; i++){
            m_hearts[i].GetComponent<LifeUra>().GainHeart();
        }
    }

    public void LoseHeart()
    {
        m_hearts[GameManager.Instance.PlayerHealth].GetComponent<LifeUra>().LoseHeart();
    }

    public void GainExtraHeart(){

        float cameraSize = Camera.main.scaledPixelWidth;

        m_separationBetweenHearts = cameraSize * (m_cameraSeparation/100)
        ;

        GameObject gameobject = Instantiate(m_life);
        gameobject.transform.SetParent(this.gameObject.transform);
        gameobject.transform.position = transform.position;
        gameobject.transform.localScale = new Vector3(1,1,1);
        m_hearts.Add(gameobject);
        gameobject.transform.position += new Vector3(m_separationBetweenHearts * (m_hearts.Count - 1), 0, 0);
        GameManager.Instance.RestorePlayerToFullHealth();
    }

}