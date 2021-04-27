using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;
    // Tiedoston sijainti (hakemisto)
    [HideInInspector] public string filePath;
    [SerializeField] private GameObject world;
    // Taulukko objekteista jotka halutaan luoda uuteen maailmaan. N‰iden pit‰‰
    // ehdottomasti olla PREFABEJA prefabs kansiosta. Ei saa olla suoraan peliscenest‰.
    [SerializeField] private GameObject[] worldObjectPrefabs;

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
        Application.targetFrameRate = 120;
    }
    // Enkoodaus kun tallennetaan pelitilanne, muuttaa JSON datan ei luettavaan (base64string) muotoon.
    private string Encode(string text)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
        return System.Convert.ToBase64String(bytes);
    }
    // Dekoodaus kun ladataan pelitilanne, muuttaa datan (base64string) takaisin JSON muotoon.
    private string Decode(string text)
    {
        byte[] bytes = System.Convert.FromBase64String(text);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
    public void CreateFolderIfNotExists()
    {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
    }

    public void SaveWorld(string fileName)
    {
        // Etsit‰‰n PlayerController maailmasta
        PlayerController pc = world.GetComponentInChildren<PlayerController>();
        // Etsit‰‰n kaikki ker‰tt‰v‰t maailmasta. mm. HealthCollectible, MoveSpeedCollectible jne.
        CollectibleBase[] collectibles = world.GetComponentsInChildren<CollectibleBase>();

        // WorldDataan tallennetaan kaikki maailman tiedot, jotka siirret‰‰n tiedostoon.
        WorldData worldData = new WorldData();
        // Tallennetaan WorldData playerData muuttujaan kaikki tiedot, mukaan lukien
        // pelaajan positio + rotaatio
        worldData.playerData = pc.playerData;
        worldData.playerData.position = pc.transform.position;
        worldData.playerData.rotation = pc.transform.rotation;
        worldData.playerData.playTime = pc.playerData.playTime + Time.realtimeSinceStartup;

        // Alustetaan worlddatasta lˆytyv‰ CollectibleData taulukko. T‰m‰ on tietenkin yht‰ suuri
        // kuin maailmasta lˆytyvien ker‰tt‰vien m‰‰r‰.
        worldData.collectibles = new CollectibleData[collectibles.Length];

        for (int i = 0; i < collectibles.Length; i++)
        {
            worldData.collectibles[i] = collectibles[i].collectibleData;
            worldData.collectibles[i].type = collectibles[i].GetType().ToString();
            worldData.collectibles[i].position = collectibles[i].transform.position;
            worldData.collectibles[i].rotation = collectibles[i].transform.rotation;
        }


        // Muutetaan worldData muuttuja ns. JSON -muotoon.
        string jsonData = JsonUtility.ToJson(worldData, true);
        string finalData = Encode(jsonData);
        CreateFolderIfNotExists();
        File.WriteAllText(filePath + fileName, finalData);
        Debug.Log(filePath);
        Debug.Log(jsonData);
    }

    // Tuhotaan ja luodaan uusi tyhj‰ maailma
    private void DestroyWorld()
    {
        Destroy(world);
        world = new GameObject("World");
        world.transform.position = Vector3.zero;
        world.transform.rotation = Quaternion.identity;
        world.transform.SetParent(null);
    }

    private GameObject FindGameObjectByComponentName(string componentName)
    {
        foreach (GameObject obj in worldObjectPrefabs)
        {
            Component component = obj.GetComponent(componentName);
            if (component)
            {
                return obj;
            }
        }
        Debug.Log($"Component {componentName} not found in any of the objects in WorldObjectPrefabs.");
        return null;
    }

    public void LoadWorld(string fileName)
    {
        // Tarkistetaan, lˆytyykˆ hakemistoa.
        CreateFolderIfNotExists();
        // Tarkistetaan, lˆytyykˆ tiedostoa.
        if (!File.Exists(filePath + fileName))
        {
            Debug.Log($"File {fileName} not found.");
            return;
        }
        DestroyWorld();
        // Haetaan tiedostosta Base64string muodossa oleva tieto.
        string fileData = File.ReadAllText(filePath + fileName);
        // Muutetaan tieto JSON muotoon
        string jsonData = Decode(fileData);
        // Muutetaan JSON muodossa oleva tieto C# -luokaksi (WorldData).
        WorldData worldData = JsonUtility.FromJson<WorldData>(jsonData);
        
        GameObject newPlayer = Instantiate(FindGameObjectByComponentName("PlayerController"), world.transform);
        newPlayer.GetComponent<PlayerController>().playerData = worldData.playerData;
         // Transformin positio ja rotaatio pit‰‰ aina asetta erikseen
        newPlayer.transform.position = worldData.playerData.position;
        newPlayer.transform.rotation = worldData.playerData.rotation;

        // K‰yd‰‰n kaikki WorldDatasta lˆytyv‰t ker‰tt‰v‰t l‰pi.
        foreach (CollectibleData cData in worldData.collectibles)
        {
            // K‰ytet‰‰n CollectibleDatasta lˆytyv‰‰ type muuttujaa p‰‰ttelem‰‰n mink‰ tyyppinen 
            // ker‰tt‰v‰ on kyseess‰. Esim. HealthCollectible tai MoveSpeedCollectible.
            GameObject newCollectible = Instantiate(FindGameObjectByComponentName(cData.type), world.transform);

            // Koska kaikki ker‰tt‰v‰t perii CollectibleBasesta, voidaan suoraan muuttaa arvoja sielt‰.
            newCollectible.GetComponent<CollectibleBase>().collectibleData = cData;
            newCollectible.transform.position = cData.position;
            newCollectible.transform.rotation = cData.rotation;
        }

    }
}
public class WorldData
{
    public PlayerData playerData;
    public CollectibleData[] collectibles;
}
