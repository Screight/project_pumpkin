using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SelectSaveButton : MonoBehaviour
{
    public enum SPIRITS { FIRE, DARK, GROUNDBREAKER, LAST_NO_USE}

    [SerializeField] int m_slot;
    [SerializeField] LoadingScreen m_script;
    string m_savePath;
    [SerializeField] GameObject[] m_spirits;
    bool m_loadGame;
    [SerializeField] GameObject m_options;

    [SerializeField] Transform m_heartStartingPosition;
    float m_scale;
    [SerializeField] float m_separation;
    [SerializeField] TMPro.TextMeshProUGUI m_zone;

    private void Start() {
        m_savePath = "game_" + m_slot + ".sav";
        // 320 es la anchura de referencia

        m_scale = Screen.width / 320;
        SetUpUI();
    }

    public void StartGame(){
        Game.SceneManager.Instance.LoadGame = m_loadGame;
        Game.SceneManager.Instance.SavePath = m_savePath;
        if(!m_loadGame){
            m_script.LoadScene(SCENE.GAME);
        }
        else{
            m_options.SetActive(true);
            m_options.GetComponent<OptionBox>().OpenBox();
        }
    }

    public string Path {
        get { return m_savePath; }
    }

    public bool LoadGame{
        get { return m_loadGame; }
        set { m_loadGame = value; }
    }

    public void SetUpUI(){
        SaveLoadGame.LoadMenuParameters(this);
    }

    public void SetSpiritTo(bool p_isActive, SPIRITS p_spirit){
        m_spirits[(int)p_spirit].SetActive(p_isActive);
    }

    public void SetZoneTo(ZONE p_zone){
        string zoneName;
        switch(p_zone){
            case ZONE.FOREST:
                zoneName = "Forest";
            break;
            case ZONE.MINE:
                zoneName = "Abandoned Mine";
            break;
            default:
                zoneName = "New Game";
            break;
        }
        m_zone.text = zoneName;
    }

    public void SetUpHearts(int p_numberOfHearts){
        Vector3 position;
        GameObject heart;
        for(int i = 0; i < p_numberOfHearts; i++){
            
            position = new Vector3(m_heartStartingPosition.position.x + i * m_separation * m_scale, m_heartStartingPosition.position.y, transform.position.z );
            heart = Instantiate(Resources.Load<GameObject>("heartIcon"));
            heart.transform.SetParent(m_heartStartingPosition.transform.parent);
            heart.transform.position = position;
            heart.transform.localScale = new Vector3(1,1,1);
        }
    }

}
