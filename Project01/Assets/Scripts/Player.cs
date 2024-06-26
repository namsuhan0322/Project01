using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private Rigidbody rigidbody;
    
    [Header("플레이어 속도")]
    [SerializeField]
    private float speed;

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
    
    void Start() 
    {
        rigidbody = GetComponent<Rigidbody>();
        isJump = false;
    }

    void Update()
    {
        Move();                 // 키보드 입력에 따라 이동
        CameraRotation();       // 마우스를 위아래(Y) 움직임에 따라 카메라 X 축 회전 
        CharacterRotation();    // 마우스 좌우(X) 움직임에 따라 캐릭터 Y 축 회전 
        Jump();                 // 스페이스 입력시 점프
    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");  
        float moveDirZ = Input.GetAxisRaw("Vertical");  
        Vector3 moveHorizontal = transform.right * moveDirX; 
        Vector3 moveVertical = transform.forward * moveDirZ; 

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed; 

        rigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    private void CameraRotation()  
    {
        float xRotation = Input.GetAxisRaw("Mouse Y"); 
        float cameraRotationX = xRotation * lookSensitivity;
        
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()  // 좌우 캐릭터 회전
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(characterRotationY));
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJump)
            {
                isJump = true;
                rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJump = false;
        }
    }
}
