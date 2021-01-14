using System;
using System.Collections.Generic;
using System.Text;

namespace QRSpace.Server.Entities
{
    public class GameSession
    {
        public record Step
        {
            // 0x0033 => 0,0->3,3
            public short FromTo { get; set; }
            // 0x00 => 
            public byte PieceType { get; set; }
        }
        
        public Guid SessionId { get; }
        public string InviterName { get; }
        public string ReceiverName { get; }
        private List<Step> Steps { get; }

        public GameSession(string inviterName, string receiverName)
        {
            SessionId = Guid.NewGuid();
            InviterName = inviterName;
            ReceiverName = receiverName;
            Steps = new List<Step>();
        }

        public void AddStep(Step step)
        {
            Steps.Add(step);
        }

        public string WriteRecord()
        {
            var result = new StringBuilder();
            foreach (var step in Steps)
            {
                var fromTo = Convert.ToString(step.FromTo, 16);
                var piece = Convert.ToString(step.PieceType, 16);
                result.Append(fromTo);
                result.Append(piece);
            }

            return result.ToString();
        }
    }
}