using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class MediapipeRTStream : MonoBehaviour
{
    NamedPipeServerStream server;
    // Start is called before the first frame update
    public float angleRightElbow = 0f;
    public float angleRightShoulder = 0f;
    public float angleRightHip = 0f;
    public float angleRightKnee = 0f;
    public float angleLeftElbow = 0f;
    public float angleLeftShoulder = 0f;
    public float angleLeftHip = 0f;
    public float angleLeftKnee = 0f;
    public BinaryReader br;
    const int LINES_COUNT = 11;
    public string[] lines;
    public Text angleRE;
    void Awake()
    {
       // runPython();

        // Open the named pipe.
        server = new NamedPipeServerStream("VRBodyTrack", PipeDirection.InOut, 99, PipeTransmissionMode.Message);
        UnityEngine.Debug.Log("Waiting for connection...");
        server.WaitForConnection();
        UnityEngine.Debug.Log("Connected.");
        
        //initialise a BinaryReader to get the data from the named pipe
        br = new BinaryReader(server);

        //start a thread to get the data through it from the pipe
        Thread t = new Thread(new ThreadStart(connected));
        t.Start();
    }
    void Update()
    {
        connected();
    }
    void connected()
    {
        
        
            try
            {
                /*For each frame, len gets the data from the pipe which is then transformed into
                string in str to be able to separate the lines (each line representing a Mediapipe node) to parse them and reform the coordinates into Vector3 objects.
                In this script, the nodes that are saved are the nodes for the angles of interest of the body (elbows, shoulders, hips and knees)
                */
                var len = (int)br.ReadUInt32();
                var str = new string(br.ReadChars(len));

                lines = str.Split('\n');
                List<Vector3> frameInterest = new();
                for (int i = 0; i < 13; i++)
                {
                    frameInterest.Add(new Vector3());
                }
                foreach (string l in lines)
                {
                    if (string.IsNullOrWhiteSpace(l))
                        continue;
                    string[] s = l.Split('|');
                    if (s.Length < 4) continue;
                    float x = float.Parse(s[1]);
                    float y = float.Parse(s[2]);
                    float z = float.Parse(s[3]);
                    //15, 13, 11, 23, 25, 27, 16, 14, 12, 24, 26, 28
                    switch (s[0])
                    {
                        case "15":

                            frameInterest[0] = new Vector3(x, y, z);
                            break;
                        case "13":

                            frameInterest[1] = new Vector3(x, y, z);
                            break;
                        case "11":

                            frameInterest[2] = new Vector3(x, y, z);
                            break;
                        case "23":

                            frameInterest[3] = new Vector3(x, y, z);
                            break;
                        case "25":

                            frameInterest[4] = new Vector3(x, y, z);
                            break;
                        case "27":

                            frameInterest[5] = new Vector3(x, y, z);
                            break;
                        case "16":

                            frameInterest[6] = new Vector3(x, y, z);
                            break;
                        case "14":

                            frameInterest[7] = new Vector3(x, y, z);
                            break;
                        case "12":

                            frameInterest[8] = new Vector3(x, y, z);
                            break;
                        case "24":

                            frameInterest[9] = new Vector3(x, y, z);
                            break;
                        case "26":

                            frameInterest[10] = new Vector3(x, y, z);
                            break;
                        case "28":

                            frameInterest[11] = new Vector3(x, y, z);
                            break;
                    }
                    //UnityEngine.Debug.Log(l); 
                    Vector3 rForearm = (frameInterest[0] - frameInterest[1]).normalized;
                    Vector3 rArm = (frameInterest[1] - frameInterest[2]).normalized;
                    Vector3 rTorso = (frameInterest[2] - frameInterest[3]).normalized;
                    Vector3 rLeg = (frameInterest[3] - frameInterest[4]).normalized;
                    Vector3 rCalf = (frameInterest[4] - frameInterest[5]).normalized;
                    Vector3 lForearm = (frameInterest[6] - frameInterest[7]).normalized;
                    Vector3 lArm = (frameInterest[7] - frameInterest[8]).normalized;
                    Vector3 lTorso = (frameInterest[8] - frameInterest[9]).normalized;
                    Vector3 lLeg = (frameInterest[9] - frameInterest[10]).normalized;
                    Vector3 lCalf = (frameInterest[10] - frameInterest[11]).normalized;

                    angleRightElbow = Vector3.Angle(rForearm, rArm);
                    angleRightShoulder = Vector3.Angle(rArm, rTorso);
                    angleRightHip = Vector3.Angle(rTorso, rLeg);
                    angleRightKnee = Vector3.Angle(rLeg, rCalf);
                    angleLeftElbow = Vector3.Angle(lForearm, lArm);
                    angleLeftShoulder = Vector3.Angle(lArm, lTorso);
                    angleLeftHip = Vector3.Angle(lTorso, lLeg);
                    angleLeftKnee = Vector3.Angle(lLeg, lCalf);
                    //UnityEngine.Debug.Log("Right Elbow Angle in real-time stream: " + angleRightElbow);
                    // UnityEngine.Debug.Log("Right Shoulder Angle in real-time stream: " + angleRightShoulder);
                    // UnityEngine.Debug.Log("Right Hip Angle in real-time stream: " + angleRightHip);
                    // UnityEngine.Debug.Log("Right Knee Angle in real-time stream: " + angleRightKnee);
                    // UnityEngine.Debug.Log("Left Elbow Angle in real-time stream: " + angleLeftElbow);
                    // UnityEngine.Debug.Log("Left Shoulder Angle in real-time stream: " + angleLeftShoulder);
                    //UnityEngine.Debug.Log("Left Hip Angle in real-time stream: " + angleLeftHip);
                    // UnityEngine.Debug.Log("Left Knee Angle in real-time stream: " + angleLeftKnee);
                }

            }
            catch (EndOfStreamException)
            {
                                    // When client disconnects
            }
        

    }
    void runPython()
    {
        //WIP, this code should start the python MediaPipe server at the start of the project


        string pythonPath = "C:/Users/matei/AppData/Local/Programs/Python/Python39/python.exe"; // e.g., "C:/Python39/python.exe"

        // Replace this with the path to your Python script
        string pythonScriptPath = "../../../ServerOpen/main.py"; // e.g., "Assets/Scripts/YourScript.py"

        ProcessStartInfo startInfo = new(pythonPath)
        {
            Arguments = pythonScriptPath,

            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = new()
        {
            StartInfo = startInfo
        };

        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();
    }

    private void OnDisable()
    {
        print("Client disconnected.");
        server.Close();
        server.Dispose();
    }
}