using UnityEngine;

namespace BlueNoah.PathFinding.Int
{
    public class GridSetting
    {

        public int halfNodeWidth = 50;

        public int diagonalPlus = 140;

        public Vector3Int startPos = new Vector3Int(0, 0, 0);

        public int xCount = 100;

        public int zCount = 50;

        public int nodeWidth
        {
            get
            {
                return halfNodeWidth * 2;
            }
        }

    }
}

