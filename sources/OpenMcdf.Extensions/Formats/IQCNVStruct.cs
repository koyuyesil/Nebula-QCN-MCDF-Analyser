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
            NvNumberedItems0 = new NvNumberedItems();
            NvNumberedItems1 = new NvNumberedItems();
            NvNumberedItems2 = new NvNumberedItems();

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

        public NvNumberedItems NvNumberedItems0 { get; set; }

        public NvNumberedItems NvNumberedItems1 { get; set; }

        public NvNumberedItems NvNumberedItems2 { get; set; }

        public void LoadFromCompoundFile(CompoundFile cf)
        {
            int i1 = cf.GetNumDirectories();
            byte[] i2 = cf.GetDataBySID(5);
            string root = cf.RootStorage.Name;
            string namss = cf.GetNameDirEntry(5);

            List<CFItem> lstFileVersionItem = (List<CFItem>)cf.GetAllNamedEntries("File_Version");
            List<CFItem> lstFutureMaskItem = (List<CFItem>)cf.GetAllNamedEntries("Feature_Mask");
            List<CFItem> lstMobilePropertiesItem = (List<CFItem>)cf.GetAllNamedEntries("Mobile_Property_Info");

            List<CFItem> lstNvItemArray0 = (List<CFItem>)cf.GetAllNamedEntries("NV_ITEM_ARRAY");
            List<CFItem> lstNvItemArray1 = (List<CFItem>)cf.GetAllNamedEntries("NV_ITEM_ARRAY_SIM_1");
            List<CFItem> lstNvItemArray2 = (List<CFItem>)cf.GetAllNamedEntries("NV_ITEM_ARRAY_SIM_2");

            CFStream cFStreamMobileProp = (CFStream)lstMobilePropertiesItem[0];
            byte[] dataMobileProp = cFStreamMobileProp.GetData();
            Stream streamMobileProp = new MemoryStream(dataMobileProp);
            BinaryReader brMobileProp = new BinaryReader(streamMobileProp);
            MobilePropertyInfoSum.Read(brMobileProp);

            CFStream cFStreamNvItemArray0 = (CFStream)lstNvItemArray0[0];
            byte[] dataNvItemArray0 = cFStreamNvItemArray0.GetData();
            Stream streamNvItemArray0 = new MemoryStream(dataNvItemArray0);
            BinaryReader brNvItemArray0 = new BinaryReader(streamNvItemArray0);
            NvNumberedItems0.Read(brNvItemArray0);

            CFStream cFStreamNvItemArray1 = (CFStream)lstNvItemArray1[0];
            byte[] dataNvItemArray1 = cFStreamNvItemArray1.GetData();
            Stream streamNvItemArray1 = new MemoryStream(dataNvItemArray1);
            BinaryReader brNvItemArray1 = new BinaryReader(streamNvItemArray1);
            NvNumberedItems1.Read(brNvItemArray1);

            CFStream cFStreamNvItemArray2 = (CFStream)lstNvItemArray2[0];
            byte[] dataNvItemArray2 = cFStreamNvItemArray2.GetData();
            Stream streamNvItemArray2 = new MemoryStream(dataNvItemArray2);
            BinaryReader brNvItemArray2 = new BinaryReader(streamNvItemArray2);
            NvNumberedItems2.Read(brNvItemArray2);


        }

        public void LoadFromXQCNFile(XmlDocument xml)
        {

        }
    }
}
