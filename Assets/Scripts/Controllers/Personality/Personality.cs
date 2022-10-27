using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personality : MonoBehaviour
{
    [SerializeField]
    private float focus; // The amount of time this will chase a creature before it gets bored. ???
    [SerializeField]
    private coteriesEnum initialCoterieSetting;
    private coterie coterie;

    // Start is called before the first frame update
    void Start()
    {
        coterie = coterie.GetCoterie(initialCoterieSetting);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool isHostile(coteriesEnum targetCoterie)
    {
        if (coterie != null)
        {
            return coterie.getCoterieRelationsNumber(targetCoterie) < 0;
        }
        return false;
    }
    public coteriesEnum getCoterieName()
    {
        return coterie.getCoterieName();
    }
}
