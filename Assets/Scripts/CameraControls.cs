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
    public bool isPC;
    public Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if(Application.isMobilePlatform)
        {
            isPC = false;
        }
        else isPC= true;

    }

    // Update is called once per frame
    void Update()
    {
        //mainCamera.transform.LookAt(mainCamTarget);

        //keep camera at certain distance from player
        /*Vector3 offset = mainCamTarget.parent.transform.position + new Vector3(0, 1.5f, -1*distToCam);
        float distance = Vector3.Distance(mainCamera.transform.position, offset);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, offset, distance*Time.deltaTime);
*/
        

    }

    private void LateUpdate()
    {
       
        Vector3 offset = new Vector3(0, 1, 0);
        if (mainCamTarget.position != playerPos.position + offset)
        {
            chaseSpeed += Time.deltaTime;
            mainCamTarget.position = Vector3.Lerp(mainCamTarget.position, playerPos.position+offset, chaseSpeed*Time.deltaTime);

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
       
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            float contX = Input.GetAxis("Debug Horizontal");
            float contY = Input.GetAxis("Debug Vertical");

            if(contX==0||contY==0)
            {
                turn.x = mouseX * rotSpeed * Time.deltaTime;
                turn.y = mouseY * rotSpeed * Time.deltaTime;
            }
            
            if(contX!=0||contY!=0) 
            {
                turn.x = contX * rotSpeed * Time.deltaTime;
                turn.y = -contY * rotSpeed * Time.deltaTime;
            }
            

            XRot -= turn.y;
            YRot += turn.x;
            mainCamTarget.localRotation = Quaternion.Euler(XRot, YRot, 0);

            XRot = Mathf.Clamp(XRot, -30, 30);

      

    }

    void PhoneRotateCamera()
    {
            joystick.gameObject.SetActive(true);

            float mouseX = joystick.Horizontal;
            float mouseY = joystick.Vertical;

            turn.x = mouseX * rotSpeed * Time.deltaTime;
            turn.y = mouseY * rotSpeed * Time.deltaTime;
            XRot -= turn.y;
            YRot += turn.x;
            mainCamTarget.localRotation = Quaternion.Euler(XRot, YRot, 0);

            XRot = Mathf.Clamp(XRot, -30, 30);
      
    }
}
