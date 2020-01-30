using System;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

namespace GIBS.PARentals.Components
{
    public class PARentalsInfo
    {
        //private vars exposed thro the
        //properties
        private int moduleId;
        private int itemId;
        private string content;
        private int createdByUserID;
        private int updatedByUserID;
        private DateTime createdDate;
        private string createdByUserName = null;

        private int propertyID;
        private int propertyTypeID;
        private string propertyAddress;
        private string address;
        private string propertyCity;
        private string village;

        private int bedrooms;
        private int bathrooms;

        private int fullBaths;
        private int halfBaths;

        private DateTime dateStart;
        private DateTime dateEnd;
        private double rentalAmount;
        private double price;
        private double securityDeposit;
        private string status;

        private string fullName;
        private string email;
        private string telephone;

        private int bookingID;
        private int reservationID;
        private int agentID;
        private int tenantID;

        private double depositAmt = 0;
        private DateTime depositPaidDate;
        private double balanceAmt = 0;
        private DateTime balancePaidDate;
        private string notes;

        private bool leasePrint;

        /// <summary>
        /// empty cstor
        /// </summary>
        public PARentalsInfo()
        {
        }


        #region properties

        public bool LeasePrint
        {
            get { return leasePrint; }
            set { leasePrint = value; }
        }

        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public double DepositAmt
        {
            get { return depositAmt; }
            set { depositAmt = value; }
        }

        public DateTime DepositPaidDate
        {
            get { return depositPaidDate; }
            set { depositPaidDate = value; }
        }

        public DateTime BalancePaidDate
        {
            get { return balancePaidDate; }
            set { balancePaidDate = value; }
        }

        public double BalanceAmt
        {
            get { return balanceAmt; }
            set { balanceAmt = value; }
        }


        public int AgentID
        {
            get { return agentID; }
            set { agentID = value; }
        }

        public int TenantID
        {
            get { return tenantID; }
            set { tenantID = value; }
        }



        public int BookingID
        {
            get { return bookingID; }
            set { bookingID = value; }
        }

        public int ReservationID
        {
            get { return reservationID; }
            set { reservationID = value; }
        }	

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }	


        public DateTime DateStart
        {
            get { return dateStart; }
            set { dateStart = value; }
        }

        public DateTime DateEnd
        {
            get { return dateEnd; }
            set { dateEnd = value; }
        }

        public double RentalAmount
        {
            get { return rentalAmount; }
            set { rentalAmount = value; }
        }
        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        //
        public double SecurityDeposit
        {
            get { return securityDeposit; }
            set { securityDeposit = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
    
        public int PropertyID
        {
            get { return propertyID; }
            set { propertyID = value; }
        }

        public int PropertyTypeID
        {
            get { return propertyTypeID; }
            set { propertyTypeID = value; }
        }

        public string PropertyAddress
        {
            get { return propertyAddress; }
            set { propertyAddress = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }


        public string Village
        {
            get { return village; }
            set { village = value; }
        }
        public string PropertyCity
        {
            get { return propertyCity; }
            set { propertyCity = value; }
        }


        public int Bedrooms
        {
            get { return bedrooms; }
            set { bedrooms = value; }
        }
        
        public int Bathrooms
        {
            get { return bathrooms; }
            set { bathrooms = value; }
        }

        public int FullBaths
        {
            get { return fullBaths; }
            set { fullBaths = value; }
        }

        public int HalfBaths
        {
            get { return halfBaths; }
            set { halfBaths = value; }
        }

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public int CreatedByUserID
        {
            get { return createdByUserID; }
            set { createdByUserID = value; }
        }

        public int UpdatedByUserID
        {
            get { return updatedByUserID; }
            set { updatedByUserID = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public string CreatedByUserName
        {
            get
            {
                if (createdByUserName == null)
                {
                    //int portalId = PortalController.GetCurrentPortalSettings().PortalId;
                    //UserInfo user = UserController.GetUser(portalId, createdByUser, false);
                    //createdByUserName = user.DisplayName;

                    createdByUserName = "";
                }

                return createdByUserName;
            }
        }



        private string ownerFirstName;
        public string OwnerFirstName
        {
            get { return ownerFirstName; }
            set { ownerFirstName = value; }
        }

        private string ownerLastName;
        public string OwnerLastName
        {
            get { return ownerLastName; }
            set { ownerLastName = value; }
        }

        private string ownerEmail;
        public string OwnerEmail
        {
            get { return ownerEmail; }
            set { ownerEmail = value; }
        }

        private string ownerTelephone;
        public string OwnerTelephone
        {
            get { return ownerTelephone; }
            set { ownerTelephone = value; }
        }

        private string ownerCell;
        public string OwnerCell
        {
            get { return ownerCell; }
            set { ownerCell = value; }
        }

        private string ownerAddress;
        public string OwnerAddress
        {
            get { return ownerAddress; }
            set { ownerAddress = value; }
        }

        private string ownerCity;
        public string OwnerCity
        {
            get { return ownerCity; }
            set { ownerCity = value; }
        }

        private string ownerState;
        public string OwnerState
        {
            get { return ownerState; }
            set { ownerState = value; }
        }

        private string ownerZip;
        public string OwnerZip
        {
            get { return ownerZip; }
            set { ownerZip = value; }
        }


        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }



        private string cell;
        public string Cell
        {
            get { return cell; }
            set { cell = value; }
        }

        private string street;
        public string Street
        {
            get { return street; }
            set { street = value; }
        }

        private string city;
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        private string region;
        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        private string postalCode;
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }	

        #endregion
    }
}
