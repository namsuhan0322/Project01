using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Animator _animator;

    public GameObject scanObj;

    public int allPrice;

    [Header("플레이어 속도")]
    [SerializeField]
    private float speed;
    private bool isMoving;
    public float normalSpeed, runSpeed;

    [Header("카메라 속도")]
    [SerializeField]
    private float lookSensitivity;

    [Header("카메라 가동범위")]
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX;

    [Header("카메라")]
    [SerializeField]
    private Camera theCamera;

    [Header("플레이어 점프")]
    [SerializeField]
    private float jumpForce;
    private bool isJump;

    private bool isScanning = false;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        isJump = false;
        scanObj.SetActive(false);
        allPrice = 0;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        Move();
        CameraRotation();
        CharacterRotation();
        Jump();
        StartScan();
    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");
        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        rigidbody.MovePosition(transform.position + velocity * Time.deltaTime);

        if (moveDirX == 0 && moveDirZ == 0)
        {
            _animator.SetBool("isWalk", false);
        }
        else
        {
            _animator.SetBool("isWalk", true);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
            _animator.SetBool("isRun", true);
        }
        else
        {
            speed = normalSpeed;
            _animator.SetBool("isRun", false);
        }
    }

    private void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;

        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(characterRotationY));
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJump)
            {
                isJump = true;
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _animator.SetBool("isJump", true);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJump = false;
            _animator.SetBool("isJump", false);
        }
    }

    private void StartScan()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isScanning)
        {
            isScanning = true;
            scanObj.SetActive(true);
            Scan();
        }
    }

    private void Scan()
    {
        scanObj.transform.DOScale(new Vector3(40, 40, 40), 2).OnComplete(() =>
        {
            scanObj.SetActive(false);
            scanObj.transform.localScale = Vector3.one;
            isScanning = false;
            
            allPrice = 0; // 스캔 완료 후 가격 초기화
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            ItemPickUp itemPickUp = other.GetComponent<ItemPickUp>();
            
            if (itemPickUp != null)
            {
                Debug.Log($"아이템 이름: {itemPickUp.item.itemName}");
                Debug.Log($"아이템 가격: {itemPickUp.item.Value}");
                
                allPrice += itemPickUp.item.Value;
                Debug.Log($"총 아이템 가격: {allPrice}");
            }
        }
    }
}
