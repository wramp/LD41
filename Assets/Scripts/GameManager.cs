﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //this can be removed just for debug
    public GameModes CurrentMode = GameModes.PLATFORMER;

    public delegate void ChangedGameMode(GameModes gm);


    public event ChangedGameMode OnGameModeChange;

    [Header("Player Inventory")]
    public PlayerInventory playerInventory;

    [Header("Camera Switching")]
    public Camera PlatformerCamera;
    public Camera BuildingCamera;

    [Header("Building phase")]
    public int maxInventorySlots = 5;
    public List<GameObject> AllSlots = new List<GameObject>();
    public GameObject TopPanel;
    public GameObject BuildingPhasePanel;
    public GameObject SlotPrefab;


    private int slotCounter = 0;

    private void Start()
    {
        GenerateSlots();
        OnGameModeChange += GameManager_OnGameModeChange;
        playerInventory.OnItemCollected += PlayerInventory_OnItemCollected;

    }

    //Returns how many slots are full
    public int GetOccupiedSlotCounter()
    {
        var result = 0;
        for (int i = 0; i < AllSlots.Count; i++)
        {
            if (AllSlots[i].transform.childCount > 0)
            {
                result++;
            }
        }
        return result;
    }

    public bool AreSlotsFull()
    {
        if (GetOccupiedSlotCounter() >= maxInventorySlots)
        {
            return true;
        }
        return false;
    }

    private void PlayerInventory_OnItemCollected(GameObject uibox)
    {
        //add item to empty slot availiable
        for (int i = 0; i < AllSlots.Count; i++)
        {
            if (AllSlots[i].transform.childCount <= 0)
            {
                AddItemToSlot(i, uibox);
                break;
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            switch (CurrentMode)
            {
                //IF currently game is platformer change it to building
                case GameModes.PLATFORMER:
                    if (OnGameModeChange != null)
                        OnGameModeChange(GameModes.BUILDING);
                    CurrentMode = GameModes.BUILDING;
                    break;
                default:
                    if (OnGameModeChange != null)
                        OnGameModeChange(GameModes.PLATFORMER);
                    CurrentMode = GameModes.PLATFORMER;
                    break;
            }
        }
    }



    private void GameManager_OnGameModeChange(GameModes gm)
    {
        if (gm == GameModes.PLATFORMER)
        {
            //if its changed to platformer
            PlatformerCamera.gameObject.SetActive(true);
            BuildingCamera.gameObject.SetActive(false);
            BuildingPhasePanel.SetActive(false);
            UnFreezeGame();
        }
        else
        {
            PlatformerCamera.gameObject.SetActive(false);
            BuildingCamera.gameObject.SetActive(true);
            BuildingPhasePanel.SetActive(true);
            FrezzeGame();
        }
    }

    //Autogenerate all slots
    private void GenerateSlots()
    {
        for (int i = 0; i < maxInventorySlots; i++)
        {
            var slot = Instantiate(SlotPrefab);
            slot.transform.SetParent(TopPanel.transform);
            AllSlots.Add(slot);
        }
    }

    private void AddItemToSlot(int id, GameObject uibox)
    {
        if (id < AllSlots.Count)
        {
            var box = Instantiate(uibox);
            box.transform.SetParent(AllSlots[id].transform);
        }
    }

    private void FrezzeGame()
    {
        Time.timeScale = 0f;
    }

    private void UnFreezeGame()
    {
        Time.timeScale = 1f;
    }
}

[Serializable]
public enum GameModes
{
    PLATFORMER,
    BUILDING
}