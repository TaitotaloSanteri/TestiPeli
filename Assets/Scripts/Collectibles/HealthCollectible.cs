using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : CollectibleBase
{
    public override void OnCollected(PlayerData playerData)
    {
        playerData.health += collectibleData.value;
    }
}
