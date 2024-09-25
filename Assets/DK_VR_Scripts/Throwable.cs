using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public float throwableVelocity { get; set; }

    [SerializeField]
    [Tooltip("If 0, item wont break with velocity")]
    float _breakableVelocity;

    [SerializeField]
    GameObject _breakableEffect;

    private void OnCollisionEnter(Collision collision)
    {
        // If breakable velocity is set & thrown velocity is greater than breakable velocity, will break object
        if (_breakableVelocity != 0 && throwableVelocity >= _breakableVelocity)
            BreakObject();
    }

    void BreakObject()
    {
        if (_breakableEffect != null)
        {
            GameObject breakableObj = Instantiate(_breakableEffect);

            breakableObj.transform.SetParent(null);

            breakableObj.transform.position = transform.position;
        }

        Destroy(gameObject);
    }
}
