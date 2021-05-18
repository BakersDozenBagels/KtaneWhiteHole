using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BlackHole;
using KModkit;
using UnityEngine;
using Rnd = UnityEngine.Random;

/// <summary>
/// On the Subject of White Hole
/// Created by BakersDozenBagels
/// </summary>
public class WhiteHoleModule : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMBombModule Module;
    public KMAudio Audio;
    public KMRuleSeedable RuleSeedable;

    public Texture[] SwirlTextures;
    public Texture[] PlanetTextures;
    public MeshRenderer ImageTemplate;
    public GameObject ContainerTemplate;
    public TextMesh TextTemplate;
    public Transform SwirlContainer;
    public KMSelectable Selectable;

    private Transform[] _swirlsVisible;
    private Transform[] _swirlsActive;
    private Texture[][] _planetTextures;

    private static int _moduleIdCounter = 1;
    private int _moduleId;
    private bool _isSolved = false;
    private bool _isMouseDown = false;

    const float _planetSize = .5f;

    private readonly int[][] _grid = new[] { 3, 4, 1, 0, 2, 3, 1, 2, 0, 4, 1, 3, 0, 2, 4, 1, 2, 3, 4, 0, 3, 2, 4, 2, 1, 3, 0, 0, 1, 4, 4, 0, 0, 1, 3, 4, 2, 2, 1, 3, 1, 2, 1, 3, 0, 0, 4, 3, 4, 2, 4, 0, 2, 3, 4, 1, 3, 0, 2, 1, 2, 1, 3, 1, 3, 0, 4, 4, 0, 2, 2, 4, 4, 0, 0, 2, 1, 1, 3, 3, 0, 1, 3, 4, 2, 2, 0, 4, 3, 1, 0, 3, 2, 4, 1, 4, 3, 1, 2, 0 }
        .Split(10).Select(gr => gr.ToArray()).ToArray();

    private readonly Color[] _colors = new[] {
        new Color(0xe7/255f, 0x09/255f, 0x09/255f),
        new Color(0xed/255f, 0x80/255f, 0x0c/255f),
        new Color(0xde/255f, 0xda/255f, 0x16/255f),
        new Color(0x17/255f, 0xb1/255f, 0x29/255f),
        new Color(0x10/255f, 0xa0/255f, 0xa8/255f),
        new Color(0x28/255f, 0x26/255f, 0xff/255f),
        new Color(0xbb/255f, 0x0d/255f, 0xb0/255f)
    };

    sealed class WhiteHoleBombInfo
    {
        public List<WhiteHoleModule> UnlinkedModules = new List<WhiteHoleModule>();
        public Dictionary<WhiteHoleModule, GameObject> ModulePairs = new Dictionary<WhiteHoleModule, GameObject>();
    }

    private static readonly Dictionary<string, WhiteHoleBombInfo> _infos = new Dictionary<string, WhiteHoleBombInfo>();
    private WhiteHoleBombInfo _info;
    private int _digitsEntered;
    private int _digitsExpected;

    private Coroutine countdown;

    void Start()
    {
        _moduleId = _moduleIdCounter++;

        _planetTextures = new Texture[12][];
        for (int i = 0; i < 12; i++)
            _planetTextures[i] = new Texture[7];
        foreach (var tx in PlanetTextures)
        {
            var p1 = tx.name.IndexOf('-');
            var p2 = tx.name.LastIndexOf('-');
            _planetTextures[int.Parse(tx.name.Substring(p1 + 1, p2 - p1 - 1))][int.Parse(tx.name.Substring(p2 + 1))] = tx;
        }

        var serialNumber = Bomb.GetSerialNumber();
        if (!_infos.ContainsKey(serialNumber))
            _infos[serialNumber] = new WhiteHoleBombInfo();
        _info = _infos[serialNumber];
        _info.UnlinkedModules.Add(this);

        Bomb.OnBombExploded += delegate { _infos.Clear(); };
        Bomb.OnBombSolved += delegate
        {
            // This check is necessary because this delegate gets called even if another bomb in the same room got solved instead of this one
            if (Bomb.GetSolvedModuleNames().Count == Bomb.GetSolvableModuleNames().Count)
                _infos.Remove(serialNumber);
        };

        StartCoroutine(Initialize(null));
    }

    private IEnumerator Initialize(string serialNumber)
    {
        yield return null;

        List<KMBombModule> blackHoles = transform.root.GetComponentsInChildren<KMBombModule>().Where(m => m.ModuleDisplayName == "Black Hole").ToList();

        if (blackHoles.Count != 0)
        {
            
        }

    }
}