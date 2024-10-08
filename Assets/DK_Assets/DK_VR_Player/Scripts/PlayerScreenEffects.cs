using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerScreenEffects : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    Animator _blindnessAnimator;

    bool _headInsideObject;

    // Check to see if head enters an object
    private void OnTriggerEnter(Collider other)
    {
        if (!_headInsideObject && other.gameObject.CompareTag("Wall"))
        {
            _headInsideObject = true;
            CloseVision();
        }
    }

    // Checks to see if head leaves an object
    private void OnTriggerExit(Collider other)
    {
        if (_headInsideObject && other.gameObject.CompareTag("Wall"))
        {
            _headInsideObject = false;
            ClearVision();
        }
    }

    public void CloseVision()
    {
        // Disable movement if vision is closing
        _playerController.disableMovement = true;

        // Enable & Close Vision
        _blindnessAnimator.gameObject.SetActive(true);
        _blindnessAnimator.Play("CloseVision");
    }

    public async void ClearVision()
    {
        // Play animation to clear vision
        _blindnessAnimator.Play("ClearVision");

        // wait 3 seconds before disabling visual gameobject and enabling movement
        await Task.Delay(3000);
        _blindnessAnimator.gameObject.SetActive(false);

        _playerController.disableMovement = false;
    }
}
