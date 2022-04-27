using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleSettings : MonoBehaviour
{
    [SerializeField] Slider m_effectsVolumeLevel;
    [SerializeField] Slider m_backgroundVolumeLevel;
    [SerializeField] Sprite m_greenFill;
    [SerializeField] Sprite m_darkGreenFill;
    [SerializeField] Image[] m_fillSlider;
    int m_currentSlider = 0;

    private void OnEnable() {
        /*m_fillSlider[0].sprite = m_darkGreenFill;
        m_currentSlider = 0;
        for(int i = 1; i < m_fillSlider.Length; i++){
            m_fillSlider[i].sprite = m_greenFill;
        }*/
    }

    private void Update() {
        /*if(InputManager.Instance.UpButtonPressed || InputManager.Instance.VerticalAxisRaw == 1){
            m_fillSlider[m_currentSlider].sprite = m_greenFill;
            if(m_currentSlider > 0){
                m_currentSlider--;
            }
            else{ m_currentSlider = m_fillSlider.Length - 1;}
            m_fillSlider[m_currentSlider].sprite = m_darkGreenFill;
        }
        else if(InputManager.Instance.DownButtonPressed || InputManager.Instance.VerticalAxisRaw == -1){
            m_fillSlider[m_currentSlider].sprite = m_greenFill;
            if(m_currentSlider < m_fillSlider.Length - 1){
                m_currentSlider++;
            }
            else{ m_currentSlider = 0;}
            m_fillSlider[m_currentSlider].sprite = m_darkGreenFill;
        }*/
    }

    private void Start() {
        m_effectsVolumeLevel.value = SoundManager.Instance.EffectVolume;
        m_backgroundVolumeLevel.value = SoundManager.Instance.BackgroundVolume;
    }

    public void SetBackgroundVolume(){
        SoundManager.Instance.SetBackgroundVolume(m_backgroundVolumeLevel.value);
    }

    public void SetEffectsVolume(){
        SoundManager.Instance.SetEffectsVolume(m_effectsVolumeLevel.value);
    }


}
