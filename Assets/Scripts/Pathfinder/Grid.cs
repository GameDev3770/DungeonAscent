using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[Serializable]
public struct Layer {
    public LayerMask LayerMask;
    public float weight;
    public float Weight {
        set {
            weight = value + 1;
        }
        get {
            return weight;
        }
    }
}

public class Grid
{
    // Start is called before the first frame update
    public bool debug;
    bool gizmos;
    bool handles;

    Layer[] WalkableLayers = new Layer[0];
    Layer[] UnWalkableLayers = new Layer[0];
    float nodeScale = 1;

    List<GameObject> walkables = new List<GameObject>();
    List<GameObject> unWalkables = new List<GameObject>();
    Dictionary<float, Dictionary<float, Node>> map;
    List<Node> list;

    private GameObject Collider_Container;
    private GameObject NodeCollider;

    private GameObject DebugGameObject;

    public Grid(bool _debug, bool _gizmos, bool _handles, Layer[] _WalkableLayers, Layer[] _UnWalkableLayers, float _nodeScale, GameObject debugGameObject) {
        this.debug = _debug;
        this.gizmos = _gizmos;
        this.handles = _handles;
        this.WalkableLayers = _WalkableLayers;
        this.UnWalkableLayers = _UnWalkableLayers;
        this.nodeScale = _nodeScale;


        this.Collider_Container = new GameObject();
        this.NodeCollider = new GameObject();
        this.NodeCollider.AddComponent<BoxCollider>();
        this.NodeCollider.AddComponent<TrackingNode>();
        this.NodeCollider.GetComponent<BoxCollider>().size = new Vector3(nodeScale, 0.1f, nodeScale);
        this.NodeCollider.layer = LayerMask.NameToLayer("Floor");


        this.Collider_Container.name = "Collider Container";
        this.NodeCollider.name = "Node Collider";

        this.DebugGameObject = debugGameObject;
    }

    public void BuildGrid(GameObject gameObject)
    {
        Collider_Container.transform.position = gameObject.transform.position;

        this.map = new Dictionary<float, Dictionary<float, Node>>();
        this.list = new List<Node>();

        for (int i = 0; i < WalkableLayers.Count(); i++)    WalkableLayers[i].Weight = WalkableLayers[i].Weight == 0 ? 1 : WalkableLayers[i].Weight;
        for (int i = 0; i < UnWalkableLayers.Count(); i++)  UnWalkableLayers[i].Weight = UnWalkableLayers[i].Weight == 0 ? 1 : UnWalkableLayers[i].Weight;

        IterateChildren(gameObject);
        CreateGrid();
        Collider_Container.transform.parent = gameObject.transform;
    }

    /**
     Because I don't want to make eveything a single layer in a prefab maybe, this will allow for prefabs in prefabs in prefabs in etc to all be put together.
     
     */
    void IterateChildren(GameObject root) {
        for (int i = 0; i < root.transform.childCount; i++) {
            GameObject obj = root.transform.GetChild(i).gameObject;
            if (LayerContains(WalkableLayers, obj.layer)) {
                walkables.Add(obj);
            }
            else if (LayerContains(UnWalkableLayers, obj.layer)) {
                unWalkables.Add(obj);
            }
            IterateChildren(obj);
        }
    }

    bool LayerContains(Layer[] layers, LayerMask layerMask) {
        foreach(Layer layer in layers) {
            if (layer.LayerMask == (layer.LayerMask| (1 << layerMask))) return true;
        } return false;
    }

