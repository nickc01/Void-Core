﻿using UnityEngine;
using WeaverCore.Assets;
using WeaverCore.Enums;
using WeaverCore.Utilities;

namespace WeaverCore.Components.DeathEffects
{
    /// <summary>
    /// The death effects for uninfected enemies
    /// </summary>
    public class UninfectedDeathEffects : BasicDeathEffects
    {
        [Header("Uninfected Config")]
        [SerializeField]
        protected GameObject uninfectedDeathPt;

        protected GameObject slashEffectGhost1;

        protected GameObject slashEffectGhost2;

        [SerializeField]
        protected GameObject whiteWave;

        [SerializeField]
        protected bool doCorpseParticles = true;

        [SerializeField]
        protected ParticleSystem corpseParticlesPrefab;

        [SerializeField]
        protected float corpseParticlesIntensity = 300;

        FlingInfo ghost1Fling;
        FlingInfo ghost2Fling;

        protected override void Awake()
        {
            base.Awake();
            slashEffectGhost1 = EffectAssets.SlashGhost1Prefab;
            slashEffectGhost2 = EffectAssets.SlashGhost2Prefab;

            ghost1Fling = new FlingInfo
            {
                Prefab = slashEffectGhost1,
                PrefabAmountMin = 8,
                PrefabAmountMax = 8,
                VelocityMin = 2f,
                VelocityMax = 35f,
                AngleMin = 0f,
                AngleMax = 360f
            };

            ghost2Fling = new FlingInfo
            {
                Prefab = slashEffectGhost2,
                PrefabAmountMin = 2,
                PrefabAmountMax = 3,
                VelocityMin = 2f,
                VelocityMax = 35f,
                AngleMin = 0f,
                AngleMax = 360f
            };
        }

        public override void EmitEffects()
        {
            base.EmitEffects();
            foreach (var flasher in GetComponentsInChildren<SpriteFlasher>())
            {
                flasher.flashFocusHeal();
            }
            Pooling.Instantiate(uninfectedDeathPt, transform.TransformPoint(EffectsOffset), Quaternion.identity);

            ShakeCameraIfVisible(ShakeType.EnemyKillShake);

            Flings.SpawnFlings(ghost1Fling, transform.TransformPoint(EffectsOffset));
            Flings.SpawnFlings(ghost2Fling, transform.TransformPoint(EffectsOffset));

            Pooling.Instantiate(whiteWave, transform.TransformPoint(EffectsOffset), Quaternion.identity);

            if (doCorpseParticles && corpseParticlesPrefab != null)
            {
                var particles = Pooling.Instantiate(corpseParticlesPrefab, transform);
                particles.transform.SetLocalPosition(EffectsOffset.x,EffectsOffset.y);
                var emit = particles.emission;
                emit.rateOverTimeMultiplier = corpseParticlesIntensity;
            }
        }
    }
}
