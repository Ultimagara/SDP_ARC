using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class ProfileController : MonoBehaviour
{
    // Input array to populate the dropdown menu.
    public List<string> options;

    // Global variable to store the selected value.
    public static string selectedOption;

    // Reference to the dropdown menu UI component.
    private TMP_Dropdown dropdown;

    void Start()
    {
        // Get a reference to the dropdown menu UI component.
        dropdown = GetComponent<TMP_Dropdown>();

        // Clear any existing options from the dropdown menu.
        dropdown.ClearOptions();


        // Attempt to get the filenames of .fst files in the datapath
        options = new List<string> {"<Select Profile>"};
        options.AddRange(Directory.GetFiles(Application.streamingAssetsPath, "*.fst").Select(Path.GetFileNameWithoutExtension));

        // Add each option from the input array to the dropdown menu.
        dropdown.AddOptions(new List<string>(options));

        // Set the initial value of the dropdown menu to the first item in the list.
        dropdown.value = 0;

        // Set the initial selected value to the first item in the list.
        selectedOption = options[0];
    }

    public void OnProfileValueChanged(int value)
    {
        // Update the global variable with the new selected value.
        selectedOption = options[value];
    }
}