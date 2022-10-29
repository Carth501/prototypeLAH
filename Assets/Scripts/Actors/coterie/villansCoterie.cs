using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class villansCoterie : coterie
{
    public villansCoterie()
    {
        coterieName = coteriesEnum.villans;
        defaultRelations = -1;
        relationsMap.Add(coteriesEnum.heroes, -1);
    }
}
