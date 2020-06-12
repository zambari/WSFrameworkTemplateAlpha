using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Z.ObjectID
{
    public static class ObjectIDBenchmarks
    {

        static int benchount = 100000;
        //var bytes = BitConverter.GetBytes(identifier);
        static byte[] allocb1 = new byte[8];

        // [UnityEditor.MenuItem("Tools/ObjectID/Benchmark uniquen")]
        // static void benchuniqe()
        // {
        //     var rando = (Int32) UnityEngine.Random.Range(0, System.Int32.MaxValue);
        //     ulong id = ObjectIDExtensions.CreateNewTimeAndInstanceBasedIdentifier(rando);
        //     for (int i = 0; i < 30; i++)
        //     {
        //         Debug.Log(id.MakeValueUnique().ToColorfulString());
        //     }

        // }

        static ulong gettestid { get { return "hello world".GetHashFromString(); } }

        [UnityEditor.MenuItem("Tools/ObjectID/Benchmark compacts")]
        static void benchCompact()
        {
            var rando = (Int32) UnityEngine.Random.Range(0, System.Int32.MaxValue);
            for (int i = 0; i < 30; i++)
            {
                rando = (Int32) UnityEngine.Random.Range(0, System.Int32.MaxValue);
                ulong id = ObjectIDExtensions.CreateNewTimeAndInstanceBasedIdentifier(rando);
                Debug.Log(id.ToColorfulString());
                // id = ObjectIDExtensions.CreateNewTimeAndInstanceBasedIdentifier(rando);

                Debug.Log(((ulong) (id.Compact())).ToColorfulString());

            }
        }

        [UnityEditor.MenuItem("Tools/ObjectID/Benchmark GensomeIds")]
        static void GensomeIds()
        {
       //   ObjectID.ResetIncremental();
       //     ObjectID.ResetIncremental();
            GameObject[] g = UnityEditor.Selection.gameObjects;
            int read = 0;
            if (g == null)
            {
                Debug.Log("selth");
                return;
            }
            Int32 rando = (Int32) UnityEngine.Random.Range(0, System.Int32.MaxValue);;
            for (int i = 0; i < 500; i += 1)
            {
                //CreateNewTimeAndInstanceBasedIdentifier
                if (i % 130 == 0)
                {
                    //   =(ulong)(long) (g.GetInstanceID()); // & 0b111111111111111;
                    //incrementedOnIDGeneration += 2;
                    rando = (Int32) UnityEngine.Random.Range(0, System.Int32.MaxValue);
                }
                ulong id = ObjectIDExtensions.CreateNewTimeAndInstanceBasedIdentifier(rando);
                read++;
                if (read >= g.Length) read = 0;
                Debug.Log(i + "  " + id.ToColorfulString());
            }

        }

        [UnityEditor.MenuItem("Tools/ObjectID/Benchmark shifts")]
        static void shifts()
        {
            //            ulong xx =(ulong)((float)( System.UInt64.MaxValue)* UnityEngine.Random.value);
            ulong xx = 3; // (ulong)System.Int32.MaxValue;
            for (int i = 0; i < 4; i++)
            {
                xx = (ulong) (xx | (ulong) (long) (3 << i * 6));
            }
            for (int i = 0; i < 35; i++)
            {
                Debug.Log(xx.ToColorfulString());
                xx = xx << 1;
            }
            Debug.Log("-");
            xx = 451;
            for (int i = 0; i < 35; i++)
            {
                Debug.Log(xx.ToColorfulString());
                xx = xx << 1;
            }
        }

        //   [UnityEditor.MenuItem("Tools/ObjectID/Benchmark getting bytes")]
        static void BenchmarGettingbytes()
        {
            ulong id = gettestid;
            int cnt = 0;
            cnt = 0;

            // zBench.Start("stringbuilder");
            // for (int i = 0; i < benchount; i++)
            // 	cnt += naive(id++).Length;
            // zBench.EndMillis("naive", " cnt=" + cnt + " id " + id.ToFingerprintString());
            // cnt = 0;

            zBench.Start("aloc1");
            for (int i = 0; i < benchount; i++)
                cnt += aloc1(id++).Length;
            zBench.EndMillis("aloc1", " do nothing");
            cnt = 0;

            zBench.Start("aloc2");

            for (int i = 0; i < benchount; i++)
                cnt += aloc2(id++).Length;
            zBench.EndMillis("aloc2", " fill exisng ");
            cnt = 0;

            zBench.Start("aloc3");

            for (int i = 0; i < benchount; i++)
                cnt += aloc3(id++).Length;
            zBench.EndMillis("aloc3", " cnt=" + cnt);

        }
        public static string aloc1(this ulong identifier)
        {
            allocb1 = BitConverter.GetBytes(identifier);
            return allocb1[0].ToString();
        }
        public static string aloc2(this ulong identifier)
        {
            var bytes = BitConverter.GetBytes(identifier);

            return bytes[0].ToString();
        }
        public static string aloc3(this ulong identifier)
        {
            var bytes = BitConverter.GetBytes(identifier);
            return bytes[0].ToString();
        }

        [UnityEditor.MenuItem("Tools/ObjectID/Benchmark string building")]
        static void Benchmarstringbuildingnative()
        {
            ulong id = gettestid;
            int cnt = 0;

            cnt = 0;

            // zBench.Start("stringbuilder");
            // for (int i = 0; i < benchount; i++)
            // 	cnt += naive(id++).Length;
            // zBench.EndMillis("naive", " cnt=" + cnt + " id " + id.ToFingerprintString());
            // cnt = 0;

            zBench.Start("mixed");
            for (int i = 0; i < benchount; i++)
                cnt += mixed(id++).Length;
            zBench.EndMillis("mixed", " cnt=" + cnt);
            cnt = 0;

            zBench.Start("naivevarient");

            for (int i = 0; i < benchount; i++)
                cnt += bToStringAsHex(id++).Length;
            zBench.EndMillis("naivevarient", " cnt=" + cnt);

            zBench.Start("stringbuilder");
            for (int i = 0; i < benchount; i++)
                cnt += stribgu(id++).Length;
            zBench.EndMillis("stringbuilder", " cnt=" + cnt + " id " + id.ToFingerprintString());
        }

        public static string bToStringAsHex(this ulong identifier, bool reverse = true)
        {
            var bytes = BitConverter.GetBytes(identifier);
            string s = "";
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                s += "[";
                s += bytes[i].ToString("X2");
                s += "]";
            }
            return s;
        }
        static string stribgu(ulong val)
        {
            var bytes = BitConverter.GetBytes(val);
            var sb = new System.Text.StringBuilder();
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                sb.Append("[");
                sb.Append(bytes[i].ToString("X2"));
                sb.Append("]");
            }
            return sb.ToString();

        }

        static string mixed(ulong val)
        {
            var bytes = BitConverter.GetBytes(val);
            var sb = new System.Text.StringBuilder();
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                sb.Append("[" + bytes[i].ToString("X2") + "]");
            }
            return sb.ToString();
        }
        static string naivevarient(ulong val)
        {
            string s = "";
            var bytes = BitConverter.GetBytes(val);
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                var sb = new System.Text.StringBuilder();
                sb.Append("[");
                sb.Append(bytes[i].ToString("X2"));
                sb.Append("]");
                s += sb.ToString();
            }

            return s;
        }
    }
    //  string s = "";
    //         for (int i = bytes.Length - 1; i >= 0; i--)
    //         {
    //             s += "[";
    //             s += bytes[i].ToString("X2");
    //             s += "]";
    //         }
    //         return s;
}