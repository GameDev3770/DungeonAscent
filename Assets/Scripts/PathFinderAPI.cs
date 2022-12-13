using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinderAPI : MonoBehaviour
{
    public bool debug;
    public bool gizmos;
    public bool handles;
    public float[] debug_coords = new float[2];

    public Layer[] WalkableLayers = new Layer[0];
    public Layer[] UnWalkableLayers = new Layer[0];
    public float nodeScale = 1;
    public GameObject debugGameObject;

    // Start is called before the first frame update
    AStar aStar;
    Grid grid;

    void Start() {
        this.grid = new Grid(debug, gizmos, handles, WalkableLayers, UnWalkableLayers, nodeScale, debugGameObject);
        this.grid.BuildGrid(gameObject);

        this.aStar = new AStar(this.grid, debug_coords);
    }
    public List<Node> GetPathing(Vector3 StartObject, Vector3 EndObject) {
        return this.GetPathing(GetNode(StartObject), GetNode(EndObject));
    }
    public List<Node> GetPathing(GameObject StartObject, Vector3 EndObject)     { return this.GetPathing(GetNode(StartObject), GetNode(EndObject)); }
    public List<Node> GetPathing(Vector3 StartObject, GameObject EndObject)     { return this.GetPathing(GetNode(StartObject), GetNode(EndObject)); }
    public List<Node> GetPathing(GameObject StartObject, GameObject EndObject)  { return this.GetPathing(GetNode(StartObject), GetNode(EndObject)); }

    public List<Node> GetPathing(Node StartNode, Node EndNode) {
        try {
            return this.aStar.FindPath(StartNode, EndNode);
        } catch (NullReferenceException) {
            return new List<Node>();
        }
    }

    public Node GetNode(GameObject gameobject) {
        return this.grid.GetNode(gameobject.transform.position);
        //return this.grid.GetNode(gameobject.transform.position.x, gameobject.transform.position.z);
    }
    public Node GetNode(Vector3 vector) {
        return this.grid.GetNode(vector);
        //return this.grid.GetNode(gameobject.transform.position.x, gameobject.transform.position.z);
    }

    void OnDrawGizmos() {
        if (aStar != null) grid.OnDrawGizmos(aStar);
    }
}
