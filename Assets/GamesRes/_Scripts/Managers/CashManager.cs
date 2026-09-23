using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public static CashManager Instance;

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("Settings")]
    [SerializeField] private int coins;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        LoadData();
        UpdateCoinContainer();
    }
    public void AddCoins(int amount)
    {
        coins += amount;
        //Debug.Log("We have " + coins + " now!");

        UpdateCoinContainer();
        SaveData();
    }
    public void UseCoin(int amount) => AddCoins(-amount);
    public void UpdateCoinContainer() => coinText.text = coins.ToString();
    public int GetCoins() => coins;
    private void LoadData() => coins = PlayerPrefs.GetInt("Coins");
    private void SaveData() => PlayerPrefs.SetInt("Coins", coins);

    // Testing Purpose
    [NaughtyAttributes.Button]
    private void Add500Coins() => AddCoins(500);
}
