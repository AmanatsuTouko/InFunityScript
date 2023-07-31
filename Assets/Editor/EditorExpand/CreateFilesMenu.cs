using UnityEditor;
static internal class CreateFilesMenu
{
    private const string PLUGIN_PATH = "Assets/Editor/EditorExpand";

    private const string MENU_ITEM_ROOT = "Assets/Create/InFunityScript/";
    private const string MENU_SCENARIO = "Scenario";
    private const string MENU_CHARACTER = "Character";

    private const int SCRIPT_PRIOPRITY = 30;

    [MenuItem(MENU_ITEM_ROOT + MENU_SCENARIO, priority = SCRIPT_PRIOPRITY)]
    private static void CreateScript_Scenario() => CreateScriptFile("ScenarioTemplate.txt", "NewScenario.cs");

    [MenuItem(MENU_ITEM_ROOT + MENU_CHARACTER, priority = SCRIPT_PRIOPRITY)]
    private static void CreateScript_Character() => CreateScriptFile("CharacterTemplate.txt", "NewCharacter.cs");

    private static void CreateScriptFile(string templateFileName, string newFileName)
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(System.IO.Path.Combine(PLUGIN_PATH, $"{templateFileName}"), newFileName);
    }
}