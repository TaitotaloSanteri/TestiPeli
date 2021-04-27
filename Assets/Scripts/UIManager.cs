using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel, saveGamePanel, loadGamePanel;
    [SerializeField]
    private SaveLoadButton[] saveLoadButtons;
    public static bool isPaused;

    private void OnValidate()
    {
        saveLoadButtons = GetComponentsInChildren<SaveLoadButton>(true);
    }

    private void Start()
    {
        pausePanel.SetActive(false);
        saveGamePanel.SetActive(false);
        loadGamePanel.SetActive(false);
    }
    public void ActivatePausePanel()
    {
        pausePanel.SetActive(true);
        saveGamePanel.SetActive(false);
        loadGamePanel.SetActive(false);
    }
    public void ActivateSaveGamePanel()
    {
        pausePanel.SetActive(false);
        saveGamePanel.SetActive(true);
        loadGamePanel.SetActive(false);
        SetSaveLoadButtonValues(true);
    }
    public void ActivateLoadGamePanel()
    {
        pausePanel.SetActive(false);
        saveGamePanel.SetActive(false);
        loadGamePanel.SetActive(true);
        SetSaveLoadButtonValues(false);
    }
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        ActivatePausePanel();
    }
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void ConfirmSave(string fileName)
    {
        WorldManager.instance.SaveWorld(fileName);
        ActivatePausePanel();
    }
    public void ConfirmLoad(string fileName)
    {
        WorldManager.instance.LoadWorld(fileName);
        ActivatePausePanel();
    }

    public int CheckForDigits(string text)
    {
        string numberString = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (int.TryParse(text[i].ToString(), out int newNumber))
            {
                numberString += newNumber;
            }
        }
        return int.Parse(numberString);
    }


    // Save bool muuttujassa l�hetet��n tieto halutaanko k�ytt�� nappuloita tallentamiseen vai lataamiseen.
    // K�ytet��n siis samaa funktiota molempiin operaatioihin.
    public void SetSaveLoadButtonValues(bool save)
    {
        // Tarkistetaan l�ytyyk� hakemistoa
        WorldManager.instance.CreateFolderIfNotExists();
        // Etsit��n kaikki .dat p��tteiset tiedostot hakemistosta
        string[] files = Directory.GetFiles(WorldManager.instance.filePath, "*.dat");
        // Alustetaan SaveLoadButtonit
        for (int i = 0; i < saveLoadButtons.Length; i++)
        {
            // For loopin i muuttuja, pit�� tallentaa toiseen muuttujaan. Muuten sen arvo ei l�hety oikein
            // ConfirmSave / ConfirmLoad funktioon.
            int saveGameIndex = i;
            saveLoadButtons[i].texts[0].text = "";
            saveLoadButtons[i].texts[1].text = "New Save";
            saveLoadButtons[i].texts[2].text = "";
            // Poistetaan onClick eventist� kaikki listenerit, jotta ne eiv�t p��se kasaantumaan. N�in varmistetaan
            // se, ett� kutsutaan aina vain yht� funktiota kun nappulaa painetaan.
            saveLoadButtons[i].button.onClick.RemoveAllListeners();
            if (save)
            {
                // Lis�t��n AddListener lambda funktiona.
                saveLoadButtons[i].button.onClick.AddListener(() => ConfirmSave($"{saveGameIndex}.dat"));
                saveLoadButtons[i].transform.SetParent(saveGamePanel.transform);
            }
            else
            {
                saveLoadButtons[i].button.onClick.AddListener(() => ConfirmLoad($"{saveGameIndex}.dat"));
                saveLoadButtons[i].transform.SetParent(loadGamePanel.transform);
            }
        }
        for (int i = 0; i < files.Length; i++)
        {
            bool isValidSaveFile = int.TryParse(files[i].Substring(files[i].Length - 5, 1), out int index);
            if (isValidSaveFile)
            {
                WorldManager.instance.LoadPreviewData(files[i], saveLoadButtons[index]);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                if (pausePanel.activeSelf)
                {
                    Resume();
                }
                else
                {
                    ActivatePausePanel();
                }
            }
        }
    }


}
