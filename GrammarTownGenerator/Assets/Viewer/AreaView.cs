using UnityEngine;
using System.Collections;
using UnityEngine.U2D;

namespace Viewer
{
    public class AreaView : MonoBehaviour
    {
        Color color;
        GeneratorObject generatorObject;
        SpriteShapeController shape;
        UIController controller;
        bool interactable;

        private void OnMouseEnter()
        {
            if(interactable)
                shape.spriteShapeRenderer.color = Color.yellow;
        }

        private void OnMouseExit()
        {
            shape.spriteShapeRenderer.color = color;
        }

        private void OnMouseDown()
        {
            if(interactable)
                controller.View(generatorObject);
        }

        public void setObjects(GeneratorObject generatorObject, UIController controller)
        {
            this.generatorObject = generatorObject;
            this.controller = controller;
            interactable = false;
        }

        public void setInteractable(bool value)
        {
            interactable = value;
        }
        private void Start()
        {
            shape = GetComponent<SpriteShapeController>();
            color = shape.spriteShapeRenderer.color;
        }
    }
}