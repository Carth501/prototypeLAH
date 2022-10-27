using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class villansCoterie : coterie
{
    public villansCoterie()
    {
        defaultRelations = -1;
        relationsMap.Add(coteriesEnum.heroes, -1);
    }
}
