using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_WeaponManager : MonoBehaviour
{
    public Camera playerCamera;
    public SC_Weapon primaryWeapon;
    public SC_Weapon secondaryWeapon;


    [HideInInspector]
    public SC_Weapon selectedWeapon;

    public Font pixel;

    // Start is called before the first frame update
    void Start()
    {
        //At the start we enable the primary weapon and disable the secondary
        primaryWeapon.ActivateWeapon(true);
        secondaryWeapon.ActivateWeapon(false);
        selectedWeapon = primaryWeapon;
        primaryWeapon.manager = this;
        secondaryWeapon.manager = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Select secondary weapon when pressing 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            primaryWeapon.ActivateWeapon(false);
            secondaryWeapon.ActivateWeapon(true);
            selectedWeapon = secondaryWeapon;
        }

        //Select primary weapon when pressing 2
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            primaryWeapon.ActivateWeapon(true);
            secondaryWeapon.ActivateWeapon(false);
            selectedWeapon = primaryWeapon;
        }
    }

    void OnGUI()
    {

        GUIStyle crosshair = new GUIStyle(GUI.skin.button);
        crosshair.fontSize = 24;
        crosshair.alignment = TextAnchor.MiddleCenter;
        crosshair.font = pixel;

        GUI.backgroundColor = new Color(0, 0, 0, 0);
 
        GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "+", crosshair);
    }
}
