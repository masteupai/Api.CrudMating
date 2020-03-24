using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Queries
{
    public class ClienteQuery
    {
        public const string GET = @"
          
            SELECT CLIENTEID,
                   NOME,                   
                   DOCUMENTO,
                   DATANASCIMENTO,
                   ATIVO
              FROM DB_Mecanic.dbo.TBCLIENTES
             WHERE CLIENTEID = @Id;
           
        ";

        public const string PAGINATE = @"
            SELECT CLIENTEID,                   
                   NOME,
                   DOCUMENTO,
                   DATANASCIMENTO,
                   ATIVO                 
              FROM DB_Mecanic.dbo.TBCLIENTES
             WHERE CLIENTEID > @Offset AND CLIENTEID < @Limit AND ATIVO = 1
          ORDER BY NOME ASC;             
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
            FROM DB_Mecanic.dbo.TBCLIENTES;
        ";

        public const string INSERT = @"
            INSERT INTO DB_Mecanic.dbo.TBCLIENTES
                       (NOME,
                        DOCUMENTO,
                        DATANASCIMENTO,
                        ATIVO)
                VALUES (@Nome,
                        @Documento,
                        @DataNascimento,
                        @Ativo);       
                SELECT @@IDENTITY;                              
        ";

        public const string UPDATE = @"
            UPDATE DB_Mecanic.dbo.TBCLIENTES 
               SET NOME = @Nome,
                   DATANASCIMENTO = @DataNascimento,                   
                   DOCUMENTO = @Documento,
                   ATIVO = @Ativo
             WHERE CLIENTEID = @Id;
        ";

        public const string DELETE = @"
             UPDATE DB_Mecanic.dbo.TBCLIENTES 
               SET ATIVO = 0                   
             WHERE CLIENTEID = @Id;
        ";

        public const string ATIVAR = @"
             UPDATE DB_Mecanic.dbo.TBCLIENTES 
               SET ATIVO = 1                   
             WHERE CLIENTEID = @Id;
        ";

        public const string EXIST_CLIENTE = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBCLIENTES 
             WHERE DOCUMENTO = @Cpf;
        ";
        public const string EXIST_CLIENTE_ID = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBCLIENTES 
             WHERE CLIENTEID = @Id;
        ";
    }
}
