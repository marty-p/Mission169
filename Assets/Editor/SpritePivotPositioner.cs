using System;
using UnityEditor;
using UnityEngine;

internal class SpritePivotPositioner : EditorWindow {
    #region Private Fields

    private static Texture2D _rectTexture;
    private static bool _newPivotSelected;
    private static int _newPivotXPixel;
    private static int _newPivotYPixel;
    private static Sprite _sprite;

    private static float _frameMargin = 10;
    private static int _scale = 8;
    private static Color _borderColor = Color.gray;
    private static float _shade = 0.7f;
    private static Color _frameColor = new Color(_shade, _shade, _shade);

    private static Color _currentPivotColor = Color.green;
    private static Color _mousePosColor = Color.gray;
    private static Color _newPivotColor = Color.blue;
    private static Color _misplacedPivotColor = Color.red;

    private float _outerFrameWidth;
    private float _outerFrameHeight;
    private float _innerFrameWidth;
    private float _innerFrameHeight;
    private float _x;
    private float _y;

    #endregion Private Fields

    #region Public Methods

    [MenuItem("Window/Sprite Pivot Positioner")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(SpritePivotPositioner));
    }

    #endregion Public Methods

    #region Private Methods

    private void Update() {
        Repaint();
    }

    private void OnGUI() {
        if (!SelectSprite()) return;
        DrawSprite();
        DrawMousePos();
        DrawNewPivot();
        DrawCurrentPivot();
        ProcessMouseClick();
        ProcessApplyButton();
    }

    private bool SelectSprite() {
        if (Selection.activeObject != null &&
            Selection.activeObject is UnityEngine.Sprite) {
            if (_sprite != (Sprite)Selection.activeObject) {
                _sprite = (Sprite)Selection.activeObject;
                _newPivotSelected = false;
            }

            return true;
        }
        return false;
    }

    private void DrawSprite() {
        _innerFrameWidth = _sprite.rect.width * _scale;
        _innerFrameHeight = _sprite.rect.height * _scale;

        _outerFrameWidth = _innerFrameWidth + 2 * _frameMargin;
        _outerFrameHeight = _innerFrameHeight + 2 * _frameMargin;

        var rect = EditorGUILayout.GetControlRect(true, _outerFrameHeight);
        _x = rect.min.x;
        _y = rect.min.y;

        //draw the rect that fills the scroll:
        GUIExtensions.DrawRect(new Rect(_x, _y, _outerFrameWidth, _outerFrameHeight), _borderColor, ref _rectTexture);

        //draw the background colour of each frame:
        _x += _frameMargin;
        _y += _frameMargin;
        GUIExtensions.DrawRect(new Rect(_x, _y, _innerFrameWidth, _innerFrameHeight), _frameColor, ref _rectTexture);

        //draw the sprite
        Texture texture = _sprite.texture;
        Rect textureRect = _sprite.textureRect;
        var textureCoords = new Rect(textureRect.x / texture.width, textureRect.y / texture.height,
                                     textureRect.width / texture.width, textureRect.height / texture.height);
        var positionRect = new Rect(_x, _y, _innerFrameWidth, _innerFrameHeight);
        GUI.DrawTextureWithTexCoords(positionRect, texture, textureCoords);
    }

    private void DrawPixel(float x, float y, Color color, bool invertY = true) {
        bool intergerPosition = (x == Math.Floor(x) && y == Math.Floor(y));
        x = _x + x * _scale;
        if (invertY) y = _sprite.rect.height - 1 - y;
        y = _y + y * _scale;
        GUIExtensions.DrawRect(new Rect(x, y, _scale, _scale), intergerPosition ? color : _misplacedPivotColor, ref _rectTexture);
    }

    private void DrawCurrentPivot() {
        float x = _sprite.pivot.x;
        float y = _sprite.pivot.y;
        DrawPixel(x, y, _currentPivotColor);
    }

    private void DrawNewPivot() {
        if (!_newPivotSelected) return;
        DrawPixel(_newPivotXPixel, _newPivotYPixel, _newPivotColor);
    }

    private void DrawMousePos() {
        int x, y;
        if (GetMousePixel(out x, out y)) {
            DrawPixel(x, y, _mousePosColor, false);
        }
    }

    private bool GetMousePixel(out int x, out int y) {
        x = (int)((Event.current.mousePosition.x - _x) / _scale);
        y = (int)((Event.current.mousePosition.y - _y) / _scale);
        return x >= 0 && x < _sprite.rect.width &&
               y >= 0 && y < _sprite.rect.height;
    }

    private void ProcessMouseClick() {
        int x, y;
        if (GetMousePixel(out x, out y) &&
            Event.current.isMouse) {
            _newPivotSelected = true;
            _newPivotXPixel = x;
            _newPivotYPixel = (int)_sprite.rect.height - 1 - y;
        }
    }

    private void ProcessApplyButton() {
        GUI.enabled = _newPivotSelected;
        if (!GUILayout.Button("Apply Changes")) return;
        GUI.enabled = true;

        string path = AssetDatabase.GetAssetPath(_sprite.texture);
        var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        var spritesheet = textureImporter.spritesheet;
        for (int i = 0; i < spritesheet.Length; i++) {
            if (spritesheet[i].name != _sprite.name)
                continue;
            textureImporter.isReadable = true;
            var spriteMetaData = spritesheet[i];

            spriteMetaData.alignment = (int)SpriteAlignment.Custom;
            float xFraction = _newPivotXPixel / (float)_sprite.rect.width;
            float yFraction = _newPivotYPixel / (float)_sprite.rect.height;
            spriteMetaData.pivot = new Vector2(xFraction, yFraction);

            spritesheet[i] = spriteMetaData;
            textureImporter.spritesheet = spritesheet;
            textureImporter.isReadable = false; //apparently this must be before the AssetDatabase.ImportAsset(...) call
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            break;
        }
        _newPivotSelected = false;
    }

    private void OnDestroy() {
        _rectTexture = null;
    }
    #endregion Private Methods
}

public class GUIExtensions {
    static private GUIStyle _rectStyle;
    //I am passing the rectTexture rather than using a local 
    //static variable because it will leak memory otherwise
    public static void DrawRect(Rect position, Color color, ref Texture2D _rectTexture) {
        if (_rectTexture == null) {
            _rectTexture = new Texture2D(1, 1);
            _rectTexture.hideFlags = HideFlags.DontSaveInEditor;
        }
        if (_rectStyle == null) {
            _rectStyle = new GUIStyle();
        }
        _rectTexture.SetPixel(0, 0, color);
        _rectTexture.Apply();
        _rectStyle.normal.background = _rectTexture;
        GUI.Box(position, GUIContent.none, _rectStyle);
    }
}