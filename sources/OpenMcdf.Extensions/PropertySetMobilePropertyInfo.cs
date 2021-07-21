
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.ComponentModel;

namespace OpenMcdf.Extensions
{
    [CategoryAttribute("Mobile Properties Summary"), DescriptionAttribute("Summary info for Qualcomm NV Calibration File (QCN)")]
    public class PropertySetMobilePropertyInfo
    {
        [CategoryAttribute("Mobile Properties Summary"), DescriptionAttribute("Electronic Serial Number")]
        public uint ESN { get; set; }

        [CategoryAttribute("Mobile Properties Summary"), DescriptionAttribute("Qualcomm Baseband Chip Idendifier")]
        public uint MODEM { get; set; }
       
        [CategoryAttribute("Mobile Properties Summary"), DescriptionAttribute("File Major Version")]
        public byte MajorVersion { get; set; }

        [CategoryAttribute("Mobile Properties Summary"), DescriptionAttribute("File Minor Version")]
        public byte MinorVersion { get; set; }

        [CategoryAttribute("Pointer"), DescriptionAttribute("Modem Firmware Version string size")]
        public int ModemFirmwareVersionPointer { get; set; }

        [CategoryAttribute("Mobile Properties Summary"), DescriptionAttribute("Modem Firmware version")]
        public string ModemFirmwareVersion { get; set; }
        [CategoryAttribute("Pointer"), DescriptionAttribute("Software Version string size")]
        public int SoftwareVersionPointer { get; set; }
        [CategoryAttribute("Mobile Properties Summary"), DescriptionAttribute("This file created by this software")]
        public string SoftwareVersion { get; set; }

        public PropertySetMobilePropertyInfo()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        public void Read(BinaryReader br)
        {
            ESN= br.ReadUInt32();
            MODEM= br.ReadUInt16();
            MajorVersion = br.ReadByte();
            MinorVersion = br.ReadByte();
            ModemFirmwareVersionPointer= br.ReadUInt16();
            ModemFirmwareVersion = Encoding.ASCII.GetString(br.ReadBytes(ModemFirmwareVersionPointer));
            SoftwareVersionPointer = br.ReadUInt16();
            SoftwareVersion = Encoding.ASCII.GetString(br.ReadBytes(SoftwareVersionPointer));
        }


    }
}