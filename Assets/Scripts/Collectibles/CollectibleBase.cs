using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectibleBase : MonoBehaviour
{
    public CollectibleData collectibleData;
    public abstract void OnCollected(PlayerData playerData);
}

// System.Serializable ehdottoman tärkeä, tarvitaan tallennukseen ja myös siihen, että muuttujat
// näkyvät Unityn inspectorissa.
[System.Serializable]
public class CollectibleData
{
    public float value;
    // Pidetään type muuttujassa kirjaa kerättävän varsinaisesta tyypistä
    // esim. HealthCollectible tai MoveSpeedCollectible.
    [HideInInspector] public string type;
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Quaternion rotation;
}
