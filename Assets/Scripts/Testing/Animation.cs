using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Animation : MonoBehaviour
{
    Animator m_animator;
    [SerializeField] string[] m_animationName;
    int[] m_animationHash;
    float[] m_animationDuration;
    List<UnityEvent>[]  m_animationEvents;
    // Array of lists of the time at which every event should happen
    List<float>[] m_eventsTime;

    private void Awake() {
        m_animator = GetComponent<Animator>();
        m_animationHash = new int[m_animationName.Length];
        m_animationDuration = new float[m_animationName.Length];

    }

    private void Start() {
        foreach(AnimationClip m_animationClip in m_animator.runtimeAnimatorController.animationClips)
        {
            for(int i = 0; i < m_animationName.Length; i++){
                if(m_animationClip.name == m_animationName[i]){ 
                    m_animationHash[i] = Animator.StringToHash(m_animationName[i]);
                    m_animationDuration[i] = m_animationClip.length;
                }
            }
        }

        m_animationEvents = new List<UnityEvent>[NumberOfAnimations];
        m_eventsTime = new List<float>[NumberOfAnimations];

    }

    private void Update() {
        
    }

    public void AddAnimationEvent(int p_animationIndex, float p_eventTime, UnityAction p_listener){
        if (p_animationIndex >= NumberOfAnimations){
            return ;
        }

        float eventTime;
        if(p_eventTime >= m_animationDuration[p_animationIndex]){ eventTime = m_animationDuration[p_animationIndex]; }
        else { eventTime =  p_eventTime;}

        m_animationEvents[p_animationIndex].Add(new UnityEvent());
    }

    public int NumberOfAnimations{ get {return m_animationName.Length;} }

}
