using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Nav;
using static Drop;
using static DropAR;
public class Pathfinding : MonoBehaviour
{
  [Serializable]
  public class Node_Property
  {
    public class Node2
    {
      public Vector3 position { get; set; }
      public List<Tuple<int, double>> adjacentNodes { get; set; }
    }
    /*public void Ccc(Vector3 a, List<Tuple<int, double>> b)
    {
        Node temp = new Node2 { position = a, adjacentNodes = b };
        nodes.Add(temp);
    }*/
    public List<Node2> nodes = new List<Node2>();
  }

  [Serializable]
  public class SerializableVector3
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
  public class Floor
  {
    [Serializable]
    public class Node
    {
      public string nodeName { get; set; }
      public bool isElevator;
      public bool isStairs;
      public Vector3 position { get; set; }
      public List<Tuple<int, double>> adjacentNodes { get; set; }
    }

    public int floorNum { get; set; }
    public bool haveElevators { get; set; }
    public bool haveStairs { get; set; }
    public List<Node> nodes { get; set; }

  }
  public static List<Floor> node_info = new List<Floor>();
  //public static List<Node_Property> node_info = new List<Node_Property>();
  public static Deque<int> Path = new Deque<int>();
  private static List<double> distance1 = new List<double>();
  private static List<double> distance2 = new List<double>();
  List<Vector3> user = new List<Vector3> { new Vector3(-8.03f, 1.0f, -1.26f), new Vector3(-6.6f, 1.0f, -1.24f), new Vector3(-6.6f, 1.0f, -2.5f) };
  int kkk = 0;
  public Text DebugText;
  public Text ButtonText;
    public Text warningContent;
    public Animator animator;
  private static int stair_num;
  public static Vector3 User_pos;
  public static int current_floor;
  public static bool is_Navigation = false;
  public bool infoPass = false;
  private static bool walk_Stairs = true;
  private static bool out_of_node = false;
  private static bool on_node = false;
  private static int floor_s;
  private static int floor_d;
  private static int start_point;
  private static int destination;
  private static int Past_nevigation_node_index;
  private static int current_node;
  private static int floor_change = 0;
  public static Vector3 targetPos;
  public static int boundary;
  public int stairs;
  public int elevators;
  public GameObject mCam;
  public GameObject arrow;
  [SerializeField]
  public float threshold;
  public class Deque<T>
  {
    T[] queue = new T[50];
    int head = 0;
    int tail = 49;
    int Size = 0;
    internal void push_back(T node)
    {
      Size++;
      tail = tail + 1 > 49 ? 0 : tail + 1;
      queue[tail] = node;
    }
    internal void push_front(T node)
    {
      Size++;
      head = head - 1 < 0 ? 49 : head - 1;
      queue[head] = node;
    }
    internal void pop_back()
    {
      Size--;
      tail = tail - 1 < 0 ? 49 : tail - 1;
    }
    internal void pop_front()
    {
      Size--;
      head = head + 1 > 49 ? 0 : head + 1;
    }
    public int size()
    {
      return Size;
    }
    public T back()
    {
      return queue[tail];
    }
    public T front()
    {
      return queue[head];
    }
    public T index(int num)
    {
      return queue[(head + num) % 50];
    }
    public void clear()
    {
      Size = 0;
      head = 0;
      tail = 49;
    }
    public void set_boundary(int boundary, bool front)
    {
      if (front)
      {
        head = (head + boundary) % 50;
        Size -= boundary;
      }
      else
      {
        tail = (head + boundary) % 50;
        Size = boundary + 1;
      }
    }
  }
  protected class PriorityQueue<T>
  {
    private class Node
    {
      public double Priority { get; set; }
      public T Object { get; set; }
    }
    List<Node> Queue = new List<Node>();
    Dictionary<T, int> Pos = new Dictionary<T, int>();
    private void swap(int index1, int index2)
    {
      Node temp = Queue[index1];
      Queue[index1] = Queue[index2];
      Queue[index2] = temp;
      Pos[Queue[index1].Object] = index1;
      Pos[Queue[index2].Object] = index2;
    }
    private void BuildMinHeap(int i)
    {
      while (i > 1 && Queue[i / 2].Priority > Queue[i].Priority)
      {
        swap(i, (i / 2));
        i = (i / 2);
      }
    }
    private void MinHeapify(int index)
    {
      int left = index * 2;
      int right = index * 2 + 1;
      int min = index;
      if (left <= Queue.Count - 1)
        min = Queue[min].Priority < Queue[left].Priority ? min : left;
      if (right <= Queue.Count - 1)
        min = Queue[min].Priority < Queue[right].Priority ? min : right;

      if (min != index)
      {
        swap(index, min);
        MinHeapify(min);
      }
    }
    public virtual void Enqueue(T Object, double priority)
    {
      Node new_Node = new Node() { Priority = priority, Object = Object };
      Queue.Add(new_Node);
      Pos[new_Node.Object] = Queue.Count - 1;
      BuildMinHeap(Pos[new_Node.Object]);
    }
    public virtual T Dequeue()
    {
      Node output = Queue[1];
      Queue[1] = Queue[Queue.Count - 1];
      //Debug.Log("Queue: " + Queue.Count);
      Pos[Queue[1].Object] = 1;
      Queue.RemoveAt(Queue.Count - 1);
      MinHeapify(1);
      return output.Object;
    }
    public virtual void DecreaseKey(T Object, double priority)
    {
      Queue[Pos[Object]].Priority = priority;
      BuildMinHeap(Pos[Object]);
    }
    public virtual void clear()
    {
      Queue.Clear();
      Pos.Clear();
    }
  }
  private class Dijkstra : PriorityQueue<int>
  {
    List<bool> shortestPath;
    List<double> distance;
    public Dijkstra(int node_num)
    {
      shortestPath = new List<bool>(node_num);
      distance = new List<double>(node_num);
      for (int i = 0; i < node_num; i++)
      {
        shortestPath.Add(false);
        distance.Add(int.MaxValue);
      }
    }
    public override void Enqueue(int Object, double priority)
    {
      base.Enqueue(Object, priority);
    }
    public override int Dequeue()
    {
      int node = base.Dequeue();
      shortestPath[node] = true;
      return node;
    }
    public override void DecreaseKey(int Object, double priority)
    {
      base.DecreaseKey(Object, priority);
    }
    internal List<double> heuristic(int floor, int start_point)
    {
      Enqueue(1000, 333);
      distance[start_point] = 0;
      Enqueue(start_point, 0);
      for (int i = 0; i < shortestPath.Capacity - 1; i++)
      {
        int min = Dequeue();
        shortestPath[min] = true;
        foreach (Tuple<int, double> node in node_info[floor].nodes[min].adjacentNodes)
        {
          //Debug.Log("node: " + node.Item1);
          if (distance[node.Item1] == int.MaxValue)
          {
            distance[node.Item1] = distance[min] + node.Item2;
            Enqueue(node.Item1, distance[node.Item1]);
            continue;
          }
          if (!shortestPath[node.Item1] && distance[min] + node.Item2 < distance[node.Item1])
          {
            distance[node.Item1] = distance[min] + node.Item2;
            DecreaseKey(node.Item1, distance[node.Item1]);
          }
        }
      }
      return distance;
    }
  }
  private class AStar : PriorityQueue<int>
  {
    public override void Enqueue(int Object, double priority)
    {
      base.Enqueue(Object, priority);
    }
    public override int Dequeue()
    {
      int node = base.Dequeue();
      return node;
    }
    public override void DecreaseKey(int Object, double priority)
    {
      base.DecreaseKey(Object, priority);
    }
    public override void clear()
    {
      base.clear();
    }
    internal void AStarSearch(int floor, int start_point, int destination, bool renevigation, List<double> heuristic_processd = null)
    {
      clear();
      Enqueue(1000, 333);
      int length = node_info[floor].nodes.Count;
      Deque<int> temp_path = new Deque<int>();
      List<bool> Inclosed = new List<bool>(length);
      List<bool> InOpen = new List<bool>(length);
      List<double> heuristic = new List<double>(length);
      List<double> g = new List<double>(length);
      for (int i = 0; i < length; i++)
      {
        Inclosed.Add(false);
        InOpen.Add(false);
        heuristic.Add(0);
        g.Add(0);
      }
      if (heuristic_processd == null)
      {
        Dijkstra dijkstra = new Dijkstra(length);
        heuristic = dijkstra.heuristic(floor, destination);
        distance1 = heuristic;
      }
      else if (renevigation)
        heuristic = heuristic_processd;
      else
      {
        heuristic = heuristic_processd;
        distance2 = heuristic_processd;
      }
      Enqueue(start_point, heuristic[start_point]);
      g[start_point] = 0;
      var current_node = Dequeue();
      Inclosed[current_node] = true;
      if (renevigation)
        temp_path.push_front(current_node);
      else
        Path.push_back(current_node);
      while (current_node != destination)
      {
        bool flag = true;
        List<double> priority = new List<double>();
        foreach (Tuple<int, double> node in node_info[floor].nodes[current_node].adjacentNodes)
        {
          if (InOpen[node.Item1] == true)
          {
            if (g[current_node] + node.Item2 < g[node.Item1])
              DecreaseKey(node.Item1, g[current_node] + node.Item2 + heuristic[node.Item1]);
            continue;
          }
          else if (Inclosed[node.Item1])
            continue;
          else
          {
            g[node.Item1] = g[current_node] + node.Item2;
            InOpen[node.Item1] = true;
            double Priority = g[node.Item1] + heuristic[node.Item1];
            foreach (double i in priority)
              if (Priority == i)
              {
                flag = false;
                break;
              }
            if (flag)
            {
              Enqueue(node.Item1, Priority);
              priority.Add(Priority);
            }
          }
        }
        current_node = Dequeue();
        InOpen[current_node] = false;
        Inclosed[current_node] = true;
        if (renevigation)
          temp_path.push_front(current_node);
        else
          Path.push_back(current_node);
      }
      if (renevigation)
      {
        temp_path.pop_front();
        Path.set_boundary(boundary, true);
        boundary = Path.size();
        for (int i = 0; i < (floor_d - floor_s - 1); i++)
          Path.push_front(temp_path.front());
        for (int i = 0; i < temp_path.size(); i++)
          Path.push_front(temp_path.index(i));
        boundary = Path.size() - boundary;
      }
    }
  }
  private Tuple<int, int> find_edge(int floor)
  {
    double close_ratio = 0;
    Tuple<int, int> edge = Tuple.Create(0, 0);
    foreach (Tuple<int, double> node in node_info[floor].nodes[current_node].adjacentNodes)
    {
      double ratio = node.Item2 / (Math.Sqrt(Math.Pow(User_pos.x - node_info[floor].nodes[current_node].position.x, 2) + Math.Pow(User_pos.z - node_info[floor].nodes[current_node].position.z, 2)) + Math.Sqrt(Math.Pow(User_pos.x - node_info[floor].nodes[node.Item1].position.x, 2) + Math.Pow(User_pos.z - node_info[floor].nodes[node.Item1].position.z, 2)));
      if (Math.Abs(1 - ratio) < Math.Abs(1 - close_ratio))
      {
        edge = Tuple.Create(current_node, node.Item1);
        close_ratio = ratio;
      }
    }
    return edge;
  }
  private int update_pos(int floor, int past_node = 1000)
  {
    int min_node = 0;
    if (past_node == 1000)
    {
      double min_distance = 1000;
      for (int i = 0; i < node_info[floor].nodes.Count; i++)
      {
        double distance = Math.Sqrt(Math.Pow(User_pos.x - node_info[floor].nodes[i].position.x, 2) + Math.Pow(User_pos.z - node_info[floor].nodes[i].position.z, 2));
        if (distance < min_distance)
        {
          min_distance = distance;
          min_node = i;
        }
      }
      /* Tuple<int, int> edge = find_edge(floor);
      min_node = (Math.Sqrt(Math.Pow(User_pos.x - node_info[floor].nodes[edge.Item1].position.x, 2) + Math.Pow(User_pos.z - node_info[floor].nodes[edge.Item1].position.z, 2)) < Math.Sqrt(Math.Pow(User_pos.x - node_info[floor].nodes[edge.Item2].position.x, 2) + Math.Pow(User_pos.z - node_info[floor].nodes[edge.Item2].position.z, 2))) ? edge.Item1 : edge.Item2; */
    }
    else
    {
      min_node = past_node;
      bool past_state = on_node;
      if (Math.Sqrt(Math.Pow(User_pos.x - node_info[floor].nodes[past_node].position.x, 2) + Math.Pow(User_pos.z - node_info[floor].nodes[past_node].position.z, 2)) < threshold)
        on_node = true;
      else
        on_node = false;
      for (int i = 0; i < node_info[floor].nodes[past_node].adjacentNodes.Count; i++)
      {
        if (Math.Sqrt(Math.Pow(User_pos.x - node_info[floor].nodes[node_info[floor].nodes[past_node].adjacentNodes[i].Item1].position.x, 2) + Math.Pow(User_pos.z - node_info[floor].nodes[node_info[floor].nodes[past_node].adjacentNodes[i].Item1].position.z, 2)) < threshold)
          min_node = node_info[floor].nodes[past_node].adjacentNodes[i].Item1;
      }
      if (past_state && !on_node)
        out_of_node = true;
    }
    return min_node;
  }
  private void delete_path_node(int floor)
  {
    Tuple<int, int> edge = find_edge(floor);
    if ((edge.Item1 == Path.front() || edge.Item1 == Path.index(1)) && (edge.Item2 == Path.front() || edge.Item2 == Path.index(1)))
      Path.pop_front();
  }
  private void change_target(int floor)
  {
    out_of_node = false;
    Tuple<int, int> edge = find_edge(floor);
    if ((edge.Item1 == Path.index(Past_nevigation_node_index + 1) || edge.Item2 == Path.index(Past_nevigation_node_index + 1)))
      return;
    else
    {
            warningContent.text = "方向錯誤!";
      animator.SetTrigger("appear");
      print("方向錯誤!");
    }
  }
  private void navigation()
  {
    start_point = update_pos(floor_s);
    AStar aStar = new AStar();
    if (floor_s == floor_d)
      aStar.AStarSearch(floor_s, start_point, destination, false);
    else
    {
      stair_num = Math.Abs(floor_d - floor_s) - 1;
      List<double> heuristic2 = new List<double>();
      Dijkstra dijkstra2 = new Dijkstra(node_info[floor_d].nodes.Count);
      heuristic2 = dijkstra2.heuristic(floor_d, destination);
      if (walk_Stairs == true)
      {
        List<double> heuristic1 = new List<double>();
        Dijkstra dijkstra = new Dijkstra(node_info[floor_s].nodes.Count);
        heuristic1 = dijkstra.heuristic(floor_s, start_point);
        int min_index = 1;
        double min = Int32.MaxValue;
        for (int i = 1; i <= stairs; i++)
        {
          Debug.Log("H1: " + heuristic1[i] + "H2: " + heuristic2[i]);
          if (heuristic1[i] + heuristic2[i] <= min)
          {
            min = heuristic1[i] + heuristic2[i];
            min_index = i;
          }
        }
        aStar.AStarSearch(floor_s, start_point, min_index, false);
        if (walk_Stairs)
          for (int i = 0; i < stair_num; i++)
            Path.push_back(Path.back());
        boundary = Path.size();
        Debug.Log("floor_d: " + floor_d + ", min_index: " + min_index + ", dest: " + destination);
        aStar.AStarSearch(floor_d, min_index, destination, false, heuristic_processd: heuristic2);
      }
      else
      {
        aStar.AStarSearch(floor_s, start_point, 0, false);
        if (walk_Stairs)
          for (int i = 0; i < stair_num; i++)
            Path.push_back(Path.back());
        boundary = Path.size();
        aStar.AStarSearch(floor_d, 0, destination, false, heuristic_processd: heuristic2);
      }
    }
    Past_nevigation_node_index = 0;
    current_node = start_point;
    delete_path_node(floor_s);
    Debug.Log("------------------------");
    for (int i = 0; i < Path.size(); i++)
        Debug.Log(Path.index(i));
    Debug.Log("------------------------");
  }
  public void call_navigation(int floor_des = 0, int target = 0)
  {
    if (is_Navigation)
    {
      Path.clear();
      distance1.Clear();
      distance2.Clear();
      is_Navigation = false;
    }
    else
    {
      floor_s = current_floor;
      floor_d = floor_des - 1;
      destination = target;
      navigation();
      next_node();
      is_Navigation = true;
    }
  }
  public void Start_Navigation(int floor_des = 0, int target = 0)
  {
    floor_s = current_floor;
    floor_d = floor_des - 1;
    destination = target;
    navigation();
    next_node();
    is_Navigation = true;
  }

