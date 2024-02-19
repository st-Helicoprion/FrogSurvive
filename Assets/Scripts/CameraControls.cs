using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Camera mainCamera;
    public Transform mainCamTarget, playerPos;
    public float chaseSpeed, rotSpeed;
    public float XRot, YRot;
    public Vector2 turn;
    public bool isPC, isFree;
    public Joystick joystick;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if(Application.isMobilePlatform)
        {
            isPC = false;
            rotSpeed *= 2;
        }
        else isPC= true;

    }

    // Update is called once per frame
    void Update()
    {
        if (isFree)
        {
            StopHuggingplayer();
        }
    }

    private void LateUpdate()
    {
       
        Vector3 offset = new Vector3(0, 1, 0);
        if (mainCamTarget.position != playerPos.position + offset)
        {
            chaseSpeed += Time.deltaTime;
            mainCamTarget.position = Vector3.Lerp(mainCamTarget.position, playerPos.position+offset, chaseSpeed*Time.deltaTime*Vector3.Distance(mainCamTarget.position, playerPos.position+offset));

        }
        else chaseSpeed = 0;

        if(isPC)
        {
            PCRotateCamera();
        }
        else PhoneRotateCamera();
    }

    void PCRotateCamera()
    {
            joystick.gameObject.SetActive(false);

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            float contX = Input.GetAxis("Debug Horizontal");
            float contY = Input.GetAxis("Debug Vertical");

            if(contX==0||contY==0)
            {
                turn.x = mouseX * rotSpeed * Time.unscaledDeltaTime;
                turn.y = mouseY * rotSpeed * Time.unscaledDeltaTime;
            }
            
            if(contX!=0||contY!=0) 
            {
                turn.x = contX * rotSpeed * Time.unscaledDeltaTime;
                turn.y = contY * rotSpeed * Time.unscaledDeltaTime;
            }
            

            XRot -= turn.y;
            YRot += turn.x;
            mainCamTarget.localRotation = Quaternion.Euler(XRot, YRot, 0);

            XRot = Mathf.Clamp(XRot, -45, 10);

      

    }

    void PhoneRotateCamera()
    {
            joystick.gameObject.SetActive(true);

            float mouseX = joystick.Horizontal;
            float mouseY = joystick.Vertical;

            turn.x = mouseX * rotSpeed * Time.unscaledDeltaTime;
            turn.y = mouseY * rotSpeed * Time.unscaledDeltaTime;
            XRot -= turn.y;
            YRot += turn.x;
            mainCamTarget.localRotation = Quaternion.Euler(XRot, YRot, 0);

            XRot = Mathf.Clamp(XRot, -45, 10);
      
    }

   void CameraHugPlayer()
    {
        if(mainCamera.transform.localPosition.z<-1.5f)
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, new Vector3(0, 0, -1.5f),5*Time.deltaTime);

        }
    }

    void StopHuggingplayer()
    {
      if(mainCamera.transform.localPosition.z>-6)
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, new Vector3(0, 2, -6), 5*Time.deltaTime);
        }
           
      
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isFree = false;
            CameraHugPlayer();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isFree = true;
        }
    }

}
