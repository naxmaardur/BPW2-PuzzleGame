using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToSource : MonoBehaviour
{
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, 25 * Time.deltaTime);
        if(Vector3.Distance(transform.position,transform.parent.position) < 0.3f)
        {
            Destroy(this.gameObject);
        }
    }
}
