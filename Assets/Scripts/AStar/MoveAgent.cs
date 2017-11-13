using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YYGAStar;

public class MoveAgent : MonoBehaviour {

	public float speed = 0.1f;
	public List<Node> path;
	public bool movable = true;

	int mCurrentIndex = 0;
	Transform mTrans;

	void Awake(){
		mTrans = transform;
	}

	//以帧的方式移动。
	void Update(){
		if(path!=null && path.Count > mCurrentIndex && movable){
			float moved = 0;
			Vector3 startPos = mTrans.position;
			while(moved < speed){
				if (path.Count ==  mCurrentIndex) {
					break;
				}
				Node node = path[mCurrentIndex];
				mTrans.LookAt (node.pos);
				float dis = Vector3.Distance (mTrans.position,node.pos);
				if (speed - moved > dis) {
					moved += dis;
					mCurrentIndex++;
					mTrans.position = node.pos;
				} else {
					mTrans.position += mTrans.forward * (speed - moved);
					moved = speed;
				}
			}
		}
	}

	public void Move(List<Node> path){
		this.path = path;
		mCurrentIndex = 0;
	}

}
