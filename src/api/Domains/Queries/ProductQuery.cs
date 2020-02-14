using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Queries
{
    public class ProductQuery
    {
        public const string GET = @"
            SELECT IdProduct,
                   CodProduct,
                   ProductName,
                   Quant,
                   Value,
                   ProdType
              FROM DBSERVICE.Product
             WHERE CodProduct = @CodProduct;
        ";
        //AND CreatedBy = @CreatedBy;

        public const string PAGINATE = @"
            SELECT IdProduct,
                   CodProduct,
                   ProductName,
                   Descricao,
                   Quant,
                   Value,
                   ProdType,
                   Active
              FROM DBSERVICE.Product
             WHERE CodProduct > @Offset
             AND CreatedBy = @CreatedBy
          ORDER BY CodProduct ASC
             LIMIT @Limit;
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
              FROM DBSERVICE.Product
             WHERE CreatedBy = @CreatedBy;
        ";

        public const string INSERT = @"
            INSERT INTO DBSERVICE.Product 
                       (IdProduct,
                        CodProduct,
                        ProductName,
                        Descricao,
                        Quant,
                        Value,
                        ProdType,
                        Active,
                        CreatedBy)
                VALUES (@IdProduct,
                        @CodProduct,
                        @ProductName,
                        @Descricao,
                        @Quant,
                        @Value,
                        @ProdType,
                        @Active,
                        @CreatedBy);
                SELECT LAST_INSERT_ID();                  
        ";

        public const string UPDATE = @"
            UPDATE DBSERVICE.Product 
               SET IdProduct = @IdProduct,
                   CodProduct = @CodProduct,
                   ProductName = @ProductName,
                   Descricao = @Descricao,
                   Quant = @Quant,
                   Value = @Value,
                   ProdType = @ProdType ,
                   Active = @Active,
                   CreatedBy = @CreatedBy
             WHERE CodProduct = @CodProduct;
        ";

        public const string DELETE = @"
            DELETE FROM DBSERVICE.Product
                  WHERE IdProduct = @IdProduct;
        ";
        public const string EXIST_PRODUCT = @"
            SELECT count(1) 
              FROM DBSERVICE.Product
             WHERE CodProduct = @CodProduct;
        ";
    }
}
