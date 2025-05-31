using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class playerMoney : MonoBehaviour
{
    public static playerMoney Instance;

    [SerializeField] private float currentMoney = 0;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddMoney(float amount)
    {
        currentMoney += amount;
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = currentMoney.ToString("F2") + " YTL";
    }

}
