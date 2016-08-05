using UnityEngine;
using System.Collections;

// BAD : does not enforce source to be the correct child
// + requires cast
public interface IData
{
    // public IData(IMySerializable source)
    // {
    //      populate(source);
    // }
    void populate(IMySerializable source);
}
