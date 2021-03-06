using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerData playerData;

    private void Update()
    {
        if (UIManager.isPaused)
            return;
        playerData.playTime += Time.deltaTime;
        float xAxis = Input.GetAxisRaw("Horizontal");
        float zAxis = Input.GetAxisRaw("Vertical");

        if (xAxis != 0f)
        {
            transform.Rotate(transform.up * xAxis * playerData.rotationSpeed * Time.deltaTime);
        }
        if (zAxis != 0f)
        {
            transform.position += transform.forward * zAxis * playerData.moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Collectible"))
       {
            other.GetComponent<CollectibleBase>().OnCollected(playerData);
            Destroy(other.gameObject);
       }
    }
}


// System.Serializable ehdottoman t?rke?, tarvitaan tallennukseen ja my?s siihen, ett? muuttujat
// n?kyv?t Unityn inspectorissa.
[System.Serializable]
// T?h?n luokkaan kaikki tieto, mit? halutaan tallentaa.
public class PlayerData
{
    public string playerName;
    public float moveSpeed;
    public float rotationSpeed;
    public float health;

    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Quaternion rotation;
    [HideInInspector]
    public float playTime;
}
