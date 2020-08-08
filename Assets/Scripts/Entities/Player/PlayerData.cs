using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{

    public float MaxHealth;
    public float HealthRegen;

    public float MoveSpeed;
    public float SprintMultiplier;
    
    public float DashSpeed;
    public float DashTime;
    public float DashCooldown;

    public int[] ItemIDs;


    public PlayerData(Player player)
    {
        PlayerMovement playerMovement = player.GetComponentInChildren<PlayerMovement>();
        PlayerHealth playerHealth = player.GetComponentInChildren<PlayerHealth>();
        Inventory playerInventory = player.GetComponentInChildren<Inventory>();

        MaxHealth = playerHealth.MaxHealth;
        HealthRegen = playerHealth.HealthRegen;
        MoveSpeed = playerMovement.MoveSpeed;
        SprintMultiplier = playerMovement.SprintMultiplier;
        DashSpeed = playerMovement.DashSpeed;
        DashTime = playerMovement.DashTime;
        DashCooldown = playerMovement.DashCooldown;

        ItemIDs = playerInventory.ItemIDs();
    }
}
