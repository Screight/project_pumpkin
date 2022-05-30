using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SceneManager : MonoBehaviour
    {
        static SceneManager m_instance;
        Scene m_currentScene = null;
        SCENE m_currentSceneID;
        int m_numberOfTotalScenes;
        string m_savePath;
        bool m_loadGame = false;

        static public SceneManager Instance
        {
            get { return m_instance; }
            private set { }
        }

        private void Awake()
        {

            if (m_instance == null)
            {
                m_instance = this;
                Initialize();
                DontDestroyOnLoad(gameObject);
            }
            else { Destroy(this); }

        }
        void Initialize()
        {
            m_numberOfTotalScenes = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            m_currentSceneID = (SCENE)UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }

        public void SetCurrentScene(Scene p_scene)
        {
            m_currentScene = p_scene;
            m_currentSceneID = p_scene.SCENE;
            SoundManager.Instance.PlayBackground(m_currentScene.BackgroundMusic);
        }

        public void LoadScene(int p_scene)
        {
            if (p_scene >= m_numberOfTotalScenes)
            {
                Debug.LogError("Scene index not found. Check if all the scenes have been added to Unity Scene Manager (File -> Build Settings).");
                return;
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)p_scene);
            m_currentSceneID = (SCENE)p_scene;
        }

        public BACKGROUND_CLIP GetCurrentBackgroudClipName() { return m_currentScene.GetCurrentBackgroudClipName(); }

        public void ExitGame()
        {
            Application.Quit();
        }

        public SCENE Scene { get { return m_currentSceneID; } }

        public string SavePath { 
            get { return m_savePath; }
            set { m_savePath = value; }
        }

        public bool LoadGame{
            get { return m_loadGame; }
            set { m_loadGame = value; }
        }

    }

}