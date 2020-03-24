using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Queries
{
    public class ServicoProdutoQuery
    {
        public const string GET = @"          
            SELECT SERVICOID,
                   SERVICOPRODUTOID,                   
                   PRODUTOID,
                   QUANTIDADE
              FROM DB_Mecanic.dbo.TBSERVICOPRODUTOS
             WHERE SERVICOPRODUTOID = @Id;           
        ";

        public const string PAGINATE = @"
            SELECT SERVICOID,
                   SERVICOPRODUTOID,                   
                   PRODUTOID,
                   QUANTIDADE             
              FROM DB_Mecanic.dbo.TBSERVICOPRODUTOS
             WHERE SERVICOPRODUTOID > @Offset AND SERVICOPRODUTOID < @Limit AND SERVICOID = @SERVICOID
          ORDER BY SERVICOPRODUTOID ASC;             
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
            FROM DB_Mecanic.dbo.TBSERVICOPRODUTOS
            WHERE SERVICOID = @SERVICOID;
        ";

        public const string INSERT = @"
            INSERT INTO DB_Mecanic.dbo.TBSERVICOPRODUTOS
                       (SERVICOID,                  
                        PRODUTOID,
                        QUANTIDADE)
                VALUES (@SERVICOID,
                        @PRODUTOID,
                        @QUANTIDADE); 
               SELECT @@IDENTITY;
        ";

        public const string UPDATE = @"
            UPDATE DB_Mecanic.dbo.TBSERVICOPRODUTOS 
               SET SERVICOID = @SERVICOID,                   
                   PRODUTOID = @PRODUTOID,
                   QUANTIDADE = @QUANTIDADE
             WHERE SERVICOPRODUTOID = @Id;
        ";

        public const string DELETE = @"
             DELETE DB_Mecanic.dbo.TBSERVICOPRODUTOS 
             WHERE SERVICOPRODUTOID = @Id;
        ";

        public const string EXIST_SERVICO_ID = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBSERVICOPRODUTOS 
             WHERE SERVICOPRODUTOID = @Id;
        ";
        public const string EXIST_SERVICO_PRODUTOID = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBSERVICOPRODUTOS 
             WHERE PRODUTOID = @Id;
        ";
    }
}
