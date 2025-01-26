using System;
using UnityEngine;
using Oculus.Interaction;

public class Selector : PointableElement
{
    [SerializeField]
    private Transform _targetTransform;

    [SerializeField]
    private Renderer _renderer;

    [SerializeField]
    private Rigidbody _rigidbody;

    protected override void Awake()
    {
        base.Awake();
        if (_targetTransform == null)
        {
            _targetTransform = transform;
        }
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void ProcessPointerEvent(PointerEvent evt)
    {
        switch (evt.Type)
        {
            case PointerEventType.Select:
                OnSelected();
                break;
            case PointerEventType.Unselect:
                OnDeselected();
                break;
        }
    }

    private void OnSelected()
    {
        Debug.Log("Object Selected: " + gameObject.name);
        HideObject();
    }

    private void OnDeselected()
    {
        Debug.Log("Object Deselected: " + gameObject.name);
        ShowObject();
    }

    public void HideObject()
    {
        if (_renderer != null)
        {
            _renderer.enabled = false;
        }
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.detectCollisions = false;
        }
    }

    public void ShowObject()
    {
        if (_renderer != null)
        {
            _renderer.enabled = true;
        }
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.detectCollisions = true;
        }
    }
}