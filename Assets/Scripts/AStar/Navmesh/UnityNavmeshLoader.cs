using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MMO
{
	public class UnityNavmeshLoader : MonoBehaviour
	{

		public Vector3[] vertices;
		public int[] indices;

		void Start ()
		{
			NavMeshTriangulation meshData = NavMesh.CalculateTriangulation ();
			vertices = meshData.vertices;
			indices = meshData.indices;
		}

		void Update ()
		{
		
		}
	}
}