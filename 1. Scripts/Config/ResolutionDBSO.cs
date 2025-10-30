using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    [CreateAssetMenu(fileName = "Resolution DBSO", menuName = "ScriptableObjects/Config/Resolution DBSO")]
    public class ResolutionDBSO : ScriptableObject
    {
        public List<ResolutionSO> container;
    }
}