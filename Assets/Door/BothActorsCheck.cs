using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BothActorsCheck : MonoBehaviour
{

    public GameObject[] targets;
    private int _targetsInside;

    bool _HasTriggered;



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
        if (_HasTriggered) { return; }
        PlayerActor p;
        if (other.TryGetComponent<PlayerActor>(out p))
        {
            _targetsInside++;
            InsideUpdated(_targetsInside);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(_HasTriggered) { return; }
        PlayerActor p;
        if (other.TryGetComponent<PlayerActor>(out p))
        {
            _targetsInside--;
            InsideUpdated(_targetsInside);
        }
    }

    void InsideUpdated(int count)
    {
        if (_HasTriggered) { return; }
        if (count == 2)
        {
            Trigger();
        }
    }


    void Trigger()
    {
        _HasTriggered = true;
        foreach (GameObject target in targets)
        {
            target.SendMessage("SwapActiveState");
        }
    }
}
