using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Queries
{
    public class EnderecosCliQuery
    {
        public const string GET = @"
          
            SELECT ENDERECOCLIID,
                   CLIENTEID,
                   LOGRADOURO,                   
                   BAIRRO,
                   CIDADE,
                   UF,
                   CEP
              FROM DB_Mecanic.dbo.TBENDERECOSCLI
             WHERE ENDERECOCLIID = @Id;
           
        ";

        public const string PAGINATE = @"
             SELECT ENDERECOCLIID,
                   CLIENTEID,
                   LOGRADOURO,                   
                   BAIRRO,
                   CIDADE,
                   UF,
                   CEP              
              FROM DB_Mecanic.dbo.TBENDERECOSCLI
             WHERE ENDERECOCLIID > @Offset AND ENDERECOCLIID < @Limit AND CLIENTEID = @CLIENTEID
          ORDER BY BAIRRO ASC;             
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
            FROM DB_Mecanic.dbo.TBENDERECOSCLI
            WHERE CLIENTEID = @CLIENTEID;
        ";

        public const string INSERT = @"
            INSERT INTO DB_Mecanic.dbo.TBENDERECOSCLI
                       (CLIENTEID,
                        LOGRADOURO,                   
                        BAIRRO,
                        CIDADE,
                        UF,
                        CEP )
                VALUES (@CLIENTEID,
                        @LOGRADOURO,
                        @BAIRRO,
                        @CIDADE,
                        @UF,
                        @CEP);     
                SELECT @@IDENTITY;                                
        ";

        public const string UPDATE = @"
            UPDATE DB_Mecanic.dbo.TBENDERECOSCLI
               SET CEP = @CEP,
                   CLIENTEID = @CLIENTEID,
                   LOGRADOURO = @LOGRADOURO,
                   BAIRRO = @BAIRRO,
                   CIDADE = @CIDADE,
                   UF = @UF
             WHERE ENDERECOCLIID = @Id;
        ";

        public const string DELETE = @"
             DELETE DB_Mecanic.dbo.TBENDERECOSCLI                                 
             WHERE ENDERECOCLIID = @Id;
        ";
        public const string EXIST_ENDERECO_CEP = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBENDERECOSCLI 
             WHERE CEP = @CEP AND CLIENTEID = @CLIENTEID;
        ";
        public const string EXIST_ENDERECO_ID = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBENDERECOSCLI 
             WHERE ENDERECOCLIID = @ID;
        ";
    }
}
