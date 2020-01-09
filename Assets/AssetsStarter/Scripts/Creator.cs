using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Creator : Editor
{
    private static string[] folderNames = { "_AppAssets", "_Scenes", "Scripts", "Prefabs", "Sprites", "Materials", "Sounds", "Animations" };

    [MenuItem("Tools/Create Main Assets Folder", false, 0)]
    private static void CreateAssets()
    {
        // Create the MainFolder
        if (!AssetDatabase.IsValidFolder("Assets/" + folderNames[0]))
        {
            AssetDatabase.CreateFolder("Assets", folderNames[0]);
        }

        // Create SubFolder
        for (int i = 1; i < folderNames.Length; i++)
        {
            if (!AssetDatabase.IsValidFolder("Assets/" + folderNames[0] + "/" + folderNames[i]))
            {
                if (folderNames[i].Equals("_Scenes") && AssetDatabase.IsValidFolder("Assets/Scenes"))
                {
                    AssetDatabase.MoveAsset("Assets/Scenes", "Assets/" + folderNames[0] + "/" + folderNames[i]);
                }
                else
                {
                    AssetDatabase.CreateFolder("Assets/" + folderNames[0], folderNames[i]);
                }
            }
        }

        AssetDatabase.Refresh();
    }
}
