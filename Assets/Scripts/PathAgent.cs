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

		public Grid grid;
		public Node currentNode;
		public Node targetNode;
		//add,remove,order.
		public List<Node> openList = new List<Node> ();
		//add,remove,contain.
		public HashSet<Node> openSet = new HashSet<Node> ();
		//add,contain.
		public HashSet<Node> closeSet = new HashSet<Node> ();
		//whether synchronization.
		public bool isSynchronization = true;

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

		public void StartFinder (Vector3 pos)
		{
			Node target = GetNode (pos);
			StartFinder (target);
		}

		public void StartFinder (Node target)
		{
			openList.Clear ();
			closeSet.Clear ();
			openSet.Clear ();
			StopCoroutine ("_Move");
			targetNode = target;
			currentNode = GetNode (transform.position);
			openList.Add (currentNode);
			Debug.Log (currentNode.pos);
			grid.Clear ();
			if (isSynchronization) {
				Find ();
			} else {
				StartCoroutine (_Find ());	
			}
		}

		//同期
		void Find(){
			while (openList.Count > 0) {
				//リスト中にF値一番小さいのノード
				Node node = openList [0];
				if (node == targetNode) {
					openList.Clear ();
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
			}
			Debug.Log ("Find Done");
			Move (targetNode);
		}

		//非同期
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
			Move (targetNode);
		}

		//ノードをFで順番で openlist へ置いて
		//付きキュー
		void AddOpenList (Node node)
		{
			bool added = false;
			for (int i = 0; i < openList.Count; i++) {
				//node.isWallSide時優先に探する。
				if (openList [i].F >= node.F || node.isWallSide) {
					openList.Insert (i, node);
					added = true;
					break;
				}
			}
			if (!added) {
				openList.Insert (openList.Count, node);
			}
		}

		public void Move (Node node)
		{
			path.Clear ();
			GetMovePath (node);
			StartCoroutine ("_Move");
		}

		float speed = 10;
		//Move Agentが必要です
		IEnumerator _Move ()
		{
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
					t += Time.deltaTime * speed;
					yield return null;
				}
				path.RemoveAt (0);
				yield return null;
			}
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

		List<Node> path = new List<Node> ();

		public void GetMovePath (Node node)
		{
			path.Insert (0, node);
			if (node.previous != null) {
				GetMovePath (node.previous);
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


