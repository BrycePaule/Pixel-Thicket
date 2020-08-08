
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
        player.MaxHealth = data.MaxHealth;
        player.HealthRegen = data.HealthRegen;
        player.MoveSpeed = data.MoveSpeed;
        player.SprintMultiplier = data.SprintMultiplier;
        player.DashSpeed = data.DashSpeed;
        player.DashTime = data.DashTime;
        player.DashCooldown = data.DashCooldown;

        player.Inventory.ClearInventory();
        foreach (var ID in data.ItemIDs)
        {
            player.Inventory.Add(ID);
        }
    }
}
