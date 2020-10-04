using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    CursorLockMode lockMode;
    
    void Awake () {
        lockMode = CursorLockMode.Locked;
        Cursor.lockState = lockMode;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Starting GameScene.");
    }
  
    // Update is called once per frame
    void Update()
    {
        
    }
}
