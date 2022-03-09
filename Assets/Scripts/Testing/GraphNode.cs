using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Because often you will require that a node contains additional information,
///GraphNode is typically used as a base class from which to derive custombuilt nodes. For example, a navigation graph’s nodes must store spatial
///information, and a dependency graph’s nodes must contain information
///about the assets they represent.
/// </summary>
public class GraphNode
{
    // every node has an index > 0 that represents it
    protected int m_index;
    public GraphNode(int p_index) {
        m_index = p_index;
    }

    public int Index
    {
        get { return m_index; }
        set { m_index = value; }
    }

}
