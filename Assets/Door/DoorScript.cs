using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private bool _closedOnActive;
    private bool _IsOpen;
    private AudioSource _source;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
    }

    public void SwapActiveState()
    {
        _IsOpen = !_IsOpen;
        _animator.SetBool("Open", _IsOpen);
        _source.Play();
    }

    public void SetActiveState(bool b)
    {
        if (_closedOnActive)
        {
            b = !b;
        }
        _IsOpen = b;
        _animator.SetBool("Open", b);
        _source.Play();
    }
}
