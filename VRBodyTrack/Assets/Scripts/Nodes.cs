using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Nodes : MonoBehaviour
{
    public MediapipeRTStream data;
    public GameObject avatarY;
    public GameObject avatarX;
    public GameObject node_pref;
    private BinaryReader reader;
    public List<GameObject> nodes = new List<GameObject>();
    private List<SkinnedMeshRenderer> y;
    private List<SkinnedMeshRenderer> x;
    // Start is called before the first frame update
    void Start()
    {
        //gets the mediapipe data through the binaryreader from the MediapipeRTStream script
        reader = data.br;

        //instantiates the spheres that will be used as nodes in the 3D enviroment
        for (int i = 0; i < 33; i++) {
            
            var node=Instantiate(node_pref);
            nodes.Add(node);
        }

        //derenders the avatars at the start
        y = avatarY.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
        x = avatarX.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
        foreach (var os in y)
        {
            os.enabled = false;
        }
        foreach (var os in x)
        {
            os.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            //similar to MediapipeRTStream.cs, but here all the nodes are saved as Vector3 objects into a list
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
    
   
    public void pressedButton1()
    {
        //nodes button in unity, derenders the avatars and rerenders the nodes
        foreach (var node in nodes)
        {
            node.GetComponentInChildren<MeshRenderer>().enabled = true;
            Debug.Log("boshe");
        }
        foreach (var os in y)
        {
            os.enabled = false;
        }
        foreach (var os in x)
        {
            os.enabled = false;
        }
    }
    public void pressedButton2()
    {
        //avatar button in unity, derenders avatar X and the nodes and rerenders avatar Y
        foreach (var node in nodes)
        {
            node.GetComponentInChildren<MeshRenderer>().enabled = false;

            Debug.Log("sad");
        }


        foreach (var os in y)
        {
            os.enabled = true;
        }
        foreach (var os in x)
        {
            os.enabled = false;
        }
    }
    
    //avatar list in unity, derenders the avatar and the nodes and rerenders the avatar depending on the name
    //note: buggy, might be resolved from the unity editor
    public void listOption1()
    {
        
        foreach (var node in nodes)
        {
            node.GetComponentInChildren<MeshRenderer>().enabled = false;

            Debug.Log("sad");
        }


        foreach (var os in y)
        {
            os.enabled = true;
        }

        foreach (var os in x)
        {
            os.enabled = false;
        }
    }
    
    public void listOption2()
    {
        foreach (var node in nodes)
        {
            node.GetComponentInChildren<MeshRenderer>().enabled = false;

            Debug.Log("sad");
        }


        foreach (var os in y)
        {
            os.enabled = false;
        }

        foreach (var os in x)
        {
            os.enabled = true;
        }
    }
}

