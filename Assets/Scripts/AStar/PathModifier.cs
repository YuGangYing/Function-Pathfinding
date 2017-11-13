using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YYGAStar
{
	[System.Serializable]
	public class PathModifier
	{
		//レイするのかどうか。
		public bool isRaycast = false;

		public List<Node> SmoothPath (List<Node> path)
		{
			if (path.Count == 0)
				return new List<Node> ();
			List<Node> smoothedPath = Smooth (path);
			if (isRaycast) {
				smoothedPath = SmonthWithRay (path);
			}
			return smoothedPath;
		}

		//TODO 簡単化にすることが必要です
		List<Node> Smooth (List<Node> path)
		{
			Vector2 preDirection = Vector2.zero;
			Node preNode = path [0];
			List<Node> smoothedPath = new List<Node> ();
			for (int i = 1; i < path.Count; i++) {
				Vector2 dir = new Vector2 (path [i].x - path [i - 1].x, path [i].y - path [i - 1].y); //path [i].pos - preNode.pos;
				if (dir != preDirection) {
					smoothedPath.Add (preNode);
					if (preNode != path [i - 1])
						smoothedPath.Add (path [i - 1]);
					preNode = path [i];
					preDirection = dir;
				}
			}
			smoothedPath.Add (path [path.Count - 1]);
			return smoothedPath;
		}

		List<Node>  SmonthWithRay (List<Node> path)
		{
			//forでpreNodeはnullかどうか、判断すること要らないために。
			Node startNode = path [0];
			Node endNode = null;
			List<Node> smoothedPath = new List<Node> ();
			RaycastHit hit;
			for (int i = 1; i < path.Count; i++) {
				endNode = path [i];
				if(Physics.Raycast(startNode.pos,(endNode.pos - startNode.pos).normalized,out hit,Vector3.Distance(startNode.pos,endNode.pos),1<<Grid.blockLayer)){
					smoothedPath.Add (path [i-1]);
					startNode = path [i-1];
				}
			}
			smoothedPath.Add (path[path.Count-1]);
			Debug.Log (smoothedPath.Count);
			return smoothedPath;
		}

	}
}
