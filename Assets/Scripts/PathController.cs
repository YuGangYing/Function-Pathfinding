using UnityEngine;
using System.Collections;

namespace YYGAStar
{
	public class PathController : MonoBehaviour
	{

		public PathAgent pathAgent;
		public Vector3 hitPos;
		public GameObject block;

		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("Ground"))) {
					hitPos = hit.point;
					pathAgent.StartFinder (hitPos);
				}
			}
			if (Input.GetKeyDown (KeyCode.H)) {
				block.SetActive (true);
				pathAgent.grid.CalculateBlockNode ();
			}
		}

		void OnDrawGizmos ()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube (hitPos, Vector3.one);
		}
	}
}