using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPlayerHealth : MonoBehaviour
{
    Player player;
    TextMeshProUGUI playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();    
    }

    // Update is called once per frame
    void Update()
    {
        string health = player.getHealth().ToString();
        this.playerHealth.text = health;
    }
}
