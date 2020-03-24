using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Queries
{
    public class ContatoFuncionarioQuery
    {

        public const string GET = @"
          
            SELECT CONTATOFUNID,
                   FUNCIONARIOID,
                   TELEFONE,                   
                   EMAIL
              FROM DB_Mecanic.dbo.TBCONTATOSFUN
             WHERE CONTATOFUNID = @Id;
           
        ";

        public const string PAGINATE = @"
            SELECT CONTATOFUNID,                   
                   EMAIL,
                   FUNCIONARIOID,
                   TELEFONE              
              FROM DB_Mecanic.dbo.TBCONTATOSFUN
             WHERE CONTATOFUNID > @Offset AND CONTATOFUNID < @Limit AND FUNCIONARIOID = @FUNCIONARIOID
          ORDER BY EMAIL ASC;             
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
            FROM DB_Mecanic.dbo.TBCONTATOSFUN;
        ";

        public const string INSERT = @"
            INSERT INTO DB_Mecanic.dbo.TBCONTATOSFUN
                       (FUNCIONARIOID,
                        EMAIL,
                        TELEFONE)
                VALUES (@FUNCIONARIOID,
                        @EMAIL,
                        @TELEFONE);       
                SELECT @@IDENTITY;                              
        ";

        public const string UPDATE = @"
            UPDATE DB_Mecanic.dbo.TBCONTATOSFUN
               SET EMAIL = @EMAIL,
                   FUNCIONARIOID = @FUNCIONARIOID,
                   TELEFONE = @TELEFONE
             WHERE CONTATOFUNID = @Id;
        ";

        public const string DELETE = @"
             DELETE DB_Mecanic.dbo.TBCONTATOSFUN                                 
             WHERE CONTATOFUNID = @Id;
        ";

        public const string ATIVAR = @"
             UPDATE DB_Mecanic.dbo.TBCONTATOSFUN 
               SET ATIVO = 1                   
             WHERE FUNCIONARIOID = @Id;
        ";

        public const string EXIST_CONTATOFUN_TELEFONE = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBCONTATOSFUN 
             WHERE TELEFONE = @TELEFONE;
        ";
        public const string EXIST_CONTATOFUN_EMAIL = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBCONTATOSFUN 
             WHERE EMAIL = @EMAIL;
        ";
        public const string EXIST_CONTATOFUN_ID = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBCONTATOSFUN 
             WHERE CONTATOFUNID = @ID;
        ";
    }
}
