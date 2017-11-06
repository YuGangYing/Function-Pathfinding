using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		//add,remove,order.
		public List<Node> openList = new List<Node> (1000);
		//add,remove,contain.
//		public HashSet<Node> openSet = new HashSet<Node> ();
		//add,contain.
		public HashSet<Node> closeSet = new HashSet<Node> ();
		//whether synchronization.
//		bool isSynchronization = true;

		void Start ()
		{
			currentNode = GetNode (transform.position);
		}

		public Node GetNode (Vector3 pos)
		{
			int xIndex = Mathf.RoundToInt (pos.x / grid.edgeLength);
			int yIndex = Mathf.RoundToInt (pos.z / grid.edgeLength);
			Node node = grid.nodes [xIndex, yIndex];
			return node;
		}

		float mTime;
		float mTime1;
		float mOrderTime;
		float mOrderTime1;

		public void StartFinder (Vector3 pos)
		{
			mOrderTime = 0;
			mOrderTime1 = 0;
			Node target = GetNode (pos);
			mTime = Time.realtimeSinceStartup;
			StartFinder (target);
			Move ();
			Debug.Log ("Total Find Time:" + (Time.realtimeSinceStartup - mTime));
			Debug.Log (mTime1);
			Debug.Log (mOrderTime);
			Debug.Log (mOrderTime1);
		}

		public void StartFinder (Node target)
		{
			agentIndex++;
			mTime1 = Time.realtimeSinceStartup;
			openList.Clear ();
			closeSet.Clear ();
//			openSet.Clear ();
			if (mIsMoving)
				StopCoroutine ("_Move");
			targetNode = target;
			currentNode = GetNode (transform.position);
			openList.Add (currentNode);
//			openSet.Add (currentNode);
			grid.Clear ();
			mTime1 = Time.realtimeSinceStartup - mTime1;
			Find ();
		}

		void AddToOpenList(Node node){
			node.isOpen = agentIndex;
			AddOpenList (node);
		}

		Node RemoveFirstFromOpenList(){
			Node node = openList [0];
			openList.RemoveAt (0);
			return node;
		}

		void AddToCloseList(Node node){
			closeSet.Add (node);
			node.isClose = agentIndex;
		}

		//同期
		void Find ()
		{
			float t1 = Time.realtimeSinceStartup;
			while (openList.Count > 0) {
				//リスト中にF値一番小さいのノード
				Node node = RemoveFirstFromOpenList();// openList [0];
				if (node == targetNode) {
					openList.Clear ();
//					openSet.Clear ();
				} else {
//					openList.Remove (node);
//					openList.RemoveAt (0);//always index 0 の　ノード。
//					openSet.Remove (node);
					AddToCloseList (node);
					float t = Time.realtimeSinceStartup;
					for (int i = 0; i < node.neighbors.Count; i++) {
						if (node.neighbors [i].isOpen != agentIndex && node.neighbors [i].isClose != agentIndex && !node.neighbors [i].isBlock) {
//							if (!openSet.Contains (node.neighbors [i]) && !closeSet.Contains (node.neighbors [i]) && !node.neighbors [i].isBlock) {
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
					mOrderTime += (Time.realtimeSinceStartup - t);
				}
			}
			mOrderTime1 = Time.realtimeSinceStartup - t1;
			path.Clear ();
			GetMovePath (targetNode);
		}

		//非同期 BUG中、TODO。
		IEnumerator _Find ()
		{
			while (openList.Count > 0) {
				//リスト中にF値一番小さいのノード
				Node node = openList [0];
				if (node == targetNode) {
					openList.Clear ();
					yield return null;
				} else {
					openList.Remove (node);
					closeSet.Add (node);
					for (int i = 0; i < node.neighbors.Count; i++) {
						if (!openList.Contains (node.neighbors [i]) && !closeSet.Contains (node.neighbors [i]) && !node.neighbors [i].isBlock) {
							//Calculate G
							node.neighbors [i].G = node.G + node.consumes [i];
							//Calculate H
							node.neighbors [i].H = Mathf.Abs (targetNode.x - node.neighbors [i].x) + Mathf.Abs (targetNode.y - node.neighbors [i].y);
							//Calculate F
							node.neighbors [i].F = node.neighbors [i].G + node.neighbors [i].H;
							//insert openlist order by F;
							AddOpenList (node.neighbors [i]);
							node.neighbors [i].previous = node;
						}
					}
				}
				yield return null;
			}
			yield return null;
			Debug.Log ("Find Done");
			Move ();
		}

		//ノードをFで順番で openlist へ置いて
		//付きキュー
		void AddOpenList (Node node)
		{
			bool added = false;
			for (int i = 0; i < openList.Count; i++) {
				//node.isWallSide時優先に探する。
				if (openList [i].F >= node.F){      // || node.isWallSide) {
					openList.Insert (i, node);
					added = true;
					break;
				}
			}
			if (!added) {
				openList.Insert (openList.Count, node);
			}
//			openSet.Add (node);
		}

		public void Move ()
		{
			StartCoroutine ("_Move");
		}

		float mSpeed = 10;
		bool mIsMoving = false;
		//Move Agentが必要です
		IEnumerator _Move ()
		{
			mIsMoving = true;
			while (path.Count > 0) {
				float t = 0;
				Vector3 pos = transform.position;
				while (t < 1) {
					if (IsPathBlock ()) {
						StartFinder (targetNode);
						StopCoroutine ("_Move");
					}
					transform.position = Vector3.Lerp (pos, path [0].pos, t);
					transform.LookAt (path [0].pos);
					t += Time.deltaTime * mSpeed;
					yield return null;
				}
				path.RemoveAt (0);
				yield return null;
			}
			mIsMoving = false;
		}

		bool IsPathBlock ()
		{
			foreach (Node node in path) {
				if (node.isBlock) {
					return true;
				}
			}
			return false;
		}

		List<Node> path = new List<Node> (1000);
		//経路をゲートする。
		public void GetMovePath (Node node)
		{
			Node currentNode = node;
			path.Insert (0, currentNode);
			while (currentNode.previous != null) {
				path.Insert (0, currentNode.previous);
				currentNode = currentNode.previous;
			}
		}

		void OnDrawGizmos ()
		{
			Gizmos.color = Color.yellow;
			foreach (Node node in closeSet) {
				Gizmos.DrawCube (node.pos, Vector3.one);
			}
			Gizmos.color = Color.red;
			foreach (Node node in openList) {
				Gizmos.DrawCube (node.pos, Vector3.one);
			}
		}

	}
}


