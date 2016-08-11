using UnityEngine;
using System.Collections;

public partial class Move : IMySerializable
{
    private void setAttributesFromName(string name)
    {
        setDefaultValue();
        switch (name)
        {
            case "Tackle":
                EnemyEffect = new Statistics(140, 0, 0, 0, 0, 0, 0, 0);
                SelfEffect = new Statistics(0, 0, 0, 0, 0, 0, 0, 0);
                MaxPP = 35;
                SelfStatus = eStatus.NORMAL;
                EnemyStatus = eStatus.NORMAL;
                Type = new Normal();
                CriticalChanceModifier = 1;
                Accuracy = 100;
                break;

            case "blabla":
                EnemyEffect = new Statistics(0, 1, 0, 0, 0, 0, 0, 0);
                SelfEffect = new Statistics(0, 0, 0, 0, 0, 0, 0, 0);
                MaxPP = 20;
                SelfStatus = eStatus.NORMAL;
                EnemyStatus = eStatus.NORMAL;
                Type = new Normal();
                CriticalChanceModifier = 1;
                Accuracy = 100;
                break;

            default:
                Debug.Log("Should not be here");
                break;
        }
        checkValue();
    }

    private void setDefaultValue()
    {
        EnemyEffect = null;
        SelfEffect = null;
        MaxPP = -1;
        SelfStatus = eStatus.NULL;
        EnemyStatus = eStatus.NULL;
        Type = null;
        CriticalChanceModifier = -1;
        Accuracy = -1;

    }

    private void checkValue()
    {
        Debug.Assert((EnemyEffect != null)
            && (SelfEffect != null)
            && (MaxPP != -1)
            && (SelfStatus != eStatus.NULL)
            && (EnemyStatus != eStatus.NULL)
            && (Type != null)
            && (CriticalChanceModifier != -1)
            && (Accuracy != -1));
    }
}
