using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Queries
{
    public class FuncionarioQuery
    {
        public const string GET = @"
          
            SELECT FUNCIONARIOID,
                   NOME,
                   SALARIO,
                   DOCUMENTO,
                   ATIVO
              FROM DB_Mecanic.dbo.TBFUNCIONARIOS
             WHERE FUNCIONARIOID = @Id;
           
        ";

        public const string PAGINATE = @"
            SELECT FUNCIONARIOID,                   
                   NOME,
                   SALARIO,
                   DOCUMENTO,
                   ATIVO                 
              FROM DB_Mecanic.dbo.TBFUNCIONARIOS
             WHERE FUNCIONARIOID > @Offset AND FUNCIONARIOID < @Limit AND ATIVO = 1
          ORDER BY FUNCIONARIOID ASC;             
        ";

        public const string TOTAL = @"
            SELECT COUNT(1)
            FROM DB_Mecanic.dbo.TBFUNCIONARIOS;
        ";

        public const string INSERT = @"
            INSERT INTO DB_Mecanic.dbo.TBFUNCIONARIOS
                       (NOME,
                        SALARIO,
                        DOCUMENTO,
                        ATIVO)
                VALUES (@Nome,
                        @Salario,
                        @Documento,
                        @Ativo);                                 
        ";

        public const string UPDATE = @"
            UPDATE DB_Mecanic.dbo.TBFUNCIONARIOS 
               SET NOME = @Nome,
                   SALARIO = @Salario,
                   DOCUMENTO = @Documento
             WHERE FUNCIONARIOID = @Id;
        ";

        public const string DELETE = @"
             UPDATE DB_Mecanic.dbo.TBFUNCIONARIOS 
               SET ATIVO = 0                   
             WHERE FUNCIONARIOID = @FuncionarioId;
        "; 
        
        public const string ATIVAR = @"
             UPDATE DB_Mecanic.dbo.TBFUNCIONARIOS 
               SET ATIVO = 1                   
             WHERE FUNCIONARIOID = @FuncionarioId;
        ";

        public const string EXIST_FUNCIONARIO = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBFUNCIONARIOS 
             WHERE DOCUMENTO = @Cpf;
        ";
        public const string EXIST_FUNCIONARIO_ID = @"
            SELECT count(1) 
              FROM DB_Mecanic.dbo.TBFUNCIONARIOS 
             WHERE FUNCIONARIOID = @Id;
        ";
    }
}
