using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadButton : MonoBehaviour
{
    public Text[] texts;
    public Button button;

    private void OnValidate()
    {
        texts = GetComponentsInChildren<Text>();
        button = GetComponent<Button>();
    }
}
