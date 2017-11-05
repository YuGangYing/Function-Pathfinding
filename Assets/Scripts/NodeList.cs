using UnityEngine;
using System.Collections;
using System;

public interface INode<T> : IComparable<T> {
	int NodeIndex {
		get;
		set;
	}
}

public class NodeList<T> where T : INode<T> {
	
	T[] mItems;
	int mCurrentCount;
	
	public NodeList(int maxSize) {
		mItems = new T[maxSize];
	}
	
	public void Add(T item) {
		item.NodeIndex = mCurrentCount;
		mItems[mCurrentCount] = item;
		SortUp(item);
		mCurrentCount++;
	}

	public T RemoveFirst() {
		T firstItem = mItems[0];
		mCurrentCount--;
		mItems[0] = mItems[mCurrentCount];
		mItems[0].NodeIndex = 0;
		SortDown(mItems[0]);
		return firstItem;
	}

	public void UpdateItem(T item) {
		SortUp(item);
	}

	public int Count {
		get {
			return mCurrentCount;
		}
	}

	public bool Contains(T item) {
		return Equals(mItems[item.NodeIndex], item);
	}

	void SortDown(T item) {
		while (true) {
			int childIndexLeft = item.NodeIndex * 2 + 1;
			int childIndexRight = item.NodeIndex * 2 + 2;
			int swapIndex = 0;
			if (childIndexLeft < mCurrentCount) {
				swapIndex = childIndexLeft;
				if (childIndexRight < mCurrentCount) {
					if (mItems[childIndexLeft].CompareTo(mItems[childIndexRight]) < 0) {
						swapIndex = childIndexRight;
					}
				}
				if (item.CompareTo(mItems[swapIndex]) < 0) {
					Swap (item,mItems[swapIndex]);
				}
				else {
					return;
				}
			}
			else {
				return;
			}
		}
	}
	
	void SortUp(T item) {
		int parentIndex = (item.NodeIndex-1)/2;
		while (true) {
			T parentItem = mItems[parentIndex];
			if (item.CompareTo(parentItem) > 0) {
				Swap (item,parentItem);
			}
			else {
				break;
			}
			parentIndex = (item.NodeIndex-1)/2;
		}
	}
	
	void Swap(T itemA, T itemB) {
		mItems[itemA.NodeIndex] = itemB;
		mItems[itemB.NodeIndex] = itemA;
		int itemAIndex = itemA.NodeIndex;
		itemA.NodeIndex = itemB.NodeIndex;
		itemB.NodeIndex = itemAIndex;
	}
}
