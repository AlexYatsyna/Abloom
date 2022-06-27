using AbloomWorkingNode.Messages;
using Akka.Actor;
using Akka.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Text;

namespace AbloomWorkingNode.Serializers
{
    public class MySerializer : SerializerWithStringManifest
    {
        private const string SendToWorkerManifest = "stw";
        private const string RespondPasswordManifest = "rps";

        public override int Identifier { get; } = 239;

        public MySerializer(ExtendedActorSystem system) : base(system)
        {

        }
        public override object FromBinary(byte[] bytes, string manifest)
        {
            return manifest switch
            {
                SendToWorkerManifest => JsonConvert.DeserializeObject<SendToWorkinNode>(Encoding.UTF8.GetString(bytes)),
                RespondPasswordManifest => JsonConvert.DeserializeObject<RespondPassword>(Encoding.UTF8.GetString(bytes)),
                _ => throw new SerializationException(),
            };
        }

        public override string Manifest(object o)
        {
            return o switch
            {
                SendToWorkinNode => SendToWorkerManifest,
                RespondPassword => RespondPasswordManifest,
                _ => throw new NotImplementedException(),
            };
        }

        public override byte[] ToBinary(object obj)
        {
            return obj switch
            {
                SendToWorkinNode d => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(d)),
                RespondPassword d => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(d)),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
