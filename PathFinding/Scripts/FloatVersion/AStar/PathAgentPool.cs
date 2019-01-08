using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BlueNoah.PathFinding
{
	public class PathAgentPool : SingleMonoBehaviour<PathAgentPool> {

		public List<PathAgentQueueItem> pathAgentList;
		public int maxSearchNodePerFrame = 1000;

		protected override void Awake ()
		{
			base.Awake ();
			pathAgentList = new List<PathAgentQueueItem> ();
		}

		public void Remove(PathAgentQueueItem item){
			pathAgentList.Remove (item);
		}

		public PathAgentQueueItem StartFind(PathAgent pathAgent,Node startNode,Node endNode,UnityAction<List<Node>> onComplete){
			PathAgentQueueItem item = new PathAgentQueueItem ();
			item.pathAgent = pathAgent;
			item.startNode = startNode;
			item.endNode = endNode;
			item.onComplete = onComplete;
			pathAgentList.Add (item);
			return item;
		}

		void Update(){
			while(pathAgentList.Count> 0 && PathAgent.nodeCountSearched < maxSearchNodePerFrame){
				PathAgentQueueItem item = pathAgentList [0];
				pathAgentList.RemoveAt (0);
				List<Node> path = item.pathAgent.StartFind (item.startNode,item.endNode);
				if (item.onComplete != null)
					item.onComplete (path);
			}
			PathAgent.nodeCountSearched = 0;
		}

	}

	public class PathAgentQueueItem{
		public PathAgent pathAgent;
		public Node startNode;
		public Node endNode;
		public UnityAction<List<Node>> onComplete; 
	}

}
