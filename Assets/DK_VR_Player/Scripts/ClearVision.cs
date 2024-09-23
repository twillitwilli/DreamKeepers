using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearVision : MonoBehaviour
{
    public void VisionCleared()
    {
        gameObject.SetActive(false);
    }
}
