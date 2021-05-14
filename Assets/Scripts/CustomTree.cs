using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;

// transformation data of turtle
public class Transformation {
    public Vector3 position;
    public Quaternion rotation;
    public int depth;
}

// struct to store a character/rule and its data
public struct Node {

    public Node(char n, float i = -1f, float j = -1f) {
        name = n;
        l = i;
        w = j;
    }
    
    public char name;
    public float l;
    public float w;
}

public class CustomTree : MonoBehaviour
{
    [SerializeField] private GameObject Branch; // branch object to instantiate
    [SerializeField] private GameObject Leaf;

    // customizable globals
    [SerializeField] private int treeType;
    [SerializeField] private float treeLength;
    [SerializeField] private float treeWidth;
    [SerializeField] private float r1;       // contraction ratios 
    [SerializeField] private float r2;
    [SerializeField] private float a0;       // branching angles
    [SerializeField] private float a2;
    [SerializeField] private float d;        // divergence angles
    [SerializeField] private float wr;       // trunk width decrease rate
    [SerializeField] private int iterations; // number of iterations to generate tree

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private int currentDepth;

    // List of characters in L system
    List<Node> nodes = new List<Node>();
    // List of branches to delete upon update
    List<GameObject> branches = new List<GameObject>();
    List<GameObject> leafs = new List<GameObject>();
    // Stack of transformation data
    private Stack<Transformation> transformStack;

    // sets global variables from TreeGlobals
    void setGlobals() {

        iterations = TreeGlobals.iterations;
        r1 = TreeGlobals.r1;
        r2 = TreeGlobals.r2;
        a0 = TreeGlobals.a0;
        a2 = TreeGlobals.a2;
        d = TreeGlobals.d;
        wr = TreeGlobals.wr;
        treeType = TreeGlobals.type;
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        
        setGlobals(); 

        var scale = new Vector3(1f, 1f, .5f);
        Branch.transform.localScale = scale;
        treeLength = 2f;
        treeWidth = .3f;
        nodes.Add(new Node('A', treeLength, treeWidth));
        transformStack = new Stack<Transformation>();
        
        GenerateTree();
    }

    // rule A - adds a set of characters with values
    private void addA(Node A, List<Node> nodesList) {
        nodesList.Add(new Node('!', A.w));
        nodesList.Add(new Node('F', A.l));
        nodesList.Add(new Node('['));
        nodesList.Add(new Node('&', a0));
        nodesList.Add(new Node('B', A.l * r2, A.w * wr));
        nodesList.Add(new Node(']'));
        nodesList.Add(new Node('/', d));
        nodesList.Add(new Node('A', A.l * r1, A.w * wr));
    }

    // rule B - adds a set of characters with values
    private void addB(Node B, List<Node> nodesList) {
        nodesList.Add(new Node('!', B.w));
        nodesList.Add(new Node('F', B.l));
        nodesList.Add(new Node('['));
        nodesList.Add(new Node('-', a2));
        nodesList.Add(new Node('$'));
        nodesList.Add(new Node('C', B.l * r2, B.w * wr));
        nodesList.Add(new Node(']'));
        nodesList.Add(new Node('C', B.l * r1, B.w * wr));
    }

    // rule C - adds a set of characters with values
    private void addC(Node C, List<Node> nodesList) {
        nodesList.Add(new Node('!', C.w));
        nodesList.Add(new Node('F', C.l));
        nodesList.Add(new Node('['));
        nodesList.Add(new Node('+', a2));
        nodesList.Add(new Node('$'));
        nodesList.Add(new Node('B', C.l * r2, C.w * wr));
        nodesList.Add(new Node(']'));
        nodesList.Add(new Node('B', C.l * r1, C.w * wr));
    }

     // rule  - adds a set of characters with values
    private void addD(Node D, List<Node> nodesList) {
        nodesList.Add(new Node('!', D.w));
        nodesList.Add(new Node('F', D.l));
        nodesList.Add(new Node('['));
        nodesList.Add(new Node('&', a0));
        nodesList.Add(new Node('E', D.l * r1, D.w * wr));
        nodesList.Add(new Node(']'));
        nodesList.Add(new Node('/', 180f));
        nodesList.Add(new Node('['));
        nodesList.Add(new Node('&', a2));
        nodesList.Add(new Node('E', D.l * r2, D.w * wr));
        nodesList.Add(new Node(']'));
    }

