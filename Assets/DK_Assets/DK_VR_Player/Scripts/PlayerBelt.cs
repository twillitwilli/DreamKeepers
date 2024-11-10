using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBelt : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    [Range(-1f, 1f)]
    public float backAttachments = 0;

    [Range(0, 1f)]
    public float heightStandingPlayer = 0.65f;

    [Range(0, 1f)]
    public float heightSittingPlayer = 0.185f;

    [Range(0, 1f)]
    public float zAdjustmentForSittingPlayer = 0.145f;

    void LateUpdate()
    {
        PositionUnderHead();
        HeightUnderHead();
        RotateWithHead();
    }

    void PositionUnderHead()
    {
        if (_player.isStanding)
            transform.localPosition = new Vector3(_player.head.localPosition.x, HeightUnderHead(), _player.head.localPosition.z);

        else
            transform.localPosition = new Vector3(_player.head.localPosition.x, HeightUnderHead(), (_player.head.localPosition.z + zAdjustmentForSittingPlayer));
    }

    float HeightUnderHead()
    {
        Vector3 adjustedHeight = _player.head.localPosition;

        if (_player.isStanding)
        {   
            adjustedHeight.y = Mathf.Lerp(0, adjustedHeight.y, heightStandingPlayer);
            return adjustedHeight.y;
        }

        else
        {
            if (!_player.isCrouched)
            {
                adjustedHeight.y = Mathf.Lerp(0, adjustedHeight.y, heightSittingPlayer);
                return adjustedHeight.y;
            }

            else
            {
                adjustedHeight.y = Mathf.Lerp(0, adjustedHeight.y, 0);
                return adjustedHeight.y;
            }
        }
    }

    void RotateWithHead()
    {
        transform.localEulerAngles = new Vector3(0, (_player.head.eulerAngles.y - _player.head.eulerAngles.z) - _player.transform.eulerAngles.y, 0);
    }

    public void DefaultSettings()
    {
        backAttachments = 0;
        heightStandingPlayer = 0.65f;
        heightSittingPlayer = 0.185f;
        zAdjustmentForSittingPlayer = 0.145f;
    }
}
