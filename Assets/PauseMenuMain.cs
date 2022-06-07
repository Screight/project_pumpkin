using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuMain : MonoBehaviour
{
    private void Update() {
        if(InputManager.Instance.Skill2ButtonPressed){
            MenuManager.Instance.GoTo(3);
        }
    }
}
