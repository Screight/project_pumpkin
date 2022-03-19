using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderTest : MonoBehaviour
{
    [SerializeField] Transform m_startingPoint;
    [SerializeField] int m_height;
    [SerializeField] int m_width;
    [SerializeField] int m_tileSize;

    Vector2 m_initialPosition;
    Vector2 m_targetPosition;
    int m_initialNode;
    int m_targetNode;
    bool m_isInANode;

    bool m_hasNewPathStarted = true;

    [SerializeField] LayerMask m_obstacleLayer;

    SparseGraph m_graph;
    GraphSearchBFS m_graphSearch;

    float m_speed = 10.0f;
    int m_currentNodeIndex;
    List<int> pathToTarget;
    Dictionary<int, int> m_pathToTarget;

    private void Awake()
    {
        //m_target = GameObject.FindGameObjectWithTag("Player");
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

    private void Start()
    {
        m_isInANode = true;
        for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if (m_graph.GetNode(i).IsNearEnoughThisNode(5, m_initialPosition))
            {
                m_initialNode = i;
            }
            if (m_graph.GetNode(i).IsNearEnoughThisNode(16, m_targetPosition))
            {
                m_targetNode = i;
            }
        }

    }

    void SearchTarget()
    {

        m_currentNodeIndex = m_initialNode;
        transform.position = m_graph.GetNode(m_initialNode).Position;

        m_graphSearch = new GraphSearchBFS(m_graph, m_initialNode, m_targetNode);
        if (m_graphSearch.IsTargetFound)
        {
            pathToTarget = m_graphSearch.GetPathToTarget();
            ClearPathToTarget();
            for (int i = 0; i < pathToTarget.Count - 1; i++)
            {
                m_pathToTarget.Add(pathToTarget[i + 1], pathToTarget[i]);
            }
        }
        m_isInANode = false;
    }

    void ClearPathToTarget()
    {
        m_pathToTarget.Clear();
    }

    private void Update()
    {
        
    }

    public void NavigateToTargetPosition()
    {
        if(!m_hasNewPathStarted){
            MoveToStartingNode();
        }
        else if (m_currentNodeIndex != m_targetNode)
        {
            if (m_isInANode)
            {
                SetTargetNode(m_targetPosition);
                SearchTarget();
            }
            else { MoveToNextNode(); }
        }
    }

    void MoveToStartingNode(){
        if((m_graph.GetNode(m_initialNode).Position - transform.position).magnitude <= 1.0f){
            transform.position = m_graph.GetNode(m_initialNode).Position;
            m_hasNewPathStarted = true;
            m_currentNodeIndex = m_initialNode;
            m_isInANode = true;
            return;
        }
        Vector3 direction = new Vector3 (m_graph.GetNode(m_initialNode).Position.x - transform.position.x, m_graph.GetNode(m_initialNode).Position.y - transform.position.y, 0.0f);
        direction = direction.normalized;
        Debug.Log(Time.deltaTime);
        transform.position += direction * m_speed * Time.deltaTime;
    }

    void MoveToNextNode()
    {
        if (!m_graphSearch.IsTargetFound) { return; }
        Vector3 direction = (m_graph.GetNode(m_pathToTarget[m_currentNodeIndex]).Position - m_graph.GetNode(m_currentNodeIndex).Position).normalized;

        transform.position += m_speed * direction * Time.deltaTime;

        if((m_graph.GetNode(m_currentNodeIndex).Position - transform.position).magnitude > (m_graph.GetNode(m_currentNodeIndex).Position - m_graph.GetNode(m_pathToTarget[m_currentNodeIndex]).Position).magnitude)
        {
            transform.position = m_graph.GetNode(m_pathToTarget[m_currentNodeIndex]).Position;
            m_currentNodeIndex = m_pathToTarget[m_currentNodeIndex];
            m_pathToTarget.Clear();
            m_isInANode = true;
            m_initialNode = m_currentNodeIndex;
            //Debug.Log(m_currentNodeIndex);
        }
    }

    public void SetTargetNode(Vector2 p_target)
    {
        m_targetPosition = p_target;
        bool hasAnActiveNodeBeenFound = false;

        for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if (m_graph.GetNode(i).IsNearEnoughThisNode(5, m_targetPosition))
            {
                if (m_graph.GetNode(i).IsActive) {
                    m_targetNode = i;
                    hasAnActiveNodeBeenFound = true;
                }
                
            }
        }

        if (hasAnActiveNodeBeenFound) { return; }

        for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if (m_graph.GetNode(i).IsNearEnoughThisNode(15, m_targetPosition))
            {
                if (m_graph.GetNode(i).IsActive)
                {
                    m_targetNode = i;
                    hasAnActiveNodeBeenFound = true;
                }

            }
        }

    }

    public void SnapToCurrentNode()
    {
        transform.position = m_graph.GetNode(m_currentNodeIndex).Position;
    }

    public void SnapToClosestNode()
    {
        bool hasAnActiveNodeBeenFound = false;

        for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if (m_graph.GetNode(i).IsNearEnoughThisNode(5, m_targetPosition))
            {
                if (m_graph.GetNode(i).IsActive)
                {
                    m_currentNodeIndex = i;
                    hasAnActiveNodeBeenFound = true;
                }

            }
        }

        if (hasAnActiveNodeBeenFound) { return; }

        for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if (m_graph.GetNode(i).IsNearEnoughThisNode(15, m_targetPosition))
            {
                if (m_graph.GetNode(i).IsActive)
                {
                    m_currentNodeIndex = i;
                    hasAnActiveNodeBeenFound = true;
                }

            }
        }
    }

    public void SetInitialNodeToNone(){
        m_currentNodeIndex = -1;
        m_hasNewPathStarted = false;
        m_isInANode = false;
    }    

    public void SetInitialNode(Vector2 p_origin)
    {
        m_initialPosition = p_origin;
        bool hasAnActiveNodeBeenFound = false;

        for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if (m_graph.GetNode(i).IsNearEnoughThisNode(5, m_initialPosition))
            {
                if (m_graph.GetNode(i).IsActive)
                {
                    m_initialNode = i;
                    hasAnActiveNodeBeenFound = true;
                }

            }
        }
        
        //transform.position = m_graph.GetNode(m_initialNode).Position;
        //m_currentNodeIndex = m_initialNode;
        //m_isInANode = true;

        if (hasAnActiveNodeBeenFound) { return; }

        for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if (m_graph.GetNode(i).IsNearEnoughThisNode(15, m_initialPosition))
            {
                if (m_graph.GetNode(i).IsActive)
                {
                    m_initialNode = i;
                    hasAnActiveNodeBeenFound = true;
                }

            }
        }

        //transform.position = m_graph.GetNode(m_initialNode).Position;
        //m_currentNodeIndex = m_initialNode;
        //m_isInANode = true;
    }

    public void SetSpeed(float p_speed){
        m_speed = p_speed;
    }

    public bool IsFinished()
    {
        return m_currentNodeIndex == m_targetNode;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Vector3 initialPosition = m_startingPoint.position;
        /*for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if (i == m_initialNode) { Gizmos.color = Color.green; }
            else if (i == m_targetNode) { Gizmos.color = Color.red; }
            else if (!m_graph.GetNode(i).IsActive) { Gizmos.color = Color.yellow; }
            else { Gizmos.color = Color.black; }

            Gizmos.DrawSphere(new Vector3(m_graph.GetNode(i).Position.x, m_graph.GetNode(i).Position.y, m_graph.GetNode(i).Position.z), 1f);
        }*/

        if (!m_graphSearch.IsTargetFound) { return; }
        List<int> pathToTarget = m_graphSearch.GetPathToTarget();
        for (int i = 0; i < pathToTarget.Count - 1; i++)
        {
            Gizmos.DrawLine(m_graph.GetNode(pathToTarget[i]).Position, m_graph.GetNode(pathToTarget[i + 1]).Position);
        }
    }

}
