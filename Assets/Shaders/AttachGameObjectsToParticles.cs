using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    private enum LightColorOptions { ParticleColor, CustomColor, Default };

    public GameObject m_Prefab;

    private ParticleSystem m_ParticleSystem;
    private List<GameObject> m_Instances = new List<GameObject>();
    private ParticleSystem.Particle[] m_Particles;



    [SerializeField] LightColorOptions colorOpt = LightColorOptions.ParticleColor;
    private Color particleColorAvg;
    [SerializeField] private Color customColor;
    private Color colorApply;


    // Start is called before the first frame update
    void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
        switch (colorOpt)
        {
            case LightColorOptions.ParticleColor:
                particleColorAvg = (m_ParticleSystem.main.startColor.colorMin + m_ParticleSystem.main.startColor.colorMax) / 2;
                colorApply = particleColorAvg;
                break;
            case LightColorOptions.CustomColor:
                colorApply = customColor;
                break;
            case LightColorOptions.Default:
                colorApply = m_Prefab.GetComponent<Light2D>().color;
                break;
            default:
                break;
        }



        m_Prefab.GetComponent<Light2D>().color = colorApply;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count)
            m_Instances.Add(Instantiate(m_Prefab, m_ParticleSystem.transform));

        bool worldSpace = (m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < m_Instances.Count; i++)
        {
            if (i < count)
            {
                m_Instances[i].GetComponent<Light2D>().color = colorApply;
                if (worldSpace)
                    m_Instances[i].transform.position = m_Particles[i].position;

                else
                    m_Instances[i].transform.localPosition = m_Particles[i].position;
                m_Instances[i].SetActive(true);
            }
            else
            {
                m_Instances[i].SetActive(false);
            }
        }
    }
}
