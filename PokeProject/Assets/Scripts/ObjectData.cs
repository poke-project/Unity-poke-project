using UnityEngine;

// Serialized class to hold gameObject's data
public class ObjectData
{
	public Vector3	pos;
    public Vector3  rot;
	public string	name;
    public string prefabName;
	public string	tag;

	public ObjectData ()
	{
	}

	public ObjectData(GameObject go)
	{
		pos = go.transform.position;
        rot = go.transform.eulerAngles;
        //prefabName = go.GetComponent<IMySerializable>().prefabName;
		name = go.name;
		tag = go.tag;
	}
}