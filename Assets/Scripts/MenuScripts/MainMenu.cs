using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] LoadingScreen m_loadingScreen;

    public void ExitGame(){
        Application.Quit();
    }

    public void EnterGame(){
        m_loadingScreen.LoadScene(SCENE.GAME);
        Time.timeScale = 1;
    }

}