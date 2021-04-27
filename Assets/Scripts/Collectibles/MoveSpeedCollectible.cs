using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedCollectible : CollectibleBase
{
    public override void OnCollected(PlayerData playerData)
    {
        playerData.moveSpeed += collectibleData.value;
    }
}
