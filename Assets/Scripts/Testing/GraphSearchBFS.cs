using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GraphSearchBFS
{
    enum node_status { visited, unvisited, no_parent_assigned = -1 }

    SparseGraph m_graph;
    List<int> m_visitedNodes;
    List<int> m_route;

    int m_sourceIndex;
    int m_targetIndex;
    bool m_isTargetFound;

    public GraphSearchBFS(SparseGraph p_graph, int p_sourceIndex, int p_targetIndex)
    {
        m_visitedNodes = new List<int>();
        m_route = new List<int>();

        m_sourceIndex = p_sourceIndex;
        m_targetIndex = p_targetIndex;
        m_graph = p_graph;

        for (int i = 0; i < p_graph.NumberOfNodes(); i++)
        {
            m_visitedNodes.Add((int)node_status.unvisited);
            m_route.Add((int)node_status.no_parent_assigned);
        }

        m_isTargetFound = Search(p_sourceIndex, p_targetIndex);
    }

    public GraphSearchBFS(SparseGraph p_graph)
    {
        m_visitedNodes = new List<int>();
        m_route = new List<int>();

        m_graph = p_graph;

        for (int i = 0; i < p_graph.NumberOfNodes(); i++)
        {
            m_visitedNodes.Add((int)node_status.unvisited);
            m_route.Add((int)node_status.no_parent_assigned);
        }

        m_isTargetFound = Search(m_sourceIndex, m_targetIndex);

    }

    public List<int> GetPathToTarget()
    {
        int currentNodeIndex = m_targetIndex;
        List<int> pathToTarget = new List<int>();
        pathToTarget.Add(m_targetIndex);
        while (currentNodeIndex != m_sourceIndex)
        {
            pathToTarget.Add(m_route[currentNodeIndex]);
            currentNodeIndex = m_route[currentNodeIndex];
        }
        return pathToTarget;
    }

    public bool Search(int p_sourceIndex, int p_targetIndex)
    {
        Queue<GraphEdge> queue = new Queue<GraphEdge>();
        GraphEdge dummy = new GraphEdge(m_sourceIndex, m_sourceIndex, 0);
        List<GraphEdge> edges = new List<GraphEdge>();

        queue.Enqueue(dummy);

        m_visitedNodes[m_sourceIndex] = (int)node_status.visited;

        while (queue.Count > 0)
        {
            GraphEdge nextEdge = queue.Dequeue();
            m_route[nextEdge.DestinationNode] = nextEdge.OriginNode;
            m_visitedNodes[nextEdge.DestinationNode] = (int)node_status.visited;

            if (nextEdge.DestinationNode == m_targetIndex) { return true; }

            edges = m_graph.GetEdges(nextEdge.DestinationNode);
            int size = edges.Count;

            for (int i = 0; i < size; i++)
            {
                if (m_visitedNodes[edges[i].DestinationNode] == (int)node_status.unvisited && m_graph.GetNode(edges[i].DestinationNode).IsActive)
                {
                    queue.Enqueue(edges[i]);
                    m_visitedNodes[edges[i].DestinationNode] = (int)node_status.visited;
                }
            }
        }

        return false;

    }

    public bool IsTargetFound
    {
        get { return m_isTargetFound; }
        set { m_isTargetFound = value; }
    }

}
