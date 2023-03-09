using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithDistance : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        transform.localScale = (Vector3.one /6) * Vector3.Distance(transform.position, Camera.main.transform.position);
    }
}
