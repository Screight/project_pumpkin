using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Scene : MonoBehaviour
    {
        [SerializeField] BACKGROUND_CLIP m_backgroundMusic;
        BACKGROUND_CLIP m_currentBackgroundClip;
        [SerializeField] SCENE m_scene;

        private void Start()
        {
            SetUpScene();
        }

        public void SetUpScene()
        {
            SceneManager.Instance.SetCurrentScene(this);
            m_currentBackgroundClip = m_backgroundMusic;
            if(m_scene == SCENE.MAIN_MENU){
                Room.ResetID();
            }
        }

        public BACKGROUND_CLIP BackgroundMusic { get { return m_backgroundMusic; } }
        public BACKGROUND_CLIP GetCurrentBackgroudClipName() { return m_currentBackgroundClip; }

        public SCENE SCENE { get { return m_scene; } }

    }
}