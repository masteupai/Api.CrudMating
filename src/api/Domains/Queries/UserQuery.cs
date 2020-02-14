using System.Runtime.Serialization;

namespace API.Domains.Queries
{
    public static class UserQuery 
    {
        public const string GET = @"
            SELECT IdProfile,
                   IdCountry,
                   CreatedBy,
                   Name,
                   Email,
                   Document,
                   Birthdate,
                   Profile,
                   Active
              FROM DBSERVICE.User
             WHERE IdProfile = @Id;
        ";
              //AND CreatedBy = @CreatedBy;

        public const string PAGINATE = @"
            SELECT IdProfile,
                   IdCountry,
                   CreatedBy,
                   Name,
                   Email,
                   Document,
                   Birthdate,
                   Profile,
                   Active
              FROM DBSERVICE.User
             WHERE IdProfile > @Offset
               AND CreatedBy = @CreatedBy
          ORDER BY IdProfile ASC
             LIMIT @Limit;
        ";
        
        public const string TOTAL = @"
            SELECT COUNT(1)
              FROM DBSERVICE.User
             WHERE CreatedBy = @CreatedBy;
        ";

        public const string INSERT = @"
            INSERT INTO DBSERVICE.User 
                       (IdProfile,
                        IdCountry,
                        CreatedBy,
                        Name,
                        Email,
                        Document,
                        Birthdate,
                        Profile,
                        Active)
                VALUES (@IdProfile,
                        @IdCountry,
                        @CreatedBy,
                        @Name,
                        @Email,
                        @Document,
                        @Birthdate,
                        @Profile,
                        @Active);

            SELECT LAST_INSERT_ID();                  
        ";
        
        public const string UPDATE = @"
            UPDATE DBSERVICE.User 
               SET IdProfile = @IdProfile,
                   IdCountry = @IdCountry,
                   Name = @Name,
                   Email = @Email,
                   Birthdate = @Birthdate,
                   Profile = @Profile,
                   Active = @Active
             WHERE IdProfile = @IdProfile;
        ";

        public const string DELETE = @"
            DELETE FROM DBSERVICE.User
                  WHERE IdProfile = @Id
                    AND CreatedBy = @CreatedBy;
        ";

        public const string ACTIVATE_DEACTIVATE = @"
            UPDATE DBSERVICE.User 
               SET Active = @Active
             WHERE IdProfile = @Id
               AND CreatedBy = @CreatedBy;
        ";

        public const string EXISTS_EMAIL = @"
            SELECT count(1) 
              FROM DBSERVICE.User
             WHERE Email = @Email;
        ";

        public const string EXISTS_SAME_EMAIL = @"
            SELECT count(1) 
              FROM DBSERVICE.User
             WHERE Email = @Email
               AND IdProfile != @Id;
        ";

        public const string EXISTS_DOCUMENT = @"
            SELECT count(1) 
              FROM DBSERVICE.User
             WHERE Document = @Document;
        ";
    }
}
