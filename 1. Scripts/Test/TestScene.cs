using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KJ
{
    public class TestScene : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log(SceneManager.GetActiveScene().name);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}