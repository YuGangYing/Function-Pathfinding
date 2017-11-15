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
		List<Node> mSmoothedPath = new List<Node>(1000);
		List<Node> mSmoothedPathWithRay = new List<Node>(1000);

		public List<Node> SmoothPath (List<Node> path)
		{
			Debug.Log (path.Count);
			if (path.Count == 0)
				return new List<Node> ();
			List<Node> smoothedPath = Smooth (path);
			if (isRaycast) {
				smoothedPath = SmoothWithRay (smoothedPath);
			}
			return smoothedPath;
		}

		//経路をなめらかにする
		List<Node> Smooth (List<Node> path)
		{
			Vector2 preDirection = Vector2.zero;
			Debug.Log (mSmoothedPath == path);
			mSmoothedPath.Clear ();
			for (int i = 1; i < path.Count; i++) {
				Vector2 dir = new Vector2 (path [i].x - path [i - 1].x, path [i].y - path [i - 1].y);//path [i].pos - preNode.pos;
				if (dir != preDirection) {
					if (i - 1 > 0)
						mSmoothedPath.Add (path [i - 1]);
					preDirection = dir;
				}
			}
			Debug.Log (path.Count - 1);
			mSmoothedPath.Add (path [path.Count - 1]);
			return mSmoothedPath;
		}

		//レイで経路をさらになめらかにする
		List<Node>  SmoothWithRay (List<Node> path)
		{
			//forでpreNodeはnullかどうか、判断すること要らないために。
			Node startNode = path [0];
			Node endNode = null;
			mSmoothedPathWithRay.Clear ();
			RaycastHit hit;
			for (int i = 1; i < path.Count; i++) {
				endNode = path [i];
				if (Physics.Raycast (startNode.pos, (endNode.pos - startNode.pos).normalized, out hit, Vector3.Distance (startNode.pos, endNode.pos), 1 << Grid.blockLayer)) {
					mSmoothedPathWithRay.Add (path [i - 1]);
					startNode = path [i - 1];
				}
			}
			mSmoothedPathWithRay.Add (path [path.Count - 1]);
			return mSmoothedPathWithRay;
		}

	}
}
