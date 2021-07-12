using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Track
{
    sonic_walk,
    sonic_jump
}

[CreateAssetMenu(fileName = "SpriteAnimationConfig", menuName = "Configs/SpriteAnimationConfig", order = 1)]
public class SpriteAnimationConfig : ScriptableObject
{
    [Serializable]
    public class SpritesSequence
    {
        public Track Track;
        public List<Sprite> Sprites = new List<Sprite>();
    }
    public List<SpritesSequence> Sequences = new List<SpritesSequence>();
}
