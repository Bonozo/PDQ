using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

//[CustomEditor(typeof(MainLogic)), CanEditMultipleObjects]
public class MenuSystem : Editor {
	
	//public SerializedProperty VideosPerSurfaceProperty, CamerasProperty, MediaPlayers;

	void OnEnable(){
		//VideosPerSurfaceProperty = serializedObject.FindProperty ("VideosPerSurface");
		//CamerasProperty = serializedObject.FindProperty ("Cameras");
		//MediaPlayers = serializedObject.FindProperty("MediaPlayers");

	}

//	public override void OnInspectorGUI() {
//		serializedObject.Update ();
//
//		EditorGUILayout.PropertyField (VideosPerSurfaceProperty);
//
//		int videosPerSurface = (int)VideosPerSurfaceProperty.intValue;
//
//		if (videosPerSurface > 0) {
//			bool visible = true;
//			ListIterator("MediaPlayers");
//		}
//
//		serializedObject.ApplyModifiedProperties ();
//	}



//	public void ListIterator(string propertyPath, ref bool visible){
//		SerializedProperty listProperty = serializedObject.FindProperty (propertyPath);
//		visible = EditorGUILayout.Foldout (visible, listProperty.name);
//		if (visible) {
//			EditorGUI.indentLevel++;
//			for (int i = 0; i < listProperty.arraySize; i++) {
//				SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex (i);
//				Rect drawZone = GUILayoutUtility.GetRect (0f, 16f);
//				bool showChildren = EditorGUI.PropertyField (drawZone, elementProperty);
//			}
//			EditorGUI.indentLevel--;
//		}
//
//	}



	//Anda
//	public void ListIterator(string listName){
//
//		SerializedProperty listIterator = serializedObject.FindProperty (listName);
//		Rect drawZone = GUILayoutUtility.GetRect (0f, 16f);
//		bool showChildren = EditorGUI.PropertyField (drawZone, listIterator);
//		listIterator.NextVisible (showChildren);
//
//		drawZone = GUILayoutUtility.GetRect (0f, 16f);
//		showChildren = EditorGUI.PropertyField (drawZone, listIterator);
//		bool toBeContinued = listIterator.NextVisible (showChildren);
//
//		int listElement = 0; 
//		while(toBeContinued) {
//			drawZone = GUILayoutUtility.GetRect (0f, 16f);
//			showChildren = EditorGUI.PropertyField (drawZone, listIterator);
//			toBeContinued = listIterator.NextVisible (showChildren);
//			listElement++;
//		}
//	}
//
}
