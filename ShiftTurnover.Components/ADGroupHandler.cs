using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ShiftTurnover.Components
{
    public class ADGroupHandler
    {
        public static bool IsAuthenticated(string usr, string pwd)
        {
            bool flag = false;
            try
            {
                LdapConnection connection = new LdapConnection("ldap.nih.gov");
                NetworkCredential credential = new NetworkCredential(usr, pwd);
                connection.Credential = credential;
                connection.Bind();
                flag = true;
            }
            catch (LdapException lexc)
            {
                String error = lexc.ServerErrorMessage;
                flag = false;
            }
            catch (Exception exc)
            {
                String error = exc.Message;
                flag = false;
            }

            return flag;
        }
        public static bool IsAuthenticated(string usr)
        {


            bool flag = false;
            try
            {
                // set up domain context
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "NIH");

                // find a user
                UserPrincipal user = UserPrincipal.FindByIdentity(ctx, usr);

                // find the group in question
                GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, "ORF_DTR");

                if (user != null)
                {
                    // check if user is member of that group
                    if (user.IsMemberOf(group))
                    {
                        flag = true;
                    }
                }
            }
            catch (LdapException lexc)
            {
                String error = lexc.ServerErrorMessage;
                flag = false;
            }
            catch (Exception exc)
            {
                String error = exc.Message;
                flag = false;
            }

            return flag;
        }
        public static SearchResultCollection FindAccountByEmail(string pEmailAddress)
        {
            string filter = string.Format("(proxyaddresses=SMTP:{0})", pEmailAddress);

            using (DirectoryEntry gc = new DirectoryEntry("LDAP:"))
            {
                foreach (DirectoryEntry z in gc.Children)
                {
                    using (DirectoryEntry root = z)
                    {
                        using (DirectorySearcher searcher = new DirectorySearcher(root, filter, new string[] { "proxyAddresses", "objectGuid", "displayName", "distinguishedName" }))
                        {
                            searcher.ReferralChasing = ReferralChasingOption.All;
                            SearchResultCollection result = searcher.FindAll();

                            return result;
                        }
                    }
                }
            }
            return null;
        }
        public static string GetEmailAddress(string userName)
        {
            string _emailAddress = "";
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "NIH"))
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(pc, userName);
                if (up != null)
                {
                    if (up.EmailAddress != null) { _emailAddress = up.EmailAddress.Trim(); }

                }
                return _emailAddress;
            }

        }
        public static string GetPhoneNumber(string userName)
        {
            string _phonenumber = "";
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "NIH"))
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(pc, userName);

                if (up != null)
                {
                    if (up.VoiceTelephoneNumber != null)
                    {
                        _phonenumber = up.VoiceTelephoneNumber.Trim();
                        _phonenumber = _phonenumber.Replace(".", "");
                    }
                }


                return _phonenumber;
            }

        }

        public static string GetEmployeeId(string userName)
        {
            string _EmployeeId = "";
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "NIH"))
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(pc, userName);

                if (up != null)
                {
                    if (up.EmployeeId != null)
                    {
                        _EmployeeId = up.EmployeeId.Trim();
                    }
                }

                return _EmployeeId;
            }

        }
        public static void _SendEmailToRequestor(string _toEmailAddress, string _subject, string _body)
        {
            var fromAddress = new MailAddress("donotreply@nih.gov", "EFAM DTR IT Support");
            var toAddress = new MailAddress(_toEmailAddress, _toEmailAddress);

            var smtp = new SmtpClient
            {
                Host = "Mailfwd.nih.gov",
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "")
            };
            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    IsBodyHtml = true,
                    Subject = _subject,
                    Body = _body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public static void _SendEmail(string _toEmailAddress, string _subject, string _body)
        {
            var fromAddress = new MailAddress("donotreply@nih.gov", "EFAM DTR IT Support");
            var toAddress = new MailAddress(_toEmailAddress, _toEmailAddress);

            var smtp = new SmtpClient
            {
                Host = "Mailfwd.nih.gov",
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "")
            };
            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    IsBodyHtml = true,
                    Subject = _subject,
                    Body = _body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public static string GetUserEmailByFullName(string lname, string fname)
        {
            string email = "";
            // create your domain context
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

            // define a "query-by-example" principal - here, we search for a UserPrincipal 
            // and with the first name (GivenName) of "Bruce" and a last name (Surname) of "Miller"
            UserPrincipal qbeUser = new UserPrincipal(ctx);
            qbeUser.GivenName = fname;
            qbeUser.Surname = lname;

            // create your principal searcher passing in the QBE principal    
            PrincipalSearcher srch = new PrincipalSearcher(qbeUser);

            // find all matches
            foreach (var found in srch.FindAll())
            {
                if (GetEmailAddress(found.DistinguishedName).Length > 0) { email = GetEmailAddress(found.DistinguishedName); }

            }
            return email;
        }
        public static string GetUserName(string userName)
        {
            string _userName = "";
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "NIH"))
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(pc, userName);
                if (up != null)
                    _userName = up.DisplayName.Substring(0, up.DisplayName.IndexOf("(")).Trim();
                return _userName;
            }

        }

        public static bool isUserDTRITStaff(string UserName)
        {
            bool _isDTMITStaff = false;
            List<GroupPrincipal> allADGroups = GetGroups(UserName);

            foreach (GroupPrincipal ADGroups in allADGroups)
            {
                if (ADGroups.Name.ToString().Equals("DTR-EFAM Group")) { _isDTMITStaff = true; }
            }


            return _isDTMITStaff;

        }
        public static List<GroupPrincipal> GetGroups(string userName)
        {
            List<GroupPrincipal> result = new List<GroupPrincipal>();

            // establish domain context
            PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain);

            // find your user
            UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, userName);

            // if found - grab its groups
            if (user != null)
            {
                PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();

                // iterate over all groups
                foreach (Principal p in groups)
                {
                    // make sure to add only group principals
                    if (p is GroupPrincipal)
                    {
                        result.Add((GroupPrincipal)p);
                    }
                }
            }

            return result;
        }
        public static bool isCDTMITDISRSSupport(string UserName)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "NIH");
            bool _isDTMStaff = false;
            // find a user
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, UserName);
            // find the group in question
            GroupPrincipal group1 = GroupPrincipal.FindByIdentity(ctx, "CC-DTM IT Developers");
            GroupPrincipal group2 = GroupPrincipal.FindByIdentity(ctx, "CC-DTM IT DISRS Support");

            if (user != null)
            {
                // check if user is member of that group
                //  if (isUserDTMITStaff(UserName))

                if (user.IsMemberOf(group1) || UserName.ToLower().Equals("bmaheshwar") || UserName.ToLower().Equals("rallison") || UserName.ToLower().Equals("guojaj") || UserName.ToLower().Equals("zhangj25") || UserName.ToLower().Equals("hogannl") || UserName.ToLower().Equals("chidambaramsg") || UserName.ToLower().Equals("twhitten1") || user.IsMemberOf(group2))
                {
                    _isDTMStaff = true;
                }

            }

            return _isDTMStaff;
        }
        public static bool isDTMTestingStaff(string UserName)
        {
            bool IsAdmin = false;
            string m_adminusers = "";
            System.Configuration.AppSettingsReader SettingsReader = new System.Configuration.AppSettingsReader();
            m_adminusers = (string)SettingsReader.GetValue("testUsers", typeof(String));
            if (m_adminusers.Contains(UserName)) { IsAdmin = true; }
            return IsAdmin;
        }

        public static bool isDTMStaff(string UserName, string ADGroup2Match)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "NIH");
            bool _isDTMStaff = false;
            // find a user
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, UserName);

            // find the group in question
            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, ADGroup2Match);

            if (user != null)
            {
                // check if user is member of that group
                if (user.IsMemberOf(group))
                {
                    _isDTMStaff = true;
                }
            }

            return _isDTMStaff;
        }
        public static string[] GetAllDTMStaff(string lginuserid)
        {

            List<string> customers = new List<string>();
            try
            {

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "NIH");
                GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, "ORF_DTR");

                if (grp == null)
                {
                    throw new ApplicationException("We did not find that group in that domain, perhaps the group resides in a different domain?");
                }

                List<DTMUsers> lstADUsers = new List<DTMUsers>();
                foreach (Principal p in grp.GetMembers(true))
                {
                    DTMUsers u = new DTMUsers();
                    // fullname = p.DisplayName.Substring(0, p.DisplayName.IndexOf("(")).Trim();
                    // u.LastName = fullname.Substring(0, p.DisplayName.IndexOf(",")).Trim();
                    //  u.FirstName = fullname.Substring(p.DisplayName.IndexOf(",") + 1).Trim();
                    u.DisplayName = p.DisplayName.Substring(0, p.DisplayName.IndexOf("(")).Trim();
                    u.NIHUserID = p.Name;

                    if (!lginuserid.Equals(p.Name))
                    { lstADUsers.Add(u); }
                    customers.Add(string.Format("{0}-{1}", u.DisplayName, u.NIHUserID));

                }
                grp.Dispose();
                ctx.Dispose();


                return customers.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string ConvertPhoneNumber(string str)
        {
            if (str.Length > 0)
            {
                str = str.Insert(3, ".");
                str = str.Insert(7, ".");
            }


            return str;
        }
        public static ArrayList Groups()
        {
            ArrayList groups = new ArrayList();
            foreach (System.Security.Principal.IdentityReference group in
                System.Web.HttpContext.Current.Request.LogonUserIdentity.Groups)
            {
                groups.Add(group.Translate(typeof
                    (System.Security.Principal.NTAccount)).ToString());
            }
            return groups;
        }
        public static List<DTMUsers> GetADUsers(string loginID)
        {
            try
            {

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "NIH");
                GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, "ORF_DTR");

                if (grp == null)
                {
                    throw new ApplicationException("We did not find that group in that domain, perhaps the group resides in a different domain?");
                }

                List<DTMUsers> lstADUsers = new List<DTMUsers>();
                foreach (Principal p in grp.GetMembers(true))
                {
                    if (!p.Name.Equals("maglopy"))
                    {
                        DTMUsers u = new DTMUsers();
                        // fullname = p.DisplayName.Substring(0, p.DisplayName.IndexOf("(")).Trim();
                        // u.LastName = fullname.Substring(0, p.DisplayName.IndexOf(",")).Trim();
                        //  u.FirstName = fullname.Substring(p.DisplayName.IndexOf(",") + 1).Trim();
                        if (p.DisplayName.Contains("("))
                        {
                            u.DisplayName = p.DisplayName.Substring(0, p.DisplayName.IndexOf("(")).Trim();
                            u.NIHUserID = p.Name;
                            if (!loginID.Equals(p.Name))
                            { lstADUsers.Add(u); }
                        }
                    }



                }
                grp.Dispose();
                ctx.Dispose();


                return lstADUsers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DTMUsers GetADUsersInfo(string userName)
        {
            try
            {
                DTMUsers u = new DTMUsers();
                string fullname = "";
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "NIH"))
                {
                    UserPrincipal up = UserPrincipal.FindByIdentity(pc, userName);
                    if (up != null)
                    fullname = up.DisplayName.Substring(0, up.DisplayName.IndexOf("(")).Trim();
                    u.LastName = fullname.Substring(0, up.DisplayName.IndexOf(",")).Trim();
                    u.FirstName = fullname.Substring(up.DisplayName.IndexOf(",") + 1).Trim();
                    u.Email = GetEmailAddress(up.Name);
                    u.Phone = GetPhoneNumber(up.Name);
                }
               
                return u;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
            public static List<DTMUsers> GetADUsersList()
        {
            try
            {

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "NIH");
                GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, "ORF Employees");

                if (grp == null)
                {
                    throw new ApplicationException("We did not find that group in that domain, perhaps the group resides in a different domain?");
                }

                List<DTMUsers> lstADUsers = new List<DTMUsers>();
                foreach (Principal p in grp.GetMembers(true))
                {
                    DTMUsers u = new DTMUsers();
                    // fullname = p.DisplayName.Substring(0, p.DisplayName.IndexOf("(")).Trim();
                    // u.LastName = fullname.Substring(0, p.DisplayName.IndexOf(",")).Trim();
                    //  u.FirstName = fullname.Substring(p.DisplayName.IndexOf(",") + 1).Trim();

                    u.DisplayName = p.DisplayName.Substring(0, p.DisplayName.IndexOf("(")).Trim();
                    u.NIHUserID = p.Name;
                    u.Email = GetEmailAddress(p.Name);
                    u.Phone = GetPhoneNumber(p.Name);
                    lstADUsers.Add(u);
                }
                grp.Dispose();
                ctx.Dispose();


                return lstADUsers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DataTable GetAllDTRUsers()
        {
            try
            {

                DataModule _dm = new DataModule();

                DataSet ds = _dm.GetDataSet("SelectAllEMPList");

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DataTable GetAllADUsers()
        {
            try
            {

                DataModule _dm = new DataModule();

                DataSet ds = _dm.GetDataSet("SelectAllPersonList");

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static void PopulateUsers(string ADGroup)
        {
            try
            {

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "NIH");
                GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, ADGroup);

                if (grp == null)
                {
                    throw new ApplicationException("We did not find that group in that domain, perhaps the group resides in a different domain?");
                }
                foreach (Principal p in grp.GetMembers(true))
                {
                    DataModule _dm = new DataModule();
                    _dm.AddParameter("@ssoname", SqlDbType.VarChar, p.Name, 100);

                    DataSet ds = _dm.GetDataSet("getlogin"); //authorization - has to exist in database
                    if (ds != null && ds.Tables.Count > 0)

                    {

                        if (ds.Tables[0].Rows.Count == 0)//no role is found, login failed
                        {
                            AddRequestForm(p.Name, p.DisplayName.Substring(0, p.DisplayName.IndexOf("(")).Trim());
                        }
                    }
                }


                grp.Dispose();
                ctx.Dispose();


            }
            catch (Exception ex)
            {

            }
        }
        public static bool IsUserInPIUserADGrp(String USerName)
        {
            bool USerExists = false;
            // set up domain context
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            // find the group in question
            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, "PIUsers");

            // if found....

            if (group != null)
            {
                // iterate over members
                Principal U = group.GetMembers().
                    Where(p => p.SamAccountName.Equals(USerName)).FirstOrDefault();
                if (U != null) { USerExists = true; }
            }

            return USerExists;
        }
        private static void AddRequestForm(string Name, string DisplayName)
        {

            try
            {
                DataModule dataModule = new DataModule();

                string fullname = ""; string Lname = ""; string Fname = "";
                if (DisplayName.Length > 0)
                {

                    Lname = DisplayName.Substring(0, DisplayName.IndexOf(","));
                    Fname = DisplayName.Substring(DisplayName.IndexOf(",") + 1);

                }

                if (Fname == "")
                {
                    dataModule.AddParameter("@firstname", SqlDbType.VarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@firstname", SqlDbType.VarChar, Fname);
                }
                if (Lname == "")
                {
                    dataModule.AddParameter("@lastname", SqlDbType.VarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@lastname", SqlDbType.VarChar, Lname);
                }
                if (Name == "")
                {
                    dataModule.AddParameter("@userid", SqlDbType.VarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@userid", SqlDbType.VarChar, Name);
                }
                if (GetEmailAddress(Name).Length == 0)
                {
                    dataModule.AddParameter("@email", SqlDbType.VarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@email", SqlDbType.VarChar, GetEmailAddress(Name));
                }


                DataSet ds = dataModule.GetDataSet("addperson");

            }
            catch (System.Exception ex)
            {
                string msg = ex.Message;

            }



        }
        public class DTMUsers
        {

            public string NIHUserID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string DisplayName { get; set; }
        }


    }
}
