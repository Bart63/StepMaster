using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StepMaster.Models
{
    public class RankingEntry
    {
        public RankingEntry(string username, string positionNumber, int steps, bool isCurrentUser = false, string uID = null)
        {
            Username = username;
            PositionNumber = positionNumber;
            Steps = steps;
            Color = Color.FromHex(isCurrentUser ? "#70e1f5" : "#9EE493");
            IsCurrentUser = isCurrentUser;
            UID = uID;
        }

        public string Username { get; set; }
        public string PositionNumber { get; set; }
        public int Steps { get; set; }
        public Color Color { get; set; }
        public bool IsCurrentUser { get; set; }
        public string UID { get; set; }
        
}
}

