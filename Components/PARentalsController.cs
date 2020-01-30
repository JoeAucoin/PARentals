using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace GIBS.PARentals.Components
{
    public class PARentalsController : IPortable
    {

        #region public method


        public List<PARentalsInfo> Rentals_GetActiveList(int moduleId, int customAddressID, int customCityID, int customBedroomsID, int customBathroomsID, int customRentalAmountID)
        {
            return CBO.FillCollection<PARentalsInfo>(DataProvider.Instance().Rentals_GetActiveList(moduleId, customAddressID, customCityID, customBedroomsID, customBathroomsID, customRentalAmountID));	
        }



        public void Rentals_Booking_Insert(PARentalsInfo info)
        {
            //check we have some content to store
            if (info.DateStart.ToShortDateString() != string.Empty)
            {
                DataProvider.Instance().Rentals_Booking_Insert(info.ModuleId, info.PropertyID, info.DateStart, info.DateEnd, info.RentalAmount, info.Status, info.CreatedByUserID);
            }
        }

        public PARentalsInfo Rentals_Booking_CheckStatus(int propertyID, DateTime dateStart)
        {
           
            return CBO.FillObject<PARentalsInfo>(DataProvider.Instance().Rentals_Booking_CheckStatus(propertyID, dateStart));
        }

        public List<PARentalsInfo> Rentals_GetUsersByRoleName(int portalID, string roleName)
        {
            return CBO.FillCollection<PARentalsInfo>(DataProvider.Instance().Rentals_GetUsersByRoleName(portalID, roleName));
        }


        public void Rentals_Reservation_Insert(PARentalsInfo info)
        {
            //check we have some content to store
            if (info.PropertyID > 0)
            {
                DataProvider.Instance().Rentals_Reservation_Insert(info.ModuleId, info.PropertyID, info.TenantID, info.DateStart, info.DateEnd, info.RentalAmount, info.AgentID, info.Status, info.CreatedByUserID, info.DepositAmt);
            }
        }

        public PARentalsInfo Rentals_Get_Reservation(int reservationID)
        {
            
            return CBO.FillObject<PARentalsInfo>(DataProvider.Instance().Rentals_Get_Reservation(reservationID));

        }

        public void Rentals_Update_Reservation_Payment(PARentalsInfo info)
        {
            //check we have some content to update
            if (info.ReservationID > 0)
            {
                DataProvider.Instance().Rentals_Update_Reservation_Payment(info.ReservationID, info.DepositAmt, info.DepositPaidDate, info.BalancePaidDate, info.BalanceAmt, info.Notes, info.UpdatedByUserID, info.Status, info.LeasePrint);
            }
        }

        public void Rentals_Booking_Update_Status(PARentalsInfo info)
        {
            //check we have some content to update
            if (info.BookingID > 0)
            {
                DataProvider.Instance().Rentals_Booking_Update_Status(info.BookingID, info.Status, info.UpdatedByUserID, info.RentalAmount);
            }
        }

        public PARentalsInfo Rentals_Get_PropertyDetails(int propertyID)
        {
            return CBO.FillObject<PARentalsInfo>(DataProvider.Instance().Rentals_Get_PropertyDetails(propertyID));
        }

        public PARentalsInfo Rentals_Get_PropertyOwner(int propertyID, int portalID)
        {
            return CBO.FillObject<PARentalsInfo>(DataProvider.Instance().Rentals_Get_PropertyOwner(propertyID, portalID));
        }

        public void Rentals_Reservation_Delete(int reservationID, int updatedByUserID)
        {
            DataProvider.Instance().Rentals_Reservation_Delete(reservationID, updatedByUserID);
        }



        /// <summary>
        /// Adds a new PARentalsInfo object into the database
        /// </summary>
        /// <param name="info"></param>
        public void AddPARentals(PARentalsInfo info)
        {
            //check we have some content to store
            if (info.Content != string.Empty)
            {
                DataProvider.Instance().AddPARentals(info.ModuleId, info.Content, info.CreatedByUserID);
            }
        }

        /// <summary>
        /// update a info object already stored in the database
        /// </summary>
        /// <param name="info"></param>
        public void UpdatePARentals(PARentalsInfo info)
        {
            //check we have some content to update
            if (info.Content != string.Empty)
            {
                DataProvider.Instance().UpdatePARentals(info.ModuleId, info.ItemId, info.Content, info.CreatedByUserID);
            }
        }


        /// <summary>
        /// Delete a given item from the database
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="itemId"></param>
        public void DeletePARentals(int moduleId, int itemId)
        {
            DataProvider.Instance().DeletePARentals(moduleId, itemId);
        }


        #endregion

        #region ISearchable Members

        ///// <summary>
        ///// Implements the search interface required to allow DNN to index/search the content of your
        ///// module
        ///// </summary>
        ///// <param name="modInfo"></param>
        ///// <returns></returns>
        //public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        //{
        //    SearchItemInfoCollection searchItems = new SearchItemInfoCollection();

        //    List<PARentalsInfo> infos = GetPARentalss(modInfo.ModuleID);

        //    foreach (PARentalsInfo info in infos)
        //    {
        //        SearchItemInfo searchInfo = new SearchItemInfo(modInfo.ModuleTitle, info.Content, info.CreatedByUserID, info.CreatedDate,
        //                                            modInfo.ModuleID, info.ItemId.ToString(), info.Content, "Item=" + info.ItemId.ToString());
        //        searchItems.Add(searchInfo);
        //    }

        //    return searchItems;
        //}

        #endregion

        #region IPortable Members

        /// <summary>
        /// Exports a module to xml
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        public string ExportModule(int moduleID)
        {
            StringBuilder sb = new StringBuilder();

            //List<PARentalsInfo> infos = GetPARentalss(moduleID);

            //if (infos.Count > 0)
            //{
            //    sb.Append("<PARentalss>");
            //    foreach (PARentalsInfo info in infos)
            //    {
            //        sb.Append("<PARentals>");
            //        sb.Append("<content>");
            //        sb.Append(XmlUtils.XMLEncode(info.Content));
            //        sb.Append("</content>");
            //        sb.Append("</PARentals>");
            //    }
            //    sb.Append("</PARentalss>");
            //}

            return sb.ToString();
        }

        /// <summary>
        /// imports a module from an xml file
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <param name="Content"></param>
        /// <param name="Version"></param>
        /// <param name="UserID"></param>
        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            XmlNode infos = DotNetNuke.Common.Globals.GetContent(Content, "PARentalss");

            foreach (XmlNode info in infos.SelectNodes("PARentals"))
            {
                PARentalsInfo PARentalsInfo = new PARentalsInfo();
                PARentalsInfo.ModuleId = ModuleID;
                PARentalsInfo.Content = info.SelectSingleNode("content").InnerText;
                PARentalsInfo.CreatedByUserID = UserID;

                AddPARentals(PARentalsInfo);
            }
        }

        #endregion
    }
}
