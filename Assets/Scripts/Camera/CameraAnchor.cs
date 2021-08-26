using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class CameraAnchor : MonoBehaviour
{
    public enum AnchorType
    {
        Undefined,
        BottomLeft,
        BottomCenter,
        BottomRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        TopLeft,
        TopCenter,
        TopRight,
    };

    private Vector3 standardPosition;
    public AnchorType anchorType;
    public Vector3 anchorOffset;

    // Use this for initialization
    void Start()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying && anchorType == AnchorType.Undefined)
        {
            standardPosition = transform.position;
        }
        else
        {
            Vector3 newPos = GetAnchorVector() + anchorOffset;
            standardPosition = transform.position - newPos;
        }

#endif
#if !UNITY_EDITOR
        Vector3 newPos = GetAnchorVector() + anchorOffset;
        standardPosition = transform.position - newPos;
#endif


        UpdateAnchor();
    }

    Vector3 GetAnchorVector()
    {
        Vector3 anchor = Vector3.zero;
        switch (anchorType)
        {
            case AnchorType.BottomLeft:
                anchor = CameraFit.Instance.BottomLeft;
                break;
            case AnchorType.BottomCenter:
                anchor = CameraFit.Instance.BottomCenter;
                break;
            case AnchorType.BottomRight:
                anchor = CameraFit.Instance.BottomRight;
                break;
            case AnchorType.MiddleLeft:
                anchor = CameraFit.Instance.MiddleLeft;
                break;
            case AnchorType.MiddleCenter:
                anchor = CameraFit.Instance.MiddleCenter;
                break;
            case AnchorType.MiddleRight:
                anchor = CameraFit.Instance.MiddleRight;
                break;
            case AnchorType.TopLeft:
                anchor = CameraFit.Instance.TopLeft;
                break;
            case AnchorType.TopCenter:
                anchor = CameraFit.Instance.TopCenter;
                break;
            case AnchorType.TopRight:
                anchor = CameraFit.Instance.TopRight;
                break;
            case AnchorType.Undefined:
                anchor = Vector3.zero;
                break;
        }
        return anchor;
    }

    void UpdateAnchor()
    {
        Vector3 anchorVector = GetAnchorVector();
        SetAnchor(anchorVector);
    }

    void SetAnchor(Vector3 anchor)
    {
        Vector3 newPos = anchor + anchorOffset;
        if (!transform.position.Equals(standardPosition + newPos))
        {
            transform.position = standardPosition + newPos;
        }
    }


    // Update is called once per frame
#if UNITY_EDITOR
    void Update()
    {
        if (!EditorApplication.isPlaying)
            UpdateAnchor();


    }
#endif
}