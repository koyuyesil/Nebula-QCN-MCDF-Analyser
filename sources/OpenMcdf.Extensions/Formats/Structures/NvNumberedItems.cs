
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.ComponentModel;
using System.Collections;

namespace OpenMcdf.Extensions.Formats.Structures
{
    public class NvNumberedItem
    {
        public UInt16 id { get; set; }
        public UInt16 index { get; set; }
        public List<byte[]> data { get; set; }
        public NvNumberedItem()
        {
            data = new List<byte[]>();
        }
    }
    public class NvNumberedItems
    {
        public List<NvNumberedItem> items;
        public NvNumberedItems()
        {
            items = new List<NvNumberedItem>();
        }
    public void Read(BinaryReader br)
        {
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                NvNumberedItem nvNumberedItem = new NvNumberedItem();
                uint baseOffset = br.ReadUInt32();
                nvNumberedItem.id = br.ReadUInt16();
                nvNumberedItem.index = br.ReadUInt16();
                nvNumberedItem.data.Add(br.ReadBytes(16));
                nvNumberedItem.data.Add(br.ReadBytes(16));
                nvNumberedItem.data.Add(br.ReadBytes(16));
                nvNumberedItem.data.Add(br.ReadBytes(16));
                nvNumberedItem.data.Add(br.ReadBytes(16));
                nvNumberedItem.data.Add(br.ReadBytes(16));
                nvNumberedItem.data.Add(br.ReadBytes(16));
                nvNumberedItem.data.Add(br.ReadBytes(16));
                this.items.Add(nvNumberedItem);
            }
        }
    }
}