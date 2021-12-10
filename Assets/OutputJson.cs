using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace Nodes
{
    public class OutputJson : MonoBehaviour
    {
        private string filePath;

        [Serializable]
        private class SerializableVector3
        {
            public float x, y, z;

            public SerializableVector3(float X, float Y, float Z)
            {
                x = X;
                y = Y;
                z = Z;
            }

            public SerializableVector3(Vector3 v)
            {
                x = v.x;
                y = v.y;
                z = v.z;
            }

        }

        [Serializable]
        private class Node
        {
            public string nodeName { get; set; }
            public bool isElevator;
            public bool isStairs;
            public SerializableVector3 position { get; set; }
            public List<Tuple<int, double>> adjacentNodes { get; set; }
        }

        [Serializable]
        private class Floor
        {
            public int floorNum { get; set; }
            public bool haveElevators { get; set; }
            public bool haveStairs { get; set; }
            public List<Node> nodes { get; set; }
        }

        private List<Floor> info = new List<Floor>();

        // Start is called before the first frame update
        void Start()
        {
            filePath = Application.dataPath;
#if UNITY_EDITOR
            GenerateJson();
#endif
        }

        // Update is called once per frame
        void Update()
        {

        }

        void GenerateJson()
        {
            int i = 1;
            int nodeNumber = 0;
            double distance = 0;

            for (int c = 0; c < transform.childCount; c++)
            {
                bool haveElevators = false;
                bool haveStairs = false;
                NodeInfo[] nodeInfos = transform.GetChild(c).GetComponentsInChildren<NodeInfo>();

                if (nodeInfos == null) { break; }

                List<Node> floor = new List<Node>();
                for (int j = 0; j < nodeInfos.Length; j++)
                {
                    floor.Add(new Node());
                }

                foreach (NodeInfo nodeInfo in nodeInfos)
                {
                    Node node = new Node();
                    List<GameObject> adj = nodeInfo.adjacentNodes;
                    node.nodeName = nodeInfo.nodeName;
                    node.isElevator = nodeInfo.isElevator;
                    node.isStairs = nodeInfo.isStairs;
                    node.position = new SerializableVector3(nodeInfo.transform.position);
                    node.adjacentNodes = new List<Tuple<int, double>>();
                    foreach (GameObject n in adj)
                    {
                        nodeNumber = n.GetComponent<NodeInfo>().nodeNumber;
                        distance = Vector3.Distance(nodeInfo.transform.position, n.transform.position);
                        node.adjacentNodes.Add(new Tuple<int, double>(nodeNumber, distance));
                    }
                    floor[nodeInfo.nodeNumber] = node;

                    if (nodeInfo.isElevator) haveElevators = true;
                    if (nodeInfo.isStairs) haveStairs = true;
                }

                info.Add(new Floor());
                info[i - 1].floorNum = i;
                info[i - 1].haveElevators = haveElevators;
                info[i - 1].haveStairs = haveStairs;
                info[i - 1].nodes = floor;
                i++;
            }

            // Generate JSON
            string json = JsonConvert.SerializeObject(info);
            File.WriteAllText(filePath + "/Node_InformationCSE.json", json);
        }
    }
}