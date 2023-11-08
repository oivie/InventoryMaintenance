using System.Collections.Generic;
using System.IO;

namespace InventoryMaintenance
{
    public static class InventoryDB
    {
        private const string FilePath = @"..\..\..\InventoryItems.dat";

        public static List<InventoryItem> GetItems()
        {
            List<InventoryItem> items = new();

            // Verify if the file exists before attempting to read
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("The data file was not found.", FilePath);
            }

            try
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while (fs.Position < fs.Length)
                    {
                        int itemNo = br.ReadInt32();
                        string description = br.ReadString();
                        decimal price = br.ReadDecimal();
                        items.Add(new InventoryItem(itemNo, description, price));
                    }
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                // Handle the case where the directory is not found
                throw new Exception("The directory was not found.", ex);
            }
            catch (IOException ex)
            {
                // Handle general IO exceptions
                throw new Exception("An I/O error occurred while reading the file.", ex);
            }

            return items;
        }

        public static void SaveItems(List<InventoryItem> items)
        {
            try
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    foreach (InventoryItem item in items)
                    {
                        bw.Write(item.ItemNo);
                        bw.Write(item.Description);
                        bw.Write(item.Price);
                    }
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                // Handle the case where the directory is not found
                throw new Exception("The directory was not found.", ex);
            }
            catch (IOException ex)
            {
                // Handle general IO exceptions
                throw new Exception("An I/O error occurred while writing to the file.", ex);
            }
        }
    }
}
