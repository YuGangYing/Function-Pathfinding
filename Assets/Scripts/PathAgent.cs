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
		public List<Node> openList = new List<Node> (1000);
		#if UNITY_EDITOR
		public HashSet<Node> closeSet = new HashSet<Node> ();
		#endif

		void Start ()
		{
			currentNode = GetNode (transform.position);
		}

		public Node GetNode (Vector3 pos)
		{
			int xIndex = Mathf.RoundToInt ((pos.x - grid.startPos.x) / grid.edgeLength);
			int yIndex = Mathf.RoundToInt ((pos.z - grid.startPos.z) / grid.edgeLength);
			Node node = grid.nodes [xIndex, yIndex];
			return node;
		}

		float mTime;

		public void StartFinder (Vector3 pos)
		{
			mTime = Time.realtimeSinceStartup;
			Node target = GetNode (pos);
			StartFinder (target);
			Move ();
			Debug.Log ("Total Find Time:" + (Time.realtimeSinceStartup - mTime));
		}

		public void StartFinder (Node target)
		{
			agentIndex++;
			openList.Clear ();
			#if UNITY_EDITOR
			closeSet.Clear ();
			#endif
			if (mIsMoving)
				StopCoroutine ("_Move");
			targetNode = target;
			currentNode = GetNode (transform.position);
			openList.Add (currentNode);
			grid.Clear ();
//			Find ();
			//TODO For tutorial.
			StartCoroutine ("_Find");
		}

		void AddToOpenList (Node node)
		{
			node.isOpen = agentIndex;
			AddOpenList (node);
		}

		Node RemoveFirstFromOpenList ()
		{
			Node node = openList [0];
			openList.RemoveAt (0);
			return node;
		}

		void AddToCloseList (Node node)
		{
			#if UNITY_EDITOR
			closeSet.Add (node);
			#endif
			node.isClose = agentIndex;
		}

		//同期
		void Find ()
		{
			float t1 = Time.realtimeSinceStartup;
			bool searched = false;
			while (openList.Count > 0 && !searched) {
				//リスト中にF値一番小さいのノード
				Node node = RemoveFirstFromOpenList ();// openList [0];
				if (node != targetNode) {
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
				}  else {
					searched = true;
				}
			}
			path.Clear ();
			GetMovePath (targetNode);
		}

		//非同期 BUG中、TODO。
		IEnumerator _Find ()
		{
			float t1 = Time.realtimeSinceStartup;
			bool searched = false;
			while (openList.Count > 0 && !searched) {
				//リスト中にF値一番小さいのノード
				Node node = RemoveFirstFromOpenList ();// openList [0];
				if (node != targetNode) {
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
				} else {
					searched = true;
				}
				yield return null;
			}
			path.Clear ();
			GetMovePath (targetNode);
		}

		//ノードをFで順番で openlist へ置いて
		//付きキュー
		void AddOpenList (Node node)
		{
			bool added = false;
			for (int i = 0; i < openList.Count; i++) {
				//node.isWallSide時優先に探する。
				if (openList [i].F >= node.F) {      // || node.isWallSide) {
					openList.Insert (i, node);
					added = true;
					break;
				}
			}
			if (!added) {
				openList.Insert (openList.Count, node);
			}
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

		#if UNITY_EDITOR
		float mColorSpeed = 10f;
		float mColorPlus = -0.005f;
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
			int i = 0;
			foreach (Node node in path) {

				float sin = Mathf.Sin(Time.time* mColorSpeed + i * mColorPlus);
				float cos = Mathf.Cos (Time.time * mColorSpeed + i * mColorPlus);
//				Gizmos.color = Color.green;
//				if(sin > 0.999f){
				Gizmos.color = new Color(cos,sin,cos,1);
//				}
				Gizmos.DrawCube (node.pos, Vector3.one);
				i++;
			}
		}
		#endif
	}
}


