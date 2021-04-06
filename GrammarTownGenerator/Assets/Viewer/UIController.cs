using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Viewer
{
    public class UIController : MonoBehaviour
    {
        public Text name_text;
        public Text parameters;
        public VerticalLayoutGroup relations;
        public GameObject buttonPrefab;

        private List<GameObject> objects = new List<GameObject>();

        public void Clear()
        {
            name_text.text = "";
            parameters.text = "Parameters:\n";
            foreach (GameObject gameObject in objects)
                Destroy(gameObject);
            objects.Clear();
        }

        public void View(GeneratorObject generatorObject)
        {
            Clear();

            name_text.text = generatorObject.getName();
            
            foreach (KeyValuePair<string, string> parameter in generatorObject.parameters)
                if (parameter.Key != "name" && !parameter.Key.Contains("point"))
                    parameters.text += parameter.Key + ": " + parameter.Value + "\n";

            foreach (Relation relation in generatorObject.relations)
                View(relation);
        }

        public void View(Relation relation)
        {
            if (!relation.parameters.ContainsKey("text")) return;

            GameObject gameObject = new GameObject();
            gameObject.AddComponent<HorizontalLayoutGroup>().padding.left = 100;
            objects.Add(gameObject);
            gameObject.transform.parent = relations.transform;
            gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            HorizontalLayoutGroup group = gameObject.GetComponent<HorizontalLayoutGroup>();
            group.childControlHeight = false;
            group.childControlWidth = false;
            string[] text = relation.getParameter("text").Split('$');
            foreach(string element in text)
            {
                GameObject text1;
                if (relation.relationObjects.ContainsKey(element))
                {
                    text1 = Instantiate(buttonPrefab);
                    HyperLink link = text1.AddComponent<HyperLink>();
                    link.setObjects(relation.relationObjects[element], this);
                    Button button = text1.GetComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(link.OnMouseDown);
                    Text sizer = button.GetComponentInChildren<Text>();
                    button.image.rectTransform.sizeDelta = new Vector2(sizer.preferredWidth, 20);
                }
                else
                {
                    text1 = new GameObject();
                    Text t = text1.AddComponent<Text>();
                    t.text = element;
                    t.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                    t.color = Color.black;
                    text1.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                }
                
                text1.transform.parent = group.transform;
                
            }
        }
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
