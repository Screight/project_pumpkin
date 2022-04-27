using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void ExitGame(){
        Application.Quit();
    }

    public void EnterGame(){
        Game.SceneManager.Instance.LoadScene((int)SCENE.GAME);
    }

}