using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grabable))]
public class EquipableItem : MonoBehaviour
{
    public GameItems.EquipableItem equipableType;

    [SerializeField]
    Item _item;

    [SerializeField]
    Vector3
        _itemPosition,
        _itemRotation,
        _itemScale;

    public void PositionItem()
    {
        gameObject.transform.localPosition = _itemPosition;
        gameObject.transform.localEulerAngles = _itemRotation;
        gameObject.transform.localScale = _itemScale;
    }
}
