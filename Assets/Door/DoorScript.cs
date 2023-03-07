using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    Animator _animator;
    [SerializeField]
    bool _closedOnActive;

    bool _IsOpen;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }




    public void SwapActiveState()
    {
        _IsOpen = !_IsOpen;
        _animator.SetBool("Open", _IsOpen);
    }

    public void SetActiveState(bool b)
    {
        if (_closedOnActive)
        {
            b = !b;
        }
        _IsOpen = b;
        _animator.SetBool("Open", b);
    }
}
