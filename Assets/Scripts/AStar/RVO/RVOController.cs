using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YYGAStar
{
	public class RVOController : SingleMonoBehaviour<RVOController>
	{

		public Dictionary<int,RVOGroup> agentGroup;

		protected override void Awake ()
		{
			agentGroup = new Dictionary<int, RVOGroup> ();
			PathAgent[] agents = FindObjectsOfType<PathAgent> (); 
			for(int i=0;i<agents.Length;i++){
				if(!agentGroup.ContainsKey(agents[i].groupId)){
					agentGroup.Add (agents[i].groupId,new RVOGroup());
					agentGroup [agents [i].groupId].members = new List<PathAgent> ();
				}
				if (agents [i].isLeader) {
					agentGroup [agents [i].groupId].leader = agents [i];
				} else {
					agentGroup [agents [i].groupId].members.Add (agents[i]);
				}
			}
		}

		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {
				RVOGroup group0 = agentGroup [0];
				PathAgent leader = group0.leader;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << Grid.groundLayer)) {
					Vector3 hitPos = hit.point;
//					List<Node> smoothPath = leader.StartFinder (hitPos);
//					MoveAgent moveAgent = leader.GetComponent<MoveAgent> ();
//					if (moveAgent != null)
//						moveAgent.Move (smoothPath);

//					for (int i = 0; i < group0.members.Count; i++) {
//						List<Node> smoothPathMember = group0.members[i].StartFinder (leader.transform.position);
//						smoothPathMember.AddRange (smoothPath);
//						MoveAgent moveAgentMember = group0.members[i].GetComponent<MoveAgent> ();
//						if (moveAgentMember != null)
//							moveAgentMember.Move (smoothPathMember);
//					}
				}
			}
		}

	}

	public class RVOGroup{
		public PathAgent leader;
		public List<PathAgent> members;
	}

}
