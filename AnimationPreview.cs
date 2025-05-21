using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

[CustomPreview(typeof(AnimatorState))]
public class AnimationPreview : ObjectPreview
{
    #region Variables
    
    private Editor _editor;
    private int _animClipID;
    
    #endregion
    
    #region Methods

    // Initialize Method
    public override void Initialize(Object[] targets)
    {
        base.Initialize(targets);
        if (targets.Length > 1 || Application.isPlaying) return;

        AnimationClip clip = GetAnimationClip(target as AnimatorState);
        if (clip)
        {
            _editor = Editor.CreateEditor(clip);
            _animClipID = clip.GetInstanceID();
        }
    }
    
    // Preview GUI Method - Preview in Inspector
    public override bool HasPreviewGUI()
    {
        return _editor?.HasPreviewGUI() ?? false;
    }
    
    // Interact with Preview GUI Method - Previews Active Clip
    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
        base.OnInteractivePreviewGUI(r, background);
        
        AnimationClip activeClip = GetAnimationClip(target as AnimatorState);
        if (activeClip && activeClip.GetInstanceID() != _animClipID)
        {
            CleanupPreviewEditor();
            _editor = Editor.CreateEditor(activeClip);
            _animClipID = activeClip.GetInstanceID();
            return;
        }
        
        if (_editor) _editor.OnInteractivePreviewGUI(r, background);
    }

    // Get Animation Method - Plays the Selected Animation
    AnimationClip GetAnimationClip(AnimatorState state)
    {
        return state?.motion as AnimationClip;
    }
    
    // Cleanup Method
    public override void Cleanup()
    {
        base.Cleanup();
        CleanupPreviewEditor();
    }

    private void CleanupPreviewEditor()
    {
        if (_editor)
        {
            Object.DestroyImmediate(_editor);
            _editor = null;
            _animClipID = 0;
        }
    }
    #endregion
}
