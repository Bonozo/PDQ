using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class RotationSystem : MonoBehaviour {

	[Serializable]
	public struct RotationGroup{
		public RotationType rotType;
		public float RotationSpeed;
		public float XAxisRotation;
		public float YAxisRotation;
		public float ZAxisRotation;
		public float OffsetTiming;
		public float Radius;
		public List<GameObject> RotObjects;
	}

	public List<RotationGroup> RotationGroups;
	private Dictionary<GameObject,Quaternion> InicialPositions;
	public enum RotationType{
		Linear, Cirular
	};
	void Start(){
		InicialPositions = new Dictionary<GameObject,Quaternion> ();
		foreach (RotationGroup RotGroup in RotationGroups) {
			foreach (GameObject rot in RotGroup.RotObjects) {
				InicialPositions.Add(rot,rot.transform.rotation);
			}
		}
	}

	void Update () {
		foreach (RotationGroup rotGroup in RotationGroups) {
			int count = 0;
			float offset = rotGroup.OffsetTiming * (-1f);
			foreach (GameObject rotObject in rotGroup.RotObjects) {
				Quaternion inicialAngle;
				InicialPositions.TryGetValue (rotObject, out inicialAngle);
				if (rotGroup.rotType == RotationType.Linear) {
					float t = Mathf.PingPong ((Time.time - (offset * count)) * rotGroup.RotationSpeed, 1.0f);
					float xFrom = inicialAngle.eulerAngles.x - (rotGroup.XAxisRotation / 2);
					float xTo = inicialAngle.eulerAngles.x + (rotGroup.XAxisRotation / 2);
					float yFrom = inicialAngle.eulerAngles.y - (rotGroup.YAxisRotation / 2);
					float yTo = inicialAngle.eulerAngles.y + (rotGroup.YAxisRotation / 2);
					float zFrom = inicialAngle.eulerAngles.z - (rotGroup.ZAxisRotation / 2);
					float zTo = inicialAngle.eulerAngles.z + (rotGroup.ZAxisRotation / 2);
					Vector3 from = new Vector3 (xFrom, yFrom, yFrom);
					Vector3 to = new Vector3 (xTo, yTo, zTo);
					rotObject.transform.eulerAngles = Vector3.Lerp (from, to, t);

				} else {
					float angle = rotGroup.RotationSpeed * (Time.time - (offset * count));
					float x = Mathf.Cos(angle)*rotGroup.Radius + inicialAngle.eulerAngles.x;
					float y = Mathf.Sin(angle)*rotGroup.Radius + inicialAngle.eulerAngles.y;
					rotObject.transform.eulerAngles = new Vector3 (x, y);
				}
				count++;
			}
		}
	}
}

