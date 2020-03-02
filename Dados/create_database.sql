use master
go

if not exists (select * from sys.databases where name='DB_Mecanic')
create database DB_Mecanic;
go

use DB_Mecanic
go

if not exists (select * from sysobjects where name = 'TBCLIENTES' and xtype='U')
create table TBCLIENTES(
CLIENTEID INT IDENTITY,
NOME VARCHAR(30) NOT NULL,
CONSTRAINT TBCLIENTES_PK_CLIENTEID PRIMARY KEY (CLIENTEID)
)
go

if not exists (select * from sysobjects where name ='TBENDERECOSCLI' AND XTYPE='U')
create table TBENDERECOSCLI(
ENDERECOCLIID INT IDENTITY,
CLIENTEID INT,
LOGRADOURO varchar(50) not null,
BAIRRO varchar(50) not null,
CIDADE varchar(30) not null,
UF varchar(2) not null,
CONSTRAINT TBENDERECOSCLI_PK_ENDERECOCLIID PRIMARY KEY (ENDERECOCLIID),
CONSTRAINT TBENDERECOSCLI_FK_CLIENTEID FOREIGN KEY (CLIENTEID) REFERENCES TBCLIENTES(CLIENTEID)
)
GO

if not exists (select * from sysobjects where name ='TBCONTATOSCLI' AND XTYPE='U')
create table TBCONTATOSCLI(
CONTATOCLIID INT IDENTITY,
CLIENTEID INT, 
EMAIL varchar(50),
TELEFONE varchar(16),
CONSTRAINT TBCONTATOSCLI_PK_CONTATOCLIID PRIMARY KEY (CONTATOCLIID),
CONSTRAINT TBCONTATOSCLI_FK_CLIENTEID FOREIGN KEY (CLIENTEID) REFERENCES TBCLIENTES(CLIENTEID)
)
go


if not exists (select * from sysobjects where name = 'TBFUNCIONARIOS' and xtype='U')
create table TBFUNCIONARIOS(
FUNCIONARIOID INT IDENTITY,
NOME VARCHAR(30) NOT NULL,
DOCUMENTO VARCHAR(16) NOT NULL,
SALARIO decimal (6,2),
ATIVO bit,
CONSTRAINT TBFUNCIONARIOS_PK_FUNCIONARIOID PRIMARY KEY (FUNCIONARIOID)
)
go


if not exists (select * from sysobjects where name ='TBENDERECOSFUN' AND XTYPE='U')
create table TBENDERECOSFUN(
ENDERECOFUNID INT IDENTITY,
FUNCIONARIOID INT,
LOGRADOURO varchar(50) not null,
BAIRRO varchar(50) not null,
CIDADE varchar(30) not null,
UF varchar(2) not null, 
CONSTRAINT TBENDERECOSFUN_PK_ENDERECOFUNID PRIMARY KEY (ENDERECOFUNID),
CONSTRAINT TBENDERECOSFUN_FK_FUNCIONARIOID FOREIGN KEY (FUNCIONARIOID) REFERENCES TBFUNCIONARIOS(FUNCIONARIOID)
)
GO

if not exists (select * from sysobjects where name ='TBCONTATOSFUN' AND XTYPE='U')
create table TBCONTATOSFUN(
CONTATOFUNID INT IDENTITY,
FUNCIONARIOID INT, 
EMAIL varchar(50),
TELEFONE varchar(16),
CONSTRAINT TBCONTATOSFUN_PK_CONTATOFUNID PRIMARY KEY (CONTATOFUNID),
CONSTRAINT TBCONTATOSFUN_FK_FUNCIONARIOID FOREIGN KEY (FUNCIONARIOID) REFERENCES TBFUNCIONARIOS(FUNCIONARIOID)
)
go

if not exists (select * from sysobjects where name = 'TBVEICULOS' and xtype='U')
create table TBVEICULOS(
VEICULOID INT IDENTITY,
CLIENTEID INT,
MODELO VARCHAR(30),
MARCA VARCHAR(30),
ANO VARCHAR(4),
CONSTRAINT TBVEICULOS_PK_VEICULOID PRIMARY KEY (VEICULOID),
CONSTRAINT TBVEICULOS_FK_CLIENTEID FOREIGN KEY (CLIENTEID) REFERENCES TBCLIENTES(CLIENTEID)
)
go

if not exists (select * from sysobjects where name = 'TBPRODUTOS' and xtype='U')
create table TBPRODUTOS(
PRODUTOID INT IDENTITY,
CODIGO VARCHAR(30) NOT NULL,
NOME VARCHAR(30) NOT NULL,
DESCRICAO VARCHAR(50),
QUANTIDADE INT,
VALOR DECIMAL (6,2) NOT NULL,
TIPO VARCHAR(30) NOT NULL,
ATIVO bit NOT NULL, 
CONSTRAINT TBPRODUTOS_PK_PRODUTOID PRIMARY KEY (PRODUTOID)
)
go


if not exists (select * from sysobjects where name = 'TBSERVICOS' and xtype='U')
create table TBSERVICOS(
SERVICOID INT IDENTITY,
VEICULOID INT,
DATAINCIO DATETIME,
DATAFIM DATETIME,
SITUACAO VARCHAR(10),
PRECOTOTAL DECIMAL (6,2) NOT NULL,
CONSTRAINT TBSERVICOS_PK_SERVICOID PRIMARY KEY (SERVICOID),
CONSTRAINT TBSERVICOS_FK_VEICULOID FOREIGN KEY (VEICULOID) REFERENCES TBVEICULOS(VEICULOID)
)
go


if not exists (select * from sysobjects where name = 'TBPRODUTOSERVICO' and xtype='U')
create table TBPRODUTOSERVICO(
PRODUTOSERVICOID INT IDENTITY,
PRODUTOID INT,
SERVICOID INT,
CONSTRAINT TBPRODUTOSERVICO_PK_PRODUTOSERVICOID PRIMARY KEY (PRODUTOSERVICOID),
CONSTRAINT TBPRODUTOSERVICO_FK_PRODUTOID FOREIGN KEY (PRODUTOID) REFERENCES TBPRODUTOS(PRODUTOID),
CONSTRAINT TBPRODUTOSERVICO_FK_SERVICOID FOREIGN KEY (SERVICOID) REFERENCES TBSERVICOS(SERVICOID)
)
go

if not exists (select * from sysobjects where name = 'TBFUNCIONARIOSERVICO' and xtype='U')
create table TBFUNCIONARIOSERVICO(
FUNCIONARIOSERVICOID INT IDENTITY,
FUNCIONARIOID INT,
SERVICOID INT,
COMISSAO DECIMAL (6,2),
CONSTRAINT TBFUNCIONARIOSERVICO_PK_FUNCIONARIOSERVICOID PRIMARY KEY (FUNCIONARIOSERVICOID),
CONSTRAINT TBFUNCIONARIOSERVICO_FK_FUNCIONARIOID FOREIGN KEY (FUNCIONARIOID) REFERENCES TBFUNCIONARIOS(FUNCIONARIOID),
CONSTRAINT TBFUNCIONARIOSERVICO_FK_SERVICOID FOREIGN KEY (SERVICOID) REFERENCES TBSERVICOS(SERVICOID)
)
go