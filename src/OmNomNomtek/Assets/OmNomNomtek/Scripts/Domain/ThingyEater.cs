using UnityEngine;

namespace OmNomNomtek.Domain
{
  public class ThingyEater : MonoBehaviour
  {
    [SerializeField]
    private float _movementSpeed = 1.0f;

    [SerializeField]
    private float _rotationSpeed = 1.0f;

    [SerializeField]
    private GameObject _gameObjectToFollow;

    private void FixedUpdate()
    {
      if (_gameObjectToFollow != null)
      {
        Vector3 direction = _gameObjectToFollow.transform.position - this.transform.position;

        this.transform.position += direction.normalized * _movementSpeed * Time.fixedDeltaTime;

        this.transform.rotation = Quaternion.LookRotation(direction);
      }
    }

    public void StartFollowing(GameObject gameObject)
    {
      Debug.Log($"Start following {gameObject.name}!");

      _gameObjectToFollow = gameObject;
    }

    public void StopFollowing()
    {
      Debug.Log($"Stop following!");

      _gameObjectToFollow = null;
    }
  }
}
