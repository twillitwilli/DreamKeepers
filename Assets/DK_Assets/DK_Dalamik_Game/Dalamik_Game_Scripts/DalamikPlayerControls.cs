using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DalamikPlayerControls : MonoBehaviour
{
    [SerializeField]
    Text _text;

    public BoxCollider gameTrigger { get; private set; }
    public DalamikPlayer dalamikPlayer { get; set; }

    private void Awake()
    {
        gameTrigger = GetComponent<BoxCollider>();
        gameTrigger.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IndexFingerTrigger fingerTrigger;

        if (other.gameObject.TryGetComponent<IndexFingerTrigger>(out fingerTrigger))
        {
            if (DalamikGameManager.Instance.playerOrderRoll)
                dalamikPlayer.roll = true;

            else
                DalamikGameManager.Instance.PlayerTouchControl(_text.text);

            gameTrigger.enabled = false;
        }
    }

    public void ChangeTextDisplay(string newText)
    {
        _text.text = newText;
    }
}
