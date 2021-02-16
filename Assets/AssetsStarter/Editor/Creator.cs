using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KLQU_AssetsStarter
{
    public class Creator : EditorWindow
    {
        private static List<string> folderNames = new List<string> { "_Scenes", "Scripts", "Prefabs", "Sprites", "Materials", "Sounds", "Animations" };

        private static string baseFolderName = "_AppAssets";
        private Vector2 scrollPos;
        private static List<bool> folders = new List<bool> { false, false, false, false, false, false, false };
        private static bool usingGithub = true;
        private string folderName = "";
        private bool delete_AddFold = false;
        private static Object dummyAsset;


        [MenuItem("Tools/Assets Starter/CreationWindow", false, 0)]
        private static void InitWindowToMakeStarter()
        {
            GetWindowWithRect(typeof(Creator), new Rect(0, 0, 120, 235));
        }

        private void OnGUI()
        {
            //scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            GUILayout.Space(5);
            Seprator(90);
            EditorGUILayout.BeginVertical();

            #region Display Folders 
            GUILayout.Space(5);
            for (int i = 0; i < folderNames.Count; i++)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                folders[i] = GUILayout.Toggle(folders[i], folderNames[i]);
                if (folders[i])
                {
                    folderNames[i] = GUILayout.TextField(folderNames[i]);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            GUILayout.Space(5);
            #endregion

            #region Other Options
            Seprator(position.width);
            GUILayout.Space(5);
            usingGithub = GUILayout.Toggle(usingGithub, "Using GitHub");
            GUILayout.Space(5);
            #endregion

            #region Create Folders Button
            Seprator(90);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("CreateFolders", GUILayout.Height(40), GUILayout.Width(110)))
                CreateAssets();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
            #endregion

            #region Foldout Add and Delete
            delete_AddFold = EditorGUILayout.Foldout(delete_AddFold, "Add & Delete Folders");
            if (delete_AddFold)
            {
                folderName = EditorGUILayout.TextField("FolderName :", folderName);

                GUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add", GUILayout.Height(40), GUILayout.Width(110)))
                {
                    if (!folderNames.Contains(folderName))
                    {
                        folderNames.Add(folderName);
                        folders.Add(true);
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error Adding new Folder", "\"" + folderName + "\" was found in the folder list\nPlease Pick another Name", "OK");
                    }
                }
                if (GUILayout.Button("Delete", GUILayout.Height(40), GUILayout.Width(110)))
                {
                    if (folderNames.Contains(folderName))
                    {
                        folders.RemoveAt(folderNames.IndexOf(folderName));
                        folderNames.Remove(folderName);
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error Deleting Folder", "\"" + folderName + "\" wasn't found in the folder list\nPlease Pick another Name", "OK");
                    }
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            #endregion

            EditorGUILayout.EndVertical();
            //EditorGUILayout.EndScrollView();
        }

        private void Seprator(float width)
        {
            GUILayout.Space(3);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box("", GUILayout.Height(3), GUILayout.Width(width));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(3);
        }

        private static void CreateAssets()
        {
            // Create the MainFolder
            if (!AssetDatabase.IsValidFolder("Assets/" + baseFolderName))
            {
                AssetDatabase.CreateFolder("Assets", baseFolderName);
            }

            if (usingGithub)
            {
                dummyAsset = AssetDatabase.LoadAssetAtPath("Assets/AssetsStarter/Editor/DummyAsset.png", typeof(Object));
            }

            // Create SubFolder
            for (int i = 0; i < folderNames.Count; i++)
            {
                if (folders[i])
                {
                    if (!AssetDatabase.IsValidFolder("Assets/" + baseFolderName + "/" + folderNames[i]))
                    {
                        if (folderNames[i].Equals("_Scenes") && AssetDatabase.IsValidFolder("Assets/Scenes"))
                        {
                            AssetDatabase.MoveAsset("Assets/Scenes", "Assets/" + baseFolderName + "/" + folderNames[i]);
                        }
                        else
                        {
                            AssetDatabase.CreateFolder("Assets/" + baseFolderName, folderNames[i]);

                            if (usingGithub)
                            {
                                if (dummyAsset)
                                {
                                    AssetDatabase.CopyAsset("Assets/AssetsStarter/Editor/DummyAsset.png", "Assets/" + baseFolderName + "/" + folderNames[i] + "/DummyAsset.png");
                                }

                            }
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
        }
    }
}