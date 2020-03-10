using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Queries
{
    public class ContatoClienteQuery
    {
        public const string GET = @"
          
            SELECT CONTATOCLIID,
                   CLIENTEID,
                   TELEFONE,                   
                   EMAIL
              FROM DB_Mecanic.dbo.TBCONTATOSCLI
             WHERE CONTATOCLIID = @Id;
           
        ";

        public const string PAGINATE = @"
            SELECT CONTATOCLIID,                   
                   EMAIL,
                   CLIENTEID,
                   TELEFONE              
              FROM DB_Mecanic.dbo.TBCONTATOSCLI
             WHERE CONTATOCLIID > @Offset AND CONTATOCLIID < @Limit AND CLIENTEID = @CLIENTEID
          ORDER BY EMAIL ASC;             
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
            FROM DB_Mecanic.dbo.TBCONTATOSCLI;
        ";

        public const string INSERT = @"
            INSERT INTO DB_Mecanic.dbo.TBCONTATOSCLI
                       (CLIENTEID,
                        EMAIL,
                        TELEFONE)
                VALUES (@CLIENTEID,
                        @EMAIL,
                        @TELEFONE);                                 
        ";

        public const string UPDATE = @"
            UPDATE DB_Mecanic.dbo.TBCONTATOSCLI
               SET EMAIL = @EMAIL,
                   CLIENTEID = @CLIENTEID,
                   TELEFONE = @TELEFONE
             WHERE CONTATOCLIID = @Id;
        ";

        public const string DELETE = @"
             DELETE DB_Mecanic.dbo.TBCONTATOSCLI                                 
             WHERE CONTATOCLIID = @Id;
        ";

        public const string ATIVAR = @"
             UPDATE DB_Mecanic.dbo.TBCONTATOSCLI 
               SET ATIVO = 1                   
             WHERE CLIENTEID = @Id;
        ";

        public const string EXIST_CONTATOCLIENTE_TELEFONE = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBCONTATOSCLI 
             WHERE TELEFONE = @TELEFONE;
        ";
        public const string EXIST_CONTATOCLIENTE_EMAIL = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBCONTATOSCLI 
             WHERE EMAIL = @EMAIL;
        "; 
        public const string EXIST_CONTATOCLIENTE_ID = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBCONTATOSCLI 
             WHERE CONTATOCLIID = @ID;
        ";

    }
}
