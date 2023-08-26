using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Nodes : MonoBehaviour
{
    public MediapipeRTStream data;
    public GameObject avatar;
    public GameObject node_pref;
    private BinaryReader reader;
    public List<GameObject> nodes = new List<GameObject>();
    private List<SkinnedMeshRenderer> y;
    // Start is called before the first frame update
    void Start()
    {
        reader = data.br;
        for (int i = 0; i < 33; i++) {
            
            var node=Instantiate(node_pref);
            nodes.Add(node);
        }
        y = avatar.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
        foreach (var os in y)
        {
            os.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        try
        {

            var len = (int)reader.ReadUInt32();
            var str = new string(reader.ReadChars(len));

            string[] lines = str.Split('\n');
            foreach (string l in lines)
            {
                if (string.IsNullOrWhiteSpace(l))
                    continue;
                string[] s = l.Split('|');
                if (s.Length < 4) continue;
                float x = float.Parse(s[1]);
                float y = float.Parse(s[2]);
                float z = float.Parse(s[3]);

                nodes[int.Parse(s[0])].transform.position = new Vector3(-x, -y,-z);
            }
        }
        catch (EndOfStreamException)
        {
            //Debug.Log("Server disconnected");                    // When client disconnects
        }
        
    }
    
    public void pressedButton2()
    {
        foreach (var node in nodes)
        {
            node.GetComponentInChildren<MeshRenderer>().enabled = false;
            
            Debug.Log("sad");
        }

        
        foreach(var os in y)
        {
            os.enabled = true;
        }
    }

    public void pressedButton1()
    {
        foreach (var node in nodes)
        {
            node.GetComponentInChildren<MeshRenderer>().enabled = true;
            Debug.Log("boshe");
        }
        foreach (var os in y)
        {
            os.enabled = false;
        }
    }
}