    private void addE(Node E, List<Node> nodesList) {
        nodesList.Add(new Node('!', E.w));
        nodesList.Add(new Node('F', E.l));
        nodesList.Add(new Node('['));
        nodesList.Add(new Node('+', a0));
        nodesList.Add(new Node('$'));
        nodesList.Add(new Node('E', E.l * r1, E.w * wr));
        nodesList.Add(new Node(']'));
        nodesList.Add(new Node('['));
        nodesList.Add(new Node('-', a2));
        nodesList.Add(new Node('$'));
        nodesList.Add(new Node('E', E.l * r2, E.w * wr));
        nodesList.Add(new Node(']'));
    }

    private void GenerateTree() {
        currentDepth = 0;
        // Generate string of characters
        for(int i = 0; i < iterations; i++) {
            List<Node> newNodes = new List<Node>();

            foreach(Node n in nodes) {
                // follow rule or add character
                switch(n.name) {
                    case 'A':
                        addA(n, newNodes);
                        break;
                    case 'B':
                        addB(n, newNodes);
                        break;
                    case 'C':
                        addC(n, newNodes);
                        break;
                    case 'D':
                        addD(n, newNodes);
                        break;
                    case 'E':
                        addE(n, newNodes);
                        break;
                    default:
                        newNodes.Add(n);
                        break;
                }
            }
            nodes = newNodes;
        }

        
        foreach(Node n in nodes) {
            switch(n.name) {
                case 'F': // move turtle, spawn branch and leaf
                    Vector3 initialPos = transform.position;
                    transform.Translate(Vector3.up * n.l);
                    
                    GameObject branch = Instantiate(Branch, initialPos + 
                            (transform.position-initialPos)/2, transform.rotation);
                    var scale = new Vector3(branch.transform.localScale.x, n.l/2,
                            branch.transform.localScale.z);
                    branch.transform.localScale = scale;
                    branches.Add(branch);
                    // add leaf if toggled and at correct depth
                    if(TreeGlobals.toggleLeaves && currentDepth > 2) {
                        Quaternion randomRotation = Quaternion.Euler(Random.Range(0f, 360f),
                                Random.Range(0f, 360f), Random.Range(0f, 360f));
                        GameObject leaf = Instantiate(Leaf, transform.position, randomRotation);
                        leafs.Add(leaf);
                    }
                    currentDepth++;
                    break;
                case '+': // roll right
                    transform.Rotate(Vector3.forward * n.l, Space.Self);
                    break;
                case '-': // roll left
                    transform.Rotate(Vector3.back * n.l, Space.Self);
                    break;
                case '&': // pitch up
                    transform.Rotate(Vector3.left * n.l, Space.Self);
                    break;
                case '^': // pitch down
                    transform.Rotate(Vector3.right * n.l, Space.Self);
                    break;
                case '\\': // yaw right
                    transform.Rotate(Vector3.down * n.l, Space.Self);
                    break;
                case '/': // yaw left
                    transform.Rotate(Vector3.up * n.l, Space.Self);
                    break;
                case '[': // push turtle
                    transformStack.Push(new Transformation()
                    {
                        position = transform.position,
                        rotation = transform.rotation,
                        depth = currentDepth
                    });
                    break;
                case ']': // pop turtle
                    Transformation t = transformStack.Pop();
                    transform.position = t.position;
                    transform.rotation = t.rotation;
                    currentDepth = t.depth;
                    break;
                case '$': // randomly rotate turtle
                    transform.Rotate(Vector3.up * Random.Range(15.0f, 70.0f), Space.Self);
                    break;
                case '!': // reset branch scale
                    var newScale = new Vector3(n.l, Branch.transform.localScale.y, n.l);
                    Branch.transform.localScale = newScale;
                    break;
            }
        }
    }

    void Update() {
        if(TreeGlobals.updated) {
            // destroy old branches
            foreach(GameObject b in branches) {
                Destroy(b);
            }

            foreach(GameObject l in leafs) {
                Destroy(l);
            }

            // reset turtle position
            transform.position = startingPosition;
            transform.rotation = startingRotation;
            setGlobals();

            // reset branch scale
            var scale = new Vector3(1f, 1f, .5f);
            Branch.transform.localScale = scale;

            // reset stack / lists
            transformStack = new Stack<Transformation>();
            branches = new List<GameObject>();
            leafs = new List<GameObject>();
            nodes = new List<Node>();
            
            if(treeType == 0) {
                nodes.Add(new Node('A', treeLength, treeWidth));
            } else if (treeType == 1) {
                nodes.Add(new Node('D', treeLength, treeWidth));
            }

            GenerateTree();
            TreeGlobals.updated = false;
        }
    }
}
