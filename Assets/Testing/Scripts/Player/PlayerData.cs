using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Game/Data/Player Data/ Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10.0f;


    [Header("Dash State")]
    public float m_dashDistance = 75.0f;


    [Header("Jump State")]
    public float m_minHeight = 10.0f;
    public float m_maxHeight = 55.0f;
    [HideInInspector] public float m_gravityUp = 220.0f;
    [HideInInspector] public float m_gravityDown = 300.0f;
    [Tooltip("Time from the start of the jump until the player reaches max height.")]
    public float m_timeToReachMaxHeight = 0.6f;
    [Tooltip("Time from when the players starts falling (velocity = 0) until it covers a distance equal to max Height.")]
    public float m_timeToFallMaxHeight = 0.5f;
    [HideInInspector] public float m_maxJumpSpeed = 200.0f;
    [HideInInspector] public float m_minJumpSpeed = 50.0f;


    [Header("Groundbreaker State")]
    public float m_groundbreakerSpeed = -260.0f;


    [Header("Fireball State")]
    public GameObject m_fireball;


    [Header("CheckVariables")]
    [Tooltip("Height and width of the box used to determine whether the player is grounded or not.")]
    public Vector2 m_groundBox = new Vector2(8,2);
    [Tooltip("The layers to check if the player is grounded or not.")]
    public LayerMask m_groundMask;

    public void Initialize(){
        m_gravityUp =  - 2 * m_maxHeight / (m_timeToReachMaxHeight * m_timeToReachMaxHeight);
        m_gravityDown =  - 2 * m_maxHeight / (m_timeToFallMaxHeight * m_timeToFallMaxHeight);

        m_minJumpSpeed = Mathf.Sqrt( 2 * Mathf.Abs(m_gravityUp) * m_minHeight);

        m_maxJumpSpeed = (m_maxHeight - (m_gravityUp * m_timeToReachMaxHeight * m_timeToReachMaxHeight / 2)) / m_timeToReachMaxHeight;
    }
}
