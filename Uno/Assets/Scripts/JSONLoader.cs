using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JSONLoader : MonoBehaviour
{
    // Variables to store the random names
    public string name1;
    public string name2;
    public string name3;
    public TextMeshProUGUI name1GUI;
    public TextMeshProUGUI name2GUI;
    public TextMeshProUGUI name3GUI;

    // Path to your JSON file in Resources folder (without the .json extension)
    public string fileName = "names";

    void Start()
    {
        // Load the JSON file as a TextAsset
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);

        if (jsonFile != null)
        {
            // Deserialize the JSON file into an object
            NameList nameList = JsonUtility.FromJson<NameList>(jsonFile.text);

            if (nameList.names.Length >= 3)
            {
                // Get 3 unique random names
                List<string> randomNames = GetRandomNames(nameList.names, 3);

                // Assign to the variables
                name1 = randomNames[0];
                name2 = randomNames[1];
                name3 = randomNames[2];

                // Output the selected names to the console
                Debug.Log("Random Name 1: " + name1);
                Debug.Log("Random Name 2: " + name2);
                Debug.Log("Random Name 3: " + name3);

                name1GUI.text= name1;
                name2GUI.text= name2;
                name3GUI.text= name3;

            }
            else
            {
                Debug.LogError("Not enough names in the list!");
            }
        }
        else
        {
            Debug.LogError("Could not find the JSON file!");
        }
    }

    // Function to get 'count' unique random names from the name array
    List<string> GetRandomNames(string[] names, int count)
    {
        List<string> nameList = new List<string>(names);
        List<string> randomNames = new List<string>();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, nameList.Count);
            randomNames.Add(nameList[randomIndex]);
            nameList.RemoveAt(randomIndex);  // Ensure uniqueness
        }

        return randomNames;
    }
}

[System.Serializable]
public class NameList
{
    public string[] names;
}
