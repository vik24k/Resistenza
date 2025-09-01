using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Resistenza.Common.Packets
{

    public abstract class PacketSerializer
    {

        public static byte[] Serialize(object Class)
        {

            string Json = System.Text.Json.JsonSerializer.Serialize(Class);
            return Encoding.UTF8.GetBytes(Json);
        }
        public static object? Deserialize(byte[] RawPacket)
        {

          
            string PacketJson = Encoding.UTF8.GetString(RawPacket, 0, RawPacket.Length);
            
            //La deserializzazione avviene in più fasi:
            //1) L'array di bytes viene convertito in un oggetto generico, dal quale è possibile dedurre il tipo, dal momento
            //che ogni pacchetto diverso deve contenere il membro type 
            //2) dopo la comprensione del tipo il pacchetto viene deserializzato una seconda volta, ma questa volta al tipo 
            //specifico di pacchetto           
            var GenericPacket = JsonConvert.DeserializeObject<dynamic>(PacketJson);
            if(GenericPacket == null)
            {
                return null; //pacchetto deformato, la deserializzazione fallisce
            }
            string? PacketTypeString = GenericPacket.Type;
            
            Type PacketType = Type.GetType(PacketTypeString); //null? nullable? Non dovrebbe poter essere null
            object SpecificPacket = JsonConvert.DeserializeObject(PacketJson, PacketType);

            return SpecificPacket;
               
        }

        
        


    }
}
