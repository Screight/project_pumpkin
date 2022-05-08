using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] GameObject[] m_parallaxLayers;

    [SerializeField] float[] m_layerDepth;

    [SerializeField] float[] m_offSetY;

    Camera m_camera;

    private void Awake() {

        m_camera = Camera.main;
        
    }

    private void Start() {

        // POSITION 2D MAP IN THE CORRECT DEPTH


        float spriteOriginalHeight = 0;
        float mainSceneDepth = CameraManager.Instance.MainSceneDepth;
        float newScale = 0;
        float newOffSetY  = 0;

        Camera m_camera = Camera.main;
        float m_cameraAngle = m_camera.fieldOfView;

        for(int i =0; i< m_parallaxLayers.Length; i++){

            SpriteRenderer currentLayer = m_parallaxLayers[i].GetComponent<SpriteRenderer>();
            spriteOriginalHeight = currentLayer.sprite.bounds.size.y;
            newScale = (m_layerDepth[i] + mainSceneDepth)/CameraManager.Instance.MainSceneDepth;
            newOffSetY = m_offSetY[i] * mainSceneDepth / mainSceneDepth;

            
            m_parallaxLayers[i].transform.localScale = new Vector3(newScale, newScale, m_parallaxLayers[i].transform.localScale.z);
            m_parallaxLayers[i].transform.position = new Vector3(m_parallaxLayers[i].transform.position.x, newOffSetY + m_parallaxLayers[i].transform.position.y, m_layerDepth[i] + mainSceneDepth);
            

            

            
        }

        

    }

}