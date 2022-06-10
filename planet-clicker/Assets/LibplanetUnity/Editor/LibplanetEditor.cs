﻿using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Libplanet;
using Libplanet.Crypto;
using Libplanet.Unity;

namespace LibplanetUnity.Editor
{
    public static class LibplanetEditor
    {
        [MenuItem("Tools/Libplanet/Delete blockchain")]
        public static void DeleteChain()
        {
            const string title = "Delete blockchain";
            DirectoryInfo dir = new DirectoryInfo(Paths.StorePath);
            if (dir.Exists)
            {
                if (EditorUtility.DisplayDialog(
                    title,
                    $"Blockchain found at {dir}.\n" +
                    "Do you want to delete it?",
                    "Ok",
                    "Cancel"))
                {
                    dir.Delete(recursive: true);
                    EditorUtility.DisplayDialog(title, "Blockchain deleted.", "Close");
                }
            }
            else
            {
                EditorUtility.DisplayDialog(title, "Blockchain not found.", "Close");
            }
        }

        [MenuItem("Tools/Libplanet/Create swarm config")]
        public static void CreateSwarmConfig()
        {
            const string title = "Create swarm config";
            if (File.Exists(Paths.SwarmConfigPath) &&
                !EditorUtility.DisplayDialog(
                    title,
                    $"Swarm config found at {Paths.SwarmConfigPath}.\n" +
                    "Do you want to overwrite it?",
                    "Ok",
                    "Cancel"))
            {
                return;
            }

            Utils.CreateSwarmConfig(Paths.SwarmConfigPath);
            EditorUtility.DisplayDialog(title, "New swarm config created.", "Close");
        }

        [MenuItem("Tools/Libplanet/Delete swarm config")]
        public static void DeleteSwarmConfig()
        {
            const string title = "Delete swarm config";
            if (File.Exists(Paths.SwarmConfigPath))
            {
                if (EditorUtility.DisplayDialog(
                    title,
                    $"Swarm config found at {Paths.SwarmConfigPath}.\n" +
                    "Do you want to delete it?",
                    "Ok",
                    "Cancel"))
                {

                    File.Delete(Paths.SwarmConfigPath);
                    EditorUtility.DisplayDialog(title, "Swarm config deleted.", "Close");
                }
            }
            else
            {
                EditorUtility.DisplayDialog(title, "Swarm config not found.", "Close");
            }
        }

        [MenuItem("Tools/Libplanet/Create genesis block")]
        public static void CreateGenesisBlock()
        {
            const string title = "Create genesis block";
            if (File.Exists(Paths.GenesisBlockPath) &&
                !EditorUtility.DisplayDialog(
                    title,
                    $"Genesis block found at {Paths.GenesisBlockPath}.\n" +
                    "New genesis block will not be compatible with existing chain.\n" +
                    "Do you want to overwrite it?",
                    "Ok",
                    "Cancel"))
            {
                return;
            }

            Utils.CreateGenesisBlock(Paths.GenesisBlockPath);
            EditorUtility.DisplayDialog(title, "New genesis block created.", "Close");
        }

        [MenuItem("Tools/Libplanet/Delete genesis block")]
        public static void DeleteGenesisBlock()
        {
            const string title = "Delete genesis block";
            if (File.Exists(Paths.GenesisBlockPath))
            {
                if (EditorUtility.DisplayDialog(
                    title,
                    $"Genesis block found at {Paths.GenesisBlockPath}.\n" +
                    "Do you want to delete it?",
                    "Ok",
                    "Cancel"))
                {

                    File.Delete(Paths.GenesisBlockPath);
                    EditorUtility.DisplayDialog(title, "Genesis block deleted.", "Close");
                }
            }
            else
            {
                EditorUtility.DisplayDialog(title, "Genesis block not found.", "Close");
            }
        }

        [MenuItem("Tools/Libplanet/Create private key")]
        public static void CreatePrivateKey()
        {
            const string title = "Create private key";
            if (File.Exists(Paths.PrivateKeyPath) &&
                !EditorUtility.DisplayDialog(
                    title,
                    $"Private key found at {Paths.PrivateKeyPath}.\n" +
                    "Do you want to overwrite it?",
                    "Ok",
                    "Cancel"))
            {
                return;
            }

            Utils.CreatePrivateKey(Paths.PrivateKeyPath);
            EditorUtility.DisplayDialog(title, "New private key created.", "Close");
        }

        [MenuItem("Tools/Libplanet/Delete private key")]
        public static void DeletePrivateKey()
        {
            const string title = "Delete private key";
            if (File.Exists(Paths.PrivateKeyPath))
            {
                if (EditorUtility.DisplayDialog(
                    title,
                    $"Are you sure you want to delete private key found at {Paths.PrivateKeyPath}?",
                    "Ok",
                    "Cancel"))
                {
                    File.Delete(Paths.PrivateKeyPath);
                    EditorUtility.DisplayDialog(title, "Private key deleted.", "Close");
                }
            }
            else
            {
                EditorUtility.DisplayDialog(title, "Private key not found.", "Close");
            }
        }
    }

    public class GenerateBoundPeerStringWindow : EditorWindow
    {
        string privateKeyString = "";
        string host = "";
        int port = 0;
        string boundPeerString = "";

        [MenuItem("Tools/Libplanet/Generate bound peer string")]
        static void Init()
        {
            const string title = "Generate bound peer string";
            var window = EditorWindow.GetWindowWithRect(
                typeof(GenerateBoundPeerStringWindow),
                new Rect(0, 0, 800, 200),
                true,
                title);
            window.Show();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Bound peer information", EditorStyles.boldLabel);
            privateKeyString = EditorGUILayout.TextField("Private key string", privateKeyString);
            host = EditorGUILayout.TextField("Host", host);
            port = EditorGUILayout.IntField("Port", port);

            // Zero port is excluded.
            if (port < 1 || port > 65535)
            {
                boundPeerString = "Invalid port number";
            }
            else if (host.Length < 1)
            {
                boundPeerString = "Invalid host";
            }
            else
            {
                try
                {
                    PrivateKey privateKey = new PrivateKey(ByteUtil.ParseHex(privateKeyString));
                    string publicKeyString = ByteUtil.Hex(privateKey.PublicKey.Format(true));
                    boundPeerString = $"{publicKeyString},{host},{port}";
                }
                catch (Exception)
                {
                    boundPeerString = "Invalid private key string";
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Generated bound peer string", EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(boundPeerString);
        }
    }
}
