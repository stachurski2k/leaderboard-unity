using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public abstract class IParser : MonoBehaviour
{
    public abstract Dictionary<string,string> Parse(string json);
}
