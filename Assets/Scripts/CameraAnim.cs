using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnim : MonoBehaviour
{
    [SerializeField]
    private float _rotationTime = 2f;
    [SerializeField]
    private float _delayBetweenRotations = 1f;

    private WaitForSeconds _rotationDelay;
    private Quaternion _targetRot;

    private bool _rotating;
    private bool rotating = false;

    private bool middle;
    private bool middle2;
    private bool right;
    private bool left;

    public Transform SecurityCamera;

    private void Start()
    {
        _rotationDelay = new WaitForSeconds(_delayBetweenRotations);
        _targetRot = SecurityCamera.localRotation;
        middle = true;
    }

    private void Update()
    {
        if (rotating == false && !_rotating)
        {
            StartCoroutine(WaitingForRotate());
            _rotating = true;

            StartCoroutine(Rotate(_rotationTime));
        }
    }

    private IEnumerator WaitingForRotate()
    {
        rotating = true;
        yield return new WaitForSeconds(_rotationTime);
        rotating = false;
    }    

    private IEnumerator Rotate(float rotateTime)
    {
        var startRot = SecurityCamera.localRotation;
        if(middle == true)
        {
            _targetRot *= Quaternion.AngleAxis(90, Vector3.down); // 90 degrees positive moves to the left , -90 faces to the right facing in the direction of the camera
            middle = false;
            left = true;
        }
        else if(middle2 == true)
        {
            _targetRot *= Quaternion.AngleAxis(-90, Vector3.down);
            middle2 = false;
            right = true;
        }
        else if(right == true) 
        {
            _targetRot *= Quaternion.AngleAxis(90, Vector3.down);
            right = false;
            middle = true;
        }
        else if(left == true) 
        {
            _targetRot *= Quaternion.AngleAxis(-90, Vector3.down);
            left = false;
            middle2 = true;
        }

        var time = 0f;

        while (time <= 1f)
        {
            SecurityCamera.localRotation = Quaternion.Lerp(startRot, _targetRot, time);
            time += Time.deltaTime / rotateTime;
            yield return null;
        }

        SecurityCamera.localRotation = _targetRot;

        yield return _rotationDelay;

        _rotating = false;
    }
}
