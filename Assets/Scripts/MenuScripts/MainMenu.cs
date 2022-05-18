using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] LoadingScreen m_loadingScreen;

    public void ExitGame()
    {
        SoundManager.Instance.PlayOnce(AudioClipName.BUTTONCLICKED);
        Application.Quit();
    }

    public void EnterGame()
    {
        SoundManager.Instance.PlayOnce(AudioClipName.BUTTONCLICKED);
        m_loadingScreen.LoadScene(SCENE.GAME);
        Time.timeScale = 1;
    }
}