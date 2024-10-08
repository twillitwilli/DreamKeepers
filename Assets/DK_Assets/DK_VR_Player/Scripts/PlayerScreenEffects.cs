using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScreenEffects : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    Animator
        _blindnessAnimator,
        _onScreenTextAnimator;

    [SerializeField]
    Text _onScreenText;

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

    public async void DisplayTextOnScreen(string text)
    {
        // set the gameobject active
        _onScreenTextAnimator.gameObject.SetActive(true);

        // change the text on the component to the text being input
        _onScreenText.text = text;

        // plays the animation to the text fading out
        _onScreenTextAnimator.Play("OnScreenDisplayFadeOut");

        //waits 4 seconds then disables the gameobject
        await Task.Delay(4000);
        _onScreenTextAnimator.gameObject.SetActive(false);
    }
}
