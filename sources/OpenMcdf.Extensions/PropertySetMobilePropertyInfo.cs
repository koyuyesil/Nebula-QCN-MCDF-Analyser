
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace OpenMcdf.Extensions
{
    public class PropertySetMobilePropertyInfo
    {

        public uint ESN { get; set; }
        public uint MODEM { get; set; }
        public byte MajorVersion { get; set; }
        public byte MinorVersion { get; set; }
        public int ModemFirmwareVersionPointer { get; set; }
        public string ModemFirmwareVersion { get; set; }
        public int SoftwareVersionPointer { get; set; }
        public string SoftwareVersion { get; set; }

        public PropertySetMobilePropertyInfo()
        {

        }

        public void Read(System.IO.BinaryReader br)
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