    /**
     Creates the Grid. Iterates through all specified walkable layer mask objects and gridifies them. It then adds them to a dictionary because dictionaries are the number 1 data structure in my heart.
    It will then piece all of the grid pieces together.
     */
    void CreateGrid() {
        foreach (GameObject obj in walkables) {
            if (obj == DebugGameObject) {

            }
            Node node = new Node(GetMiddlePoint(obj.transform), nodeScale / 2, GetLayerWeight(WalkableLayers, obj.layer));
            foreach (Layer mask in UnWalkableLayers) {
                node.walkable = !(Physics.CheckSphere(new Vector3(node.x, 0, node.y), nodeScale / 2, mask.LayerMask));
                if (!node.walkable) {
                    break;
                }
            }

            this.AddToDict(node);
            this.list.Add(node);

            GameObject temp = GameObject.Instantiate(NodeCollider);
            temp.GetComponent<TrackingNode>().node = node;
            temp.transform.position = new Vector3(node.x, obj.transform.position.y, node.y);
            temp.transform.parent = Collider_Container.transform;

            GetMiddlePoint(obj.transform);
            /*List<Vector3> FourCorners = GetFourCorners(obj);
            float[] MinAndMax = GetMinMax(FourCorners);
            for (float x = MinAndMax[0]; x < MinAndMax[1]; x += nodeScale) {
                for (float y = MinAndMax[2]; y < MinAndMax[3]; y += nodeScale) {
                    float[] middlePoint = new float[] { x + nodeScale / 2, y + nodeScale / 2 };
                    Node node = new Node(middlePoint, nodeScale / 2, GetLayerWeight(WalkableLayers, obj.layer));
                    foreach (Layer mask in UnWalkableLayers) {
                        node.walkable = !(Physics.CheckSphere(new Vector3(node.x, 0, node.y), nodeScale / 2, mask.LayerMask));
                        if (!node.walkable) {
                            break;
                        }
                    }

                    this.AddToDict(node);
                    this.list.Add(node);

                    GameObject temp = GameObject.Instantiate(NodeCollider);
                    temp.GetComponent<TrackingNode>().node = node;
                    temp.transform.position = new Vector3(node.x, obj.transform.position.y, node.y);
                    temp.transform.parent = Collider_Container.transform;
                }
            }
            */
        }

        foreach (Node node in list) {
            float[,] temp = GetSurroundingCoordinates(node);
            for (int i = 0; i < temp.GetLength(0); i++) {
                SetNodePtr(node, i, GetNodeAtCoordinate(temp[i,0], temp[i,1]));
            }
        }
    }

    /**
     Everything Below here is probably pointless to explain. Its just some code seperated from its family.
     
     */

    List<Vector3> GetFourCorners(GameObject obj) {
        List<Vector3> VerticeList = new List<Vector3>(obj.GetComponent<MeshFilter>().sharedMesh.vertices);
        List<Vector3> VerticeListToShow = new List<Vector3>();
        VerticeListToShow.Add(obj.transform.TransformPoint(VerticeList[0]));
        VerticeListToShow.Add(obj.transform.TransformPoint(VerticeList[10]));
        VerticeListToShow.Add(obj.transform.TransformPoint(VerticeList[110]));
        VerticeListToShow.Add(obj.transform.TransformPoint(VerticeList[120]));

        return VerticeListToShow;
    }

    float[] GetMinMax(List<Vector3> FourCorners) {
        float min_x = FourCorners[0].x, max_x = FourCorners[0].x;
        float min_y = FourCorners[0].z, max_y = FourCorners[0].z;
        foreach(Vector3 vector in FourCorners) {
            if (vector.x < min_x) min_x = vector.x;
            if (vector.x > max_x) max_x = vector.x;
            if (vector.z < min_y) min_y = vector.z;
            if (vector.z > max_y) max_y = vector.z;
        }

        return new float[] { min_x, max_x, min_y, max_y };
    }

    float GetLayerWeight(Layer[] layers, LayerMask mask) {
        foreach(Layer layer in layers) {
            if (layer.LayerMask == mask) return layer.Weight;
        }
        return -1;
    }

    void AddToDict(Node node) {
        if (!map.ContainsKey(node.x)) map.Add(node.x, new Dictionary<float, Node>());
        map[node.x].Add(node.y, node);
    }

