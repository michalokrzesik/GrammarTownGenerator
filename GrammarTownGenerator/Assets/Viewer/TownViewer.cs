using UnityEngine;
using System.Collections;
using UnityEngine.U2D;

namespace Viewer
{
    public class TownViewer : MonoBehaviour
    {

        public Vector2 positionZero = new Vector2(-8.5f, 5);
        public Vector2 positionMax = new Vector2(6.7f, 9.9f);
        public Vector2 townSize = new Vector2(600, 550);        //okno ma 530x720
        public GameObject spriteShapePrefab;
        public GameObject linePrefab;
        public bool parcelEdges = false;
        float scale;

        GameObject drawArea(GeneratorObject generatorObject, Color color, bool edges = false)
        {
            GameObject newObject = GameObject.Instantiate(spriteShapePrefab);
            newObject.transform.position = new Vector3(positionZero.x, positionZero.y, 100);
            SpriteShapeController controller = newObject.GetComponent<SpriteShapeController>();
            newObject.AddComponent<PolygonCollider2D>();
            controller.spline.Clear();
            for (int i = 0; i < 4; i++)
            {
                float x = float.Parse(generatorObject.parameters["point" + i + "x"]) * scale;
                float y = float.Parse(generatorObject.parameters["point" + i + "y"]) * scale;
                controller.spline.InsertPointAt(0, new Vector3(x, -y, 0));
                controller.spline.SetTangentMode(0, ShapeTangentMode.Linear);
            }
            controller.spriteShapeRenderer.color = color;
            newObject.AddComponent<AreaView>().setObjects(generatorObject, GetComponent<UIController>());

            if (edges)
                drawLine(generatorObject, Color.grey, true, 0.02f);

            return newObject;
        }

        void drawLine(GeneratorObject generatorObject, Color color, bool loop = false, float width = 0.1f)
        {
            GameObject newObject = GameObject.Instantiate(linePrefab);
            newObject.transform.position = positionZero;
            LineRenderer line = newObject.GetComponent<LineRenderer>();
            line.positionCount = 0;
            line.widthMultiplier = 0.1f;
            line.startWidth = width;
            line.endWidth = width;
            line.loop = loop;
            line.startColor = color;
            line.endColor = color;
            for(int i = 0; generatorObject.parameters.ContainsKey("point" + i + "x"); i++)
            {
                float x = float.Parse(generatorObject.parameters["point" + i + "x"]) * scale;
                float y = float.Parse(generatorObject.parameters["point" + i + "y"]) * scale;
//                Debug.Log(x + ", " + y);
                line.positionCount += 1;
                line.SetPosition(i, new Vector3(x, -y, 0));
            }
        }

        public void drawObject(GeneratorObject generatorObject)
        {
            if (scale == 0) setScale();
            if (generatorObject.isDrawn()) return;
            generatorObject.setAsDrawn();

            switch (generatorObject.getType())
            {
                case "town":
                    break;
                case "river":
                    break;
                case "water_area":
                    drawArea(generatorObject, Color.blue);
                    break;
                case "parcel_area":
                    drawArea(generatorObject, Color.green, parcelEdges).GetComponent<AreaView>().setInteractable(true);
                    break;
                case "building_area":
                    drawArea(generatorObject, Color.black).GetComponent<AreaView>().setInteractable(true);
                    break;
                case "line":
                    drawLine(generatorObject,Color.black);
                    break;
                default:
                    return;
            }

            foreach (Relation relation in generatorObject.relations)
                drawObject(relation.getRelationObject("child"));
        }
        // Use this for initialization
        void Start()
        {
            setScale();
        }

        void setScale()
        {
            scale = positionMax.x / townSize.x;
            if (townSize.y * scale > positionMax.y) scale = positionMax.y / townSize.y;
            if (townSize.x * scale > 1.01 * positionMax.x) Debug.LogError("scale error:" + townSize.x * scale);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}