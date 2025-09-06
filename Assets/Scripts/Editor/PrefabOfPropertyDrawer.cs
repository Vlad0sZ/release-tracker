using Runtime.Models;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(PrefabOf<>), true)]
    public class PrefabOfPropertyDrawer : PropertyDrawer
    {
        private const float WarningHeight = 40f;
        private const float Spacing = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var gameObjectProperty = property.FindPropertyRelative("gameObject");

            Rect objectFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Rect warningRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + Spacing,
                position.width, WarningHeight);

            EditorGUI.PropertyField(objectFieldRect, gameObjectProperty, label);
            CheckAndDrawWarning(warningRect, gameObjectProperty.objectReferenceValue);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty gameObjectProp = property.FindPropertyRelative("gameObject");
            float height = EditorGUIUtility.singleLineHeight;

            // Добавляем высоту для предупреждения если есть объект и у него нет компонента
            if (gameObjectProp.objectReferenceValue != null)
            {
                var gameObject = gameObjectProp.objectReferenceValue as GameObject;
                var componentType = GetGenericComponentType();

                if (gameObject != null && !HasRequiredComponent(gameObject, componentType))
                    height += WarningHeight + Spacing;
            }

            return height;
        }

        private void CheckAndDrawWarning(Rect position, Object objectReferenceValue)
        {
            var gameObject = objectReferenceValue as GameObject;
            if (objectReferenceValue == null || gameObject == null)
                return;


            var componentType = GetGenericComponentType();
            if (!HasRequiredComponent(gameObject, componentType))
            {
                EditorGUI.HelpBox(position,
                    $"Prefab '{gameObject.name}' doesn't have component {componentType.Name}!",
                    MessageType.Warning);
            }
        }

        private System.Type GetGenericComponentType()
        {
            var fieldType = fieldInfo.FieldType;
            var componentType = fieldType.IsGenericType ? fieldType.GetGenericArguments()[0] : typeof(MonoBehaviour);

            return componentType;
        }

        private bool HasRequiredComponent(GameObject gameObject, System.Type componentType)
        {
            if (gameObject == null)
                return false;


            return gameObject.GetComponent(componentType) != null;
        }
    }
}