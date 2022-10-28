using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CheckpointsManager : MonoBehaviour
{
    [SerializeField] Transform m_defaultCheckpoint;

    Transform m_localCheckPoint;

    Transform m_globalCheckPoint;

    Player m_player;

    static CheckpointsManager m_instance;
    BACKGROUND_CLIP m_checkpointMusic;
    ZONE m_zone;

    private CheckpointsManager() { }

    public void SetMusicTo(ZONE p_zone)
    {
        switch (p_zone)
        {
            case ZONE.FOREST:
                {
                    m_checkpointMusic = BACKGROUND_CLIP.FORESTOFSOULS;
                }
                break;
            case ZONE.MINE:
                {
                    m_checkpointMusic = BACKGROUND_CLIP.ABANDONEDMINE;
                }
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            m_checkpointMusic = BACKGROUND_CLIP.FORESTOFSOULS;
            m_zone = ZONE.FOREST;
            
            m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        } else { Destroy(this); }

    }

    private void Start() {
        if(m_defaultCheckpoint != null){
                SetGlobalCheckPoint(m_defaultCheckpoint);
            }
            else{
                SetGlobalCheckPoint(Player.Instance.transform);
            }
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

    public void MovePlayerToGlobalCheckPoint(bool p_setVolumeToSettings = true)
    {
        m_player.ResetPlayer(PLAYER_STATE.IDLE, ANIMATION.PLAYER_IDLE);
        if (m_globalCheckPoint != null) 
        {
            m_player.transform.position = new Vector3(m_globalCheckPoint.position.x, m_globalCheckPoint.position.y, m_player.transform.position.z);
        }
        else if(m_localCheckPoint != null)
        {
            m_player.transform.position = new Vector3(m_localCheckPoint.position.x, m_localCheckPoint.position.y, m_player.transform.position.z);
        }
        SoundManager.Instance.PlayBackground(m_checkpointMusic, p_setVolumeToSettings);
        GameManager.Instance.CurrentZone = m_zone;
    }

    public BACKGROUND_CLIP Music {
        get { return m_checkpointMusic;}
        set { m_checkpointMusic = value;}
    }

    public ZONE Zone {
        get { return m_zone;}
        set { m_zone = value;}
    }

    static public CheckpointsManager Instance 
    {
        get { return m_instance; }
        private set { }
    }

    public void Save(BinaryWriter p_writer){
        p_writer.Write((int)m_zone);
        p_writer.Write((int)m_checkpointMusic);
        // globalCheckpoint
        p_writer.Write(m_globalCheckPoint.position.x);
        p_writer.Write(m_globalCheckPoint.position.y);
        p_writer.Write(m_globalCheckPoint.position.z);
    }

    public void Load(BinaryReader p_reader){
        m_zone = (ZONE)p_reader.ReadInt32();
        m_checkpointMusic = (BACKGROUND_CLIP)p_reader.ReadInt32();
        Vector3 position = new Vector3();
        position.x = p_reader.ReadSingle();
        position.y = p_reader.ReadSingle();
        position.z = p_reader.ReadSingle();
        m_globalCheckPoint.transform.position = position;
        m_localCheckPoint = m_globalCheckPoint;
        MovePlayerToGlobalCheckPoint(false);
    }

}