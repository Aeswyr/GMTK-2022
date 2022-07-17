using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupStorage : MonoBehaviour
{

    void Update()
    {
        if (gameObject.transform.childCount == 0)
        {
            Debug.Log("Ending game!");
            Time.timeScale = 0f;
            // SceneManager.LoadScene(); // load main scene
        }
    }
}
