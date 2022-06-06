using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] LoadingScreen m_loadingScreen;

    private void Awake() {
        Time.timeScale = 1;
    }

    private void Update() {
        if(InputManager.Instance.AttackButtonPressed){
            MenuManager.Instance.GoTo(4);
        }else if(InputManager.Instance.Skill2ButtonPressed){
            MenuManager.Instance.GoTo(3);
        }
    }

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