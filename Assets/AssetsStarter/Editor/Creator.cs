using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KLQU_AssetsStarter
{
    public class Creator : EditorWindow
    {
        [SerializeField]
        private List<KLQU_Folder> folders = new List<KLQU_Folder>()
        {
            new KLQU_Folder("_Scenes"),
            new KLQU_Folder("Scripts"),
            new KLQU_Folder("Prefabs"),
            new KLQU_Folder("Sprites"),
            new KLQU_Folder("Materials"),
            new KLQU_Folder("Sounds"),
            new KLQU_Folder("Animations")
        };

        [SerializeField] private string baseFolderName = "_AppAssets";

        private SerializedObject so;

        private SerializedProperty foldersProperty;
        private SerializedProperty baseFolderNameProperty;

        private Vector2 scrollPos = Vector2.zero;
        private static bool usingGithub = true;
        private static Object dummyAsset;


        [MenuItem("Tools/Assets Starter/CreationWindow", false, 0)]
        private static void InitWindowToMakeStarter()
        {
            Creator window = (Creator)EditorWindow.GetWindow(typeof(Creator));
            window.titleContent.text = "Folders Creator";
            window.minSize = new Vector2(400, 400);
            window.Init();
            window.Show();
        }

        public void Init()
        {
            ScriptableObject target = this;
            so = new SerializedObject(target);
            foldersProperty = so.FindProperty("folders");
            baseFolderNameProperty = so.FindProperty("baseFolderName");
        }

        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            GUILayout.Space(5);
            Seprator(90);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(5);


            #region Display Main folderName
            EditorGUILayout.PropertyField(
                baseFolderNameProperty,
                new GUIContent("BaseFolderName", "This will be the root folder that all folders created will nested from it ")
                );
            #endregion

            #region Display Folders 
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(foldersProperty, true);

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            #endregion

            #region Other Options
            Seprator(position.width);
            GUILayout.Space(5);
            usingGithub = GUILayout.Toggle(usingGithub, new GUIContent("Using GitHub", "This for creating dummy asset inside end level folder to store folder on Github"));
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

            EditorGUILayout.EndVertical();
            so.ApplyModifiedProperties();
            EditorGUILayout.EndScrollView();
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

        private void CreateAssets()
        {
            if (string.IsNullOrEmpty(baseFolderName))
            {
                Debug.LogError("you have BaseFolderName enmpty it must have name");
                return;
            }

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
            CreateFolders("Assets/" + baseFolderNameProperty.stringValue, folders);

            AssetDatabase.Refresh();
        }

        private void CreateFolders(string path, List<KLQU_Folder> folders)
        {
            if (folders == null || folders.Count == 0)
            {
                if (!path.Equals(this.baseFolderName))
                {
                    if (usingGithub && dummyAsset)
                    {
                        AssetDatabase.CopyAsset("Assets/AssetsStarter/Editor/DummyAsset.png", path + "/DummyAsset.png");
                    }
                }
                return;
            }

            for (int i = 0; i < folders.Count; i++)
            {
                if (string.IsNullOrEmpty(folders[i].name))
                {
                    Debug.LogError("you have enmpty name folder in path:(" + path + ",element:" + i + ") the folder must have name");
                }
                else
                {
                    if (!AssetDatabase.IsValidFolder(path + "/" + folders[i].name))
                    {
                        AssetDatabase.CreateFolder(path, folders[i].name);
                    }
                    CreateFolders(path + "/" + folders[i].name, folders[i].insideFolders);
                }
            }
        }
    }

    [System.Serializable]
    public class KLQU_Folder
    {
        public string name;
        public List<KLQU_Folder> insideFolders;

        public KLQU_Folder(string name)
        {
            this.name = name;
        }
    }
}