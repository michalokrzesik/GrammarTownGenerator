using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Viewer
{
    public class HyperLink : MonoBehaviour
    {
        public GeneratorObject generatorObject;
        public UIController controller;
        Text text;

        public void setObjects(GeneratorObject generatorObject, UIController controller)
        {
            this.controller = controller;
            this.generatorObject = generatorObject;
            text = GetComponentInChildren<Text>();
            text.text = generatorObject.getName();
            text.gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        private void OnMouseEnter()
        {
            text.color = Color.green;
        }

        private void OnMouseExit()
        {
            text.color = Color.blue;
        }

        public void OnMouseDown()
        {
            controller.View(generatorObject);
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