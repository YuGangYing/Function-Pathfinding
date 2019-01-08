using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace BlueNoah.PathFinding
{
	//エースター　A to B の最短(さいたん)経路(けいろ)を探索(たんさく)
	public class PathAgent : MonoBehaviour
	{
		static int agentIndex = 0;
		public Grid grid;
		public Node targetNode;
		public bool isSmooth = true;
		public PathModifier pathModifier;
		public List<Node> openList = new List<Node> (1000);
		#if UNITY_EDITOR
		public List<Node> closeList = new List<Node> (1000);
		#endif
		//これで計算する、リストのRemoveの方法を使わない、早く、CGがない。
		int mCurrentIndex = 0;
		public int groupId;
		public bool isLeader;

		void AddToOpenList (Node node)
		{
			node.isOpen = agentIndex;
			AddOpenList (node);
		}

		Node RemoveFirstFromOpenList ()
		{
			Node node = openList [mCurrentIndex];
			mCurrentIndex++;
			return node;
		}

		void AddToCloseList (Node node)
		{
			#if UNITY_EDITOR
			closeList.Add (node);
			#endif
			node.isClose = agentIndex;
		}

		public List<Node> StartFind (Vector3 pos)
		{
			Node currentNode = grid.GetNode (transform.position);
			Node target = grid.GetNode (pos);
			return StartFind (currentNode,target);
		}

		public static int nodeCountSearched;
		//同期
		public List<Node> StartFind (Node currentNode, Node target)
		{
			agentIndex++;
			mCurrentIndex = 0;
			targetNode = target;
			float t1 = Time.realtimeSinceStartup;
			bool searched = false;
			openList.Clear();
			#if UNITY_EDITOR
			closeList.Clear();
			#endif
			grid.Reset ();
			openList.Add (currentNode);
			while (openList.Count > mCurrentIndex && !searched) {
				//リスト中にF値一番小さいのノード
				Node node = RemoveFirstFromOpenList ();// openList [0];
				if (node != targetNode) {
					AddToCloseList (node);
					float t = Time.realtimeSinceStartup;
					for (int i = 0; i < node.neighbors.Count; i++) {
						if (node.neighbors [i].isOpen != agentIndex && node.neighbors [i].isClose != agentIndex && !node.neighbors [i].isBlock) {
							//Calculate G
							node.neighbors [i].G = node.G + node.consumes [i];
							//Calculate H
							node.neighbors [i].H = Mathf.Abs (targetNode.x - node.neighbors [i].x) + Mathf.Abs (targetNode.y - node.neighbors [i].y);
							//Calculate F
							node.neighbors [i].F = node.neighbors [i].G + node.neighbors [i].H;
							//insert openlist order by F;
							AddToOpenList (node.neighbors [i]);
							node.neighbors [i].previous = node;
							nodeCountSearched++;
						}
					}
				}  else {
					searched = true;
				}
			}
			List<Node> resultPath = new List<Node> ();
			if (searched) {
				resultPath = GetMovePath (targetNode);
			} else {
				Debug.Log ("target can't be reached!");
			}
			List<Node> realPath = new List<Node> (resultPath);
			return realPath;
		}

		//ノードをF（H）で順番で openlist へ置いて
		//付きキュー
		void AddOpenList (Node node)
		{
			bool added = false;
			for (int i = mCurrentIndex; i < openList.Count; i++) {
				if (openList [i].F >= node.F ) {    
					openList.Insert (i, node);
					added = true;
					break;
				}
			}
			if (!added) {
				openList.Insert (openList.Count, node);
			}
		}

		bool IsPathBlock ()
		{
			foreach (Node node in resultPath) {
				if (node.isBlock) {
					return true;
				}
			}
			return false;
		}

		List<Node> resultPath = new List<Node> (1000);
		//経路を探索する。
		public List<Node> GetMovePath (Node node)
		{
			resultPath.Clear ();
			Node currentNode = node;
			resultPath.Insert (0, currentNode);
			while (currentNode.previous != null) {
				resultPath.Insert (0, currentNode.previous);
				currentNode = currentNode.previous;
			}
			if (isSmooth)
				return pathModifier.SmoothPath (resultPath);
			else
				return resultPath;
		}

		#if UNITY_EDITOR
		float mColorSpeed = 10f;
		float mColorPlus = -0.005f;
		public bool showGizmos = false;
		void OnDrawGizmos ()
		{
			if (!showGizmos)
				return;
			Gizmos.color = Color.yellow;
			for(int i=0;i<closeList.Count;i++){
				Gizmos.DrawCube (closeList[i].pos, Vector3.one);
			}
			Gizmos.color = Color.red;
			for(int i=mCurrentIndex;i<openList.Count;i++){
				Gizmos.DrawCube (openList[i].pos, Vector3.one);
			}
			for(int i=0;i<resultPath.Count;i++){
				float sin = Mathf.Sin(Time.time* mColorSpeed + i * mColorPlus);
				float cos = Mathf.Cos (Time.time * mColorSpeed + i * mColorPlus);
				Gizmos.color = new Color(cos,sin,cos,1);
				Gizmos.DrawCube (resultPath[i].pos, Vector3.one);
			}
		}
		#endif
	}
}


