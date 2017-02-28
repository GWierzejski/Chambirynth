using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

/// <summary>
/// Map manager is responsible for saving and loading maps.
/// </summary>
public class MapManager : MonoBehaviour {

	string path = "MapName";
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P))
		{
			SaveMap ();
		}
		if(Input.GetKeyDown(KeyCode.L))
		{
			LoadMap ();
		}
	}

	/// <summary>
	/// Saves the map content to a file.
	/// </summary>
	public void SaveMap ()
	{
		if (!Directory.Exists (Application.dataPath + "/Maps/"))
			Directory.CreateDirectory (Application.dataPath + "/Maps/");

		BinaryFormatter bf = new BinaryFormatter ();
		SurrogateSelector surrogateSelector = new SurrogateSelector ();
		Vector3SerializationSurrogate vector3SS = new Vector3SerializationSurrogate ();

		surrogateSelector.AddSurrogate (typeof(Vector3), new StreamingContext (StreamingContextStates.All), vector3SS);
		bf.SurrogateSelector = surrogateSelector;

		FileStream file = File.Create (Application.dataPath + "/Maps/" + path + ".map");
		MapData mapData = new MapData ();
		mapData.FloorPosition = GameObject.Find ("Floor").transform.position;

		GameObject[] temp;

		temp = GameObject.FindGameObjectsWithTag ("Movable");
		Vector3[] blockPositions = new Vector3 [temp.Length];

		for (int i = 0; i < temp.Length; i++)
		{
			blockPositions [i] = temp [i].transform.position;
		}

		temp = GameObject.FindGameObjectsWithTag ("ElectricDoor");
		Vector3[] electricDoorPositions = new Vector3[temp.Length];
		string[] electricDoorNames = new string[temp.Length];
		string[][] attachedPadNames = new string[temp.Length][];

		int maximumPadsAttached = 3;

		for (int i = 0; i < temp.Length; i++) {
			attachedPadNames [i] = new string [maximumPadsAttached];
			electricDoorNames [i] = temp [i].name;
		}

		for (int i = 0; i < temp.Length; i++)
		{
			electricDoorPositions [i] = temp [i].transform.position;

			for (int j = 0; j < temp [i].GetComponent<ElectricDoor> ().attachedPads.Length; j++) {
				if (temp [i].GetComponent<ElectricDoor> ().attachedPads [j] != null)
					attachedPadNames [i] [j] = temp [i].GetComponent<ElectricDoor> ().attachedPads [j].name;
			}
		}

		temp = GameObject.FindGameObjectsWithTag ("Pad");
		Vector3[] padPositions = new Vector3[temp.Length];
		string[] padNames = new string[temp.Length];
		string[] attachedMaterialNames = new string[temp.Length];
		string[] attachedDoorNames = new string[temp.Length];

		for (int i = 0; i < temp.Length; i++)
		{
			attachedDoorNames [i] = temp [i].GetComponent<ElectricDoorPad> ().electricDoor.gameObject.name;
			attachedMaterialNames [i] = temp [i].GetComponent<Renderer> ().material.name.Replace (" (Instance)", "");
			padPositions [i] = temp [i].transform.position;
			padNames [i] = temp [i].name;
		}
			
		mapData.BlockPositions = blockPositions;
		mapData.ElectricDoorPositions = electricDoorPositions;
		mapData.PadPositions = padPositions;
		mapData.PadNames = padNames;
		mapData.ElectricDoorNames = electricDoorNames;
		mapData.AttachedPadNames = attachedPadNames;
		mapData.AttachedDoorNames = attachedDoorNames;
		mapData.AttachedMaterialNames = attachedMaterialNames;

		bf.Serialize (file, mapData);
		file.Close ();
	}

	/// <summary>
	/// Loads the map content from a file.
	/// </summary>
	public void LoadMap ()
	{
		if(File.Exists(Application.dataPath + "/Maps/" + path + ".map"))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			SurrogateSelector surrogateSelector = new SurrogateSelector ();
			Vector3SerializationSurrogate vector3SS = new Vector3SerializationSurrogate ();

			surrogateSelector.AddSurrogate (typeof(Vector3), new StreamingContext (StreamingContextStates.All), vector3SS);
			bf.SurrogateSelector = surrogateSelector;

			FileStream file = File.Open (Application.dataPath + "/Maps/" + path + ".map", FileMode.Open);
			MapData mapData = (MapData)bf.Deserialize (file);
			file.Close ();

			Instantiate (Resources.Load ("Floor"), mapData.FloorPosition, Quaternion.identity);

			foreach (Vector3 blockPosition in mapData.BlockPositions)
			{
				Instantiate (Resources.Load ("Crystal"), blockPosition, Quaternion.identity);				
			}

			GameObject [] padGameobject = new GameObject[mapData.PadNames.Length];

			for (int i = 0; i < mapData.PadNames.Length; i++)
			{
				padGameobject[i] = Instantiate (Resources.Load ("Pad"), mapData.PadPositions[i], Quaternion.identity) as GameObject;
				padGameobject[i].name = mapData.PadNames [i];
				Debug.Log (mapData.AttachedMaterialNames [i]);
				padGameobject [i].GetComponent<Renderer> ().material = Resources.Load ("Materials/" + mapData.AttachedMaterialNames[i]) as Material;//"Materials/" + mapData.AttachedMaterialNames [i], typeof(Material)) as Material;
			}

			GameObject electricDoorGameobject;
			int maximumPadsAttached = 3;

			for(int i = 0; i < mapData.ElectricDoorPositions.Length; i++)
			{
				electricDoorGameobject = Instantiate (Resources.Load ("ElectricDoor"), mapData.ElectricDoorPositions[i], Quaternion.identity) as GameObject;
				electricDoorGameobject.name = mapData.ElectricDoorNames [i];
				ElectricDoor electricDoor = electricDoorGameobject.GetComponent<ElectricDoor> ();

				for (int j = 0; j < maximumPadsAttached; j++)
					electricDoor.attachedPads [j] = GameObject.Find (mapData.AttachedPadNames [i] [j]);
			}

			for (int i = 0; i < mapData.PadNames.Length; i++)
			{
				ElectricDoorPad electricDoorPad = padGameobject [i].GetComponent<ElectricDoorPad> ();
				electricDoorPad.electricDoor = GameObject.Find (mapData.AttachedDoorNames [i]).GetComponent<ElectricDoor> ();
			}
		}
	}

	[Serializable]
	class MapData
	{
		Vector3 floorPosition = Vector3.zero;
		Vector3[] blockPositions;
		Vector3[] electricDoorPositions;
		Vector3[] padPositions;
		string[] electricDoorNames;
		string[] padNames;
		string[] attachedDoorNames;
		string[] attachedMaterialNames;
		string[][] attachedPadNames;

		public string[] AttachedDoorNames {
			get { return attachedDoorNames;	}
			set { attachedDoorNames = value; }
		}

		public string[] AttachedMaterialNames {
			get { return attachedMaterialNames;	}
			set { attachedMaterialNames = value; }
		}

		public string [] PadNames
		{
			get { return padNames; }
			set { padNames = value; }
		}

		public string [] ElectricDoorNames
		{
			get { return electricDoorNames; }
			set { electricDoorNames = value; }
		}

		public string[][] AttachedPadNames
		{
			get { return attachedPadNames; }
			set { attachedPadNames = value; }
		}

		public Vector3 FloorPosition
		{
			get { return floorPosition; }
			set { floorPosition = value; }
		}

		public Vector3[] BlockPositions
		{
			get { return blockPositions; }
			set { blockPositions = value; }
		}

		public Vector3[] ElectricDoorPositions
		{
			get { return electricDoorPositions; }
			set { electricDoorPositions = value; }
		}

		public Vector3[] PadPositions
		{
			get { return padPositions; }
			set { padPositions = value; }
		}
	}
}

