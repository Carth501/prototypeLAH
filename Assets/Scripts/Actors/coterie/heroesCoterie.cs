using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heroesCoterie : coterie
{
    public heroesCoterie()
    {
        coterieName = coteriesEnum.heroes;
        defaultRelations = 1;
        relationsMap.Add(coteriesEnum.villans, -1);
    }
}
