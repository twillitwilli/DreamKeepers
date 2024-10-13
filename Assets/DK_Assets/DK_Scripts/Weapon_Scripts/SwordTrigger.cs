using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // if sword hits throwable item will break the object
        Throwable throwableItem;
        if (other.gameObject.TryGetComponent<Throwable>(out throwableItem))
            throwableItem.BreakObject();
    }
}
