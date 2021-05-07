using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
using SimpleFileBrowser;

public class Town : MonoBehaviour
{

    GeneratorObject town;

    void loadJSON(string[] paths)
    {
        string path = paths[0];
        town = JsonConvert.DeserializeObject<GeneratorObject>(File.ReadAllText(path), new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        });
        Viewer.UIController controller = GetComponent<Viewer.UIController>();
        controller.Log("Test");
        Viewer.TownViewer townViewer = GetComponent<Viewer.TownViewer>();
        townViewer.drawObject(town);
        townViewer.GetComponent<Viewer.UIController>().View(town);
    }

    void saveJSON(string path)
    {
        File.WriteAllText(path, JsonConvert.SerializeObject(town, Formatting.Indented, new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        }));
    }

    public void onCancel() { }

    public void Load()
    {
        FileBrowser.ShowLoadDialog(loadJSON, onCancel, FileBrowser.PickMode.Files);
        //string path = OpenFile("Load from JSON", "", "json");
        //loadJSON(path);
    }

    // Use this for initialization
    void Start()
    {
        
        Flee.CalcEngine.PublicTypes.CalculationEngine test = new Flee.CalcEngine.PublicTypes.CalculationEngine();
        Flee.PublicTypes.ExpressionContext context = new Flee.PublicTypes.ExpressionContext(this);
        context.Imports.AddType(typeof(System.Math));
        context.Imports.AddType(typeof(UnityEngine.Random));
        test.Add("test", "Range(0,1f;10)", context);
        Debug.Log(test.GetResult("test"));
        /*    string[] paths =
            {
                @"Assets/próbny plan miasta.json"
            };
            loadJSON(paths);

            Debug.Log(town.toString());

            saveJSON(@"Assets/test2.json");
            */
    }

    // Update is called once per frame
    void Update()
    {

    }
}
