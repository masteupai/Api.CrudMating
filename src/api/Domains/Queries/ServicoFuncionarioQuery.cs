using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Queries
{
    public class ServicoFuncionarioQuery
    {
        public const string GET = @"          
            SELECT SERVICOID,
                   SERVICOFUNCIONARIOID,                   
                   FUNCIONARIOID,
                   COMISSAO
              FROM DB_Mecanic.dbo.TBSERVICOFUNCIONARIOS
             WHERE SERVICOFUNCIONARIOID = @Id;           
        ";

        public const string PAGINATE = @"
            SELECT SERVICOID,
                   SERVICOFUNCIONARIOID,                   
                   FUNCIONARIOID,
                   COMISSAO               
              FROM DB_Mecanic.dbo.TBSERVICOFUNCIONARIOS
             WHERE SERVICOFUNCIONARIOID > @Offset AND SERVICOFUNCIONARIOID < @Limit AND SERVICOID = @SERVICOID
          ORDER BY SERVICOFUNCIONARIOID ASC;             
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
            FROM DB_Mecanic.dbo.TBSERVICOFUNCIONARIOS
            WHERE SERVICOID = @SERVICOID;
        ";

        public const string INSERT = @"
            INSERT INTO DB_Mecanic.dbo.TBSERVICOFUNCIONARIOS
                       (SERVICOID,                  
                        FUNCIONARIOID,
                        COMISSAO)
                VALUES (@SERVICOID,
                        @FUNCIONARIOID,
                        @COMISSAO); 
               SELECT @@IDENTITY;
        ";

        public const string UPDATE = @"
            UPDATE DB_Mecanic.dbo.TBSERVICOFUNCIONARIOS 
               SET SERVICOID = @SERVICOID,                   
                   FUNCIONARIOID = @FUNCIONARIOID,
                   COMISSAO = @COMISSAO
             WHERE SERVICOFUNCIONARIOID = @Id;
        ";

        public const string DELETE = @"
             DELETE DB_Mecanic.dbo.TBSERVICOFUNCIONARIOS 
             WHERE SERVICOFUNCIONARIOID = @Id;
        ";

        public const string EXIST_SERVICO_ID = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBSERVICOFUNCIONARIOS 
             WHERE SERVICOFUNCIONARIOID = @Id;
        "; 
        public const string EXIST_SERVICO_FUNCIONARIOID = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBSERVICOFUNCIONARIOS 
             WHERE FUNCIONARIOID = @Id;
        ";
    }
}
