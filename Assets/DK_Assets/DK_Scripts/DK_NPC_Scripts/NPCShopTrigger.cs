using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShopTrigger : NPC_Trigger
{
    [SerializeField]
    Transform
        _npcShopPosition,
        _npcLeftShopPosition;

    [SerializeField]
    float _closeTime = 177;

    [SerializeField]
    QuestScrollTrigger _questTrigger;

    private void Start()
    {
        DKTime.timeChanged += ShopClosed;
    }

    public override void NPCTriggerEntered(NPCController npc)
    {
        _questTrigger.AssignShopOwner(npc);

        npc.transform.position = _npcShopPosition.position;
        npc.transform.rotation = _npcShopPosition.rotation;
    }

    public void ShopClosed()
    {
        if (_currentNPC != null && DKTime.Instance.currentTime > _closeTime)
        {
            _questTrigger.ShopOwnerLeft();

            _currentNPC.transform.position = _npcLeftShopPosition.position;
            _currentNPC.transform.rotation = _npcLeftShopPosition.rotation;

            _currentNPC = null;
        }
    }
}
