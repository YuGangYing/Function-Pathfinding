using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.PathFinding.Int
{
    //Intバージョンのグリド、精度のため。float の不正確確率
    public class Grid : MonoBehaviour
    {

        public Vector3Int startPos = new Vector3Int(0, 0, 0);

        GridSetting mGridSetting;

        Node[,] mNodeArray;

        List<Node> mNodeList;

        bool mShowGizmos = true;

        public int xCount
        {
            get
            {
                return mGridSetting.xCount;
            }
        }

        public int zCount
        {
            get
            {
                return mGridSetting.zCount;
            }
        }

        public void Init(GridSetting gridSetting)
        {
            mGridSetting = gridSetting;
            Init();
        }

        void Init()
        {
            mNodeArray = new Node[mGridSetting.xCount, mGridSetting.zCount];

            mNodeList = new List<Node>();

            for (int i = 0; i < mGridSetting.xCount; i++)
            {
                for (int j = 0; j < mGridSetting.zCount; j++)
                {
                    Node node = new Node();
                    node.x = i;
                    node.z = j;
                    node.pos = new Vector3Int(node.x * mGridSetting.nodeWidth + mGridSetting.halfNodeWidth, 0, node.z * mGridSetting.nodeWidth + mGridSetting.halfNodeWidth);
                    mNodeList.Add(node);
                    mNodeArray[i, j] = node;
                }
            }

            CalculateNeighbors();
        }

        void CalculateNeighbors()
        {
            for (int i = 0; i < mNodeList.Count; i++)
            {
                CalculateNeighbor(mNodeList[i]);
            }
        }

        void CalculateNeighbor(Node node)
        {
            bool north = false;
            bool south = false;
            bool east = false;
            bool west = false;
            node.neighbors.Clear();
            node.consumes.Clear();
            int i = node.x;
            int j = node.z;
            if (i > 0)
            {
                west = AddNeighbor(node, mNodeArray[i - 1, j]);
            }

            if (j > 0)
            {
                south = AddNeighbor(node, mNodeArray[i, j - 1]);
            }

            if (i < mGridSetting.xCount - 1)
            {
                east = AddNeighbor(node, mNodeArray[i + 1, j]);
            }

            if (j < mGridSetting.zCount - 1)
            {
                north = AddNeighbor(node, mNodeArray[i, j + 1]);
            }

            if (i > 0 && j > 0)
            {
                if (west && south)
                {
                    AddNeighbor(node, mNodeArray[i - 1, j - 1]);
                }
            }

            if (i > 0 && j < mGridSetting.zCount - 1)
            {
                if (west && north)
                {
                    AddNeighbor(node, mNodeArray[i - 1, j + 1]);
                }
            }

            if (i < mGridSetting.xCount - 1 && j > 0)
            {
                if (east && south)
                {
                    AddNeighbor(node, mNodeArray[i + 1, j - 1]);
                }
            }

            if (i < mGridSetting.xCount - 1 && j < mGridSetting.zCount - 1)
            {
                if (east && south)
                {
                    AddNeighbor(node, mNodeArray[i + 1, j + 1]);
                }
            }
        }

        bool AddNeighbor(Node node, Node neighbor)
        {
            if (!neighbor.IsBlock)
            {
                node.neighbors.Add(neighbor);
                node.consumes.Add(mGridSetting.nodeWidth);
                return true;
            }
            return false;
        }

        public Node GetNearestNode(Vector3Int pos)
        {
            int xIndex = pos.x / mGridSetting.nodeWidth;
            int yIndex = pos.z / mGridSetting.nodeWidth;
            xIndex = Mathf.Clamp(xIndex, 0, mGridSetting.xCount - 1);
            yIndex = Mathf.Clamp(yIndex, 0, mGridSetting.zCount - 1);
            return GetNode(xIndex, yIndex);
        }

        public Node GetNode(Vector3Int pos)
        {
            int xIndex = pos.x / mGridSetting.nodeWidth;
            int yIndex = pos.z / mGridSetting.nodeWidth;
            return GetNode(xIndex, yIndex);
        }

        public Node GetNode(int xIndex, int yIndex)
        {
            if (xIndex < 0 || xIndex >= mGridSetting.xCount || yIndex < 0 || yIndex >= mGridSetting.zCount)
            {
                return null;
            }
            else
            {
                return mNodeArray[xIndex, yIndex];
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (mShowGizmos && mNodeList != null)
            {
                for (int i = 0; i < mNodeList.Count; i++)
                {
                    if (mNodeList[i] != null)
                    {
                        if (!mNodeList[i].IsBlock)
                        {
                            //TODO 4面
                            if (mNodeList[i].neighbors.Count < 8 || mNodeList[i].blockNeighborCount > 0)
                            {
                                Gizmos.color = Color.blue;
                            }
                            else
                            {
                                Gizmos.color = Color.green;
                            }
                        }
                        else
                        {
                            Gizmos.color = Color.red;
                        }
                        Gizmos.DrawWireCube(new Vector3(mNodeList[i].pos.x / 100f,mNodeList[i].pos.y / 100f,mNodeList[i].pos.z / 100f)   , Vector3.one * mGridSetting.halfNodeWidth / 100f);
                    }
                }
                //if (x0 < mXCount && x0 >= 0 && y0 < mZCount && y0 >= 0)
                //{
                //    Node node = mNodeArray[x0, y0];
                //    if (node.neighbors != null)
                //    {
                //        for (int i = 0; i < node.neighbors.Count; i++)
                //        {
                //            Gizmos.color = Color.green;
                //            Gizmos.DrawWireCube(mTrans.TransformPoint(node.neighbors[i].pos), Vector3.one * mNodeSize * 0.5f);
                //        }
                //    }
                //}
            }
        }
#endif
    }
}
