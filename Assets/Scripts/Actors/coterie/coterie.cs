using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class coterie
{
    protected coteriesEnum coterieName;
    protected float defaultRelations;
    protected Dictionary<coteriesEnum, float> relationsMap = new Dictionary<coteriesEnum, float>();

    public static coterie GetCoterie(coteriesEnum coterie)
    {
        switch (coterie)
        {
            case coteriesEnum.heroes:
                return new heroesCoterie();
            case coteriesEnum.villans:
                return new villansCoterie();
            default:
                return null;
        }
    }

    public float getCoterieRelationsNumber(coteriesEnum coterie)
    {
        if (relationsMap.ContainsKey(coterie))
        {
            float relations = relationsMap[coterie];
            return relations;
        }
        else if (coterie == coterieName) {
            return 1;
        }
        else
        {
            return defaultRelations;
        }
    }

    public coteriesEnum getCoterieName()
    {
        return coterieName;
    }
}
