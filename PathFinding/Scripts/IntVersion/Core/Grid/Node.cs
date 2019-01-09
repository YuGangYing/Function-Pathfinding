using UnityEngine;
using System.Collections.Generic;

namespace BlueNoah.PathFinding.Int
{
    public class Node
    {
        public Grid grid;

        public int index;

        public int x;
        //ノードの位置
        public int z;
        //ノードの位置
        public int G;
        //スタート distance.
        public int H;
        //Manhattan distance  曼哈顿算法
        public int F;

        public List<Node> neighbors = new List<Node>();
        //このノードに接続(せつぞく)するノード。
        public List<int> consumes = new List<int>();
        //このノードから接続ノードまで、移動消費(いどうしょうひ)コスト。
        bool mIsBlock;
        //マースを使えからし。
        bool mIsEnable = true;
        //壁中にいるのかどうか,グリードの辺
        public bool isWallSide;
        public int blockNeighborCount;
        //壁に向かってるのかどうか,local position.
        public Vector3Int pos;
        //world position
        public Vector3Int worldPos;
        //ゲーム世界のポジション。
        public Node previous;
        //用以下两个参数来判断是否在open close list，这样就可以不用hashset contain方法了，这样更快。（100*100的grid情况下从8ms降低6ms）
        //用int自增，这样就不用在第二次开始的时候reset这两个值了
        public int isOpen;
        public int isClose;

        public bool IsBlock
        {
            get
            {
                return mIsBlock;
            }
        }

    }
}
