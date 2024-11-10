using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingProfiles : MonoBehaviour
{
    PlayerScreenEffects _playerScreenEffects;

    PostProcessVolume _postProcessingVolume;

    public enum PostProcessingProfileType
    {
        BasicSetup,
        Nightmare,
        DayTime,
        NightTime
    }

    public PostProcessingProfileType currentProfile { get; set; }

    [SerializeField]
    PostProcessProfile[] _postProcessingProfiles;

    private void Start()
    {
        // gets players screen effects component
        _playerScreenEffects = PlayerController.Instance.head.GetComponent<PlayerScreenEffects>();

        // gets the players post processing volume 
        _postProcessingVolume = _playerScreenEffects.postProcessingVolume;

        DKTime.timeChanged += ChangeToDayNightProfile;
    }

    public void ChangePostProcessingProfile(PostProcessingProfileType whichProfile)
    {
        // sets current profile to the one its being changed to
        currentProfile = whichProfile;

        // gets the index of enum from which profile its being changed to
        int profileIndex = (int)whichProfile;

        // sets the new post processing profile to the players post processing volume
        _postProcessingVolume.profile = _postProcessingProfiles[profileIndex];
    }

    public async void ChangeToDayNightProfile()
    {
        if (DKTime.Instance.currentTime > 2 && DKTime.Instance.currentTime < 178)
        {
            _playerScreenEffects.CloseVision();

            _playerScreenEffects.DisplayTextOnScreen("Sunrise");

            ChangePostProcessingProfile(PostProcessingProfileType.DayTime);

            await Task.Delay(3000);

            _playerScreenEffects.ClearVision();
        }
            

        else
        {
            _playerScreenEffects.CloseVision();

            _playerScreenEffects.DisplayTextOnScreen("Night");

            ChangePostProcessingProfile(PostProcessingProfileType.DayTime);

            await Task.Delay(3000);

            _playerScreenEffects.ClearVision();
        }
    }
}
