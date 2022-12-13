using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class AStar
{
    // Start is called before the first frame update

    Grid grid;

    public List<Node> MostRecentPath = new List<Node>();

    float[] debug_coords = new float[] { 0, 0 };

    //float[] debugCoordinates = new float[2];

    public AStar(Grid _grid, float[] debug_coords)
    {
        this.grid = _grid;
        this.debug_coords = debug_coords;
    }

    public Node GetNode(float x, float y) {
        return this.grid.GetNode(x, y);
    }

    public bool CheckSize(Node node1, Node node2) {
        return (node2.fCost < node1.fCost || (node2.fCost == node1.fCost && node2.hCost < node1.hCost));
    }

    public List<Node> FindPath(Node startNode, Node targetNode) {
        startNode.gCost = 0;
        startNode.hCost = 0;
        targetNode.parent = null;

        LL.LinkedList<Node> opened = new LL.LinkedList<Node>(startNode, CheckSize);
        List<Node> closed = new List<Node>();

        //opened.Add(startNode);

        while (opened.length > 0) {
            if (opened.first == null) {

            }

            Node ptr = opened.Push();
            closed.Add(ptr);

            if (ptr == targetNode) {
                return this.TracePath(startNode, targetNode);
            }
            if (ptr.x == debug_coords[0] && ptr.y == debug_coords[1]) {

            }
            foreach (Node neighbour in ptr.Directions) {
                CheckNeighour(neighbour, ptr, targetNode, opened, closed);
            }
        }
        return new List<Node>();
    }
    void CheckNeighour(Node neighbour, Node current, Node target, LL.LinkedList<Node> opened, List<Node> closed) {
        if (neighbour == null) return;
        if (!neighbour.walkable || closed.Contains(neighbour)) return;

        float movementCost = current.gCost + current.GetDistance(neighbour);
        bool neighbourInOpened = opened.Contains(neighbour);
        if (debug_coords[0] == neighbour.x && debug_coords[1] == neighbour.y) {

        }

        if (movementCost < neighbour.gCost || !neighbourInOpened) {
            neighbour.gCost = movementCost;
            neighbour.hCost = neighbour.GetDistance(target);
            neighbour.parent = current;

            if (!neighbourInOpened) {
                opened.Add(neighbour);
            }
            else {
                opened.Update(neighbour);
            }

        }
    }

    List<Node> TracePath(Node start, Node target) {
        if (target.parent == null) return null;

        List<Node> path = new List<Node>();
        for (Node ptr = target; ptr != start; ptr = ptr.parent) {
            path.Add(ptr);
        }
        path.Reverse();

        if (this.grid.debug) MostRecentPath = path;
        return path;
    }
}
