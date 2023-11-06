using System;
using System.Collections.Generic;
using System.Text.Json;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITUUID : ISUITUUID
    {
        private Guid _uuid;

        public SUITUUID()
        {
            _uuid = Guid.NewGuid();
        }

        public SUITUUID(Guid uuid)
        {
            _uuid = uuid;
        }

        public Guid UUID
        {
            get { return _uuid; }
            set { _uuid = value; }
        }

        public byte[] ToSUIT()
        {
            return _uuid.ToByteArray();
        }

        public void FromSUIT(Dictionary<string, object> data)
        {
            throw new NotImplementedException();
        }

        public void FromSUIT(byte[] data)
        {
            _uuid = new Guid(data);
        }

        public string ToJson()
        {
            return _uuid.ToString();
        }

        public SUITUUID FromJson(string jsonData)
        {
            _uuid = Guid.Parse(jsonData);
            return this;
        }

        public string ToDebug(string indent)
        {
            return $"h'{JsonSerializer.Serialize(ToJson())}' / {_uuid} /";
        }
    }
}