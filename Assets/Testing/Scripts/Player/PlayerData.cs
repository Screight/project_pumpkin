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
    public float m_gravityUp = 220.0f;
    public float m_gravityDown = 300.0f;
    public float m_jumpSpeed = 200.0f;
    [Header("CheckVariables")]
    [Tooltip("Height and width of the box used to determine whether the player is grounded or not.")]
    public Vector2 m_groundBox = new Vector2(8,2);
    [Tooltip("The layers to check if the player is grounded or not.")]
    public LayerMask m_groundMask;
}
