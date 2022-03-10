using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NavigationGraphNode: GraphNode
{
    Vector3 m_position;

    public NavigationGraphNode(int p_index, Vector2 p_position)
        :base(p_index)
    {
        m_position = p_position;
    }

    public Vector3 Position
    {
        get { return m_position; }
    }

    public bool IsNearEnoughThisNode(int p_minDistance, Vector3 p_position)
    {
        if((new Vector2(p_position.x, p_position.y) - new Vector2(m_position.x, m_position.y)).magnitude <= p_minDistance)
        {
            return true;
        }

        return false;
    }

}
