using UnityEditor;

namespace Ryders.Core.Player.DefaultBehaviour.Editor
{
    [CustomEditor(typeof(PlayerBehaviour))]
    public class PlayerBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            
            base.OnInspectorGUI();
        }
    }
}