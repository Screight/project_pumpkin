using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsefullFeatures : MonoBehaviour
{
    [SerializeField] bool m_areSkillsUnlocked = false;
    [SerializeField] bool m_isPlayerInvincible; 
    [SerializeField] Toggle m_skills;
    [SerializeField] Toggle m_invincible;

    [SerializeField] Transform m_spiderBossEntrance;
    [SerializeField] Transform m_spiderBossCheckPoint;

    private void Awake()
    {
        m_skills.onValueChanged.AddListener(SetAreSkillsUnlocked);
        m_invincible.onValueChanged.AddListener(SetPlayerInvincible);
    }

    private void Start()
    {
        m_skills.isOn = m_areSkillsUnlocked;
        SetAreSkillsUnlocked(m_skills.isOn);

        m_invincible.isOn = m_isPlayerInvincible;
        SetPlayerInvincible(m_invincible.isOn);
    }

    private void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            //RoomManager.Instance.GetCurrentRoom().Reset();
            GameManager.Instance.ModifyPlayerHealth(-10000);
            Debug.Log("Developer commited su1c1de");
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            CheckpointsManager.Instance.SetGlobalCheckPoint(m_spiderBossCheckPoint);
            Player.Instance.transform.position = new Vector3(m_spiderBossEntrance.position.x, m_spiderBossEntrance.position.y, Player.Instance.transform.position.z);
            CameraManager.Instance.ClampCameraToTarget();
            GameManager.Instance.SetIsSkillAvailable(SKILLS.DASH, true);
        }

    }

    void SetAreSkillsUnlocked(bool p_value)
    {
        m_areSkillsUnlocked = p_value;
        for (int i = 0; i < (int)SKILLS.LAST_NO_USE; i++)
        {
            GameManager.Instance.SetIsSkillAvailable((SKILLS)i, p_value);
        }
    }

    void SetPlayerInvincible(bool p_value)
    {
        m_isPlayerInvincible = p_value;
        GameManager.Instance.PlayerInvincible = p_value;
    }
}