using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpriteController : IDisposable
{
    private class Animation
    {
        public Track track;
        public List<Sprite> sprites;
        public bool loop = false;
        public float speed = 10;
        public float counter = 0;
        public bool sleeps;

        public void Update()
        {
            if (sleeps) return;
            counter += Time.deltaTime * speed;
            if (loop)
            {
                while (counter > sprites.Count)
                {
                    counter -= sprites.Count;
                }
            }
            else if (counter > sprites.Count)
            {
                counter = sprites.Count - 1;
                sleeps = true;
            }
        }
    }

    private SpriteAnimationConfig _config;
    private Dictionary<SpriteRenderer, Animation> _activeAnimations = new Dictionary<SpriteRenderer, Animation>();

    public SpriteController(SpriteAnimationConfig config)
    {
        _config = config;
    }

    public void StartAnimation(SpriteRenderer spriteRenderer, Track track, bool loop, float speed)
    {
        if (_activeAnimations.TryGetValue(spriteRenderer, out var animation))
        {
            animation.loop = loop;
            animation.speed = speed;
            animation.sleeps = false;
            if (animation.track != track)
            {
                animation.track = track;
                animation.sprites = _config.Sequences.Find(sequence => sequence.Track == track).Sprites;
                animation.counter = 0;
            }
        }
        else
        {
            _activeAnimations.Add(spriteRenderer, new Animation()
            {
                track = track,
                sprites = _config.Sequences.Find(sequence => sequence.Track == track).Sprites,
                loop = loop,
                speed = speed
            });
        }
    }

    public void StopAnimation(SpriteRenderer sprite)
    {
        if (_activeAnimations.ContainsKey(sprite))
        {
            _activeAnimations.Remove(sprite);
        }
    }

    public void Update()
    {
        foreach (var animation in _activeAnimations)
        {
            animation.Value.Update();
            animation.Key.sprite = animation.Value.sprites[(int)animation.Value.counter];
        }
    }

    public void Dispose()
    {
        _activeAnimations.Clear();
    }
}
