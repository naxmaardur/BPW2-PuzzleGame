using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithDistance : MonoBehaviour
{
    void Update()
    {
        transform.localScale = (Vector3.one /6) * Vector3.Distance(transform.position, Camera.main.transform.position);
        if(transform.localScale.x > 2)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
