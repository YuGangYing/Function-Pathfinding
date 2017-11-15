using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace YYGAStar
{
	public class PathController : MonoBehaviour
	{

		Vector3 mHitPos;
		PathAgent[] mPathAgents;

		void Awake(){
			mPathAgents = FindObjectsOfType<PathAgent> ();
		}

		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("Ground"))) {
					mHitPos = hit.point;
					for(int i=0;i<mPathAgents.Length;i++){
						List<Node> path = mPathAgents[i].StartFinder (mHitPos);
						mPathAgents [i].GetComponent<MoveAgent> ().Move (path);
					}
				}
			}
		}

		void OnDrawGizmos ()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube (mHitPos, Vector3.one);
		}
	}
}