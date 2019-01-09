using UnityEngine;
using System.Collections;
using BlueNoah.AI;
using System.Collections.Generic;

namespace BlueNoah.PathFinding.Int
{

    public class GridTester : MonoBehaviour
    {
        public Material material;

        PathAgent mPathAgent;

        public UnityEngine.Transform target;

        GridView mGridView;

        void Awake()
        {
            Grid grid = gameObject.AddComponent<Grid>();
            GridSetting gridSetting = new GridSetting();
            gridSetting.diagonalPlus = 140;
            gridSetting.halfNodeWidth = 50;
            gridSetting.startPos = new Vector3Int(0, 0, 0);
            gridSetting.xCount = 50;
            gridSetting.zCount = 100;
            grid.Init(gridSetting);
            mGridView = gameObject.AddComponent<GridView>();
            mGridView.InitGridView(gridSetting.xCount, gridSetting.zCount, gridSetting.nodeWidth / 100f, 0.1f, material, 0);
            mGridView.ShowGrid();
            mGridView.gameObject.layer = 10;
            mGridView.boxCollider.gameObject.layer = 10;

            mPathAgent = new PathAgent(grid);
            mPathAgent.transform.position = new Vector3Int(2, 0, 2);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 viewPos = (Vector3)Input.mousePosition;
                viewPos.z = 10;

                mPathAgent.transform.position = Vector3Int.RoundToInt(target.position * 100);

                Vector3 pos = Camera.main.ScreenToWorldPoint(viewPos);
                Debug.Log(Input.mousePosition);
                RaycastHit raycastHit;
                if (Physics.Raycast(Camera.main.transform.position, (pos - Camera.main.transform.position).normalized, out raycastHit, Mathf.Infinity, 1 << 10))
                {
                    Vector3 hitPosition = raycastHit.point * 100;
                    Vector3Int intPosition = Vector3Int.CeilToInt(hitPosition);
                    List<Node> nodeList = mPathAgent.StartFind(intPosition);

                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        Node node = nodeList[i];
                        mGridView.HoverNodeColorByWorldPosition((Vector3)node.pos * 0.01f);
                    }
                    mGridView.ApplyColors();
                    Debug.Log(nodeList.Count);
                }
            }
        }
    }

}

