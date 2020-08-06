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
        MaxHealth = player.MaxHealth;
        HealthRegen = player.HealthRegen;
        MoveSpeed = player.MoveSpeed;
        SprintMultiplier = player.SprintMultiplier;
        DashSpeed = player.DashSpeed;
        DashTime = player.DashTime;
        DashCooldown = player.DashCooldown;

        ItemIDs = player.Inventory.ItemIDs();
    }
}
