using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PickUpsManager : MonoBehaviour
{
    [SerializeField]
    GameObject pickUpPrefab;

    [SerializeField]
    Transform holder;

    [SerializeField]
    [ReorderableList]
    List<PickUpTier> tiers;

    [SerializeField]
    [ReorderableList]
    List<PickUpData> datas;

    [ReadOnly]
    public List<PickUp> pickUps;

    [ReadOnly]
    [SerializeField]
    List<RarityTier> tiersProbability = new List<RarityTier>();
    [ReadOnly]
    [SerializeField]
    List<PickUpType> typesProbability = new List<PickUpType>();

    [SerializeField]
    Dictionary<RarityTier, PickUpTier> dicTiers = new Dictionary<RarityTier, PickUpTier>();
    [SerializeField]
    Dictionary<PickUpType, PickUpData> dicDatas = new Dictionary<PickUpType, PickUpData>();

    private void OnValidate()
    {
        PopulateDictionary();
    }

    private void Awake()
    {
        OnValidate();
    }

    public void SpawnPickUps(List<Tile> tiles)
    {
        ClearPickUps();

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].content == TileContentType.Empty)
            {
                GameObject obj = Instantiate(pickUpPrefab, tiles[i].transform.position, Quaternion.identity, holder);
                PickUp pickUp = obj.GetComponent<PickUp>();
                SetPickUp(pickUp, tiles[i]);
                pickUps.Add(pickUp);
            }
        }
    }

    void SetPickUp(PickUp pickUp, Tile tile)
    {
        var tier = GetTier();
        var data = GetData();

        pickUp.type = data.type;
        pickUp.value = data.value * tier.multiplier;
        pickUp.spriterRenderer.sprite = data.sprite;
        pickUp.spriterRenderer.color = tier.color;

        pickUp.tile = tile;
        tile.content = TileContentType.Collectable;
        tile.pickUp = pickUp;
    }

    [Button]
    public void ClearPickUps()
    {
        for (int i = 0; i < pickUps.Count; i++)
        {
            pickUps[i].RemoveFromTile();
            
            if (Application.isPlaying)
            {
                Destroy(pickUps[i].gameObject);
            }
            else
            {
                DestroyImmediate(pickUps[i].gameObject);
            }
        }

        pickUps.Clear();
    }

    PickUpTier GetTier()
    {
        if (tiersProbability.Count == 0)
        {
            for (int i = 0; i < tiers.Count; i++)
            {
                for (int j = 0; j < tiers[i].rarity; j++)
                {
                    tiersProbability.Add(tiers[i].tier);
                }
            }
        }

        int id = Random.Range(0, tiersProbability.Count);
        RarityTier rarity = tiersProbability[id];
        tiersProbability.Remove(rarity);

        PickUpTier tier = dicTiers[rarity];

        return tier;
    }

    PickUpData GetData()
    {
        if (typesProbability.Count == 0)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                for (int j = 0; j < datas[i].rarity; j++)
                {
                    typesProbability.Add(datas[i].type);
                }
            }
        }

        int id = Random.Range(0, typesProbability.Count);
        PickUpType type = typesProbability[id];
        typesProbability.Remove(type);

        PickUpData data = dicDatas[type];

        return data;
    }

    [Button]
    void PopulateDictionary()
    {
        dicTiers.Clear();
        dicDatas.Clear();

        for (int i = 0; i < tiers.Count; i++)
        {
            dicTiers.Add(tiers[i].tier, tiers[i]);
        }

        for (int i = 0; i < datas.Count; i++)
        {
            dicDatas.Add(datas[i].type, datas[i]);
        }
    }
}
