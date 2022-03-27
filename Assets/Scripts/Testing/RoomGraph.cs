using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGraph : MonoBehaviour
{
    [SerializeField] Transform m_startingPoint;
    [SerializeField] int m_height;
    [SerializeField] int m_width;
    [SerializeField] int m_tileSize;
    SparseGraph m_graph;
    [SerializeField] LayerMask m_obstacleLayer;
    private void Awake()
    {
        //m_target = GameObject.FindGameObjectWithTag("Player");
        m_graph = new SparseGraph(true);
        
        for (int j = 0; j < m_height; j++)
        {
            for (int i = 0; i < m_width; i++)
            {
                m_graph.AddNode(new NavigationGraphNode(i+j,new Vector3(i*m_tileSize, j*m_tileSize, 0) + m_startingPoint.transform.position));
            }
        }

        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                if( i >= 0 && i < m_width - 1) { m_graph.AddEdge(new GraphEdge(i + j * m_width, (i + 1) + j * m_width)); }
                if( i >= 0 && i < m_width - 1) { m_graph.AddEdge(new GraphEdge((i + 1) + j * m_width, i + j * m_width)); }

                if( j >= 0 && j < m_height - 1) { m_graph.AddEdge(new GraphEdge(i + j * m_width, i + (j * m_width + m_width))); }
                if( j >= 0 && j < m_height - 1) { m_graph.AddEdge(new GraphEdge(i + (j * m_width + m_width), i + j * m_width)); }
            }
        }

        for(int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            Collider2D[] obstacles = Physics2D.OverlapCircleAll(m_graph.GetNode(i).Position, 4, m_obstacleLayer,-100,100);

            foreach (Collider2D enemy in obstacles)
            {
                if (enemy.gameObject.tag == "floor") { m_graph.GetNode(i).IsActive = false; }
            }

        }

    }

    public SparseGraph Graph { get { return m_graph;}}

    private void OnDrawGizmos() {
        // DRAW ALL NODES
        Gizmos.color = Color.black;
        Vector3 initialPosition = m_startingPoint.position;
        for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if (!m_graph.GetNode(i).IsActive) { Gizmos.color = Color.yellow; }
            else { Gizmos.color = Color.black; }

            Gizmos.DrawSphere(new Vector3(m_graph.GetNode(i).Position.x, m_graph.GetNode(i).Position.y, m_graph.GetNode(i).Position.z), 1f);
        }
    }

}
