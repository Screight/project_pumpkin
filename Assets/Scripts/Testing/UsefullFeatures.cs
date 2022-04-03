using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefullFeatures : MonoBehaviour
{
    private void Update() {
        if(Input.GetKeyDown("k")){
            RoomManager.Instance.GetCurrentRoom().Reset();
            Debug.Log("patata");
        }

    }
}
