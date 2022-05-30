using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadGame : MonoBehaviour
{

    static SaveLoadGame m_instance;
    public static SaveLoadGame Instance { get { return m_instance; } }

    bool m_isGameLoaded = false;
    [SerializeField] InteractiveItem[] m_interactiveItems;
    [SerializeField] GameObject[] m_cutScenes;
    [SerializeField] GameObject[] m_breakableGround;
    MiniMap m_miniMap;
    string m_path;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            m_interactiveItems[2] = FindObjectOfType<GroundbreakerRune>();
            m_miniMap = FindObjectOfType<MiniMap>();
            m_path = Game.SceneManager.Instance.SavePath;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (!m_isGameLoaded)
        {
            Load();
            m_isGameLoaded = true;
        }
    }

    public void Save()
    {

        FileStream fileStream = new FileStream(m_path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        BinaryWriter writer = new BinaryWriter(fileStream);

        writer.Write(GameManager.Instance.GetIsSkillAvailable(SKILLS.FIRE_BALL));
        writer.Write(GameManager.Instance.GetIsSkillAvailable(SKILLS.DASH));
        writer.Write(GameManager.Instance.GetIsSkillAvailable(SKILLS.GROUNDBREAKER));

        CheckpointsManager.Instance.Save(writer);
        GameManager.Instance.Save(writer);
        for (int i = 0; i < m_interactiveItems.Length; i++)
        {
            m_interactiveItems[i].Save(writer);
        }

        for (int i = 0; i < m_cutScenes.Length; i++)
        {
            if (!m_cutScenes[i].activeSelf)
            {
                writer.Write(false);
            }
            else { writer.Write(true); }
        }

        for (int i = 0; i < m_breakableGround.Length; i++)
        {
            if (m_breakableGround[i] != null)
            {
                writer.Write(true);
            }
            else { writer.Write(false); }
        }


        m_miniMap.Save(writer);

        fileStream.Close();
    }

    public void Load()
    {

        if (!Game.SceneManager.Instance.LoadGame) { return; }

        string path = m_path;

        FileStream fileStream;

        try
        {
            fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(fileStream);

            reader.ReadBoolean();
            reader.ReadBoolean();
            reader.ReadBoolean();

            CheckpointsManager.Instance.Load(reader);
            GameManager.Instance.Load(reader);

            for (int i = 0; i < m_interactiveItems.Length; i++)
            {
                m_interactiveItems[i].Load(reader);
            }

            for (int i = 0; i < m_cutScenes.Length; i++)
            {
                bool isActive = reader.ReadBoolean();
                m_cutScenes[i].SetActive(isActive);
                m_cutScenes[i].GetComponent<Timeline>().SetEntitiesTo(isActive);
            }

            for (int i = 0; i < m_breakableGround.Length; i++)
            {
                bool isActive = reader.ReadBoolean();
                if (!isActive)
                {
                    Destroy(m_breakableGround[i]);
                }
            }

            m_miniMap.Load(reader);

            fileStream.Close();
        }
        catch
        {
            Debug.LogError("FILE '" + path + "'  NOT FOUND");
        }
    }

    static public void LoadMenuParameters(SelectSaveButton p_saveState)
    {
        try
        {
            FileStream fileStream = new FileStream(p_saveState.Path, FileMode.Open, FileAccess.Read);

            BinaryReader reader = new BinaryReader(fileStream);
            bool value;
            value = reader.ReadBoolean();
            p_saveState.SetSpiritTo(value, SelectSaveButton.SPIRITS.FIRE);
            value = reader.ReadBoolean();
            p_saveState.SetSpiritTo(value, SelectSaveButton.SPIRITS.DARK);
            value = reader.ReadBoolean();
            p_saveState.SetSpiritTo(value, SelectSaveButton.SPIRITS.GROUNDBREAKER);

            p_saveState.LoadGame = true;
            Debug.Log(p_saveState.LoadGame);
            fileStream.Close();
        }
        catch{
            Debug.LogError("File " + p_saveState.Path + " NOT found.");
            p_saveState.LoadGame = false;
        }
    }

}
