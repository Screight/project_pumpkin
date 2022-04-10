using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingIndexedPriorityQueue : MonoBehaviour
{
    IndexedPriorityQueueFloat m_pq;
    List<float> m_cost;
    List<int> m_index;
    private void Awake() {
        SparseGraph graph = new SparseGraph(true);
        graph.AddNode(new NavigationGraphNode(0,new Vector3(0,0,0)));
        graph.AddNode(new NavigationGraphNode(1,new Vector3(1,0,0)));
        graph.AddNode(new NavigationGraphNode(2,new Vector3(2,0,0)));
        graph.AddNode(new NavigationGraphNode(3,new Vector3(0,1,0)));
        graph.AddNode(new NavigationGraphNode(4,new Vector3(1,1,0)));
        graph.AddNode(new NavigationGraphNode(5,new Vector3(2,1,0)));
        graph.AddEdge(new GraphEdge(0,1,1));

        graph.AddEdge(new GraphEdge(0,3,1));
        graph.AddEdge(new GraphEdge(3,0,1));
        graph.AddEdge(new GraphEdge(0,4,1.4f));
        graph.AddEdge(new GraphEdge(4,0,1.4f));
        graph.AddEdge(new GraphEdge(1,3,1.4f));
        graph.AddEdge(new GraphEdge(3,1,1.4f));
        graph.AddEdge(new GraphEdge(0,1,1.4f));
        graph.AddEdge(new GraphEdge(1,0,1));
        graph.AddEdge(new GraphEdge(1,4,1));
        graph.AddEdge(new GraphEdge(4,1,1));
        graph.AddEdge(new GraphEdge(3,4,1));
        graph.AddEdge(new GraphEdge(4,3,1));
        graph.AddEdge(new GraphEdge(1,2,1));
        graph.AddEdge(new GraphEdge(2,1,1));
        graph.AddEdge(new GraphEdge(1,5,1.4f));
        graph.AddEdge(new GraphEdge(5,1,1.4f));
        graph.AddEdge(new GraphEdge(4,5,1));
        graph.AddEdge(new GraphEdge(5,4,1));
        graph.AddEdge(new GraphEdge(2,5,1));
        graph.AddEdge(new GraphEdge(5,2,1));
        graph.AddEdge(new GraphEdge(2,4,1.4f));
        graph.AddEdge(new GraphEdge(4,2,1.4f));
        GraphSearchBFS search = new GraphSearchBFS(graph, 2,0);
        if(!search.IsTargetFound){
            Debug.Log("TARGET NOT FOUND");
            return;    
        }
        List<int> route = search.GetPathToTarget();
        for(int i = 0; i < route.Count; i++){
            Debug.Log(route[i]);
        }

        Graph_SearchDijkstra search_2 = new Graph_SearchDijkstra(graph,2,0);
        
        Debug.Log("--------------------");

        List<int> route_2 = search_2.GetPathToTarget();
        for(int i = 0; i < route.Count; i++){
            Debug.Log(route[i]);
        }

    }
}
