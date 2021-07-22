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

        public byte[] FeatureMask { get; set; }

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
