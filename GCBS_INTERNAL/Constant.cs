using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCBS_INTERNAL
{
    public class Constant
    {
        public const string image = "data:image/png;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAMCAgICAgMCAgIDAwMDBAYEBAQEBAgGBgUGCQgKCgkICQkKDA8MCgsOCwkJDRENDg8QEBEQCgwSExIQEw8QEBD/2wBDAQMDAwQDBAgEBAgQCwkLEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBD/wAARCAAgACADASIAAhEBAxEB/8QAGAABAQEBAQAAAAAAAAAAAAAACQoIBgf/xAAnEAACAQMDBAICAwAAAAAAAAABAgMEBREGBwgACRITITEUQVJhYv/EABQBAQAAAAAAAAAAAAAAAAAAAAD/xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwBPHdI0aSRwqICzMxwAB9k9T+8ueam6XJLcG7TR6puls0RBUyw2WxU1Q0MC0wPiskypj2yuB5MX8sFiq4UY6oAnghqYJKaojWSKVCjowyGUjBB/rHU6vKfjHr3jDubctI6mtFWbJJVStYbz6W/GuNLnKMr/AF7FUqHTOVbP2CCQ6riNzU3S427g2maTVN0ueiJ6mKG9WKpqGmgamJ8WkhV8+qVAfJSnjkqFbKnHVASOkiLJG4ZHAZWU5BB+iOp0+LHGPXvJ7c226R0zaKsWSOqia/Xn0t+NbqXOXZn+vYyhgiZyzY+gCRRVBBDTQR01PGscUSBERRgKoGAB/WOgPXuFdx6+7H6mm2S2NNOmrqNY5L3eqmnSeO3eyMOkEMb5V5SrIzM4KqCFwzE+GJNB9y/lPpEV1JqfUNo3CtlxcyTW7V1uWsgVifnwMZjdR+gnl4D9KOkO59dvKk5MyLubtlVUNo3DpYVgqVqiY6W8wIMIsjKD4TIBhZMEEYVsAKyEVuRx53w2iudVatxdrdR2Z6Nirzy0LvSsP5JUIDFIv+lYjoPZ9edy/lPq4UNJpjUNo29tlucSQ27SNuWjgZgfjzMhkdh+inl4H9qett9vXuPX7fDU8OyO+Ip5NW1qSSWW9UtOkEdw9cZeSGaNMKkoVHZWQBWAIwrAeZfbbcdt8d3rnS2rbra3Ud4erYBKiOheOlUfyeocCKNf9MwHS78Be3rR8Yy+5W5VXQ3jcSshaCAUpL0tmgcYdImYAvK4+HkwABlF+CzOH//Z";

        public const string DATE_FORMAT = "dd-MM-yyyy";
        public const string SERVICE_PROVIDER = "Service Partner";
        public const string CUSTOMER = "Customer";
        public const string PENDING = "Pending";
        public const string ACCEPTED = "Accepted";
        public const string PROCESSING = "Processing";
        public const string COMPLETED = "Completed";
        public const string DECLINE = "Decline";
        public const string CANCEL = "Cancel";
        public const string ONLINE = "Online";
        public const string OFFLINE = "Offline";

        //Folder
        public const string SUPER_ADMIN_FLODER_TYPE = "Admin";
        public const string SERVICE_PROVIDER_FOLDER_TYPE = "ServiceProvider";
        public const string SERVICE_MANAGER_FOLDER_TYPE = "Manager";
        public const string SUPPORT_TEAM_FOLDER_TYPE = "Support";
        public const string HUMAN_RESOURCE_FOLDER_TYPE = "Hr";
        public const string CUSTOMER_FOLDER_TYPE = "Customer";
        public const string AGENCIES_FOLDER_TYPE = "Agencies";
        public const string HOTEL_FOLDER_TYPE = "Hotels";
        public const string SITE_BANNER_FOLDER_TYPE = "SiteBanner";

        public const int PARTNER_ROLE_ID = 3;
        public const int CUSTOMER_ROLE_ID = 9;
        public const int ExpireTime = 15;

        public const int CUSTOMER_BOOKING_STATUS_BOOKED = 0;
      
        public const int PARTNER_BOOKING_STATUS_OPENED = 1;
        public const int PARTNER_BOOKING_STATUS_CLOSED = 2;
        public const int PARTNER_BOOKING_STATUS_COMPLETED = 3;
        public const int PARTNER_BOOKING_STATUS_REJECTED = 4;

       
        public const int CUSTOMER_BOOKING_STATUS_OPENED = 1;
        public const int CUSTOMER_BOOKING_STATUS_CLOSED = 2;
        public const int CUSTOMER_BOOKING_STATUS_COMPLETED = 3;
        public const int CUSTOMER_BOOKING_STATUS_REJECTED = 4;

        public const string PAYMENT_STATUS_PENDING_STR = "PENDING";
        public const int PAYMENT_STATUS_PENDING = 1;

        public const string PAYMENT_STATUS_COMPLETED_STR = "COMPLETED";
        public const int PAYMENT_STATUS_COMPLETED = 2;

        public const string PAYMENT_STATUS_REJECTED_STR = "REJECTED";
        public const int PAYMENT_STATUS_REJECTED = 3;

        /// <summary>
        /// Booking time intervel minutes
        /// </summary>
        public const int BOOKING_TIME_INTERVEL = 30;




    }
}