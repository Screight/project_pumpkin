using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMainMenu : MonoBehaviour
{
    private void Update() {
        if(InputManager.Instance.InteractButtonPressed){
            Game.SceneManager.Instance.LoadScene((int)SCENE.MAIN_MENU);
        }
    }
}
