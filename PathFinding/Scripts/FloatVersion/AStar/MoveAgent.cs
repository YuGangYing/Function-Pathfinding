using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.PathFinding
{
    
    public class MoveAgent : MonoBehaviour
    {

        public float movePerFrame = 0.8f;
        public List<Node> path;
        public bool movable = true;
        int mCurrentIndex = 0;
        Transform mTrans;

        void Awake()
        {
            mTrans = transform;
        }

        //時間を使用しない。
        void Update()
        {
            if (path != null && path.Count > mCurrentIndex && movable)
            {
                float moved = 0;
                Vector3 startPos = mTrans.position;
                while (moved < movePerFrame)
                {
                    if (path.Count == mCurrentIndex)
                    {
                        break;
                    }
                    Node node = path[mCurrentIndex];
                    mTrans.LookAt(node.pos);
                    float dis = Vector3.Distance(mTrans.position, node.pos);
                    if (movePerFrame - moved > dis)
                    {
                        moved += dis;
                        mCurrentIndex++;
                        mTrans.position = node.pos;
                    }
                    else
                    {
                        mTrans.position += mTrans.forward * (movePerFrame - moved);
                        moved = movePerFrame;
                    }
                }
            }
        }

        public void Move(List<Node> path)
        {
            this.path = path;
            mCurrentIndex = 0;
        }
    }
}
