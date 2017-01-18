using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetManager : NetworkManager
{
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        var connectText = GameObject.Find("Connection Text");
        if (connectText != null)
            connectText.GetComponent<Text>().text = "Can't connect to " + GetComponent<NetworkManager>().networkAddress;
        base.OnClientDisconnect(conn);
    }
}
