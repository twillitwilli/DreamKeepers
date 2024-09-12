using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField]
    PlayerController _player;

    float _yValue;
    Vector3 _changeYValue;
    Vector3 _overlapBoxSize = new Vector3(0.16f, 0.1f, 0.16f);

    public bool GroundCheck()
    {
        PositionUnderHead();

        Collider[] groundobjects = Physics.OverlapBox(transform.position, _overlapBoxSize, transform.rotation);

        foreach (Collider col in groundobjects)
        {
            if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Rock"))
                return true;
        }
        return false;
    }

    public void PositionUnderHead()
    {
        //world space
        Vector3 xyValue = new Vector3();

        // move box in local space
        xyValue.x = _player.head.position.x;
        xyValue.z = _player.head.position.z;

        //apply
        transform.position = xyValue;

        //local space
        _changeYValue = transform.localPosition;

        if (!_player.playerStanding && !_player.isCrouched)
            _yValue = -0.93f;

        else
            _yValue = 0f;

        transform.localPosition = new Vector3(_changeYValue.x, _yValue, _changeYValue.z);
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    //    void OnDrawGizmos()
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireCube(transform.position, (_overlapBoxSize * 2));
    //    }
}
