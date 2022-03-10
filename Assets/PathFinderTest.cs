using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderTest : MonoBehaviour
{
    [SerializeField] Transform m_startingPoint;
    [SerializeField] int m_height;
    [SerializeField] int m_width;
    [SerializeField] int m_tileSize;
    [SerializeField] Transform m_initialPosition;
    [SerializeField] Transform m_targetPosition;
    int m_initialNode;
    int m_targetNode;

    [SerializeField] LayerMask m_obstacleLayer;

    SparseGraph m_graph;
    GraphSearchBFS m_graphSearch;

    [SerializeField] float m_speed;
    int m_currentNodeIndex;
    List<int> pathToTarget;
    Dictionary<int, int> m_pathToTarget;

    private void Awake()
    {
        m_graph = new SparseGraph(true);
        m_pathToTarget = new Dictionary<int, int>();
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

                //if( i < m_width - 1) { m_graph.AddEdge(new GraphEdge(i + j, i + 1 + j)); }
                //if( i < m_width - 1) { m_graph.AddEdge(new GraphEdge(i + 1 + j, i + j)); }


                if( j >= 0 && j < m_height - 1) { m_graph.AddEdge(new GraphEdge(i + j * m_width, i + (j * m_width + m_width))); }
                if( j >= 0 && j < m_height - 1) { m_graph.AddEdge(new GraphEdge(i + (j * m_width + m_width), i + j * m_width)); }

                //if( j < m_height - 1) { m_graph.AddEdge(new GraphEdge(i + j, i + (j + m_height))); }
                //if( j < m_height - 1) { m_graph.AddEdge(new GraphEdge(i + (j + m_height), i+j)); }

            }
        }

        for(int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            Collider2D[] obstacles = Physics2D.OverlapCircleAll(m_graph.GetNode(i).Position, 5, m_obstacleLayer,-100,100);

            foreach (Collider2D enemy in obstacles)
            {
                if (enemy.gameObject.tag == "floor") { m_graph.GetNode(i).IsActive = false; }
            }

        }

    }

    private void Start()
    {

        for(int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if (m_graph.GetNode(i).IsNearEnoughThisNode(5, m_initialPosition.position)) {
                m_initialNode = i; }
            if (m_graph.GetNode(i).IsNearEnoughThisNode(5, m_targetPosition.position)) {
                m_targetNode = i; }
        }

        m_currentNodeIndex = m_initialNode;
        transform.position = m_graph.GetNode(m_initialNode).Position;

        m_graphSearch = new GraphSearchBFS(m_graph, m_initialNode, m_targetNode);
        if (m_graphSearch.IsTargetFound)
        {
            pathToTarget = m_graphSearch.GetPathToTarget();
            for (int i = 0; i < pathToTarget.Count - 1; i++)
            {
                m_pathToTarget.Add(pathToTarget[i + 1], pathToTarget[i]);
            }
        }

    }
    private void Update()
    {
        if (m_currentNodeIndex != m_targetNode)
        {
            Move();
        }
         
    }

    void Move()
    {
        if (!m_graphSearch.IsTargetFound) { return; }
        Vector3 direction = (m_graph.GetNode(m_pathToTarget[m_currentNodeIndex]).Position - m_graph.GetNode(m_currentNodeIndex).Position).normalized;

        transform.position += m_speed * direction * Time.deltaTime;

        if((m_graph.GetNode(m_currentNodeIndex).Position - transform.position).magnitude > (m_graph.GetNode(m_currentNodeIndex).Position - m_graph.GetNode(m_pathToTarget[m_currentNodeIndex]).Position).magnitude)
        {
            transform.position = m_graph.GetNode(m_pathToTarget[m_currentNodeIndex]).Position;
            m_currentNodeIndex = m_pathToTarget[m_currentNodeIndex];
            Debug.Log(m_currentNodeIndex);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Vector3 initialPosition = m_startingPoint.position;
        for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if(i == m_initialNode) { Gizmos.color = Color.green; }
            else if(i == m_targetNode) { Gizmos.color = Color.red; }
            else if(!m_graph.GetNode(i).IsActive) { Gizmos.color = Color.yellow; }
            else { Gizmos.color = Color.black; }

            Gizmos.DrawSphere(new Vector3(m_graph.GetNode(i).Position.x, m_graph.GetNode(i).Position.y, m_graph.GetNode(i).Position.z), 1f);
        }

        if (!m_graphSearch.IsTargetFound) { return; }
        List<int> pathToTarget = m_graphSearch.GetPathToTarget();
        for (int i = 0; i < pathToTarget.Count - 1; i++)
        {
            Gizmos.DrawLine(m_graph.GetNode(pathToTarget[i]).Position, m_graph.GetNode(pathToTarget[i+1]).Position);
        }
    }

}
