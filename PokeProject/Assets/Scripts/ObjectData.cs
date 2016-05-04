using UnityEngine;

public class GameObjectData
{
	public struct Data
	{
		public float	x;
		public float	y;
		public float	z;
		public float	rotX;
		public float	rotY;
		public float	rotZ;
		public string	name;
        public string   tag;
	}

	public Data		data;

	public GameObjectData ()
	{
	}

	public GameObjectData(GameObject go)
	{
		data.x = go.transform.position.x;
		data.y = go.transform.position.y;
		data.z = go.transform.position.z;
		data.rotX = go.transform.rotation.x;
		data.rotY = go.transform.rotation.y;
		data.rotZ = go.transform.rotation.z;
		data.name = go.name;
        data.tag = go.tag;
	}

}