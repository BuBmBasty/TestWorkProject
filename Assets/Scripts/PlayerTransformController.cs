using UnityEngine;

public class PlayerTransformController : MonoBehaviour
{
   [SerializeField] private float _speed;

   private Transform _playerOne, _playerTwo;
   private TwoPlayersInputSystem _playersInputSystem;
   private Vector3 _direction;
   

   private void Awake()
   {
      _playersInputSystem = new TwoPlayersInputSystem();
      _playerOne = GameObject.FindGameObjectWithTag("PlayerOne").transform;
      _playerTwo = GameObject.FindGameObjectWithTag("PlayerTwo").transform;
   }

   private void OnEnable()
   {
      _playersInputSystem.Enable();
   }

   private void OnDisable()
   {
      _playersInputSystem.Disable();
   }

   private void Update()
   {
      _direction = (_playerTwo.position - _playerOne.position).normalized;
      PlayerOneMovement(_playersInputSystem.Player1.Move.ReadValue<Vector2>());
      PlayerTwoMovement(_playersInputSystem.Player2.Move.ReadValue<Vector2>());
      _playerOne.forward = _direction;
      _playerTwo.forward = -_direction;
   }


   private void PlayerOneMovement(Vector2 direction)
   {
      _playerOne.position += _speed * Time.deltaTime*new Vector3(direction.x,0,direction.y);
   }
   
   private void PlayerTwoMovement(Vector2 direction)
   {
      _playerTwo.position += _speed * Time.deltaTime*new Vector3(direction.x,0,direction.y);
   }
}
