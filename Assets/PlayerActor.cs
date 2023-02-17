using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField]
    private Transform _cameraPoint;
    [SerializeField]
    private GameObject _camera;

    [SerializeField]
    private bool active;

    [SerializeField] private float maxUpAngle = 80.0f;
    [SerializeField] private float maxDownAngle = 80.0f;


    private float _rotationX = 0.0f;
    private float _rotationY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _camera.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Rotate(float rotationX = 0.0f,float rotationY = 0.0f)
    {
        _rotationX += rotationX;
        _rotationY += active?rotationY:-rotationY;
        transform.localRotation = Quaternion.Euler(0, _rotationY, 0.0f);
        Quaternion cameraRotation = Quaternion.Euler(_rotationX,0,0);
        Quaternion clampedRotation = ClampRotation(cameraRotation);
        // Apply the clamped rotation to the cameraPoint transform
        _cameraPoint.localRotation = clampedRotation;

        _rotationX = _cameraPoint.eulerAngles.x;
        _rotationY = transform.eulerAngles.y;
    }

    Quaternion ClampRotation(Quaternion rotation)
    {
        // Clamp the rotation to limit how far up/down the camera can look
        Vector3 eulerAngles = rotation.eulerAngles;
        eulerAngles.x = ClampAngle(eulerAngles.x, maxDownAngle, maxUpAngle);
        return Quaternion.Euler(eulerAngles);
    }

    float ClampAngle(float angle, float min, float max)
    {
        float minAngle = 360 - min;
        if (angle >= minAngle -10)
        {
            angle = Mathf.Clamp(angle, minAngle, 360);
        }
        else
        {
           angle = Mathf.Clamp(angle, -90, max);
        }
        return angle;
    }


    public void Move(Vector3 vector)
    {
        vector.x = active ? vector.x : -vector.x;
        _controller.Move(transform.TransformDirection(vector));
    }

    public void SwapActive()
    {
        active = !active;
        _camera.SetActive(active);
    }

}