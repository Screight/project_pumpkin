using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    Vector2 m_initialPosition;
    Vector2 m_targetPosition;
    [SerializeField] int m_initialNode;
    [SerializeField] int m_targetNode;
    bool m_isInANode;
    bool m_hasNewPathStarted = true;
    SparseGraph m_graph;
    Graph_SearchDijkstra m_graphSearch;
    [SerializeField] RoomGraph m_roomGraphScript;
    float m_speed = 10.0f;
    int m_currentNodeIndex;
    List<int> pathToTarget;
    Dictionary<int, int> m_pathToTarget;
    Vector2 m_direction;

    private void Awake() { m_pathToTarget = new Dictionary<int, int>(); }

    private void Start()
    {
        m_graph = m_roomGraphScript.Graph;
        m_isInANode = true;
        for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if (m_graph.GetNode(i).IsNearEnoughThisNode(5, m_initialPosition)) { m_initialNode = i; }
            if (m_graph.GetNode(i).IsNearEnoughThisNode(16, m_targetPosition)) { m_targetNode = i; }
        }
    }

    void SearchTarget()
    {
        m_currentNodeIndex = m_initialNode;
        transform.position = m_graph.GetNode(m_initialNode).Position;

        m_graphSearch = new Graph_SearchDijkstra(m_graph, m_initialNode, m_targetNode);
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

    void ClearPathToTarget() { m_pathToTarget.Clear(); }

    public void NavigateToTargetPosition()
    {
        if (!m_hasNewPathStarted) { MoveToStartingNode(); }
        else if (m_currentNodeIndex != m_targetNode)
        {
            if (m_isInANode)
            {
                SetTargetNode(m_targetPosition);
                SearchTarget();
            }
            MoveToNextNode();
        }
    }

    void MoveToStartingNode(){
        if ((m_graph.GetNode(m_initialNode).Position - transform.position).magnitude <= 1.0f)
        {
            transform.position = m_graph.GetNode(m_initialNode).Position;
            m_hasNewPathStarted = true;
            m_currentNodeIndex = m_initialNode;
            m_isInANode = true;
            return;
        }
        Vector3 direction = new Vector3 (m_graph.GetNode(m_initialNode).Position.x - transform.position.x, m_graph.GetNode(m_initialNode).Position.y - transform.position.y, 0.0f);

        direction = direction.normalized;
        transform.position += direction * m_speed * Time.deltaTime;

        float directionX = direction.x;
        if(directionX != 0) { directionX = directionX / Mathf.Abs(directionX);}

        float directionY = direction.y;
        if(directionY != 0) { directionY = directionY / Mathf.Abs(directionY);}

        m_direction = new Vector2(directionX, directionY);
    }

    void MoveToNextNode()
    {
        if (!m_graphSearch.IsTargetFound) { return; }
        Vector3 direction = (m_graph.GetNode(m_pathToTarget[m_currentNodeIndex]).Position - m_graph.GetNode(m_currentNodeIndex).Position).normalized;

        transform.position += m_speed * direction * Time.deltaTime;
        float directionX = direction.x;
        if(directionX != 0) { directionX = directionX / Mathf.Abs(directionX);}
        float directionY = direction.y;
        if(directionY != 0) { directionY = directionY / Mathf.Abs(directionY);}

        m_direction = new Vector2(directionX, directionY);

        if ((m_graph.GetNode(m_currentNodeIndex).Position - transform.position).magnitude > (m_graph.GetNode(m_currentNodeIndex).Position - m_graph.GetNode(m_pathToTarget[m_currentNodeIndex]).Position).magnitude)
        {
            transform.position = m_graph.GetNode(m_pathToTarget[m_currentNodeIndex]).Position;
            m_currentNodeIndex = m_pathToTarget[m_currentNodeIndex];
            m_pathToTarget.Clear();
            m_isInANode = true;
            m_initialNode = m_currentNodeIndex;
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

    public void SnapToCurrentNode() { transform.position = m_graph.GetNode(m_currentNodeIndex).Position; }

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

    public void SetInitialNodeToNone()
    {
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
    }

    public NavigationGraphNode GetTargetNode() { return m_graph.GetNode(m_targetNode); }
    public void SetSpeed(float p_speed) { m_speed = p_speed; }
    public Vector2 GetDirection() { return m_direction; }
    public bool IsFinished() { return m_currentNodeIndex == m_targetNode; }

    /*private void OnDrawGizmos()
    {
        // DRAW CURRENT PATH
        if (!m_graphSearch.IsTargetFound) { return; }

        List<int> pathToTarget = m_graphSearch.GetPathToTarget();
        for (int i = 0; i < pathToTarget.Count - 1; i++)
        {
            Gizmos.DrawLine(m_graph.GetNode(pathToTarget[i]).Position, m_graph.GetNode(pathToTarget[i + 1]).Position);
        }
        Gizmos.color = Color.black;
        // DRAW BEGINNING AND END OF THE PATH
        for (int i = 0; i < m_graph.NumberOfNodes(); i++)
        {
            if (m_graph.GetNode(i).Index == m_initialNode)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(new Vector3(m_graph.GetNode(i).Position.x, m_graph.GetNode(i).Position.y, m_graph.GetNode(i).Position.z), 1f);
            }
            else if (m_graph.GetNode(i).Index == m_targetNode)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(new Vector3(m_graph.GetNode(i).Position.x, m_graph.GetNode(i).Position.y, m_graph.GetNode(i).Position.z), 1f);
            }
        }
    }
    */
}