  public void End_Navigation()
  {
    DebugText.text = "";
    Path.clear();
    distance1.Clear();
    distance2.Clear();
    is_Navigation = false;
  }
  public void preference(bool flag)
  {
    if (walk_Stairs == flag)
      return;
    else
    {
      if (is_Navigation)
      {
        is_Navigation = false;
        Path.clear();
        distance1.Clear();
        distance2.Clear();
        walk_Stairs = !walk_Stairs;
        navigation();
        next_node();
      }
      else
        walk_Stairs = !walk_Stairs;
    }
  }
  public void next_node()
  {
    int floor = current_floor;
    if (Past_nevigation_node_index >= 0)
    {
      if (Path.index(Past_nevigation_node_index) == Path.index(Past_nevigation_node_index + 1))
      {
        if (floor_change != 0)
        {
          if (floor_d - floor_s > 0)
          {
            if (floor_change == 1)
            {
              Past_nevigation_node_index++;
              if (current_floor != floor_d)
                  floor++;
            }
            else
            {
              Past_nevigation_node_index--;
              if (current_floor != floor_d)
                  floor--;
            }
          }
          else
          {
            if (floor_change == -1)
            {
              Past_nevigation_node_index++;
              if (current_floor != floor_d)
                  floor--;
            }
            else
            {
              Past_nevigation_node_index--;
              if (current_floor != floor_d)
                  floor++;
            }
          }
          floor_change = 0;
        }
        else
        {
          if (walk_Stairs)
            floor = floor_d - floor_s > 0 ? floor + 1 : floor - 1;
          else
            floor = floor_d;
        }
      }
    }
    Nav.nextpoint = node_info[floor].nodes[Path.index(Past_nevigation_node_index + 1)].position;
    print("下個目標: " + (floor + 1) + "F的節點" + Path.index(Past_nevigation_node_index + 1));
        
            if (current_floor > floor)
            {
                warningContent.text = "請下樓";
                animator.SetTrigger("appear");
            }

            else if (current_floor < floor)
            {
            Debug.Log(User_pos.ToString());
                warningContent.text = "請上樓";
                animator.SetTrigger("appear");
            }
            //DebugText.text = "下個目標: " + (floor + 1) + "F的節點" + Path.index(Past_nevigation_node_index + 1);
            targetPos = node_info[floor].nodes[Path.index(Past_nevigation_node_index + 1)].position;
  }
  public void GetPosition(int floor, Vector3 position)
  {
        if (current_floor > floor - 1)
        {
            floor_change = -1;

        }

    else if (current_floor < floor - 1)
        {
            floor_change = 1;

        }

        //DebugText.text = floor_change.ToString();
    current_floor = floor - 1;

    User_pos = position;
  }
  // Start is called before the first frame update
  void Start()
  {

    //File.WriteAllText(Application.dataPath + "/Node_Info_test.json", JsonConvert.SerializeObject(node_info));
    Scene scene = SceneManager.GetActiveScene();
    TextAsset txtAsset;
    String sceneName = scene.name;
    if (sceneName == "VRmode")
      txtAsset = (TextAsset)Resources.Load("Node_InformationCSE", typeof(TextAsset));
    else if (sceneName == "ARmode")
    {
      txtAsset = (TextAsset)Resources.Load("Node_InformationCSEAR", typeof(TextAsset));
    }
    else txtAsset = (TextAsset)Resources.Load("Node_InformationCSE", typeof(TextAsset));
    string tileFile = txtAsset.text;
    node_info = JsonConvert.DeserializeObject<List<Floor>>(txtAsset.text);
    Debug.Log("node_info: " + node_info[2].nodes[0].adjacentNodes[0]);
    Drop.infoGot = true;
    DropAR.infoGot = true;
    //node_info = JsonConvert.DeserializeObject<List<Node_Property>>(File.ReadAllText(Application.dataPath + "/Node_InformationCSE.json"));
    //Debug.Log(node_info[1].nodes[16].nodeName);
    //Drop.showtest(node_info);

  }

