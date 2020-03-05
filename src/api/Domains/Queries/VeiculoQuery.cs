using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Queries
{
    public class VeiculoQuery
    {

        public const string GET = @"
          
            SELECT VEICULOID,
                   CLIENTEID,                   
                   MODELO,
                   MARCA,
                   ANO,
                   PLACA,
                   COR
              FROM DB_Mecanic.dbo.TBVEICULOS
             WHERE VEICULOID = @Id;           
        ";

        public const string PAGINATE = @"
            SELECT VEICULOID,
                   CLIENTEID,                   
                   MODELO,
                   MARCA,
                   ANO,
                   PLACA,
                   COR               
              FROM DB_Mecanic.dbo.TBVEICULOS
             WHERE VEICULOID > @Offset AND VEICULOID < @Limit
          ORDER BY ANO ASC;             
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
            FROM DB_Mecanic.dbo.TBVEICULOS;
        ";

        public const string INSERT = @"
            INSERT INTO DB_Mecanic.dbo.TBVEICULOS
                       (CLIENTEID,                   
                        MODELO,
                        MARCA,
                        ANO,
                        PLACA,
                        COR)
                VALUES (@CLIENTEID,
                        @MODELO,
                        @MARCA,
                        @ANO,
                        @PLACA,
                        @COR);                                 
        ";

        public const string UPDATE = @"
            UPDATE DB_Mecanic.dbo.TBVEICULOS 
               SET CLIENTEID = @CLIENTEID,
                   MODELO = @MODELO,                   
                   MARCA = @MARCA,
                   ANO = @ANO,
                   PLACA = @PLACA,
                   COR = @COR
             WHERE VEICULOID = @Id;
        ";

        public const string DELETE = @"
             DELETE DB_Mecanic.dbo.TBVEICULOS 
             WHERE VEICULOID = @Id;
        ";

        public const string ATIVAR = @"
             UPDATE DB_Mecanic.dbo.TBVEICULOS 
               SET ATIVO = 1                   
             WHERE VEICULOID = @Id;
        ";

        public const string EXIST_VEICULO_DO_CLIENTE = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBVEICULOS 
             WHERE PLACA = @Placa AND CLIENTEID = @ClienteId;
        ";
        public const string EXIST_VEICULO_ID = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBVEICULOS 
             WHERE VEICULOID = @Id;
        ";
    }
}
