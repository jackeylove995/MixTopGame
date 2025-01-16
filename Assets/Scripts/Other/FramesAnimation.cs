using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MTG
{
    public class FramesAnimation : MonoBehaviour
    {
        [Serializable]
        public class Animation
        {
            public string name;
            public List<Sprite> sprites;

            [HideInInspector]
            public PlayType playType;

            [HideInInspector]
            public int frameIndex;

            public Sprite GetCurrentSprite()
            {
                return sprites[frameIndex];
            }

            public bool IndexPlus()
            {
                frameIndex++;
                if (frameIndex >= sprites.Count)
                {
                    if (playType == PlayType.Loop)
                    {
                        frameIndex = 0;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public enum PlayType
        {
            Once,
            Loop
        }

        public float secondToNextFrame = 0.1f;
        public SpriteRenderer spriteRenderer;
        public List<Animation> animations = new List<Animation>();
        public Animation currentAnimation;

        private WaitForSeconds timeToNextFrame;
        void Awake()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
                Debug.LogError("Sprite Renderer都没有，你想干啥");

            timeToNextFrame = new WaitForSeconds(secondToNextFrame);
        }
        public void ClearAllAnimation()
        {
            StopAllCoroutines();
            currentAnimation = null;
            animations = new List<Animation>();
        }

        public void AddAnimation(string name, Sprite[] sprites)
        {
            Animation animation = new Animation();
            animation.name = name;
            animation.sprites = sprites.ToList();
            animations.Add(animation);
        }

        private Animation GetAnimation(string name)
        {
            foreach (Animation animation in animations)
            {
                if (animation.name == name)
                {
                    return animation;
                }
            }
            return null;
        }

        private bool SetCurrentAnimation(string name)
        {
            currentAnimation = GetAnimation(name);
            if (currentAnimation == null)
            {
                Debug.LogError("The anim name you called is not set. name: " + name);
                return false;
            }
            return true;
        }

        public void PlayLoop(string name)
        {
            if (currentAnimation != null && currentAnimation.name == name)
                return;
            if (!SetCurrentAnimation(name))
                return;
            currentAnimation.playType = PlayType.Loop;
            currentAnimation.frameIndex = 0;
            UpdateInternal();
        }

        public void PlayOnce(string name)
        {
            if (!SetCurrentAnimation(name)) return;
            currentAnimation.playType = PlayType.Once;
            currentAnimation.frameIndex = 0;
            UpdateInternal();
        }


        void UpdateInternal()
        {
            StopAllCoroutines();
            StartCoroutine(AnimUpdate());
        }

        IEnumerator AnimUpdate()
        {
            if (currentAnimation == null)
                yield break;

            while (true)
            {
                SetSprite(currentAnimation.GetCurrentSprite());
                if (!currentAnimation.IndexPlus())
                    PlayIdle();
                yield return timeToNextFrame;
            }
        }

        void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        void PlayIdle()
        {
            PlayLoop("idle");
        }
    }
}

