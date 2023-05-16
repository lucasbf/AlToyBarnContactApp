USE master;
CREATE DATABASE CONTATOS;
USE CONTATOS;
CREATE TABLE Cliente (
    Id INT IDENTITY,
    Nome VARCHAR(100) NOT NULL,
    Endereco VARCHAR(200),
    CONSTRAINT PK_Cliente PRIMARY KEY (Id)
);

CREATE TABLE Contato (
    Id INT IDENTITY,
    Perfil VARCHAR(100),
    Tipo VARCHAR(10),
    IdCliente INT,
    CONSTRAINT PK_Contato PRIMARY KEY (Id, IdCliente),
    CONSTRAINT FK_Contato_Cliente FOREIGN KEY (IdCliente)
        REFERENCES Cliente (Id)
);