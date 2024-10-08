using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoT.Classes
{
    public class RenderTrigger : MonoBehaviour
    {
        public GameObject[]
            enableObjs,
            disableObjs;

        private void OnTriggerEnter(Collider other)
        {
            PlayerController player;

            // checks to see if player entered trigger
            if (other.gameObject.TryGetComponent<PlayerController>(out player))
            {
                // enables objects
                if (enableObjs.Length > 0)
                {
                    foreach (GameObject obj in enableObjs)
                        obj.SetActive(true);
                }

                // disables objects
                if (disableObjs.Length > 0)
                {
                    foreach (GameObject obj in disableObjs)
                        obj.SetActive(false);
                }
            }
        }
    }
}
