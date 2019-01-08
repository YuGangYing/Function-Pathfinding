using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.PathFinding.Int
{
    //Intバージョンのグリド、精度のため。float の不正確確率
    public class Grid : MonoBehaviour
    {

        public int nodeWidth = 100;

        public int diagonalPlus = 140;

        public Vector3Int startPos = new Vector3Int(0, 0, 0);

        public int mXCount = 100;

        public int mZCount = 50;

        GridSetting mGridSetting;

        Node[,] mNodeArray;

        List<Node> mNodeList;

        public int xCount
        {
            get
            {
                return mXCount;
            }
        }

        public int zCount
        {
            get
            {
                return mZCount;
            }
        }

        public void Init(GridSetting gridSetting)
        {
            mGridSetting = gridSetting;
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
                    node.pos = new Vector3Int(node.x * mGridSetting.nodeWidth,0,node.z * mGridSetting.nodeWidth);
                    mNodeList.Add(node);
                    mNodeArray[i, j] = node;
                }
            }

        }




    }
}
