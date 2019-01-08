using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BlueNoah.PathFinding
{
	//Grid(経路が探索られることができるノードを作成したり、保存(格納かくのう)したり)
	//ノードが格納される場所(ばしょ)
	public class Grid : MonoBehaviour
	{
		
		public float edgeLength = 1.0f;
		public float anglePlus = 1.4f;
		public Vector3 startPos = new Vector3 (0.5f, 0, 0.5f);
		public const int blockLayer = 9;
		public const int groundLayer = 8;
		public int xCount;
		public int yCount;
		public Node[,] nodes;
		public List<Node> allNodes;
		public bool recalculateBlocks = true;
		#if UNITY_EDITOR
		public int x0;
		public int y0;
		#endif
		public static Grid current;
		public bool showGizmos = true;

		void Awake ()
		{
			current = this;
			Init ();
		}

		void Update ()
		{
			if (recalculateBlocks) {
				CalculateBlockNode ();
				CalculateNeighbors ();
				recalculateBlocks = false;
			}
		}

		public void Reset ()
		{
			for (int i = 0; i < allNodes.Count; i++) {// Node node in allNodes)
				allNodes [i].previous = null;
			}	
		}

		void Init ()
		{
			xCount = Mathf.Max (1, xCount);
			yCount = Mathf.Max (1, yCount);
			nodes = new Node[xCount, yCount];
			allNodes = new List<Node> ();
			//start at (0,0,0),end at (xCount,0,yCount);
			//今は８方面である
			for (int j = 0; j < yCount; j++) {
				for (int i = 0; i < xCount; i++) {
					Node node = new Node ();
					node.x = i;
					node.y = j;
					node.pos = new Vector3 (i * edgeLength, 0, j * edgeLength) + startPos;
					nodes [i, j] = node;
					allNodes.Add (node);
				}
			}
			CalculateBlockNode ();
			CalculateNeighbors ();
		}

		public void CalculateBlockNode ()
		{
			for (int i = 0; i < allNodes.Count; i++) {
				Collider[] cols = Physics.OverlapSphere (allNodes [i].pos, edgeLength / 4, 1 << blockLayer);
				if (cols.Length > 0) {
					allNodes [i].isBlock = true;
					for (int j = 0; j < allNodes [i].neighbors.Count; j++) {
						allNodes [i].neighbors [j].isWallSide = true;
					}
				}
			}
		}

		public void CalculateNeighbors ()
		{
			for (int i = 0; i < allNodes.Count; i++) {
				allNodes [i].neighbors.Clear ();
				allNodes [i].consumes.Clear ();
			}
			for (int j = 0; j < yCount; j++) {
				for (int i = 0; i < xCount; i++) {
					Node node = nodes [i, j];
					if (node.isBlock) {
						continue;
					}
					CalculateNeighbor (node);
				}
			}
		}

		public void CalculateNeighbor(Node node){

			bool north = false;
			bool south = false;
			bool east = false;
			bool west = false;
			node.neighbors.Clear ();
			node.consumes.Clear ();
			int i = node.x;
			int j = node.y;
			if (i > 0) {
				if (!nodes [i - 1, j].isBlock) {
					node.neighbors.Add (nodes [i - 1, j]);
					node.consumes.Add (edgeLength);
					west = true;
				} 
			}
			if (j > 0) {
				if (!nodes [i, j - 1].isBlock) {
					node.neighbors.Add (nodes [i, j - 1]);
					node.consumes.Add (edgeLength);
					south = true;
				}
			}
			if (i < xCount - 1) {
				if (!nodes [i + 1, j].isBlock) {
					node.neighbors.Add (nodes [i + 1, j]);
					node.consumes.Add (edgeLength );
					east = true;
				}
			}
			if (j < yCount - 1 ) {
				if (!nodes [i, j + 1].isBlock) {
					node.neighbors.Add (nodes [i , j + 1]);
					node.consumes.Add (edgeLength );
					north = true;
				}
			}

			if(i > 0 && j > 0){
				if (!nodes [i - 1, j - 1].isBlock && west && south) {
					node.neighbors.Add (nodes [i - 1, j - 1]);
					node.consumes.Add (edgeLength * anglePlus);
				}
			}
			if(i > 0 && j < yCount - 1 ){
				if (!nodes [i - 1, j + 1].isBlock && west && north) {
					node.neighbors.Add (nodes [i - 1, j + 1]);
					node.consumes.Add (edgeLength * anglePlus);
				}
			}
			if(i < xCount - 1 && j > 0){
				if (!nodes [i + 1, j - 1].isBlock && east && south) {
					node.neighbors.Add (nodes [i + 1, j - 1]);
					node.consumes.Add (edgeLength * anglePlus);
				}
			}
			if(i < xCount - 1 && j < yCount - 1){
				if (!nodes [i + 1, j + 1].isBlock && east && south) {
					node.neighbors.Add (nodes [i + 1, j + 1]);
					node.consumes.Add (edgeLength * anglePlus);
				}
			}
		}

		public Node GetNode (Vector3 pos)
		{
			int xIndex = Mathf.RoundToInt ((pos.x - startPos.x) / edgeLength);
			int yIndex = Mathf.RoundToInt ((pos.z - startPos.z) / edgeLength);
			Node node = nodes [xIndex, yIndex];
			return node;
		}


		int mSearchIndex = 0;
		public List<Node> GetNearest(Node start,int count){
			mSearchIndex++;
			List<Node> openList = new List<Node> ();
			openList.Add (start);
			start.isNearest = mSearchIndex;
			int j = 0;
			while(openList.Count <= count && openList.Count > j){
				Node node = openList [j];
				for (int i = 0; i < node.neighbors.Count; i++) {
					if (node.neighbors [i].isNearest != mSearchIndex && !node.neighbors [i].isBlock) {
						if (openList.Count <= count) {
							openList.Add (node.neighbors [i]);
							node.neighbors [i].isNearest = mSearchIndex;
						}
					}
				}
				j++;
			}
			openList.RemoveAt (0);
			return openList;
		}

		//これはコストは一番少ない(TODO 直すが必要です)
		//ノードをチェクする方法
		//startNodeからendNodeまでのxのint値段。例えば(0.5,4.5) to (2.5,0.5) 経路のx 1,2 経路のy 4,3,2,1
		//y = ax + b;
		//4.5 = 0.5a + b;
		//0.5 = 2.5a + b;
		//公式で減った、a = -2; b= 5.5;
		//y = -2x + 5.5;
		//通るノードは　(x=1,y=3.5),(x=2,y=1.5),(x=0.75,y=4),(x=1.25,y=3),(x=1.75,y=2),(x=2.25,y=1)
		public bool Raycast(Node startNode, Node endNode,out Node hit){
			Vector3 direction = (endNode.pos - startNode.pos).normalized;
			int maxNodeCount = Mathf.FloorToInt(Vector3.Magnitude(endNode.pos - startNode.pos) / edgeLength);
			for(int i =0;i<maxNodeCount;i++){
				Vector3 pos = startNode.pos + direction * edgeLength * i;
				Node node = GetNode (pos);
				if(node.isBlock){
					hit = node;
					return true;
				}
			}
			if(endNode.isBlock){
				hit = endNode;
				return true;
			}
			hit = null;
			return false;
		}

		#if UNITY_EDITOR
		void OnDrawGizmos ()
		{
			if (showGizmos && allNodes != null) {
				for (int i = 0; i < allNodes.Count; i++) {
					if (!allNodes [i].isBlock) {
						Gizmos.color = Color.blue;
					} else {
						Gizmos.color = Color.red;
					}
					Gizmos.DrawWireCube (allNodes [i].pos, Vector3.one * edgeLength * 0.5f);
				}

				#if UNITY_EDITOR
				if (x0 < xCount && x0 >= 0 && y0 < yCount && y0 >= 0) {
					Node node = nodes [x0, y0];
					if (node.neighbors != null) {
						for (int i = 0; i < node.neighbors.Count; i++) {
							Gizmos.color = Color.green;
							Gizmos.DrawWireCube (node.neighbors [i].pos, Vector3.one * edgeLength * 0.5f);
						}
					}
				}
				#endif

			}
		}
		#endif

	}

	public class Node
	{
		public int index;
		public int x;
		//ノードの位置
		public int y;
		//ノードの位置
		public float G;
		//スタート distance.
		public float H;
		//Manhattan distance  曼哈顿算法
		public float F;

		public List<Node> neighbors = new List<Node> ();
		//このノードに接続(せつぞく)するノード。
		public List<float> consumes = new List<float> ();
		//このノードから接続ノードまで、移動消費(いどうしょうひ)コスト。
		public bool isBlock;
		//壁中にいるのかどうか
		public bool isWallSide;
		//壁に向かってるのかどうか
		public Vector3 pos;
		//ゲーム世界のポジション。
		public Node previous;
		//

		//用以下两个参数来判断是否在open close list，这样就可以不用hashset contain方法了，这样更快。（100*100的grid情况下从8ms降低6ms）
		//用int自增，这样就不用在第二次开始的时候reset这两个值了
		public int isOpen;
		public int isClose;
		public int isNearest;
	}
}
