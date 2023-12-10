using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    void Start()
    {
        transform.position = new Vector3(transform.position.x + Screen.width, Screen.height / 2, 0);    
    }
}
