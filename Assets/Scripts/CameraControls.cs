using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Camera mainCamera;
    public Transform mainCamTarget, playerPos;
    public float distToCam, chaseSpeed, rotSpeed;
    public float XRot, YRot;
    public Vector2 turn;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
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
        if (mainCamTarget.position != playerPos.position + new Vector3(0, 1, 0))
        {
            chaseSpeed += Time.deltaTime;
            mainCamTarget.position = Vector3.Lerp(mainCamTarget.position, playerPos.position+new Vector3(0,1,0), chaseSpeed * Time.deltaTime);

        }
        else chaseSpeed = 0;

        PCRotateCamera();
    }

    void ZoomIn()
    {
        if (distToCam <= 2) distToCam = 2;
    }

    void ZoomOut()
    {
        if (distToCam >= 9) distToCam = 9;
    }

    void PCRotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        turn.x = mouseX * rotSpeed * Time.deltaTime;
        turn.y = mouseY * rotSpeed * Time.deltaTime;
        XRot -= turn.y;
        YRot += turn.x;
        mainCamTarget.localRotation = Quaternion.Euler(XRot, YRot, 0);

        XRot = Mathf.Clamp(XRot, -30, 30);
        

    }
}
