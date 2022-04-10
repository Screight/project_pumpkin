using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GraphEdge {
    protected int m_originNode;
    protected int m_destinationNode;

    float m_cost;

    public GraphEdge(int p_originNode, int p_destinationNode, float p_cost)
    {
        m_originNode = p_originNode;
        m_destinationNode = p_destinationNode;
        m_cost = p_cost;
    }

    public GraphEdge(int p_originNode, int p_destinationNode)
    {
        m_originNode = p_originNode;
        m_destinationNode = p_destinationNode;
        m_cost = 1.0f;
    }

    public GraphEdge()
    {
        m_originNode = int.MinValue;
        m_destinationNode = int.MinValue;
        m_cost = 1.0f;
    }

    public int OriginNode
    {
        set { m_originNode = value; }
        get {  return m_originNode; }
    }

    public int DestinationNode
    {
        set { m_destinationNode = value; }
        get { return m_destinationNode; }
    }

    public float Cost
    {
        get { return m_cost;}
        set { m_cost = value; }
    }

}

/*f you are working on a platform where memory use is a much greater concern than the speed of searching a graph, you can get good savings on
cell-based graphs (or graphs of equal or greater density) by not explicitly
storing the cost of each edge. Instead, you can save memory by omitting
the cost field from the GraphEdge class and calculate the cost “on-the-fly”
using a function of attributes of its two adjacent nodes.
Because there are usually eight times more edges than vertices in this type
of graph, the memory savings can be considerable when large numbers of
nodes are involved.*/