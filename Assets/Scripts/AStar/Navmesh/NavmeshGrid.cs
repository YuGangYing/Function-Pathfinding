using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MMO
{
	
	public class NavmeshGrid : MonoBehaviour
	{

		public Vector3[] vertices;
		public int[] indices;

		public bool debug_mesh = true;
		Mesh mMesh;
		MeshFilter mMeshFilter;
		MeshRenderer mMeshRenderer;
		Material mMat;

		public AABBNode[,] nodes;
		public List<AABBNode> nodeList;
		public Vector3 startPos;
		public int AABBSize = 10;
		public int xCount;
		public int yCount;
		NavmeshTriangulation[] navmeshTriangulations;

		void Awake ()
		{
			NavMeshTriangulation meshData = NavMesh.CalculateTriangulation ();
			vertices = meshData.vertices;
			indices = meshData.indices;
			if (debug_mesh)
				InitTestMesh ();
			InitAABBGrid ();
			InitTriangulationNode ();
			InitConnections ();
		}

		void Update(){
			if(Input.GetKeyDown(KeyCode.H)){
				StartCoroutine (_ChangeTriangulationColors());
			}
		}

		#region mesh to debug the path and navmesh.
		void InitTestMesh ()
		{
			mMesh = new Mesh ();
			mMeshFilter = gameObject.GetComponent<MeshFilter> ();
			if (mMeshFilter == null)
				mMeshFilter = gameObject.AddComponent<MeshFilter> ();
			mMeshRenderer = gameObject.GetComponent<MeshRenderer> ();
			if (mMeshRenderer == null)
				mMeshRenderer = gameObject.AddComponent<MeshRenderer> ();
			mMat = new Material (Shader.Find ("Vertex-Color-Unlit-CG"));
			mMeshRenderer.material = mMat;
			//消除共用顶点，便于演示。共通点を消した。演出のため。
			Vector3[] meshVertics = new Vector3[indices.Length];
			Color[] meshColors = new Color[indices.Length];
			int[] meshIndices = new int[indices.Length];
			for(int i =0;i< indices.Length / 3 ;i++){
				int index = i * 3;
				meshVertics [index] = this.vertices [indices [index]];
				meshColors [index] = Color.red;
				meshIndices [index] = index;
				index = i * 3 + 1;
				meshVertics [index] = this.vertices [indices [index]];
				meshColors [index] = Color.red;
				meshIndices [index] = index;
				index = i * 3 + 2;
				meshVertics [index] = this.vertices [indices [index]];
				meshColors [index] = Color.red;
				meshIndices [index] = index;
			}
			mMesh.vertices = meshVertics;
			mMesh.triangles = meshIndices;
			mMesh.colors = meshColors;
			mMeshFilter.mesh = mMesh;
		}

		IEnumerator _ChangeTriangulationColors(){
			Debug.Log ("_ChangeTriangulationColors");
			int[] meshIndices = mMesh.triangles;
			Color targetColor = new Color (Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1);
			Color[] meshColors = mMesh.colors;
			for(int i =0;i< meshIndices.Length / 3 ;i++){
				int index = i * 3;
				meshColors [index] = targetColor;
				index = i * 3 + 1;
				meshColors [index] = targetColor;
				index = i * 3 + 2;
				meshColors [index] = targetColor;
				mMesh.colors = meshColors;
				yield return new WaitForSeconds (0.1f);
			}
		}
		#endregion

		void InitAABBGrid ()
		{
			xCount = Mathf.Max (1, xCount);
			yCount = Mathf.Max (1, yCount);
			nodes = new AABBNode[xCount, yCount];
			nodeList = new List<AABBNode> ();
			for (int j = 0; j < yCount; j++) {
				for (int i = 0; i < xCount; i++) {
					AABBNode node = new AABBNode ();
					node.x = i;
					node.y = j;
					node.pos = new Vector3 (i * AABBSize, 0, j * AABBSize) + startPos;
					nodes [i, j] = node;
					nodeList.Add (node);
				}
			}
		}

		Dictionary<int,List<int>> mNodeConnections;

		void InitTriangulationNode(){
			navmeshTriangulations = new NavmeshTriangulation[indices.Length / 3];
			NavmeshTriangulation triangulation;
			for(int i =0;i< indices.Length / 3 ;i++){
				triangulation = new NavmeshTriangulation ();
				triangulation.index = i;
				triangulation.vertics = new Vector3[3];
				triangulation.indices = new int[3];
				navmeshTriangulations [i] = triangulation;
				int index = i * 3;
				triangulation.vertics [0] = this.vertices [indices [index]];
				triangulation.indices [0] = index;
				index = index + 1;
				triangulation.vertics [1] = this.vertices [indices [index]];
				triangulation.indices [1] = index;
				index = index + 2;
				triangulation.vertics [2] = this.vertices [indices [index]];
				triangulation.indices [2] = index;
			}
		}

		void InitConnections(){
			mNodeConnections = new Dictionary<int, List<int>> ();
			for(int i =0;i< indices.Length / 3 ;i++){
				int[] indexs = new int[3];
				indexs [0] = indices [i * 3];
				indexs [1] = indices [i * 3 + 1];
				indexs [2] = indices [i * 3 + 2];
				SetConnection (indexs);
			}
		}

		void SetConnection(int[] indexs){
			for(int i =0;i< indexs.Length ;i++){
				int current = indexs[i];
				if(!mNodeConnections.ContainsKey(current)){
					mNodeConnections.Add (current,new List<int>());
				}
				for (int j = 0; i < indexs.Length; j++) {
					if(current!=indexs[j]){
						if (!mNodeConnections [current].Contains (indexs[j])) {
							mNodeConnections [current].Add (indexs[j]);
						}
						if (!mNodeConnections [current].Contains (indexs[j])) {
							mNodeConnections [current].Add (indexs[j]);
						}
					}
				}
			}
		}

		void InitNodes ()
		{
			for (int i = 0; i < indices.Length / 3; i++) {
				AABBNode node = new AABBNode ();

			}
   		}
	}

	public class NavmeshTriangulation
	{
		public int index;
		public Vector3[] vertics;
		public int[] indices;
	}



	public class AABBNode
	{
		public int index;
		//ノードの位置x
		public int x;
		//ノードの位置y
		public int y;
		//ノード
		public List<AABBNode> navmeshNodeList;
		//このノードから接続ノードまで、移動消費(いどうしょうひ)コスト。
		public bool isBlock;
		//壁中にいるのかどうか
		public bool isWallSide;
		//壁に向かってるのかどうか
		public Vector3 pos;
		//用以下两个参数来判断是否在open close list，这样就可以不用hashset contain方法了，这样更快。（100*100的grid情况下从8ms降低6ms）
		//用int自增，这样就不用在第二次开始的时候reset这两个值了
		public int isOpen;
		public int isClose;
		public int isNearest;
	}

}