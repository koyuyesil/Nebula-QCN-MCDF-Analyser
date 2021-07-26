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
        public FileVersion FileVersionReader { get; set; }
        public FeatureMask FeatureMaskReader { get; set; }
        public MobilePropertyInfo MobilePropertyInfoSum { get; set; }
        public NvNumberedItems NvNumberedItems0 { get; set; }
        public NvNumberedItems NvNumberedItems1 { get; set; }
        public NvNumberedItems NvNumberedItems2 { get; set; }
        public IQCNVStruct()
        {
            FileVersionReader = new FileVersion();
            FeatureMaskReader = new FeatureMask();
            MobilePropertyInfoSum = new MobilePropertyInfo();
            NvNumberedItems0 = new NvNumberedItems();
            NvNumberedItems1 = new NvNumberedItems();
            NvNumberedItems2 = new NvNumberedItems();

        }
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
            int i1 = cf.GetNumDirectories();
            byte[] i2 = cf.GetDataBySID(5);
            string root = cf.RootStorage.Name;
            string namss = cf.GetNameDirEntry(5);

            List<CFItem> lstFileVersionItem = (List<CFItem>)cf.GetAllNamedEntries("File_Version");
            CFStream cFFileVersion = (CFStream)lstFileVersionItem[0];
            byte[] dataFileVersion = cFFileVersion.GetData();
            Stream streamFileVersion = new MemoryStream(dataFileVersion);
            BinaryReader brFileVersion = new BinaryReader(streamFileVersion);
            FileVersionReader.Read(brFileVersion);

            List<CFItem> lstFutureMask = (List<CFItem>)cf.GetAllNamedEntries("Feature_Mask");
            CFStream cFFutureMask = (CFStream)lstFutureMask[0];
            byte[] dataFutureMask = cFFutureMask.GetData();
            Stream streamFutureMask = new MemoryStream(dataFutureMask);
            BinaryReader brFutureMask = new BinaryReader(streamFutureMask);
            FeatureMaskReader.Read(brFutureMask);


            List<CFItem> lstMobilePropertiesItem = (List<CFItem>)cf.GetAllNamedEntries("Mobile_Property_Info");
            CFStream cFStreamMobileProp = (CFStream)lstMobilePropertiesItem[0];
            byte[] dataMobileProp = cFStreamMobileProp.GetData();
            Stream streamMobileProp = new MemoryStream(dataMobileProp);
            BinaryReader brMobileProp = new BinaryReader(streamMobileProp);
            MobilePropertyInfoSum.Read(brMobileProp);

            List<CFItem> lstNvItemArray0 = (List<CFItem>)cf.GetAllNamedEntries("NV_ITEM_ARRAY");
            CFStream cFStreamNvItemArray0 = (CFStream)lstNvItemArray0[0];
            byte[] dataNvItemArray0 = cFStreamNvItemArray0.GetData();
            Stream streamNvItemArray0 = new MemoryStream(dataNvItemArray0);
            BinaryReader brNvItemArray0 = new BinaryReader(streamNvItemArray0);
            NvNumberedItems0.Read(brNvItemArray0);

            List<CFItem> lstNvItemArray1 = (List<CFItem>)cf.GetAllNamedEntries("NV_ITEM_ARRAY_SIM_1");
            CFStream cFStreamNvItemArray1 = (CFStream)lstNvItemArray1[0];
            byte[] dataNvItemArray1 = cFStreamNvItemArray1.GetData();
            Stream streamNvItemArray1 = new MemoryStream(dataNvItemArray1);
            BinaryReader brNvItemArray1 = new BinaryReader(streamNvItemArray1);
            NvNumberedItems1.Read(brNvItemArray1);

            List<CFItem> lstNvItemArray2 = (List<CFItem>)cf.GetAllNamedEntries("NV_ITEM_ARRAY_SIM_2");
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
