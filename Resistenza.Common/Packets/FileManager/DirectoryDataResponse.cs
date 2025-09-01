using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;





namespace Resistenza.Common.Packets
{
    public class DirectoryDataResponse : IPacket    
    {
       
        public string Type { get; set; }
        public string Directory {  get; set; } //viene usato per aggiornamenti indipendenti del filesystem del client
        public List<FileSystemEntry> AllEntries{ get; set;}
        public string Error { get; set; }

        


        public DirectoryDataResponse() { 
            Type = this.GetType().ToString();
           
            
        }  
    }

    public class FileSystemEntry
    {
        
       
        public bool IsDirectory { get; set; }
        public int FileSizeBytes { get; set; }
        public string Name { get; set; }
        public string LastChange { get; set; }

        


    }
}
