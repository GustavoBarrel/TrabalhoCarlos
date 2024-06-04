using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // ocorre quando inicia
        Debug.Log("start");

    }

    // Update is called once per frame
    void Update()
    {
        // ocorre a cada frame

        Debug.Log("update");

    }
    public void RestartGame(string lvlName) {
        
        SceneManager.LoadScene(lvlName);
    }
   
}
