using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heroesCoterie : coterie
{
    public heroesCoterie()
    {
        defaultRelations = 1;
        relationsMap.Add(coteriesEnum.villans, -1);
    }
}
