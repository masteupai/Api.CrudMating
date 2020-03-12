using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Queries
{
    public class ServicosQuery
    {
        public const string GET = @"
          
            SELECT SERVICOID,
                   VEICULOID,                   
                   DATAINICIO,
                   DATAFIM,
                   SITUACAO,
                   QUILOMETRAGEM,
                   PRECOTOTAL
              FROM DB_Mecanic.dbo.TBSERVICOS
             WHERE SERVICOID = @Id;           
        ";

        public const string PAGINATE = @"
            SELECT SERVICOID,
                   VEICULOID,                   
                   DATAINICIO,
                   DATAFIM,
                   SITUACAO,
                   QUILOMETRAGEM,
                   PRECOTOTAL               
              FROM DB_Mecanic.dbo.TBSERVICOS
             WHERE SERVICOID > @Offset AND SERVICOID < @Limit
          ORDER BY DATAINICIO ASC;             
        ";
        public const string PAGINATEPERVEICULO = @"
            SELECT SERVICOID,
                   VEICULOID,                   
                   DATAINICIO,
                   DATAFIM,
                   SITUACAO,
                   QUILOMETRAGEM,
                   PRECOTOTAL               
              FROM DB_Mecanic.dbo.TBSERVICOS
             WHERE SERVICOID > @Offset AND SERVICOID < @Limit AND VEICULOID = @VEICULOID
          ORDER BY DATAINICIO ASC;             
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
            FROM DB_Mecanic.dbo.TBSERVICOS;
        ";

        public const string INSERT = @"
            INSERT INTO DB_Mecanic.dbo.TBSERVICOS
                       (VEICULOID,
                        DATAINICIO,
                        DATAFIM,
                        QUILOMETRAGEM,
                        PRECOTOTAL)
                VALUES (@VEICULOID,
                        @DATAINICIO,
                        @DATAFIM,
                        @QUILOMETRAGEM,
                        @PRECOTOTAL); 
               SELECT @@IDENTITY;
        ";

        public const string UPDATE = @"
            UPDATE DB_Mecanic.dbo.TBSERVICOS 
               SET VEICULOID = @VEICULOID,                   
                   DATAINICIO = @DATAINICIO,
                   DATAFIM = @DATAFIM,
                   QUILOMETRAGEM = @QUILOMETRAGEM,
                   PRECOTOTAL = @PRECOTOTAL
             WHERE SERVICOID = @Id;
        ";

        public const string DELETE = @"
             DELETE DB_Mecanic.dbo.TBSERVICOS 
             WHERE SERVICOID = @Id;
        ";

        public const string EXIST_SERVICO_ID = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBSERVICOS 
             WHERE SERVICOID = @Id;
        ";       
    }
}
