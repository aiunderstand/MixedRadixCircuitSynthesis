using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserModeDropdown : MonoBehaviour
{
    public TMP_Dropdown usermodeDropdown;

    public void Start()
    {
        usermodeDropdown = GetComponent<TMP_Dropdown>();
    }

    public void UpdateUserMode()
    {
        if (usermodeDropdown.options[usermodeDropdown.value].text.Equals("TinyTapeout 2"))
        {
            //set technology mapping to binary-CMOS
            applicationmanager.curSeletectedTech = applicationmanager.TechnologyMappings.binaryCMOS;

            //set defaultradix to binary
            FindObjectOfType<DefaultRadixDropdown>().defaultRadixDropDown.value = 2;

            //add verilog ip cell library: problem cannot create netlist since HDL
            //load single .v file with verilog modules, create new verilog type savedcomponent tht cannot be spice simulated/create netlist from
            //when going to lowest abstraction level, the .v text is shown
            //add git login creds, new tt repo with name, sync w repo
            //integrate Octokit 
        }
        else
        {
            //set technology mapping to binary-CMOS
            applicationmanager.curSeletectedTech = applicationmanager.TechnologyMappings.ternaryCNTFET;

            //set defaultradix to balanced ternary
            FindObjectOfType<DefaultRadixDropdown>().defaultRadixDropDown.value = 0;

            //remove verilog ip cells and github ui
        }
    }
}
