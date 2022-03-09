using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NavigationGraphNode: GraphNode
{
    Vector2 m_position;

    public NavigationGraphNode(int p_index, Vector2 p_position)
        :base(p_index)
    {
        m_position = p_position;
    }

}
