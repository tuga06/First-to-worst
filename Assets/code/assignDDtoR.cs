using UnityEngine;
using TMPro;

public class assignDDtoR : MonoBehaviour
{
    public Dropdowns dropdowns;
    public TMP_Dropdown[] masterDropdowns;
    public TMP_Dropdown[] player1Dropdowns;
    public TMP_Dropdown[] player2Dropdowns;
    public TMP_Dropdown[] player3Dropdowns;
    public TMP_Dropdown[] player4Dropdowns;
    public TMP_Dropdown[] player5Dropdowns;
    public TMP_Dropdown[] player6Dropdowns;
    void Start()
    {
        dropdowns.AssignDropdownsToRoles("maestru", masterDropdowns);
        dropdowns.AssignDropdownsToRoles("player1", player1Dropdowns);
        dropdowns.AssignDropdownsToRoles("player2", player2Dropdowns);
        dropdowns.AssignDropdownsToRoles("player3", player3Dropdowns);
        dropdowns.AssignDropdownsToRoles("player4", player4Dropdowns);
        dropdowns.AssignDropdownsToRoles("player5", player5Dropdowns);
        dropdowns.AssignDropdownsToRoles("player6", player6Dropdowns);
    }
}
