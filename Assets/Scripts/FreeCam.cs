using UnityEngine;
using UnityEngine.Animations;

namespace Unity.AI.Navigation.Samples
{
    public enum LineState
    {
        NONE,
        NEXT,
        PREV,
        INTERN
    }

    /// <summary>
    /// Manipulating the camera with standard inputs
    /// </summary>
    public class FreeCam : MonoBehaviour
    {
        public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
        public RotationAxes axes = RotationAxes.MouseXAndY;
        public float sensitivityX = 15F;
        public float sensitivityY = 15F;

        public float minimumX = -360F;
        public float maximumX = 360F;

        public float minimumY = -60F;
        public float maximumY = 60F;

        public float moveSpeed = 1.0f;

        public bool lockHeight = false;

        float rotationY = 0F;

        Transform pointer;
        GameObject draggedObject;
        PositionConstraint constraint;

        public GameObject linePrefab;
        Node lineNode;
        Connection line;
        LineState lineState;

        private void Start()
        {
            Cursor.visible = false;
            pointer = transform.GetChild(0);
        }

        void Update()
        {
            cameraOperation();
            blockDragging();
            lineDragging();
        }

        void blockDragging()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                foreach (Collider collider in Physics.OverlapSphere(pointer.transform.position, pointer.transform.lossyScale.x))
                {
                    if (collider.gameObject.layer == LayerMask.NameToLayer("Node"))
                    {
                        Debug.Log("startDrag");
                        draggedObject = collider.gameObject;
                        constraint = draggedObject.AddComponent<PositionConstraint>();
                        constraint.constraintActive = true;
                        ConstraintSource source = new ConstraintSource();
                        source.sourceTransform = pointer.transform;
                        source.weight = 1;
                        constraint.AddSource(source);
                        constraint.weight = 1;
                        break;
                    }
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                Destroy(constraint);
                draggedObject = null;
            }
        }

        void lineDragging()
        {
            //START
            if(Input.GetButtonDown("Fire2"))
            {
                foreach (Collider collider in Physics.OverlapSphere(pointer.transform.position, pointer.transform.lossyScale.x))
                {
                    if (collider.gameObject.layer == LayerMask.NameToLayer("Node"))
                    {
                        Debug.Log("startLine");
                        lineNode = collider.gameObject.GetComponent<Node>();
                        if(lineNode.next) lineNode.next.lineRenderer.enabled = false;
                        line = Instantiate(linePrefab, lineNode.transform.parent).GetComponent<Connection>();
                        line.start = lineNode.gameObject;
                        line.end = pointer.gameObject; 
                        lineState = LineState.NEXT;
                        break;
                    }
                }
            }

            //TOGGLE
            if (lineState != LineState.NONE && Input.GetKeyDown(KeyCode.Q))
            {
                switch(lineState)
                {
                    case LineState.NEXT: 
                        lineState = LineState.PREV;
                        line.end = lineNode.gameObject;
                        line.start = pointer.gameObject;
                        if (lineNode.next) lineNode.next.lineRenderer.enabled = true;
                        if (lineNode.prev) lineNode.prev.lineRenderer.enabled = false;
                        break;
                    case LineState.PREV: 
                        lineState = LineState.NEXT;
                        line.start = lineNode.gameObject;
                        line.end = pointer.gameObject;
                        if (lineNode.next) lineNode.next.lineRenderer.enabled = false;
                        if (lineNode.prev) lineNode.prev.lineRenderer.enabled = true;
                        break;
                    case LineState.INTERN:
                        line.lineRenderer.colorGradient.colorKeys[0].color = Color.green;
                        line.lineRenderer.colorGradient.colorKeys[1].color = Color.red;
                        line.lineRenderer.colorGradient.colorKeys[0].color = ColorManager.instance.cInternStart;
                        line.lineRenderer.colorGradient.colorKeys[1].color = ColorManager.instance.cInternEnd;
                        break;
                }
            }

            //ERASE
            if (lineState != LineState.NONE && Input.GetKeyDown(KeyCode.E))
            {
                switch (lineState)
                {
                    case LineState.NEXT: destroyLine(lineNode, true); break;
                    case LineState.PREV: destroyLine(lineNode, false); break;
                }

                cancelLineDrag();

                //UNSET
                lineNode = null;
                line = null;
                lineState = LineState.NONE;
            }

            //END
            if (lineState != LineState.NONE && Input.GetButtonUp("Fire2"))
            {
                Node target = null;
                foreach (Collider collider in Physics.OverlapSphere(pointer.transform.position, pointer.transform.lossyScale.x))
                {
                    if (collider.gameObject.layer == LayerMask.NameToLayer("Node") && collider.gameObject != lineNode.gameObject)
                    {
                        target = collider.gameObject.GetComponent<Node>();
                    }
                }

                if (target) 
                {
                    switch(lineState)
                    {
                        case LineState.NEXT:
                            if (lineNode.prev && target.gameObject == lineNode.prev.start) { cancelLineDrag(); break; }

                            line.end = target.gameObject;
                            destroyLine(lineNode, true);
                            destroyLine(target, false);
                            lineNode.next = line;
                            target.prev = line;
                            break;


                        case LineState.PREV:
                            if (lineNode.next && target.gameObject == lineNode.next.end) { cancelLineDrag(); break; }

                            line.start = target.gameObject;
                            destroyLine(lineNode, false);
                            destroyLine(target, true);
                            lineNode.prev = line;
                            target.next = line;
                            break;
                    }
                }
                else
                {
                    cancelLineDrag();
                }

                //UNSET
                lineNode = null;
                line = null;
                lineState = LineState.NONE;
            }
        }

        void cancelLineDrag()
        {
            if (lineNode.next) lineNode.next.lineRenderer.enabled = true;
            if (lineNode.prev) lineNode.prev.lineRenderer.enabled = true;
            Destroy(line.gameObject);
        }

        void destroyLine(Node lineNode, bool next)
        {
            if(next)
            {
                if (lineNode.next)
                {
                    Connection connection = lineNode.next;
                    lineNode.next = null;
                    connection.end.GetComponent<Node>().prev = null;
                    Destroy(connection.gameObject);
                }
            }
            else
            {
                if (lineNode.prev)
                {
                    Connection connection = lineNode.prev;
                    lineNode.prev = null;
                    connection.start.GetComponent<Node>().next = null;
                    Destroy(connection.gameObject);
                }
            }
        }

        void cameraOperation()
        {
            if (axes == RotationAxes.MouseXAndY)
            {
                float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
            else if (axes == RotationAxes.MouseX)
            {
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
            }
            else
            {
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
            }

            var xAxisValue = Input.GetAxis("Horizontal");
            var zAxisValue = Input.GetAxis("Vertical");
            if (lockHeight)
            {
                var dir = transform.TransformDirection(new Vector3(xAxisValue, 0.0f, zAxisValue) * moveSpeed);
                dir.y = 0.0f;
                transform.position += dir;
            }
            else
            {
                transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue) * moveSpeed);
            }
        }

    }
}