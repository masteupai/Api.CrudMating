using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Queries
{
    public class EnderecosFunQuery
    {

        public const string GET = @"
          
            SELECT ENDERECOFUNID,
                   FUNCIONARIOID,
                   LOGRADOURO,                   
                   BAIRRO,
                   CIDADE,
                   UF,
                   CEP
              FROM DB_Mecanic.dbo.TBENDERECOSFUN
             WHERE ENDERECOFUNID = @Id;
           
        ";

        public const string PAGINATE = @"
             SELECT ENDERECOFUNID,
                   FUNCIONARIOID,
                   LOGRADOURO,                   
                   BAIRRO,
                   CIDADE,
                   UF,
                   CEP              
              FROM DB_Mecanic.dbo.TBENDERECOSFUN
             WHERE ENDERECOFUNID > @Offset AND ENDERECOFUNID < @Limit AND FUNCIONARIOID = @FUNCIONARIOID
          ORDER BY BAIRRO ASC;             
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
            FROM DB_Mecanic.dbo.TBENDERECOSFUN
            WHERE FUNCIONARIOID = @FUNCIONARIOID;
        ";

        public const string INSERT = @"
            INSERT INTO DB_Mecanic.dbo.TBENDERECOSFUN
                       (FUNCIONARIOID,
                        LOGRADOURO,                   
                        BAIRRO,
                        CIDADE,
                        UF,
                        CEP )
                VALUES (@FUNCIONARIOID,
                        @LOGRADOURO,
                        @BAIRRO,
                        @CIDADE,
                        @UF,
                        @CEP);       
                SELECT @@IDENTITY;                              
        ";

        public const string UPDATE = @"
            UPDATE DB_Mecanic.dbo.TBENDERECOSFUN
               SET CEP = @CEP,
                   FUNCIONARIOID = @FUNCIONARIOID,
                   LOGRADOURO = @LOGRADOURO,
                   BAIRRO = @BAIRRO,
                   CIDADE = @CIDADE,
                   UF = @UF
             WHERE ENDERECOFUNID = @Id;
        ";

        public const string DELETE = @"
             DELETE DB_Mecanic.dbo.TBENDERECOSFUN                                 
             WHERE ENDERECOFUNID = @Id;
        ";
        public const string EXIST_ENDERECO_CEP = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBENDERECOSFUN 
             WHERE CEP = @CEP AND FUNCIONARIOID = @FUNCIONARIOID;
        ";
        public const string EXIST_ENDERECO_ID = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBENDERECOSFUN 
             WHERE ENDERECOFUNID = @ID;
        ";
    }
}
