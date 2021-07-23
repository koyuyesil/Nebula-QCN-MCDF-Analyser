using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;

namespace OpenMcdf.Extensions.Formats
{
    abstract class IQCNVStruct:INvSctruct
    {
        public byte[] FileVersion { get; set; }

        // 3*2 bytes integers

        public byte[] FeatureMask { get; set; }
        // first two bytes skip(header)
        // folowing bytes convert bit
        // bit ordering same as folowing
        // 7 6 5 4 3 2 1 0 - 15 14 13 12 11 10 9 8 - 23 22 21 20 19 18 17 16 

        public byte[] MobilePropertyInfo { get; set; }

        public byte[] Baseband { get; set; }

        public ValueTuple<int, byte[], byte[]> NvItems { get; set; }

        public ValueTuple<int, byte[], byte[]> EfsBackup { get; set; }

        public ValueTuple<int, byte[], byte[]> ProvisioningItemFiles { get; set; }

        public byte[] NV_ITEM_ARRAY { get; set; }

        public byte[] NV_ITEM_ARRAY_SIM_1 { get; set; }

        public byte[] NV_ITEM_ARRAY_SIM_2 { get; set; }

        public void LoadFromCompoundFile(CompoundFile cf)
        {

        }

        public void LoadFromXQCNFile(XmlDocument xml)
        {

        }
    }
}
