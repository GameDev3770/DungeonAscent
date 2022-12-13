using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public struct Cardinal {
    public Node N;
    public Node NE;
    public Node E;
    public Node SE;
    public Node S;
    public Node SW;
    public Node W;
    public Node NW;

}
[Serializable]
public class Node
{
    public Cardinal Direction;

    public float[] Coordinates;
    public float weight;
    public float x, y;
    public bool walkable = true;

    public float gCost;
    public float hCost;
    public Node parent;


    public Node(float[] MiddlePoint, float radius, float weight) {
        this.Coordinates = new float[4] { MiddlePoint[0] - radius, MiddlePoint[0] + radius, MiddlePoint[1] - radius, MiddlePoint[1] + radius };
        this.x = MiddlePoint[0];
        this.y = MiddlePoint[1];
        this.weight = weight;
    }

    public float fCost {
        get {
            return this.gCost + this.hCost;
        }
    }

    public float GetDistance(Node target) {
        float dstX = Mathf.Abs(this.x - target.x);
        float dstY = Mathf.Abs(this.y - target.y);
        return (dstX > dstY) ? 10 * dstY + 10 * (dstX - dstY) : 10 * dstX + 10 * (dstY - dstX);
    }

    public Node[] Directions {
        get {
            return new Node[] {
                this.Direction.N,
                //this.Direction.NE,
                this.Direction.E,
                //this.Direction.SE,
                this.Direction.S,
                //this.Direction.SW,
                this.Direction.W,
                //this.Direction.NW,
            };
        }
    }

    public bool OnTop(float[] coords) {
        return this.Coordinates[0] < coords[1] && this.Coordinates[1] > coords[0] && this.Coordinates[2] < coords[3] && this.Coordinates[3] > coords[2];
    }
    public bool IsOnTile(float x, float y) {
        return this.Coordinates[0] <= x && x <= this.Coordinates[1] &&
                this.Coordinates[2] <= y && y <= this.Coordinates[3];
    }

    public bool Equals(Node node) {
        return this == node;
    }
}
