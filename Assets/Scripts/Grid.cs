using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace YYGAStar
{
	//Grid(経路が探索られることができるノードを作成したり、保存(格納かくのう)したり)
	//ノードが格納される場所(ばしょ)
	public class Grid : MonoBehaviour {
		
		public float edgeLength = 1.0f;
		public Vector3 startPos = new Vector3(0.5f,0,0.5f);
		public int groundLayer = 9;
		public int xCount;
		public int yCount;
		public Node[,] nodes;
		public List<Node> allNodes;
		public bool showGizmos = true;
		public bool recalculateBlocks = true;
#if UNITY_EDITOR
		public int x0;
		public int y0;
#endif

		void Start () {
			Init();
			CalculateBlockNode();
		}

		void Update(){
			if (recalculateBlocks) {
				CalculateBlockNode ();
				recalculateBlocks = false;
			}
		}

		public void Clear(){
			foreach(Node node in allNodes)
			{
				node.previous = null;
			}	
		}

		void Init()
		{
			xCount = Mathf.Max (1,xCount);
			yCount = Mathf.Max (1,yCount);
			nodes = new Node[xCount,yCount];
			allNodes = new List<Node>();
			//start at (0,0,0),end at (xCount,0,yCount);
			//TODO 今は四方面である、他のneighbor方法は六方面と八方面
			for(int j=0;j < yCount;j++)
			{
				for(int i=0;i < xCount;i++)
				{
					Node node = new Node();
					node.x = i;
					node.y = j;
					node.pos = new Vector3(i * edgeLength,0,j * edgeLength) + startPos;
					nodes[i,j] = node;
					Debug.Log("i=" + i + ";j=" + j);
					if(i > 0)
					{
						nodes[i-1,j].neighbors.Add(node);
						nodes[i-1,j].consumes.Add(edgeLength);
						node.neighbors.Add(nodes[i-1,j]);
						node.consumes.Add(edgeLength);
					}
					if(j > 0)
					{
						nodes[i,j-1].neighbors.Add(node);
						nodes[i,j-1].consumes.Add(edgeLength);
						node.neighbors.Add(nodes[i,j-1]);
						node.consumes.Add(edgeLength);
					}
					if(i > 0 && j > 0)
					{
						nodes[i-1,j-1].neighbors.Add(node);
						nodes[i-1,j-1].consumes.Add(edgeLength * 1.4f);
						node.neighbors.Add(nodes[i-1,j-1]);
						node.consumes.Add(edgeLength * 1.4f);
					}
					if(i < xCount-1 && j > 0)
					{
						nodes[i+1,j-1].neighbors.Add(node);
						nodes[i+1,j-1].consumes.Add(edgeLength * 1.4f);
						node.neighbors.Add(nodes[i+1,j-1]);
						node.consumes.Add(edgeLength * 1.4f);
					}
					allNodes.Add(node);
				}
			}
		}

		public void CalculateBlockNode()
		{
			for(int i=0;i<allNodes.Count;i++)
			{
				Collider[] cols = Physics.OverlapSphere(allNodes[i].pos,edgeLength/2,1<<groundLayer);
				if(cols.Length>0)
				{
					allNodes[i].isBlock = true;
					for(int j=0;j<allNodes[i].neighbors.Count;j++){
						allNodes [i].neighbors [j].isWallSide = true;
					}
				}
			}
		}

		void OnDrawGizmos()
		{
			if(showGizmos && allNodes!=null)
			{
				for(int i=0;i < allNodes.Count;i++)
				{
					if(!allNodes[i].isBlock)
					{
						Gizmos.color = Color.blue;
					}
					else
					{
						Gizmos.color = Color.red;
					}
					Gizmos.DrawWireCube(allNodes[i].pos,Vector3.one * 0.1f);
				}

				#if UNITY_EDITOR
				if(x0 < xCount && x0 >=0 && y0 < yCount && y0 >=0)
				{
					Node node = nodes[x0,y0];
					if(node.neighbors!=null)
					{
						for(int i =0;i<node.neighbors.Count;i++)
						{
							Gizmos.color = Color.green;
							Gizmos.DrawWireCube(node.neighbors[i].pos,Vector3.one * 0.1f);
						}
					}
				}
				#endif

			}
		}
	}

	public class Node
	{
		public int index;
		public int x;//ノードの位置
		public int y;//ノードの位置
		public float G;//スタート distance.
		public float H;//Manhattan distance  曼哈顿算法
		public float F;
		public List<Node> neighbors = new List<Node>();//このノードに接続(せつぞく)するノード。
		public List<float> consumes = new List<float>();//このノードから接続ノードまで、移動消費(いどうしょうひ)コスト。
		public bool isBlock;//壁中にいるのかどうか
		public bool isWallSide;//壁に向かってるのかどうか
		public Vector3 pos;//ゲーム世界のポジション。
		public Node previous;//
	}
}
