using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    private int Coin = 0;
    public TextMeshProUGUI coinText;
    public AudioClip collectSound;

    private AudioSource audioSource;
    private HashSet<GameObject> coinsCollected = new HashSet<GameObject>();

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin") && !coinsCollected.Contains(other.gameObject))
        {
            coinsCollected.Add(other.gameObject);

            Coin++;
            coinText.text = "Coins: " + Coin.ToString();
            Debug.Log("Recogida única: " + Coin);

            if (PlayerAudioManager.Instance != null)
            {
                PlayerAudioManager.Instance.PlayCoinSound();
            }


            Destroy(other.gameObject);
        }
    }
}
