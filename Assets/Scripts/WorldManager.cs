using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;
    // Tiedoston sijainti (hakemisto)
    [HideInInspector] public string filePath;
    [SerializeField] private GameObject world;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        filePath = Application.persistentDataPath + "/SaveData/";
    }
    public void SaveWorld(string fileName, PlayerController pc)
    {
        // WorldDataan tallennetaan kaikki maailman tiedot, jotka siirret‰‰n tiedostoon.
        WorldData worldData = new WorldData();
        worldData.playerData = pc.playerData;
        worldData.playerData.position = pc.transform.position;
        worldData.playerData.rotation = pc.transform.rotation;
        worldData.playerData.playTime = Time.realtimeSinceStartup;

        string jsonData = JsonUtility.ToJson(worldData, true);
        Debug.Log(jsonData);
    }
}
public class WorldData
{
    public PlayerData playerData;
}
