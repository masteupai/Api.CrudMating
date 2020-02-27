using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Queries
{
    public class ProductQuery
    {
        public const string GET = @"
          
            SELECT PRODUTOID,
                   CODIGO,
                   NOME,
                   DESCRICAO,
                   QUANTIDADE,
                   VALOR,
                   TIPO,
                   ATIVO
              FROM DB_Mecanic.dbo.TBPRODUTOS
             WHERE CODIGO = @CodProduct;
           
        ";   

        public const string PAGINATE = @"
            SELECT PRODUTOID,
                   CODIGO,
                   NOME,
                   DESCRICAO,
                   QUANTIDADE,
                   VALOR,
                   TIPO,
                   ATIVO
              FROM DB_Mecanic.dbo.TBPRODUTOS
             WHERE CODIGO > @Offset AND CODIGO < @Limit
          ORDER BY CODIGO ASC;             
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
            FROM DB_Mecanic.dbo.TBPRODUTOS;
        ";

        public const string INSERT = @"
            INSERT INTO DB_Mecanic.dbo.TBPRODUTOS
                       (CODIGO,
                        NOME,
                        DESCRICAO,
                        QUANTIDADE,
                        VALOR,
                        TIPO,
                        ATIVO))
                VALUES (@CodProduct,
                        @ProductName,
                        @Descricao,
                        @Quant,
                        @Value,
                        @ProdType,
                        @Active);                                 
        ";

        public const string UPDATE = @"
            UPDATE DB_Mecanic.dbo.TBPRODUTOS 
               SET CODIGO = @CodProduct,
                   NOME = @ProductName,
                   DESCRICAO = @Descricao,
                   QUANTIDADE = @Quant,
                   VALOR = @Value,
                   TIPO = @ProdType ,
                   ATIVO = @Active                   
             WHERE CODIGO = @CodProduct;
        ";

        public const string DELETE = @"
            DELETE FROM DB_Mecanic.dbo.TBPRODUTOS 
                  WHERE CODIGO = @CodProduct;
        ";
        public const string EXIST_PRODUCT = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBPRODUTOS 
             WHERE CODIGO = @CodProduct;
        ";
    }
}
