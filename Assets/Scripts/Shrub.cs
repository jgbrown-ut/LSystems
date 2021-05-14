using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using System;


public class Shrub : MonoBehaviour
{
    [SerializeField] private GameObject Branch;
    [SerializeField] private GameObject Leaf;
    [SerializeField] private GameObject Berry;
    [SerializeField] private float length = 10f;
    [SerializeField] private float radius = 2;
    [SerializeField] private float angle = 28f;
    [SerializeField] private int iterations = 1;

    private string axiom = "FFFA";
    private Stack<Transformation> transformStack;
    private Dictionary<char, string> rules;

    // Start is called before the first frame update
    void Start()
    {
        transformStack = new Stack<Transformation>();
        rules = new Dictionary<char, string>
        {
            {'X', "[F-[[X]+X]+F[+FX]-X]"},
            {'A', "[&&J][B]////[&&J][B]////[&&J]B"},
            {'B', "&FFFAK"}

        };
        GenerateTree();
    }

    private void GenerateTree() {
        String cur = axiom;
        length = length / iterations;
        // 
        for(int i = 0; i < iterations; i++) {

            StringBuilder sb = new StringBuilder();
            foreach(char c in cur) {
                if(rules.ContainsKey(c)) {
                    sb.Append(rules[c]);
                } else {
                    sb.Append(c.ToString());
                }
            }
            cur = sb.ToString();
        }

        Debug.Log(cur);
        
        foreach(char c in cur) {
            switch(c) {
                case 'F':
                    Vector3 initialPos = transform.position;
                    transform.Translate(Vector3.up * length);
                    
                    GameObject branch = Instantiate(Branch, initialPos +
                            (transform.position-initialPos)/2, transform.rotation);
                    var scale = new Vector3(branch.transform.localScale.x, length/2,
                            branch.transform.localScale.z);
                    branch.transform.localScale = scale;
                    break;
                case 'J':
                    transform.Translate(Vector3.up * length/2);
                    initialPos = transform.position;
                    GameObject leaf = Instantiate(Leaf, initialPos, transform.rotation);
                    scale = new Vector3(leaf.transform.localScale.x, length, leaf.transform.localScale.z);
                    leaf.transform.localScale = scale;
                    break;
                case 'K':
                    GameObject berry = Instantiate(Berry, transform.position, transform.rotation);

                    break;
                case 'X':
                    break;
                case '+':
                    transform.Rotate(Vector3.forward * angle, Space.Self);
                    break;
                case '-':
                    transform.Rotate(Vector3.back * angle, Space.Self);
                    break;
                case '&':
                    transform.Rotate(Vector3.left * angle, Space.Self);
                    break;
                case '^':
                    transform.Rotate(Vector3.right * angle, Space.Self);
                    break;
                case '\\':
                    transform.Rotate(Vector3.down * angle, Space.Self);
                    break;
                case '/':
                    transform.Rotate(Vector3.up * angle, Space.Self);
                    break;
                case '[':
                    transformStack.Push(new Transformation()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;
                case ']':
                    Transformation t = transformStack.Pop();
                    transform.position = t.position;
                    transform.rotation = t.rotation;
                    radius *= 2;
                    break;
            }
        }
    }
}
