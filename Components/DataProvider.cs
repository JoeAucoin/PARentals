using System;
using System.Data;
using DotNetNuke;
using DotNetNuke.Framework;

namespace GIBS.PARentals.Components
{
    public abstract class DataProvider
    {

        #region common methods

        /// <summary>
        /// var that is returned in the this singleton
        /// pattern
        /// </summary>
        private static DataProvider instance = null;

        /// <summary>
        /// private static cstor that is used to init an
        /// instance of this class as a singleton
        /// </summary>
        static DataProvider()
        {
            instance = (DataProvider)Reflection.CreateObject("data", "GIBS.PARentals.Components", "");
        }

        /// <summary>
        /// Exposes the singleton object used to access the database with
        /// the conrete dataprovider
        /// </summary>
        /// <returns></returns>
        public static DataProvider Instance()
        {
            return instance;
        }

        #endregion


        #region Abstract methods


        public abstract IDataReader Rentals_GetActiveList(int moduleId, int customAddressID, int customCityID, int customBedroomsID, int customBathroomsID, int customRentalAmountID);


        public abstract void Rentals_Booking_Insert(int moduleID, int propertyID, DateTime dateStart, DateTime dateEnd, double rentalAmount, string status, int createdByUserID);

        public abstract IDataReader Rentals_Booking_CheckStatus(int propertyID, DateTime dateStart);

        public abstract IDataReader Rentals_GetUsersByRoleName(int portalID, string roleName);

        public abstract void Rentals_Reservation_Insert(int moduleID, int propertyID, int tenantID, DateTime dateStart, DateTime dateEnd, double rentalAmount, int agentID, string status, int createdByUserID, double depositAmt);

        public abstract IDataReader Rentals_Get_Reservation(int reservationID);

        public abstract void Rentals_Update_Reservation_Payment(int reservationID, double depositAmt, DateTime depositPaidDate, DateTime balancePaidDate, double balanceAmt, string notes, int updatedByUserID, string status, bool leasePrint);

        public abstract void Rentals_Booking_Update_Status(int bookingID, string status, int updatedByUserID, double rentalAmount);

        public abstract IDataReader Rentals_Get_PropertyDetails(int propertyID);

        public abstract IDataReader Rentals_Get_PropertyOwner(int propertyID, int portalID);

        public abstract void Rentals_Reservation_Delete(int reservationID, int updatedByUserID);

        /* implement the methods that the dataprovider should */

        
        
        public abstract void AddPARentals(int moduleId, string content, int userId);
        public abstract void UpdatePARentals(int moduleId, int itemId, string content, int userId);
        public abstract void DeletePARentals(int moduleId, int itemId);

        #endregion

    }



}
