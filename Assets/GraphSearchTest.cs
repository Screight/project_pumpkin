using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSearchTest : MonoBehaviour
{
    SparseGraph graph;

    private void Awake()
    {
        graph = new SparseGraph(true);

        for (int i = 0; i < 13; i++)
        {
            NavigationGraphNode node = new NavigationGraphNode(i,Vector2.one);
            graph.AddNode(node);
        }

		graph.AddEdge(new GraphEdge(1, 0));
		graph.AddEdge(new GraphEdge(0, 1));
		graph.AddEdge(new GraphEdge(0, 2));
		graph.AddEdge(new GraphEdge(2, 0));
		graph.AddEdge(new GraphEdge(2, 9));
		graph.AddEdge(new GraphEdge(9, 2));
		graph.AddEdge(new GraphEdge(9, 3));
		graph.AddEdge(new GraphEdge(3, 9));
		graph.AddEdge(new GraphEdge(0, 8));
		graph.AddEdge(new GraphEdge(8, 0));
		graph.AddEdge(new GraphEdge(8, 12));
		graph.AddEdge(new GraphEdge(12, 8));
		graph.AddEdge(new GraphEdge(12, 7));
		graph.AddEdge(new GraphEdge(7, 12));
		graph.AddEdge(new GraphEdge(3, 11));
		graph.AddEdge(new GraphEdge(11, 3));
		graph.AddEdge(new GraphEdge(11, 6));
		graph.AddEdge(new GraphEdge(6, 11));
		graph.AddEdge(new GraphEdge(6, 4));
		graph.AddEdge(new GraphEdge(8, 3));
		graph.AddEdge(new GraphEdge(3, 8));
		graph.AddEdge(new GraphEdge(4, 6));
		graph.AddEdge(new GraphEdge(4, 10));
		graph.AddEdge(new GraphEdge(10, 4));
		graph.AddEdge(new GraphEdge(5, 4));
		graph.AddEdge(new GraphEdge(4, 5));

		GraphSearchBFS graphSearch = new GraphSearchBFS(graph, 5, 12);

		if (graphSearch.IsTargetFound)
		{
			List<int> pathToTarget = graphSearch.GetPathToTarget();
			for (int i = 0; i < pathToTarget.Count; i++)
			{
				Debug.Log(pathToTarget[i]);
			}
		}

	}
}
