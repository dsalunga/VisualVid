using System;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;

namespace DES.VisualVid
{
    public class Members
    {
        private int _MemberID;
        private SqlGuid _UserId;

        public int MemberID
        {
            get{ return _MemberID; }
        }

        public SqlGuid UserId
        {
            get{ return _UserId; }
        }


        private Members(int iMemberID)
        {
            _MemberID = iMemberID;
        }
    }
}
