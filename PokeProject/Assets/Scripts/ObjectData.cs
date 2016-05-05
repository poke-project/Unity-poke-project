using UnityEngine;

// Serialized class to hold gameObject's data
public class ObjectData
{
	public Vector3	pos;
	public float	rotX;
	public float	rotY;
	public float	rotZ;
	public string	name;
	public string	tag;

	public ObjectData ()
	{
	}

	public ObjectData(GameObject go)
	{
		pos = go.transform.position;
		rotX = go.transform.rotation.x;
		rotY = go.transform.rotation.y;
		rotZ = go.transform.rotation.z;
		name = go.name;
		tag = go.tag;
	}
}