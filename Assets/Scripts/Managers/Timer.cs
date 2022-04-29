using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer: MonoBehaviour
{
    float m_duration;
    float m_time;

    bool m_isFinished;
    bool m_isRunning;

    public Timer(float p_duration)
    {
        m_time = 0;
        m_isFinished = true;
        m_isRunning = false;
        m_duration = p_duration;
    }

    private void Awake()
    {
        m_time = 0;
        m_isFinished = true;
        m_isRunning = false;
    }

    private void Update()
    {
        if(!m_isFinished && m_isRunning)
        {
            m_time += Time.deltaTime;
            if(m_time >= m_duration) { 
                m_isFinished = true;
                m_isRunning = false;
                m_time = m_duration;
            }
        }
    }

    /// <summary>
    /// Starts the timer if it's not ticking
    /// </summary>
    public void Run() {
        if (m_duration > 0 && m_isFinished) {
            m_isFinished = false;
            m_isRunning = true;
            m_time = 0;
        } 
    }
    public void Stop()
    {
        m_isRunning = false;
        m_isFinished = true;
        m_time = 0;
    }
    public void Restart()
    {
        Stop();
        Run();
    }
    public void Pause() { if (m_isRunning && !m_isFinished) { m_isRunning = false; } }

    public bool IsFinished
    {
        get { return m_isFinished; }
        private set { ; }
    }

    public bool IsRunning
    {
        get { return m_isRunning && !m_isFinished; }
        private set {; }
    }

    public bool IsActive { get { return !m_isFinished; } }

    public float CurrentTime
    {
        get { return m_time; }
        private set {; }
    }

    public float Duration
    {
        get { return m_duration; }
        set { m_duration = value; }
    }
}