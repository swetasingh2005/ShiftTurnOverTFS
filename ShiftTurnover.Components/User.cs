using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;

namespace ShiftTurnover.Components
{
    [Serializable()]
    public class User
    {
        private int _personroleid;
        private string _displayname;
        private string _userid;

        public int personroleid { get { return _personroleid; } }
        public string userid { get { return _userid; } }

        public User(int intpersonroleID, string strUserID)
        {
            _personroleid = intpersonroleID;
            _userid = strUserID;
        }
        public User()
        {
        }

        public void SetUserInfo()
        {
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@personroleid", SqlDbType.Int, _personroleid, 0);

                DataSet ds = _dm.GetDataSet("selectpersonrole");

                if (ds != null)
                {
                    _displayname = ds.Tables[0].Rows[0]["displayname"].ToString();
                }
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
        }
    }

    public class Loto
    {
        private string _rowstamp;
        private string _status_description;
        private string _authorized_loto;

        private string _wosafetylink_collectionref;
        private string _wotaglock_collectionref;
        private string _href;
        private string _status;

        public string wosafetylink_collectionref { get { return _wosafetylink_collectionref; } }
        public string wotaglock_collectionref { get { return _wotaglock_collectionref; } }
        public string href { get { return _href; } }
        public string status { get { return _status; } }

        public string rowstamp { get { return _rowstamp; } }
        public string status_description { get { return _status_description; } }
        public string authorized_loto { get { return _authorized_loto; } }
    }
}
