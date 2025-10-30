using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Resolution SO", menuName = "ScriptableObjects/Config/Resolution SO")]
    public class ResolutionSO : ScriptableObject
    {
        public int width;
        public int height;

        public override string ToString()
        {
            return $"{width} X {height}";
        }
    }
}
