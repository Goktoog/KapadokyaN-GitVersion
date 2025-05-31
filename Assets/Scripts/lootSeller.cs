using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lootSeller : MonoBehaviour
{
    public static lootSeller Instance;
    [SerializeField] private GameObject lootSellLocation;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("loot"))
        {
            // LootInfo bileþenini al
            LootInfo loot = other.GetComponent<LootInfo>();

            if (loot != null)
            {
                // Para ekle
                playerMoney.Instance.AddMoney(loot.itemPrice);

                // Item'ý yok et
                Destroy(other.gameObject);
            }
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

}
