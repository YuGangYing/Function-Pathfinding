using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YYGAStar
{
	public class AgentPoolController : MonoBehaviour
	{

		PathAgent[] mPathAgents;
		public Grid grid;
		Dictionary<PathAgent,PathAgentQueueItem> mPathAgentDic;

		void Awake(){
			mPathAgentDic = new Dictionary<PathAgent, PathAgentQueueItem> ();
			mPathAgents = FindObjectsOfType<PathAgent> ();
		}

		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("Ground"))) {
					Node endNode = grid.GetNode (hit.point);
					List <Node> nearest = grid.GetNearest (endNode,mPathAgents.Length);
					for(int i=0;i<mPathAgents.Length;i++){
						Node startNode = grid.GetNode (mPathAgents[i].transform.position);
						MoveAgent moveAgent = mPathAgents [i].GetComponent<MoveAgent> ();
						Node targetNode = nearest [i];
						if(mPathAgentDic.ContainsKey(mPathAgents[i]) && mPathAgentDic[mPathAgents[i]] != null){
							PathAgentPool.GetInstance.Remove (mPathAgentDic[mPathAgents[i]]);
						}
						PathAgentQueueItem mPathAgentQueueItem = PathAgentPool.GetInstance.StartFind (mPathAgents[i],startNode,targetNode,(List<Node> path) =>{
							moveAgent.Move (path);
						});
						if (!mPathAgentDic.ContainsKey (mPathAgents [i]))
							mPathAgentDic.Add (mPathAgents [i], mPathAgentQueueItem);
						else
							mPathAgentDic [mPathAgents [i]] = mPathAgentQueueItem;
					}
				}
			}
		}

	}
}
