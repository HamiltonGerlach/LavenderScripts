using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.SceneManagement;


public class MetaMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Startup MetaMain.");
        
        // Thread.Sleep(5000);
        
        Debug.Log("Loading GameScene");
        
        AsyncOperation AsyncTask = SceneManager.LoadSceneAsync(sceneName: "GameScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
