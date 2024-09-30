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
        // If there is a breakable effect this is where it how it will spawn
        if (_breakableEffect != null)
        {
            // Spawn Effect
            GameObject breakableObj = Instantiate(_breakableEffect);

            // Set Parent to null so it doesnt get destroyed with this object
            breakableObj.transform.SetParent(null);

            // Set position to the position where this is at upon breaking
            breakableObj.transform.position = transform.position;
        }

        Destroy(gameObject);
    }
}
