using ProjeTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ProjeTakip.MyRoleProvider
{
    public class SiteRole : RoleProvider   
    {

        private WebProjeDBEntities db = new WebProjeDBEntities();


        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] result;
            var data = db.kullanici.Where(x => x.kul_isim == username).FirstOrDefault();
            var ogrdata = db.ogrenci.Where(a => a.ogr_kul == username).FirstOrDefault();
            if (data != null)
            {
                result = new string[]{ data.kul_yetki.ToString() };
            }
            else if (ogrdata != null)
            {
                result = new string[]{ ogrdata.ogr_yetki.ToString() };
            }
            else
            {
                result = new string[] { "-1" };
            }
            
            
            return result;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}