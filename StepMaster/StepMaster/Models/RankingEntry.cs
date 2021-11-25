using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StepMaster.Models
{
    public class RankingEntry
    {
        public RankingEntry(string username, string positionNumber, int steps, bool isCurrentUser = false)
        {
            Username = username;
            PositionNumber = positionNumber;
            Steps = steps;
            Color = Color.FromHex( isCurrentUser ? "#70e1f5" : "#9EE493");
        }

        public string Username { get; set; }
        public string PositionNumber { get; set; }
        public int Steps { get; set; }
        public Color Color { get; set; }

}
}

