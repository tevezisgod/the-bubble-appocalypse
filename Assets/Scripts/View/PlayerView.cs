using System.Threading.Tasks;
using Controllers;
using UnityEngine;

public class PlayerView: MonoBehaviour
{
    private PlayerController _playerController;
    private float _movementOrientation;
    internal Rigidbody2D _playerRigidbody;

    public void Init(PlayerController controller)
    {
        _playerController = controller;
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameController.Config.BallTagName))
        {
            _playerController.PlayerWasHit();
        }
    }
}