
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.ComponentModel;

namespace OpenMcdf.Extensions.Formats.Structures
{
   
    public class MobilePropertyInfo
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
        public int ModemFirmwareVersionLength { get; set; }

        [CategoryAttribute("Mobile Properties Summary"), DescriptionAttribute("Modem Firmware version")]
        public string ModemFirmwareVersion { get; set; }
        [CategoryAttribute("Pointer"), DescriptionAttribute("Software Version string size")]
        public int SoftwareVersionLength { get; set; }
        [CategoryAttribute("Mobile Properties Summary"), DescriptionAttribute("This file created by this software")]
        public string SoftwareVersion { get; set; }

        public void Read(BinaryReader br)
        {
            ESN= br.ReadUInt32();
            MODEM= br.ReadUInt16();
            MajorVersion = br.ReadByte();
            MinorVersion = br.ReadByte();
            ModemFirmwareVersionLength= br.ReadUInt16();//Pascal String
            ModemFirmwareVersion = Encoding.ASCII.GetString(br.ReadBytes(ModemFirmwareVersionLength));
            SoftwareVersionLength = br.ReadUInt16();
            SoftwareVersion = Encoding.ASCII.GetString(br.ReadBytes(SoftwareVersionLength));
        }


    }
}