using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using BaseX;
using HarmonyLib;
using NeosModLoader;
using UnityEngine;

namespace AudioCustomizer
{
    public class AudioCustomizer : NeosMod
    {
        public override string Name => "AudioCustomizer";
        public override string Author => "Fro Zen";
        public override string Version => "1.0.1";

        private static bool _first_trigger = false;
        private static ModConfiguration _config;

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<int> VoicesKey =
            new ModConfigurationKey<int>("Voice Count", "Audio Voices (Requires Restart)", () => 64);

        public override void OnEngineInit()
        {
            var runner = GameObject.FindObjectsOfType<FrooxEngineRunner>().First();
            var actions = typeof(FrooxEngineRunner)
                .GetField("actions", BindingFlags.Instance | BindingFlags.NonPublic);
            var action = actions.GetValue(runner) as SpinQueue<Action>;
            action.Enqueue(() =>
            {
                var a = AudioSettings.GetConfiguration();
                _config = GetConfiguration();
                a.numRealVoices = _config.GetValue(VoicesKey);
                AudioSettings.Reset(a);
                Msg("Changed audio voice count");
            });
            
            //this completely screws up the audio
            /*
            GetConfiguration().OnThisConfigurationChanged += config => 
            {
                var audio = AudioSettings.GetConfiguration();
                audio.numRealVoices = _config.GetValue(VoicesKey);
                AudioSettings.Reset(audio);
                Msg($"Audio voice count set to {audio.numRealVoices}");
            };
            */
        }
    }
}