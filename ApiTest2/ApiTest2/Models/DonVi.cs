using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Models
{
    public class DonVi
    {
        public int IDDonVi { get; set; }
        public string MaSoThue { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public Guid ObjectGUID { get; set; }
        public int UserUpdate { get; set; }

        public static string GetAllDonVi(out List<DonVi> lstdonvi)
        {
            return DBM.GetList("usp_DonVi_GetAll", new { }, out lstdonvi);
        }
        public string InsertorUpdate(BSS.DBM dbm)
        {
            string msg = "";

            msg = dbm.SetStoreNameAndParams("usp_DonVi_InsertorUpdate", new
            {
                IDDonVi,
                MaSoThue,
                Name,
                Address,
                Email,
                NumberPhone,
                UserUpdate
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        public static string GetOneDonViByID(int IDDonVi, out DonVi donvi)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_DonVi_GetOne", new { IDDonVi }, out donvi);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public string Update(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_DonVi_Update", new
            {
                IDDonVi,
                MaSoThue,
                Name,
                Address,
                Email,
                NumberPhone,
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }

        public string Delete(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_DonVi_Delete", new { IDDonVi });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
    }
}
