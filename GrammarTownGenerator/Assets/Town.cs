using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.IO;

public class Town : MonoBehaviour
{

    GeneratorObject town;

    void loadJSON(string path)
    {
        town = JsonConvert.DeserializeObject<GeneratorObject>(File.ReadAllText(path), new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        });
    }

    void saveJSON(string path)
    {
        File.WriteAllText(path, JsonConvert.SerializeObject(town, Formatting.Indented, new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        }));
    }

    // Use this for initialization
    void Start()
    {
        loadJSON(@"Assets/próbny plan miasta.json");

        Debug.Log(town.toString());

        saveJSON(@"Assets/test2.json");
        Viewer.TownViewer townViewer = GetComponent<Viewer.TownViewer>();
        townViewer.drawObject(town);
        townViewer.GetComponent<Viewer.UIController>().View(town);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
