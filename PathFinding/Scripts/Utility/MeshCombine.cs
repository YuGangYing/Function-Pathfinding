using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BlueNoah.PathFinding
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [ExecuteInEditMode]
    public class MeshCombine : MonoBehaviour
    {

        public bool isLoad = false;

        void Awake()
        {

        }

        void Update()
        {
            if (isLoad)
            {
                MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
                CombineInstance[] combine = new CombineInstance[meshFilters.Length];
                int i = 0;
                int count = 0;
                while (i < meshFilters.Length)
                {
                    combine[i].mesh = meshFilters[i].sharedMesh;
                    count += combine[i].mesh.vertexCount;
                    combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                    meshFilters[i].GetComponent<MeshRenderer>().enabled = false;
                    i++;
                }
                Debug.Log("Count:" + count);
                transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
                transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
                transform.gameObject.SetActive(false);
                isLoad = false;
            }
        }
    }
}
