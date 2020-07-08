using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WSFrameworkConst
{
    public enum TRSReportLevel { position, localPosition, positionAndRotation, localScale, localPositionAndRotation, localPositionRotationAndScale }

    public static class Const
    {
        static readonly public string invalid = "/invalid";
        static public readonly string hierarchyRootRequest = "/roots";
        static public readonly string hierarchyScenesRequest = "/scenes";
        static public readonly string hierarchyChildrenRequest = "/children";
        static readonly public string get = "/get";
        static readonly public string set = "/set";
        static readonly public string local = "/local";
        static readonly public string global = "/global";

        static readonly public string position = "/position";
        static readonly public string rotation = "/rotation";
        static readonly public string active = "/active";
        static readonly public string call = "/call";
        static readonly public string componentActive = "/componentActive";
        static readonly public string positionRotation = "/posAndRot";
        static readonly public string posRotScale = "/posRotScale";
        // static readonly public string smooth = "/smoth";
        static readonly public string scale = "/scale";
        static readonly public string oscMessageAddess = "/message";
        // static readonly public string objectIDKeywordAddress = "/id";
        static readonly public string objectComponentsAddress = "/components";
        static readonly public string objectComponentsDetailsAddress = "/componentDetails";
        static readonly public string release = "/releaseComponent";
        static readonly public string valueOSC = "/value";
        static readonly public string stringvalueOSC = "/stringvalue";
        // static readonly public string objectPosRotScale = "/trs";
        // static readonly public string objectPosRot = "/posRot";
        // static readonly public string localScale = "/localScale";
        // static readonly public string objectPosLocal = "/position/local";
        // static readonly public string objectPosGlobal = "/position/global";
        public static readonly Color serviceNameColor = new Color(.2f, .6f, .8f);
        public static readonly Color serviceMessageColor = new Color(.5f, .2f, .9f);
        public static readonly Color clientUsingServiceNameColor = new Color(.2f, .5f, .1f);
        public static readonly Color clientUsingServiceNameMssage = new Color(.3f, .3f, .8f);
        public static readonly Color connectionMessage = new Color(0, .9f, .2f);
        public static readonly Color disconnectionMessage = new Color(.6f, .1f, .1f);
        // public static string GetAddressFor(this TRSReportLevel level)
        // {
        //     switch (level)
        //     {
        //         case TRSReportLevel.position:
        //             return objectPosGlobal;
        //         case TRSReportLevel.localPosition:
        //             return objectPosLocal;
        //         case TRSReportLevel.positionAndRotation:
        //             return objectPosRot;
        //         case TRSReportLevel.localPositionAndRotation:
        //             return objectPosLocal;
        //         case TRSReportLevel.localScale:
        //             return localScale;
        //         default:
        //             return "unknwon trs";
        //     }
        // }
        public static string GetPossibleServiceName(this object obj)
        {
            return GetPossibleServiceName(obj.GetType().ToString());
        }

        public static string GetPossibleServiceName(string className)
        {
            string possiblename = className;
            if (possiblename.StartsWith("WS"))
                possiblename = possiblename.Substring(2);
            if (possiblename.EndsWith("Service"))
                possiblename = possiblename.Substring(0, possiblename.Length - "Service".Length);
            if (possiblename.EndsWith("Client"))
                possiblename = possiblename.Substring(0, possiblename.Length - "Client".Length);
            possiblename = possiblename.ToLower();
            return "/" + possiblename;;
        }
    }
}