  // Update is called once per frame
  void Update()
  {
        
  }
  private void FixedUpdate()
  {
    //current_node = update_pos(current_floor);
    /*cene scene = SceneManager.GetActiveScene();
    String sceneName = scene.name;
    if (sceneName != "BeginningScene")
    {
        DebugText.text = "現在樓層\n" + (current_floor + 1) + "F";
    }*/

    if (is_Navigation)
    {
      int past_node = current_node;
      current_node = update_pos(current_floor, current_node);
      string name = node_info[current_floor].nodes[current_node].nodeName;
      //DebugText.text = "current floor: " + current_floor;
      if (String.Compare(name, "無") != 0) DebugText.text = "現在位置\n" + node_info[current_floor].nodes[current_node].nodeName + ", " + (current_floor + 1) + "F";
      else DebugText.text = "現在位置\n走廊, " + (current_floor + 1) + "F";
      if (out_of_node)
        change_target(current_floor);
      if (past_node != current_node || floor_change != 0)
      {
        if (walk_Stairs && floor_change != 0)
        {
                    Debug.Log(floor_change);
          next_node();
          return;
        }
        if (current_node == Path.index(Past_nevigation_node_index + 1))
        {
          if (current_node == destination && current_floor == floor_d)
          {
            is_Navigation = false;
            Debug.Log("抵達目標");
                        warningContent.text = "抵達目的地";
                        animator.SetTrigger("appear");
            DebugText.text = "抵達目標";
            arrow.SetActive(false);
            ButtonText.text = "開始導航";
            return;
          }
          Past_nevigation_node_index++;
          next_node();
        }
        else if (Past_nevigation_node_index != 0 && current_node == Path.index(Past_nevigation_node_index - 1))
        {
          Past_nevigation_node_index--;
          next_node();
        }
        else
        {
          if (current_floor == floor_d)
          {
            if (floor_s == floor_d)
            {
              int min_index = Path.index(Past_nevigation_node_index);
              double min = int.MaxValue;
              foreach (Tuple<int, double> node in node_info[floor_d].nodes[current_node].adjacentNodes)
              {
                if (node.Item2 + distance1[node.Item1] < min)
                {
                  min_index = node.Item1;
                  min = node.Item2 + distance1[node.Item1];
                }
              }
              if (min_index != Path.index(Past_nevigation_node_index))
              {
                Path.clear();
                navigation();
               warningContent.text = "已規劃出最短路徑!";
               animator.SetTrigger("appear");
                                print("已計算新的導航路徑!");
                //DebugText.text = "已計算新的導航路徑!";
              }
              else
              {
                Past_nevigation_node_index--;
              }
              next_node();
            }
            else
            {
              double min = int.MaxValue;
              int min_index = 1000;
              foreach (Tuple<int, double> node in node_info[floor_d].nodes[current_node].adjacentNodes)
              {
                if (node.Item2 + distance2[node.Item1] < min)
                {
                  min_index = node.Item1;
                  min = node.Item2 + distance2[node.Item1];
                }
              }
              if (min_index != Path.index(Past_nevigation_node_index))
              {
                Path.set_boundary(boundary, false);
                AStar aStar = new AStar();
                aStar.AStarSearch(floor_d, current_node, destination, false, heuristic_processd: distance2);
                Past_nevigation_node_index = boundary + 1;
                next_node();
                                warningContent.text = "已規劃出最短路徑!";
                                animator.SetTrigger("appear");
                                print("已計算新的導航路徑!");
                //DebugText.text = "已計算新的導航路徑!";
              }
            }
          }
          else
          {
            double min = int.MaxValue;
            int min_index = 1000;
            if (walk_Stairs)
            {
              List<double> heuristic1 = new List<double>();
              Dijkstra dijkstra = new Dijkstra(node_info[current_floor].nodes.Count);
              heuristic1 = dijkstra.heuristic(current_floor, current_node);
              for (int i = 1; i <= stairs; i++)
              {
                if (heuristic1[i] + distance2[i] <= min)
                {
                  min = heuristic1[i] + distance2[i];
                  min_index = i;
                }
              }
              if (min_index != Path.index(boundary))
              {
                Path.clear();
                navigation();
                next_node();
                                warningContent.text = "已規劃出最短路徑!";
                                animator.SetTrigger("appear");
                                print("已計算新的導航路徑!");
                //.text = "已計算新的導航路徑!";
                return;
              }
            }
            min = int.MaxValue;
            min_index = 1000;
            foreach (Tuple<int, double> node in node_info[current_floor].nodes[current_node].adjacentNodes)
            {
              if (node.Item2 + distance1[node.Item1] < min)
              {
                min_index = node.Item1;
                min = node.Item2 + distance1[node.Item1];
              }
            }
            if (min_index != Path.index(Past_nevigation_node_index))
            {
              AStar aStar = new AStar();
              aStar.AStarSearch(current_floor, current_node, Path.index(boundary), true, heuristic_processd: distance1);
              Past_nevigation_node_index = 0;
              next_node();
              warningContent.text = "已規劃出最短路徑!";
                            animator.SetTrigger("appear");
                            print("已計算新的導航路徑!");
              //DebugText.text = "已計算新的導航路徑!";
            }
          }
        }
      }
    }
    /*else
    {
        current_node = update_pos(current_floor, current_node);
    }
    string name = node_info[current_floor].nodes[current_node].nodeName;
    if (String.Compare(name, "無") != 0) DebugText.text = "現在位置\n" + node_info[current_floor].nodes[current_node].nodeName + ", " + (current_floor + 1) + "F";
    else DebugText.text = "現在位置\n走廊, " + (current_floor + 1) + "F";*/

  }
  public void teleport(int floor, int target)
  {
    if (mCam != null)
    {
      mCam.transform.position = new Vector3(node_info[floor].nodes[target].position.x, node_info[floor].nodes[target].position.y, node_info[floor].nodes[target].position.z);
    }
  }
}
