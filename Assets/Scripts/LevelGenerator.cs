using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    public PieceData startingPoint;
    public List<PieceData> pieces = new List<PieceData>();
    public PieceData deadEnd;
    public PieceData finish;

    public int tileCount = 100;

    List<GameObject> entrances = new List<GameObject>();
    Vector3 offset;

    void Start()
    {
        entrances.AddRange(startingPoint.entrances);
        StartCoroutine(MakeLevel());
    }

    //TODO make it automatic
    void Update()
    {
        foreach (var entrance in entrances)
        {
            Debug.DrawRay(entrance.transform.position + (-entrance.transform.right - new Vector3(0,0.5f,0)), entrance.transform.right * 2, Color.red);
        }
    }

    IEnumerator MakeLevel()
    {
        while (entrances.Count > 0)
        {
            //Get the entrance the piece will be generated at
            GameObject entrance = entrances[Random.Range(0, entrances.Count)];

            //Pick a random piece
            PieceData piece = pieces[Random.Range(0, pieces.Count)];

            //calculate the offset
            offset = entrance.transform.position - entrance.transform.parent.transform.position;

            //spawn the piece
            PieceData createdPiece = Instantiate(piece, entrance.transform.position + offset, piece.transform.localRotation);

            //get a random entrance and rotate till that entrance allignes with the existing one
            GameObject randomEntrance = createdPiece.entrances[Random.Range(0, createdPiece.entrances.Count)];
            while (Vector3.Distance(randomEntrance.transform.position, entrance.transform.position) > 0.5f)
            {
                createdPiece.transform.Rotate(Vector3.up, 90);
            }


            //add piece entrances to the list
            entrances.AddRange(createdPiece.entrances.Where(x => (Vector3.Distance(x.transform.position, entrance.transform.position) > 1)));

            //remove the entrance from the list
            entrances.Remove(entrance);

            //remove entrances that collide with a wall
            for (int i = 0; i < entrances.Count; i++)
            {
                if (Physics.Raycast(entrances[i].transform.position + (-entrances[i].transform.right - new Vector3(0, 0.5f, 0)), entrances[i].transform.right, 2))
                {
                    Debug.Log("collided and removed");
                    entrances.Remove(entrances[i]);
                }
            }
            yield return null;
        }

    }
}
