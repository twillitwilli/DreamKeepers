using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bucket : MonoBehaviour
{
    public bool bucketFull { get; private set; }

    public string lookingForItem;

    public int
        totalValue = 0,
        valueNeededForQuest;

    [SerializeField]
    Text _displayValue;

    private void OnTriggerEnter(Collider other)
    {
        QuestItem questItem;

        if (other.gameObject.TryGetComponent<QuestItem>(out questItem) && questItem.itemName == lookingForItem)
        {
            totalValue += questItem.itemValue;

            if (totalValue >= valueNeededForQuest)
            {
                totalValue = valueNeededForQuest;

                bucketFull = true;
            }
                

            _displayValue.text = totalValue.ToString() + "/" + valueNeededForQuest.ToString();

            Destroy(questItem.gameObject);
        }
    }
}
