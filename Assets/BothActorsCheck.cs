using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BothActorsCheck : MonoBehaviour
{

    public GameObject[] targets;
    private int _targetsInside;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerActor p;
        if (other.TryGetComponent<PlayerActor>(out p))
        {
            _targetsInside++;
            InsideUpdated(_targetsInside);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerActor p;
        if (other.TryGetComponent<PlayerActor>(out p))
        {
            _targetsInside--;
            InsideUpdated(_targetsInside);
        }
    }

    void InsideUpdated(int count)
    {
        if(count == 2)
        {
            Activate(true);
        }
        else
        {
            Activate(false);
        }
    }


    void Activate(bool b)
    {
        Debug.Log(b);
        foreach (GameObject target in targets)
        {
            target.SendMessage("SetActiveState", b);
        }
    }
}
