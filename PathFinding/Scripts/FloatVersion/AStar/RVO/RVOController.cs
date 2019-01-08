using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.PathFinding
{
	public class RVOController : SingleMonoBehaviour<RVOController>
	{
		public Grid grid;
		public Dictionary<int,RVOGroup> agentGroup;
		public bool rvo = true;
		protected override void Awake ()
		{
			InitGroup ();
		}

		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) {
				if(rvo)
					RVOMove (0);
				else
					NormalMove(0);
			}
		}

		void InitGroup(){
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

		void RVOMove(int groupId){
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << Grid.groundLayer)) {
				StartCoroutine (_RVOMove(0,hit.point));
			}
		}

		IEnumerator _RVOMove(int groupId,Vector3 pos){
			RVOGroup group0 = agentGroup [groupId];
			PathAgent leader = group0.leader;
			float time = Time.realtimeSinceStartup;
			Node targetNode = grid.GetNode (pos);
			Node leaderNode = grid.GetNode (leader.transform.position);
			List<Node> smoothPath = leader.StartFind (leaderNode,targetNode);
			MoveAgent moveAgent = leader.GetComponent<MoveAgent> ();
			if (moveAgent != null)
				moveAgent.Move (smoothPath);
			List<Node> leaderPath = new List<Node> (smoothPath);
			List <Node> nearest = grid.GetNearest (targetNode,group0.members.Count);
			for (int i = 0; i < group0.members.Count; i++) {
				List<Node> smoothPathMember = group0.members[i].StartFind (grid.GetNode (group0.members[i].transform.position),leaderNode);
				smoothPathMember.AddRange (leaderPath);
				List<Node> endPath = group0.members [i].StartFind (smoothPathMember [smoothPathMember.Count - 2], nearest[i]);
				smoothPathMember.RemoveAt (smoothPathMember.Count-1);
				smoothPathMember.AddRange (endPath);
				MoveAgent moveAgentMember = group0.members[i].GetComponent<MoveAgent> ();
				if (moveAgentMember != null)
					moveAgentMember.Move (smoothPathMember);
				if(PathAgent.nodeCountSearched > 1000){
					PathAgent.nodeCountSearched = 0;
					yield return null;
				}
			}
			Debug.Log ("Total Search Time:" + (Time.realtimeSinceStartup - time));
			yield return null;
		}


		void NormalMove(int groupId){
			RVOGroup group0 = agentGroup [groupId];
			PathAgent leader = group0.leader;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << Grid.groundLayer)) {
				float time = Time.realtimeSinceStartup;
				Node targetNode = grid.GetNode (hit.point);
				Node leaderNode = grid.GetNode (leader.transform.position);
				List<Node> smoothPath = leader.StartFind (leaderNode,targetNode);
				MoveAgent moveAgent = leader.GetComponent<MoveAgent> ();
				if (moveAgent != null)
					moveAgent.Move (smoothPath);
				List <Node> nearest = grid.GetNearest (targetNode,group0.members.Count);
				for (int i = 0; i < group0.members.Count; i++) {
					List<Node> smoothPathMember = group0.members[i].StartFind (grid.GetNode (group0.members[i].transform.position),nearest[i]);
//					smoothPathMember.AddRange (smoothPath);
//					List<Node> endPath = group0.members [i].StartFinder (smoothPathMember [smoothPathMember.Count - 2], nearest[i]);
//					smoothPathMember.RemoveAt (smoothPathMember.Count-1);
//					smoothPathMember.AddRange (endPath);
					MoveAgent moveAgentMember = group0.members[i].GetComponent<MoveAgent> ();
					if (moveAgentMember != null)
						moveAgentMember.Move (smoothPathMember);
				}
				Debug.Log ("Total Search Time:" + (Time.realtimeSinceStartup - time));
			}
		}


//		List <Node> nearest = new List<Node>();
//		void GetNearest(){
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//			RaycastHit hit;
//			if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << Grid.groundLayer)) {
//				Node node = grid.GetNode (hit.point);
//				RVOGroup group0 = agentGroup [0];
//				nearest = grid.GetNearest (node,group0.members.Count);
//			}
//		}
//
//		void OnDrawGizmos ()
//		{
//			Gizmos.color = Color.yellow;
//			for(int i=0;i<nearest.Count;i++){
//				Gizmos.DrawCube (nearest[i].pos, Vector3.one * 1.4f);
//			}
//		}
	}

	public class RVOGroup{
		public PathAgent leader;
		public List<PathAgent> members;
	}

}
