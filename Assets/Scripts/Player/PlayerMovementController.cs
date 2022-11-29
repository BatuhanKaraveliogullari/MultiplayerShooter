using UnityEngine;

public class PlayerMovementController : PlayerController
{
    [SerializeField] private GameObject camera;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 5f;

    private float horizontalMovement;
    private float verticalMovement;
    
    private float cachedXRotation;
    private float cachedYRotation;
    
    private bool isStunned;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        currentPlayerData = new PlayerData(OwnerClientId);
        foreach (var playerController in GetComponents<PlayerController>())
        {
            playerController.Init(currentPlayerData);
        }
    }

    public override void Init(PlayerData playerData)
    {
        base.Init(playerData);
        
        if (!IsOwner)
        {
            Destroy(camera);
        }
        else
        {
            GlobalEventManager.OnOwnerSetSpecification.Invoke(NetworkObject, currentPlayerData);
        }
    }

    private void Update()
    {
        if(isMenuActive || isStunned) return;
        Move();
        Rotate();
    }

    private void Move()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        verticalMovement = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        cachedTransform.position += VectorUtils.CorrectHeight(horizontalMovement * cachedTransform.right + verticalMovement * cachedTransform.forward);
    }

    private void Rotate()
    {
        cachedXRotation += Input.GetAxis("Mouse X") * mouseSensitivity;
        cachedYRotation = Mathf.Clamp(cachedYRotation - (Input.GetAxis("Mouse Y") * mouseSensitivity), -90, 90);
        //Value we got represent the rotation angle along axises. That's why we use Y for X angle.
        cachedTransform.rotation = Quaternion.Euler(cachedYRotation, cachedXRotation, 0);
    }
    
    public void StunPlayer(float stunDuration)
    {
        isStunned = true;
        Invoke(nameof(ReleasePlayer), stunDuration);
    }

    private void ReleasePlayer()
    {
        isStunned = false;
    }
}
