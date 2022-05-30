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

    private void Start() {
        m_savePath = "game_" + m_slot + ".sav";
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

}
