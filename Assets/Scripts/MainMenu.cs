using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour
{
    public GameObject connectPanel;
    public Text connectionText;
    public Text ipAddress;

    NetworkManager netManager;

    public void Start()
    {
        netManager = GameObject.Find("Network Manager").GetComponent<NetworkManager>();
    }

    public void StartLevel()
    {
        netManager.StartHost();
    }

    public void ToggleConnectPanel()
    {
        connectPanel.SetActive(!connectPanel.activeSelf);
        if (netManager.client != null)
        {
            netManager.StopClient();
        }
        connectionText.text = string.Empty;
    }

    public void JoinGame()
    {
        if (!string.IsNullOrEmpty(ipAddress.text))
        {
            netManager.networkAddress = ipAddress.text;
            connectionText.text = string.Format("Connecting to {0} ...", netManager.networkAddress);
            netManager.StartClient();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
