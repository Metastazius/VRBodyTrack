# VRBodyTrack
VR body track with MediaPipe

The code uses MediaPipe to gather live data to send directly to unity to animate a virtual avatar and process the joint angles of the person being recorded.

The MediaPipe server is based on https://github.com/ganeshsar/UnityPythonMediaPipeBodyPose

The Unity project gets the MediaPipe data from the python server through a NamedPipeServerStream and processes it for rigging the virtual avatar/s or the MediaPipe body nodes, 
and calculates the angles of the joints of interest for excercise analysis.
