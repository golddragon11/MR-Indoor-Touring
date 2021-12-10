using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes
{
    public class NodeInfo : MonoBehaviour
    {
        public string nodeName;
        public int nodeNumber;
        public bool isElevator;
        public bool isStairs;
        public List<GameObject> adjacentNodes;
    }
}