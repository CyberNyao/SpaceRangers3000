using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DestroyByContact : NetworkBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    private GameController gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Enemy")
        {
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (other.tag == "Player")
        {
            if (playerExplosion != null)
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            }
            var isLocal = other.gameObject.GetComponent<PlayerController>().isLocalPlayer;
            if (isLocal)
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
                gameController.GameOver(isServer);
            }
        }
        else
        {
            gameController.AddScore(scoreValue);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}