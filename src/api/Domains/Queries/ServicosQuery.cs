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
                   DATAINCIO,
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
                   DATAINCIO,
                   DATAFIM,
                   SITUACAO,
                   QUILOMETRAGEM,
                   PRECOTOTAL               
              FROM DB_Mecanic.dbo.TBSERVICOS
             WHERE SERVICOID > @Offset AND SERVICOID < @Limit
          ORDER BY DATAINCIO ASC;             
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
            FROM DB_Mecanic.dbo.TBSERVICOS;
        ";

        public const string INSERT = @"
            INSERT INTO DB_Mecanic.dbo.TBSERVICOS
                       (SERVICOID,                   
                        VEICULOID,
                        DATAINCIO,
                        DATAFIM,
                        QUILOMETRAGEM,
                        PRECOTOTAL)
                VALUES (@SERVICOID,
                        @VEICULOID,
                        @DATAINCIO,
                        @DATAFIM,
                        @QUILOMETRAGEM,
                        @PRECOTOTAL);                                 
        ";

        public const string UPDATE = @"
            UPDATE DB_Mecanic.dbo.TBSERVICOS 
               SET VEICULOID = @VEICULOID,                   
                   DATAINCIO = @DATAINCIO,
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
