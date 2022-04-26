using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsManager : MonoBehaviour
{
    [SerializeField] Transform m_defaultCheckpoint;

    Transform m_localCheckPoint;

    Transform m_globalCheckPoint;

    Player m_player;

    static CheckpointsManager m_instance;

    private CheckpointsManager() { }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            m_localCheckPoint = m_defaultCheckpoint;
            m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        } else { Destroy(this.gameObject); }

    }

    public void SetLocalCheckPoint(Transform m_checkpoint)
    {
        m_localCheckPoint = m_checkpoint;
    }

    public void SetGlobalCheckPoint(Transform p_checkpoint){
        m_globalCheckPoint = p_checkpoint;
        m_localCheckPoint = p_checkpoint;
    }

    public void MovePlayerToLocalCheckPoint()
    {
        m_player.ResetPlayer(PLAYER_STATE.IDLE,ANIMATION.PLAYER_IDLE);
        if (m_localCheckPoint != null) { m_player.transform.position = new Vector3(m_localCheckPoint.position.x, m_localCheckPoint.position.y, m_player.transform.position.z); }
        else { m_player.transform.position = new Vector3(m_defaultCheckpoint.position.x, m_defaultCheckpoint.position.y, m_player.transform.position.z); }
        EnemyManager.Instance.ResetAllAliveEnemies();
    }

    public void MovePlayerToGlobalCheckPoint()
    {
        m_player.ResetPlayer(PLAYER_STATE.IDLE, ANIMATION.PLAYER_IDLE);
        if (m_globalCheckPoint != null) 
        {
            m_player.transform.position = new Vector3(m_globalCheckPoint.position.x, m_globalCheckPoint.position.y, m_player.transform.position.z);
        }
        else 
        {
            m_player.transform.position = new Vector3(m_defaultCheckpoint.position.x, m_defaultCheckpoint.position.y, m_player.transform.position.z);
        }
    }

    static public CheckpointsManager Instance 
    {
        get { return m_instance; }
        private set { }
    }
}