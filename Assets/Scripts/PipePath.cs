using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePath : MonoBehaviour
{
    private Pipe[] _pipes;

    private void Awake()
    {
        _pipes = GetComponentsInChildren<Pipe>();
    }

    public Vector3 SpawnPosition => _pipes[0].StartPosition;

    public Pipe GetPipe(int id)
    {
        if (id < _pipes.Length)
            return _pipes[id];

        return _pipes[_pipes.Length - 1];
    }

    public bool EndOfPipes(int pipe)
    {
        return pipe >= _pipes.Length;
    }
}
