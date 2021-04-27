using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectibleBase : MonoBehaviour
{
    public CollectibleData collectibleData;
    public abstract void OnCollected(PlayerData playerData);
}

// System.Serializable ehdottoman t�rke�, tarvitaan tallennukseen ja my�s siihen, ett� muuttujat
// n�kyv�t Unityn inspectorissa.
[System.Serializable]
public class CollectibleData
{
    public float value;
    // Pidet��n type muuttujassa kirjaa ker�tt�v�n varsinaisesta tyypist�
    // esim. HealthCollectible tai MoveSpeedCollectible.
    [HideInInspector] public string type;
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Quaternion rotation;
}
