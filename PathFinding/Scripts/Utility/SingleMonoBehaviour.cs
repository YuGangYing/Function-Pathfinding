using UnityEngine;

namespace BlueNoah.PathFinding
{
    public class SingleMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T t;

        public static T GetInstance
        {
            get
            {
                if (t == null)
                {
                    t = GameObject.FindObjectOfType(typeof(T)) as T;
                    if (t == null)
                    {
                        GameObject go = new GameObject();
                        t = go.AddComponent<T>();
                    }
                }
                return t;
            }
        }

        protected virtual void Awake()
        {
            if (t == null)
            {
                t = gameObject.GetComponent<T>();
            }
        }
    }
}

