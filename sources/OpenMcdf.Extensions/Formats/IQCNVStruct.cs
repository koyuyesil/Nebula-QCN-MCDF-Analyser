using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using OpenMcdf.Extensions.Formats.Structures;
using System.IO;

namespace OpenMcdf.Extensions.Formats
{
    public abstract class IQCNVStruct:INvSctruct
    {
        public IQCNVStruct()
        {
            MobilePropertyInfoSum = new MobilePropertyInfo();

        }
        public byte[] FileVersion { get; set; }

        // 3*2 bytes integers

        public byte[] FeatureMask { get; set; }
        // first two bytes skip(header)
        // folowing bytes convert bit
        // bit ordering same as folowing
        // 7 6 5 4 3 2 1 0 - 15 14 13 12 11 10 9 8 - 23 22 21 20 19 18 17 16 

        public byte[] MobilePropertyInfo { get; set; }

        public MobilePropertyInfo MobilePropertyInfoSum { get; set; }

        public byte[] Baseband { get; set; }

        public ValueTuple<int, byte[], byte[]> NvItems { get; set; }

        public ValueTuple<int, byte[], byte[]> EfsBackup { get; set; }

        public ValueTuple<int, byte[], byte[]> ProvisioningItemFiles { get; set; }

        public byte[] NV_ITEM_ARRAY { get; set; }

        public byte[] NV_ITEM_ARRAY_SIM_1 { get; set; }

        public byte[] NV_ITEM_ARRAY_SIM_2 { get; set; }

        public void LoadFromCompoundFile(CompoundFile cf)
        {
            int i1 = cf.GetNumDirectories();
            byte[] i2 = cf.GetDataBySID(5);
            string root = cf.RootStorage.Name;
            string namss = cf.GetNameDirEntry(5);

            List<CFItem> fVer = (List<CFItem>)cf.GetAllNamedEntries("File_Version");
            List<CFItem> fMask = (List<CFItem>)cf.GetAllNamedEntries("Feature_Mask");
            List<CFItem> mProp = (List<CFItem>)cf.GetAllNamedEntries("Mobile_Property_Info");

            List<CFItem> NvItemArray = (List<CFItem>)cf.GetAllNamedEntries("NV_ITEM_ARRAY");
            List<CFItem> NvItemArray1 = (List<CFItem>)cf.GetAllNamedEntries("NV_ITEM_ARRAY_SIM_1");
            List<CFItem> NvItemArray2 = (List<CFItem>)cf.GetAllNamedEntries("NV_ITEM_ARRAY_SIM_2");

            CFStream stream = (CFStream)mProp[0];
            byte[] by = stream.GetData();

            Stream sy = new MemoryStream(by);
            BinaryReader br = new BinaryReader(sy);
            MobilePropertyInfoSum.Read(br);



        }

        public void LoadFromXQCNFile(XmlDocument xml)
        {

        }
    }
}
