using UnityEngine;
using System.Collections;

// BAD : does not enforce data to be the correct child
// + requires cast
public interface IMySerializable
{
    // Used for GameObjects
    /*string prefabName
    {
        get;
    }*/
    void loadFromData(IData data);
}
