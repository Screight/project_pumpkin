using System;
using System.Collections.Generic;
using UnityEngine;
public class SparseGraph
{
    private List<NavigationGraphNode> m_nodes; // nodes that form this graph
    private List<List<GraphEdge>> m_edges; // each node index keys into the list of edges associated with that node

    private bool m_isDiGraph; 
    private int m_nextNodeIndex; // the index of the next node to be added

    public SparseGraph(bool p_diGraph)
    {
        m_nextNodeIndex = 0;
        m_isDiGraph = p_diGraph;
        m_nodes = new List<NavigationGraphNode>();
        m_edges = new List<List<GraphEdge>>();
    }

    public  NavigationGraphNode GetNode(int p_index) {
        if(m_nodes[p_index].Index != int.MinValue) {
            return m_nodes[p_index];
        }
        return null;
    }
    public int GetNextFreeNodeIndex() { return m_nextNodeIndex; }
    public int AddNode(NavigationGraphNode p_node) { 
        m_nodes.Add(p_node);
        m_edges.Add(new List<GraphEdge>());
        return m_nextNodeIndex++;
    }
    public void RemoveNode(int p_node)
    {
        if(p_node < m_nextNodeIndex && p_node > 0) { m_nodes[p_node].Index = int.MinValue;
            m_edges[p_node] = null;
        }
    }

    public void AddEdge(GraphEdge p_edge)
    {
        if (m_nodes[p_edge.OriginNode].Index != int.MinValue && m_nodes[p_edge.DestinationNode].Index != int.MinValue)
        {
            m_edges[p_edge.OriginNode].Add(p_edge);
        }
    }

    public List<GraphEdge> GetEdges(int p_index)
    {
        if(p_index >= 0 && p_index < m_nodes.Count)
        {
            return m_edges[p_index];
        }

        return null;
    }

    public void RemoveEdge(int p_originNode, int p_destinationNode)
    {
        for(int i = 0; i < m_edges[p_originNode].Count; i++)
        {
            if(m_edges[p_originNode][i].DestinationNode == p_destinationNode)
            {
                m_edges[p_originNode].RemoveAt(i);
            }
        }
    }

    public int NumberOfNodes()
    {
        return m_nodes.Count;
    }

    public int NumberOfActiveNodes()
    {
        int activeNodes = 0;
        for(int i = 0; i < m_nodes.Count; i++)
        {
            if(m_nodes[i].Index != int.MinValue) { activeNodes++; }
        }
        return activeNodes;
    }

    public int NumberOfEdges()
    {
        int activeEdges = 0;
        for (int i = 0; i < m_edges.Count; i++)
        {
            activeEdges += m_edges[i].Count;
        }
        return activeEdges;
    }

    public bool IsDiGraph() { return m_isDiGraph; }
    public bool IsEmpty()
    {
        if(m_nodes.Count == 0) { return true; }
        return false;
    }

    public bool IsNodePresent(int p_index)
    {
        if(m_nodes[p_index].Index != int.MinValue) { return true; }
        return false;
    }

    public void Clear()
    {
        for(int i = 0; i < m_nodes.Count; i++) { m_nodes.Clear(); }
        for(int i = 0; i < m_edges.Count; i++) { m_edges.Clear(); }
    }

    public List<int> GetAllActiveNodesIndex(){
        List<int> activeNodesIndex = new List<int>();
        for(int i = 0; i < m_nodes.Count; i++){
            if(m_nodes[i].IsActive){
                activeNodesIndex.Add(m_nodes[i].Index);
            }
        }
        return activeNodesIndex;
    }

}