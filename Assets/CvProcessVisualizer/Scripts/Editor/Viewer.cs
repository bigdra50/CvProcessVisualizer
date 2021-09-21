using UnityEditor;
using UnityEngine;

namespace CvProcessViewer.Editor
{
    public class Viewer : EditorWindow
    {
        [MenuItem("Tools/CvProcessViewer/Open Viewer")]
        private static void ShowWindow()
        {
            var window = GetWindow<Viewer>();
            window.titleContent = new GUIContent("CvProcessViewer");
            window.Show();
        }

        [MenuItem("Tools/CvProcessViewer/Create Visualizer")]
        private static void CreateVisualizer()
        {
            var visualizer = new GameObject("CvProcessVisualizer");
            visualizer.AddComponent<CvProcess>();
        }

        // TODO: シーン再生前に値をセットしたい
        //public CvProcessVisualizer CvProcessVisualizer.I;
        private int _id = -1;
        public float _size = 150f;
        Vector2 _scrollPosition = Vector2.zero;

        private void OnGUI()
        {
            //var serializedObject = new SerializedObject(this);
            //serializedObject.Update();
            //CvProcessVisualizer.I = LoadAsset(_id);
            //CvProcessVisualizer.I =
            //    EditorGUILayout.ObjectField(CvProcessVisualizer.I, typeof(CvProcessVisualizer), true) as CvProcessVisualizer;
            _id = SaveAsset(CvProcess.I);
            GUILayout.Space(10f);

            #region Slider

            GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Width(position.width - 10f));
            var sliderPos = GUILayoutUtility.GetLastRect();
            sliderPos = new Rect(sliderPos.x + 10f, sliderPos.y, sliderPos.width - 20f, sliderPos.height);
            _size = GUI.HorizontalSlider(sliderPos, _size, 50f, position.width - 10f);
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            #endregion

            #region Textures

            if (CvProcess.I != null)
            {
                for (var i = 0; i < CvProcess.I._textures.Count; i++)
                {
                    var tex = CvProcess.I._textures[i];
                    var size = ValidateSize(tex.width, tex.height, (int) _size);
                    GUILayout.Box(GUIContent.none, GUILayout.Width(size.width), GUILayout.Height(size.height));
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), tex, ScaleMode.ScaleToFit, true, tex.width / (float) tex.height);
                }
            }

            #endregion

            EditorGUILayout.EndScrollView();
            //so.ApplyModifiedProperties();
        }

        private CvProcess LoadAsset(int id)
        {
            if (id == -1)
            {
                id = EditorPrefs.GetInt("visualizerId", -1);
            }

            if (id != -1)
            {
                return EditorUtility.InstanceIDToObject(this._id) as CvProcess;
            }

            return CvProcess.I;
        }
        
        

        private int SaveAsset(CvProcess visualizer)
        {
            if (visualizer == null) return -1;
            var id = visualizer.GetInstanceID();
            EditorPrefs.SetInt("visualizerId", id);
            return id;
        }

        /// <summary>
        /// アスペクト比を維持しながら幅､高さを最大値に合わせる
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static (int width, int height) ValidateSize(int width, int height, int maxWidth)
        {
            if (width <= maxWidth && height <= maxWidth) return (width, height);
            if (width == height) return (width, width);

            return (maxWidth, (int) ((float) maxWidth * (float) height / width));
        }


        private void Update()
        {
            Repaint();
        }
    }
}