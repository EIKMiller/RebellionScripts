using System;
using System.Collections.Generic;
using UnityEngine;

public enum EAttachmentType
{
    MUZZLE = 1,
    SIGHT = 2,
    BARREL = 3
}

public class Attachments : MonoBehaviour
{
    [Header("Attachment Details")]
    [SerializeField] private string _AttachmentName;
    public string AttachmentName { get => _AttachmentName; }
    [SerializeField] private EAttachmentType _AttachmentType;
    public EAttachmentType AttachmentType { get => _AttachmentType; }
    
    
    [Header("Attachment Settings")]
    [SerializeField] private float _AttachmentModifier = 1.0f;
    public float AttachmentModifier { get => _AttachmentModifier; }
}