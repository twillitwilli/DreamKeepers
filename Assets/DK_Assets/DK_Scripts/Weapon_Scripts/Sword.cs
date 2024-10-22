using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    BoxCollider _swordTrigger;

    [SerializeField]
    GameObject
        _swingEffect,
        _coverEffect;

    public VRHandController currentHand { get; set; }

    private async void Update()
    {
        if (currentHand != null)
        {
            Debug.Log("current hand velocity with sword = " + currentHand.GetHandVelocity());

            if (currentHand.GetHandVelocity() > 250)
            {
                // Sword Swing Attack
                _swordTrigger.isTrigger = true;

                // Turn On Swing Effect
                _swingEffect.SetActive(true);
            }

            else if (_swordTrigger.isTrigger)
            {
                // Turn off Sword Swing Attack
                _swordTrigger.isTrigger = false;

                // wait half a second
                await Task.Delay(500);

                // turn off swing effect
                _swingEffect.SetActive(false);
            }
        }
    }

    public void TurnOnCoverEffect()
    {
        _coverEffect.SetActive(true);
    }
}
