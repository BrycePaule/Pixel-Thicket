
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    static string path = Application.persistentDataPath + "/player.save";

    public static void SavePlayerData(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(path, FileMode.Create);

        PlayerData playerData = new PlayerData(player);

        formatter.Serialize(file, playerData);
        file.Close();
    }

    public static PlayerData LoadPlayerData()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);

            PlayerData playerData = formatter.Deserialize(file) as PlayerData;
            file.Close();

            return playerData;
        }
        else
        {
            Debug.Log("Failed to load player data");
            return null;
        }
    }

    public static void LoadPlayer(PlayerData data, Player player)
    {
        PlayerMovement playerMovement = player.GetComponentInChildren<PlayerMovement>();
        PlayerHealth playerHealth = player.GetComponentInChildren<PlayerHealth>();
        Inventory playerInventory = player.GetComponentInChildren<Inventory>();

        playerHealth.MaxHealth = data.MaxHealth;
        playerHealth.HealthRegenPerSecond = data.HealthRegen;
        playerMovement.MoveSpeed = data.MoveSpeed;
        playerMovement.SprintMultiplier = data.SprintMultiplier;
        playerMovement.DashSpeed = data.DashSpeed;
        playerMovement.DashTime = data.DashTime;
        playerMovement.DashCooldown = data.DashCooldown;

        playerInventory.ClearInventory();
        foreach (var ID in data.ItemIDs)
        {
            playerInventory.Add(ID);
        }
    }
}
