using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderTest : MonoBehaviour
{
    [SerializeField] Transform m_startingPoint;
    [SerializeField] int m_height;
    [SerializeField] int m_width;
    [SerializeField] int m_tileSize;

    SparseGraph m_graph;

    private void Awake()
    {
        m_graph = new SparseGraph(true);

        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                m_graph.AddNode(new NavigationGraphNode(i+j,new Vector2(i*m_tileSize , j*m_tileSize)));
            }
        }

        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                if( i >= 0 && i < m_width - 1) { m_graph.AddEdge(new GraphEdge(i + j * m_height, (i + 1) + j * m_height)); }
                if( i >= 0 && i < m_width - 1) { m_graph.AddEdge(new GraphEdge((i + 1) + j * m_height, i + j * m_height)); }

                //if( i < m_width - 1) { m_graph.AddEdge(new GraphEdge(i + j, i + 1 + j)); }
                //if( i < m_width - 1) { m_graph.AddEdge(new GraphEdge(i + 1 + j, i + j)); }


                if( j >= 0 && j < m_height - 1) { m_graph.AddEdge(new GraphEdge(i + j * m_height, i + (j * m_height + m_height))); }
                if( j >= 0 && j < m_height - 1) { m_graph.AddEdge(new GraphEdge(i + (j * m_height + m_height), i + j * m_height)); }

                //if( j < m_height - 1) { m_graph.AddEdge(new GraphEdge(i + j, i + (j + m_height))); }
                //if( j < m_height - 1) { m_graph.AddEdge(new GraphEdge(i + (j + m_height), i+j)); }

            }
        }

        GraphSearchBFS graphSearch = new GraphSearchBFS(m_graph,0,8);
        if (graphSearch.IsTargetFound)
        {
            List<int> pathToTarget = graphSearch.GetPathToTarget();
            for (int i = 0; i < pathToTarget.Count; i++)
            {
                Debug.Log(pathToTarget[i]);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Vector3 initialPosition = m_startingPoint.position;
        for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            Gizmos.DrawSphere(new Vector3(initialPosition.x + m_graph.GetNode(i).Position.x, initialPosition.y + m_graph.GetNode(i).Position.y, initialPosition.z), 1f);
        }
    }

}
