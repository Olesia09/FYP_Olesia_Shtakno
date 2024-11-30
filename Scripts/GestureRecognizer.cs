using System;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;


namespace Assets.VRehab.Scripts
{
    public class GestureRecognizer : MonoBehaviour
    {
        public XRNode InputSource;
        public InputHelpers.Button InputButton;
        public float InputThreshold = 0.1f;
        public Transform MovementSource;

        public float NewPosThresholdDistance = 0.05f;
        public GameObject DebugCubePrefab;
        public bool CreationMode = true;
        public string NewGestureName;

        private List<Gesture> _trainingSet = new();
        private bool _isMoving = false;
        private bool _isDrawing = false; 
        private List<Vector3> _posList = new();

        public TemplateSpawner Spawner;
        public DisplayScore DisplayScore;
        
        void Start()
        {
            var gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
            foreach (var item in gestureFiles)
            {
                _trainingSet.Add(GestureIO.ReadGestureFromFile(item));
            }


        }

        void Update()
        {
            InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(InputSource), InputButton, out var isPressed, InputThreshold);

            
            switch (isPressed)
            {
                case true when !_isDrawing:
                    _isDrawing = true;
                    StartMovement();
                    break;
                case true when _isDrawing:
                    UpdateMovement();
                    break;
                default:
                {
                    if (_isDrawing)
                    {

                        EndMovement();
                        _isDrawing = false;
                    }

                    break;
                }
            }
            
        }

        private void StartMovement()
        {
            Debug.Log("Start Movement Segment");
            _isMoving = true;
            if (_posList.Count == 0)
            {
                //Start fresh list
                _posList.Clear();
            }
            _posList.Add(MovementSource.position);

            if (DebugCubePrefab)
            {
                Instantiate(DebugCubePrefab, MovementSource.position, Quaternion.identity);
            }
        }   

        private void EndMovement()
        {
            Debug.Log("End Movement Segment");
            _isMoving = false;
        }

        private void UpdateMovement()
        {
            if (!_isMoving) return;

            Debug.Log("Update Movement Segment");
            var lastPosition = _posList[^1];

            if (!(Vector3.Distance(MovementSource.position, lastPosition) > NewPosThresholdDistance)) return;
            _posList.Add(MovementSource.position);
            if (DebugCubePrefab)
            {
                Instantiate(DebugCubePrefab, MovementSource.position, Quaternion.identity);
            }
        }

        public void SubmitGesture() // Call when to save or recognize the full gesture
        {
            var templateName = Spawner.GetTemplateName(); // get the current template name
            var pointArray = new Point[_posList.Count];
            for (var i = 0; i < _posList.Count; i++)
            {
                Vector2 screenPoint = Camera.main.WorldToScreenPoint(_posList[i]);
                pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
            }

            var newGesture = new Gesture(pointArray);

            if (CreationMode)
            {
                newGesture.Name = NewGestureName;
                _trainingSet.Add(newGesture);
                var fileName = Application.persistentDataPath + "/" + NewGestureName + ".xml";
                GestureIO.WriteGesture(pointArray, NewGestureName, fileName);
            }
            else
            {
                var result = PointCloudRecognizer.Classify(newGesture, _trainingSet.ToArray());
                Debug.Log(result.GestureClass + result.Score);

                if (result.Score == 0f || result.GestureClass != templateName)
                {
                    DisplayScore.InaccurateDrawing();
                }
                else
                {
                    DisplayScore.UpdateScore(result.Score, result.GestureClass);
                }

                Spawner.SpawnNewTemplate();
            }

            _posList.Clear();  // Clear after gesture submission
            DeleteAllDebugCubes();
        }

        public void ResetGesture() // Call reset the gesture list to start drawing again
        {
            _posList.Clear();
            Debug.Log("Drawing Reset");
            DeleteAllDebugCubes();
        }

        public void DeleteAllDebugCubes()
        {
            var debugCubes = GameObject.FindGameObjectsWithTag("CubePrefab");
            foreach (var cube in debugCubes)
            {
                Destroy(cube);
            }
        }
    }
}
