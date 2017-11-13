using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YYGAStar
{

	[ExecuteInEditMode]
	public class CreateMapFromImage : MonoBehaviour
	{

		public Texture2D tex;
		public bool load;
		public int offsetX = 56;
		public int offsetY = 30;
		public float gridSize = 14f;
//pixel
		public List<GameObject> items;
		public Button btn_create;

		void Awake ()
		{
			btn_create.onClick.AddListener (() => {
				Create ();
			});
		}

		void Update ()
		{
			if (load) {
				Create ();
				load = false;
			}
		}

		void Create ()
		{
			for (int i0 = 0; i0 < items.Count; i0++) {
				DestroyImmediate (items [i0]);
			}
			items.Clear ();
			_ImmCreate ();
		}

		void _ImmCreate ()
		{
			int i = 0;
			while (offsetX + Mathf.RoundToInt (i * gridSize) < tex.width) {
				int j = 0;
				while (offsetY + Mathf.RoundToInt (j * gridSize) < tex.height) {
					if (CompareColor (tex.GetPixel (offsetX + Mathf.RoundToInt (i * gridSize), offsetY + Mathf.RoundToInt (j * gridSize)))) {
						items.Add (CreateObject (i, j));
					}
					j++;
				}
				i++;
			}
		}


		IEnumerator _Create ()
		{
			int i = 0;
			while (offsetX + Mathf.RoundToInt (i * gridSize) < tex.width) {
				int j = 0;
				while (offsetY + Mathf.RoundToInt (j * gridSize) < tex.height) {
					if (CompareColor (tex.GetPixel (offsetX + Mathf.RoundToInt (i * gridSize), offsetY + Mathf.RoundToInt (j * gridSize)))) {
						items.Add (CreateObject (i, j));
					}
					j++;
				}
				yield return new WaitForSeconds (0.1f);
				i++;
			}
			Debug.Log ("Done");
		}

		public float objectSize = 1.2f;
		public float objectOffsetX = 10f;
		public float objectOffsetY = 10f;

		GameObject CreateObject (int x, int z)
		{
			GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);
			go.transform.localScale = new Vector3 (objectSize, 1, objectSize);// Vector3.one * objectSize;
			go.transform.position = new Vector3 (x + objectOffsetX, 0, z + objectOffsetY) * objectSize;
			go.layer = Grid.blockLayer;
			return go;
		}

		IEnumerator _Move (Transform trans, Vector3 start, Vector3 target)
		{
			float t = 0;
			float duration = 0.1f;
			while (t < 1) {
				;
				if (trans == null)
					yield break;
				t += Time.deltaTime / duration;
				trans.position = Vector3.Lerp (start, target, t);
				yield return null;
			}
		}

		bool CompareColor (Color c1)
		{
			if (c1.g < 0.1f && c1.b < 0.1f) {
				return true;
			}
			return false;
		}
	}


}