    float[,] GetSurroundingCoordinates(Node node) {

        return new float[4, 2] {
            //{ node.x - nodeScale, node.y - nodeScale },
            { node.x - nodeScale, node.y },
            //{ node.x - nodeScale, node.y + nodeScale },
            { node.x, node.y - nodeScale },
            { node.x, node.y + nodeScale },
            //{ node.x + nodeScale, node.y - nodeScale },
            { node.x + nodeScale, node.y },
            //{ node.x + nodeScale, node.y + nodeScale },
        };
    }
    Node GetNodeAtCoordinate(float x, float y) {
        return map.ContainsKey(x) && map[x].ContainsKey(y) ? map[x][y] : null;
    }
    void SetNodePtr(Node node, int index, Node value) {
        switch (index) {
            //case 0:
            //    Direction.SW = value;
            //    break;
            case 0:
                node.Direction.S = value;
                break;
            //case 2:
            //    Direction.SE = value;
            //    break;
            case 1:
                node.Direction.E = value;
                break;
            //case 4:
            //    Direction.NE = value;
            //    break;
            case 2:
                node.Direction.N = value;
                break;
            //case 6:
            //    Direction.NW = value;
            //    break;
            case 3:
                node.Direction.W = value;
                break;
        }
    }
    public bool IsSameLayer(Layer layer, LayerMask layermask) {
        return layermask == (layermask | (1 << layer.LayerMask));
    }
    public float RoundToInt(float number) {
        return (Mathf.RoundToInt(number / nodeScale) * nodeScale);// + nodeScale/2;
    }
    public Node GetNode(Vector3 worldPosition) {
        RaycastHit hit;
        if (Physics.Raycast(worldPosition, Vector3.down, out hit, 2, 1 << LayerMask.NameToLayer("Floor"))) {
            GameObject node = hit.collider.gameObject;
            return hit.collider.gameObject.GetComponent<TrackingNode>().node;
        }
            
        return null;
    }

    public Node GetNode(float x, float y) {
        foreach(Node node in list) {
            if (node.IsOnTile(x, y)) return node;
        }
        return null;
    }


    float[] GetMiddlePoint(Transform trans) {
        float rotation = Mathf.RoundToInt(trans.eulerAngles.y) % 360;
        switch (rotation) {
            case 0:
                return new float[] { trans.position.x - 0.5f, trans.position.z + 0.5f };
            case 90:
                return new float[] { trans.position.x + 0.5f, trans.position.z + 0.5f };
            case 180: 
                return new float[] { trans.position.x + 0.5f, trans.position.z - 0.5f };
            case 270:
                return new float[] { trans.position.x - 0.5f, trans.position.z - 0.5f };
            default:
                //I Don't Know!!!
                break;
        }
        return new float[] { };
    }

    /**
     This is for visual Debugging
     */
    public void OnDrawGizmos(AStar aStar) {
        if (debug && list != null) {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.blue;
            foreach (Node node in list) {
                if (handles) {
                    Handles.color = Color.blue;
                    Handles.Label(new Vector3(node.x, 1, node.y), $"{node.gCost}   {node.hCost}   {node.fCost}\n{node.x}   {node.y}", style);
                }
                if (gizmos) {
                    Gizmos.color = node.walkable ? Color.green : Color.red;
                    if (aStar.MostRecentPath.Contains(node)) Gizmos.color = Color.black;
                    Gizmos.DrawCube(new Vector3(node.x, 0, node.y), new Vector3(nodeScale, nodeScale, nodeScale) * 0.9f);
                }
            }
        }
    }
}
/*
 Node node = new Node(new float[] { obj.transform.position.x, obj.transform.position.z }, nodeScale / 2, GetLayerWeight(WalkableLayers, obj.layer));
            foreach (Layer mask in UnWalkableLayers) {
                node.walkable = !(Physics.CheckSphere(new Vector3(node.x, 0, node.y), nodeScale / 2, mask.LayerMask));
                if (!node.walkable) {
                    break;
                }
            }

            this.AddToDict(node);
            this.list.Add(node);

            GameObject temp = GameObject.Instantiate(NodeCollider);
            temp.GetComponent<TrackingNode>().node = node;
            temp.transform.position = new Vector3(node.x, obj.transform.position.y, node.y);
            temp.transform.parent = Collider_Container.transform;
 */