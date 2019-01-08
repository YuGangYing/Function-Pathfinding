using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.PathFinding
{
	public class DynamicNode : MonoBehaviour
	{

		public Grid grid;

		void Update ()
		{
			if (Input.GetMouseButton (1)) {
				if (grid == null)
					return;
				RaycastHit hit;
                if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, 1 << BlueNoah.PathFinding.Grid.groundLayer)) {
					Vector3 pos = hit.point;
					Node node = grid.GetNode (pos);
					if (!node.isBlock) {
						node.isBlock = true;
						for (int i = 0; i < node.neighbors.Count; i++) {
							grid.CalculateNeighbor (node.neighbors[i]);
						}
					}
				}
			}
		}

	}
}
