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

        private void Start()
        {
            SetUpScene();
        }

        public void SetUpScene()
        {
            SceneManager.Instance.SetCurrentScene(this);
            m_currentBackgroundClip = m_backgroundMusic;
        }

        public BACKGROUND_CLIP BackgroundMusic { get { return m_backgroundMusic; } }
        public BACKGROUND_CLIP GetCurrentBackgroudClipName() { return m_currentBackgroundClip; }

    }
}