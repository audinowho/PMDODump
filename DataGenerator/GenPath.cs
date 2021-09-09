using System;
using System.Collections.Generic;

namespace DataGenerator
{
    public static class GenPath
    {
        public static string DATA_GEN_PATH = "DataAsset/";

        public static string TL_PATH { get => DATA_GEN_PATH + "String/"; }
        public static string ITEM_PATH { get => DATA_GEN_PATH + "Item/"; }
        public static string ZONE_PATH { get => DATA_GEN_PATH + "Zone/"; }
    }
}
