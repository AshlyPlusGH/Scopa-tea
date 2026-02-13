using UnityEditor;

public static class CustomScriptableObjectCreator
{
    private const string TEMPLATE_PATH =
        "Assets/Editor/ScriptTemplates/MyScriptableObjectTemplate.cs.txt";

    [MenuItem("Assets/Create/ScriptableObject", false, 80)]
    public static void CreateCustomScriptableObject()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
            TEMPLATE_PATH,
            "NewScriptableObject.cs"
        );
    }
}