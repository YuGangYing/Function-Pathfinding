using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace YYGAStar
{
	//エースター　A to B の最短(さいたん)経路(けいろ)を探索(たんさく)
	//TODO 壁に向かってるノード必要だ。
	//TODO 様々な場合を考えることがある。
	//TODO 効率化になる必要だ。
	public class PathAgent : MonoBehaviour
	{
		static int agentIndex = 0;
		public Grid grid;
		public Node currentNode;
		public Node targetNode;
		public bool isSmooth = true;
		public PathModifier pathModifier;
		public UnityAction onFindComplete;
		public List<Node> openList = new List<Node> (1000);
		#if UNITY_EDITOR
		public List<Node> closeList = new List<Node> (1000);
		#endif
//		public bool isIEnumerator = false;
		//これで計算する、リストのRemoveの方法を使わない、早く、CGがない。
		int mCurrentIndex = 0;
		public int groupId;
		public bool isLeader;

		void Start ()
		{
			currentNode = grid.GetNode (transform.position);
		}


		void AddToOpenList (Node node)
		{
			node.isOpen = agentIndex;
			AddOpenList (node);
		}

		Node RemoveFirstFromOpenList ()
		{
			Node node = openList [mCurrentIndex];
//			openList.RemoveAt (0);
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

		float mTime;
		public List<Node> StartFinder (Vector3 pos)
		{
			mTime = Time.realtimeSinceStartup;
			Node target = grid.GetNode (pos);
			return StartFinder (target);
		}

		//同期
		public List<Node> StartFinder (Node target)
		{
			agentIndex++;
			mCurrentIndex = 0;
			targetNode = target;
			float t1 = Time.realtimeSinceStartup;
			bool searched = false;
			openList.Clear();
			closeList.Clear();
			grid.Reset ();
			currentNode = grid.GetNode (transform.position);
			openList.Add (currentNode);
			while (openList.Count > 0 && !searched) {
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
						}
					}
				}  else {
					searched = true;
				}
			}
			if (searched) {
				resultPath = GetMovePath (targetNode);
				if (onFindComplete != null)
					onFindComplete ();
			} else {
				Debug.Log ("target can't be reached!");
			}
			Debug.Log ("Total Find Time:" + (Time.realtimeSinceStartup - mTime));
			return resultPath;
		}

		//非同期 
		//演出だけ
//		IEnumerator _Find ()
//		{
//			float t1 = Time.realtimeSinceStartup;
//			bool searched = false;
//			while (openList.Count > 0 && !searched) {
//				//リスト中にF値一番小さいのノード
//				Node node = RemoveFirstFromOpenList ();
//				if (node != targetNode) {
//					AddToCloseList (node);
//					float t = Time.realtimeSinceStartup;
//					for (int i = 0; i < node.neighbors.Count; i++) {
//						if (node.neighbors [i].isOpen != agentIndex && node.neighbors [i].isClose != agentIndex && !node.neighbors [i].isBlock) {
//							//Calculate G
//							node.neighbors [i].G = node.G + node.consumes [i];
//							//Calculate H
//							node.neighbors [i].H = Mathf.Abs (targetNode.x - node.neighbors [i].x) + Mathf.Abs (targetNode.y - node.neighbors [i].y);
//							//Calculate F
//							node.neighbors [i].F = node.neighbors [i].G + node.neighbors [i].H;
//							//insert openlist order by F;
//							AddToOpenList (node.neighbors [i]);
//							node.neighbors [i].previous = node;
//						}
//					}
//				} else {
//					searched = true;
//				}
//				yield return null;
//			}
//			GetMovePath (targetNode);
//			if (onFindComplete != null)
//				onFindComplete ();
//		}

		//ノードをF（H）で順番で openlist へ置いて
		//付きキュー
		void AddOpenList (Node node)
		{
			bool added = false;
			for (int i = mCurrentIndex; i < openList.Count; i++) {
				if (openList [i].H >= node.H ) {    
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


