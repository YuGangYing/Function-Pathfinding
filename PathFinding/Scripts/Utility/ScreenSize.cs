using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueNoah.PathFinding
{
    public class ScreenSize : MonoBehaviour
    {

        void Start()
        {
            Screen.SetResolution(1920 / 2, 1080 / 2, false);
        }

    }
}
