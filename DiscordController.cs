using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordController : MonoBehaviour
{
    public Discord.Discord discord;


    public void Start()
    {
        discord = new Discord.Discord(00000000000000, (System.UInt64)Discord.CreateFlags.Default);
        if (Debug.isDebugBuild)
        {
            UpdateActivity("Idle", "Development Build: v" + Globals.Version);
        }
        else UpdateActivity("Idle", "");
    }

    void Update()
    {
        discord.RunCallbacks();
    }

    public void UpdateActivity(string state, string details)
    {
        var activityManager = discord.GetActivityManager();

        var activity = new Discord.Activity
        {
            State = state,
            Details = details
        };

        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Updated Discord Activity!");
            }
        });
    }

    private void OnApplicationQuit()
    {
        discord.Dispose();
    }

}
