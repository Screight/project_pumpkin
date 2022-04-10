using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph_SearchDijkstra
{
    SparseGraph m_graph;
    // contains the edges that comprise the shortest path tree
    // A directed sub-tree of the graph that encapsulates the best paths from every node on the SPT to the source node.
    List<int> m_route;

    // this is indexed into by node index and holds the total cost of the best path found so far to the given node.
    List<float> m_costToThisNode;

    // this is an indexed (by note) vector of "parent" edges leadgint to nodes connected to the SPT but that have not been added to the SPT yet.
    List<GraphEdge> m_searchFrontier;

    int m_sourceIndex;
    int m_targetIndex;
    bool m_isTargetFound = false;


    public Graph_SearchDijkstra(SparseGraph p_graph, int p_sourceIndex, int p_targetIndex){
        m_graph = p_graph;
        m_sourceIndex = p_sourceIndex;
        m_targetIndex = p_targetIndex;

        m_route = new List<int>(new int[p_graph.NumberOfNodes()]);

        for(int i  = 0; i < m_route.Count; i++){
            m_route[i] = -1;
        }

        m_costToThisNode = new List<float>(new float[p_graph.NumberOfNodes()]);
        for(int i  = 0; i < m_costToThisNode.Count; i++){
            m_costToThisNode[i] = float.MaxValue;
        }
        m_searchFrontier = new List<GraphEdge>(new GraphEdge[p_graph.NumberOfNodes()]);

        m_isTargetFound = Search();

    }

    bool Search(){
        // create and indexed priority queue that sorts smallest to largest (front to back). The maximum number of elements the iPQ is the number of nodes since every node is represented only once

        IndexedPriorityQueueFloat priorityQueue = new IndexedPriorityQueueFloat(m_graph.NumberOfActiveNodes());

        // put the source node on the queue
        priorityQueue.Insert(0,m_graph.GetNode(m_sourceIndex).Index);
        m_route[m_sourceIndex] = m_sourceIndex;
        m_costToThisNode[m_sourceIndex] = 0;
        //m_route[m_graph.GetNode(m_sourceIndex).Index] =  GraphEdge(m_graph.GetNode(m_sourceIndex).Index, m_graph.GetNode(m_sourceIndex).Index,0);
        
        while(!priorityQueue.IsEmpty()){
            // get the lowest cost node from the queue. Don't forget, the return value is a *node index*, not the node itself. This node is the node not already on the SPT that is the closes to the source node
            int currentNode = priorityQueue.TakeNextItem();

            // move this edge from the search frontier to the shortest path tree
            //m_route[nextClosestNode] = m_searchFrontier[nextClosestNode];

            if(currentNode == m_targetIndex){
                return true;
                }

            // now to relax the edges. For each edge connected to the next closes node
            List<GraphEdge> connectedEdges = m_graph.GetEdges(currentNode);
            for(int i = 0; i < connectedEdges.Count; i++){
                GraphEdge currentEdge = connectedEdges[i];
                // the total cost to the node this edges points to is the cost to the current node plus the cost of the edge connecting them.
                float newCost = m_costToThisNode[currentNode] + currentEdge.Cost;

                // if this edge has never been on the frontier make a note of the cost to reach the node it points to, then add the edge to the frontier and the destination node to the PQ.
                if(m_route[currentEdge.DestinationNode] == -1){
                    m_costToThisNode[currentEdge.DestinationNode] = newCost;
                    priorityQueue.Insert(m_costToThisNode[currentEdge.DestinationNode], currentEdge.DestinationNode);
                    //m_searchFrontier[currentEdge.DestinationNode] = currentEdge;
                    m_route[currentEdge.DestinationNode] = currentNode;
                }

                // else test to see if the cost to reach the destination nnode via the current node is cheaper than the cheapest cost found so far. If this path is cheaper we assign the new cost to the destination nodem update its entry in the PQ to reflect the change, and add the edge to the frontier
                else if((newCost < m_costToThisNode[currentEdge.DestinationNode])){
                    m_costToThisNode[currentEdge.DestinationNode] = newCost;
                    // because the cost is less than it was previously, the PQ must be resorted to account for this
                    priorityQueue.ChangeValue(currentEdge.DestinationNode,newCost);

                    m_route[currentEdge.DestinationNode] = currentNode;
                }

            }

        }
        return false;
    }

    // returns a vector of node indexes comprimising the shortest path from the source to the target. It calculates the path by working backward through the SPT from the target node
    public List<int> GetPathToTarget()
    {
        List<int> pathToTarget = new List<int>();
        int currentNode = m_targetIndex;
        while(currentNode != m_sourceIndex){
            pathToTarget.Add(currentNode);
            currentNode = m_route[currentNode];
        }
        pathToTarget.Add(currentNode);
        return pathToTarget;
    }

    public bool IsTargetFound { get { return m_isTargetFound; }}

}
