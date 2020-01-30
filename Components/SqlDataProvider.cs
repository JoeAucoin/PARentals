using System;
using System.Data;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace GIBS.PARentals.Components
{
    public class SqlDataProvider : DataProvider
    {


        #region vars

        private const string providerType = "data";
        private const string moduleQualifier = "GIBS_";

        private ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);
        private string connectionString;
        private string providerPath;
        private string objectQualifier;
        private string databaseOwner;

        #endregion

        #region cstor

        /// <summary>
        /// cstor used to create the sqlProvider with required parameters from the configuration
        /// section of web.config file
        /// </summary>
        public SqlDataProvider()
        {
            Provider provider = (Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider];
            connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString();

            if (connectionString == string.Empty)
                connectionString = provider.Attributes["connectionString"];

            providerPath = provider.Attributes["providerPath"];

            objectQualifier = provider.Attributes["objectQualifier"];
            if (objectQualifier != string.Empty && !objectQualifier.EndsWith("_"))
                objectQualifier += "_";

            databaseOwner = provider.Attributes["databaseOwner"];
            if (databaseOwner != string.Empty && !databaseOwner.EndsWith("."))
                databaseOwner += ".";
        }

        #endregion

        #region properties

        public string ConnectionString
        {
            get { return connectionString; }
        }


        public string ProviderPath
        {
            get { return providerPath; }
        }

        public string ObjectQualifier
        {
            get { return objectQualifier; }
        }


        public string DatabaseOwner
        {
            get { return databaseOwner; }
        }

        #endregion

        #region private methods

        private string GetFullyQualifiedName(string name)
        {
            return DatabaseOwner + ObjectQualifier + moduleQualifier + name;
        }

        private object GetNull(object field)
        {
            return DotNetNuke.Common.Utilities.Null.GetNull(field, DBNull.Value);
        }

        #endregion

        #region override methods


        public override IDataReader Rentals_GetActiveList(int moduleId, int customAddressID, int customCityID, int customBedroomsID, int customBathroomsID, int customRentalAmountID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("Rentals_GetActiveList"), moduleId, customAddressID, customCityID, customBedroomsID, customBathroomsID, customRentalAmountID);
        }


        public override void Rentals_Booking_Insert(int moduleID, int propertyID, DateTime dateStart, DateTime dateEnd, double rentalAmount, string status, int createdByUserID)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("Rentals_Booking_Insert"), moduleID, propertyID, dateStart, dateEnd, rentalAmount, status, createdByUserID);
        }

        public override IDataReader Rentals_Booking_CheckStatus(int propertyID, DateTime dateStart)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("Rentals_Booking_CheckStatus"), propertyID, dateStart);
        }

        public override IDataReader Rentals_GetUsersByRoleName(int portalID, string roleName)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("Rentals_GetUsersByRoleName"), portalID, roleName);
        }

        public override void Rentals_Reservation_Insert(int moduleID, int propertyID, int tenantID, DateTime dateStart, DateTime dateEnd, double rentalAmount, int agentID, string status, int createdByUserID, double depositAmt)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("Rentals_Reservation_Insert"), moduleID, propertyID, tenantID, dateStart, dateEnd, rentalAmount, agentID, status, createdByUserID, depositAmt);
        }

        public override IDataReader Rentals_Get_Reservation(int reservationID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("Rentals_Get_Reservation"), reservationID);
        }

        public override void Rentals_Update_Reservation_Payment(int reservationID, double depositAmt, DateTime depositPaidDate, DateTime balancePaidDate, double balanceAmt, string notes, int updatedByUserID, string status, bool leasePrint)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("Rentals_Update_Reservation_Payment"), reservationID, depositAmt, depositPaidDate, balancePaidDate, balanceAmt, notes, updatedByUserID, status, leasePrint);
        }

        public override void Rentals_Booking_Update_Status(int bookingID, string status, int updatedByUserID, double rentalAmount)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("Rentals_Booking_Update_Status"), bookingID, status, updatedByUserID, rentalAmount);
        }

        public override IDataReader Rentals_Get_PropertyDetails(int propertyID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("Rentals_Get_PropertyDetails"), propertyID);
        }

        public override IDataReader Rentals_Get_PropertyOwner(int propertyID, int portalID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("Rentals_Get_PropertyOwner"), propertyID, portalID);
        }

        public override void Rentals_Reservation_Delete(int reservationID, int updatedByUserID)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("Rentals_Reservation_Delete"), reservationID, updatedByUserID);
        }	



        /// <summary>
        /// ////////////////////
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>


        public override void AddPARentals(int moduleId, string content, int userId)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("AddPARentals"), moduleId, content, userId);
        }

        public override void UpdatePARentals(int moduleId, int itemId, string content, int userId)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("UpdatePARentals"), moduleId, itemId, content, userId);
        }

        public override void DeletePARentals(int moduleId, int itemId)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DeletePARentals"), moduleId, itemId);
        }

        #endregion
    }
}
